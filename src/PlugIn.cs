//  Copyright 2005-2010 Portland State University, University of Wisconsin
//  Authors:  Robert M. Scheller, James B. Domingo

using Landis.SpatialModeling;
using Landis.Core;
using Landis.Library.Metadata;
using System.Collections.Generic;
using System.IO;
using System;
using Landis.Utilities;

namespace Landis.Extension.RootRot
{
    ///<summary>
    /// A disturbance plug-in that simulates root rot.
    /// </summary>

    public class PlugIn
        : ExtensionMain
    {
        public static readonly ExtensionType ExtType = new ExtensionType("disturbance:pathogen");
        public static MetadataTable<EventsLog> eventLog;
        public static MetadataTable<SummaryLog> summaryLog;

        public static readonly string ExtensionName = "Root Rot";
        
        private string outMapNameTemplate;
        private string tolpMapNameTemplate;
        //private StreamWriter log;
        private IInputParameters parameters;
        private static ICore modelCore;

        //---------------------------------------------------------------------

        public PlugIn()
            : base("Root Rot", ExtType)
        {
        }

        //---------------------------------------------------------------------

        public static ICore ModelCore
        {
            get
            {
                return modelCore;
            }
        }
        //---------------------------------------------------------------------

        public override void LoadParameters(string dataFile, ICore mCore)
        {
            modelCore = mCore;
            InputParameterParser parser = new InputParameterParser();
            parameters = Landis.Data.Load<InputParameters>(dataFile, parser);
        }
        //---------------------------------------------------------------------

        /// <summary>
        /// Initializes the plug-in with a data file.
        /// </summary>
        /// <param name="dataFile">
        /// Path to the file with initialization data.
        /// </param>
        /// <param name="startTime">
        /// Initial timestep (year): the timestep that will be passed to the
        /// first call to the component's Run method.
        /// </param>
        public override void Initialize()
        {
            MetadataHandler.InitializeMetadata(parameters, ModelCore);

            Timestep = parameters.Timestep;
            outMapNameTemplate = parameters.OutMapNamesTemplate;
            tolpMapNameTemplate = parameters.TOLPMapNamesTemplate;

            SiteVars.Initialize();

            //Event.Initialize(parameters.WindSeverities);

            //ModelCore.UI.WriteLine("   Opening wind log file \"{0}\" ...", parameters.LogFileName);
            //log = Landis.Data.CreateTextFile(parameters.LogFileName);
            //log.AutoFlush = true;
            //log.WriteLine("Time,Initiation Site,Total Sites,Damaged Sites,Cohorts Killed,Mean Severity");
        }

        //---------------------------------------------------------------------

        ///<summary>
        /// Run the plug-in at a particular timestep.
        ///</summary>
        public override void Run()
        {
            ModelCore.UI.WriteLine("Processing landscape for root rot ...");

            //SiteVars.Event.SiteValues = null;
            //SiteVars.Severity.ActiveSiteValues = 0;
            //SiteVars.Intensity.ActiveSiteValues = 0;

            int eventCount = 0;

            ModelCore.NormalDistribution.Mu = parameters.NumEventsMean;
            ModelCore.NormalDistribution.Sigma = parameters.NumEventsStDev;
            double numEvents = ModelCore.NormalDistribution.NextDouble();
            numEvents = ModelCore.NormalDistribution.NextDouble();

            // units of numEvents is per year per 40000km2 - convert by timestep and landscape/cell size
            double numEventsTimestep = numEvents * Timestep;
            double numEventsKM = numEventsTimestep / 40000;
            //double landscapeKM = ModelCore.Landscape.ActiveSiteCount * (PlugIn.ModelCore.CellLength * PlugIn.ModelCore.CellLength) / (1000 * 1000);
            double landscapeKM = ModelCore.Landscape.SiteCount * (PlugIn.ModelCore.CellLength * PlugIn.ModelCore.CellLength) / (1000 * 1000);
            double totalEvents = numEventsKM * landscapeKM;
            double siteProbability = totalEvents / PlugIn.ModelCore.Landscape.ActiveSiteCount;

            
            foreach (ActiveSite site in ModelCore.Landscape.ActiveSites) {
       
                    Event windEvent = Event.Initiate(site, siteProbability, parameters);
                    if (windEvent != null)
                    {
                        LogEvent(ModelCore.CurrentTime, windEvent);
                        eventCount++;
                    }
                
            }
            ModelCore.UI.WriteLine("  Wind events: {0}", eventCount);

            string path = "";
            Dimensions dimensions = new Dimensions(ModelCore.Landscape.Rows, ModelCore.Landscape.Columns);
            //  Write wind severity map
            if (mapNameTemplate != null)
            {
                path = MapNames.ReplaceTemplateVars(mapNameTemplate, ModelCore.CurrentTime);
                using (IOutputRaster<BytePixel> outputRaster = ModelCore.CreateRaster<BytePixel>(path, dimensions))
                {
                    BytePixel pixel = outputRaster.BufferPixel;
                    foreach (Site site in ModelCore.Landscape.AllSites)
                    {
                        if (site.IsActive)
                        {
                            if (SiteVars.Disturbed[site])
                                pixel.MapCode.Value = (byte)(SiteVars.Severity[site] + 1);
                            else
                                pixel.MapCode.Value = 1;
                        }
                        else
                        {
                            //  Inactive site
                            pixel.MapCode.Value = 0;
                        }
                        outputRaster.WriteBufferPixel();
                    }
                }
            }
            //  Write wind intensity map
            if (intensityMapNameTemplate != null)
            {
                path = MapNames.ReplaceTemplateVars(intensityMapNameTemplate, ModelCore.CurrentTime);
                using (IOutputRaster<IntPixel> outputRaster = ModelCore.CreateRaster<IntPixel>(path, dimensions))
                {
                    IntPixel pixel = outputRaster.BufferPixel;
                    foreach (Site site in ModelCore.Landscape.AllSites)
                    {
                        if (site.IsActive)
                        {
                            pixel.MapCode.Value = (int)(SiteVars.Intensity[site] * 100);

                        }
                        else
                        {
                            //  Inactive site
                            pixel.MapCode.Value = 0;
                        }
                        outputRaster.WriteBufferPixel();
                    }
                }
            }
            /*//  Write time of last wind map
            path = MapNames.ReplaceTemplateVars(tolwMapNameTemplate, ModelCore.CurrentTime);
            using (IOutputRaster<IntPixel> outputRaster = ModelCore.CreateRaster<IntPixel>(path, dimensions))
            {
                IntPixel pixel = outputRaster.BufferPixel;
                foreach (Site site in ModelCore.Landscape.AllSites)
                {
                    if (site.IsActive)
                    {
                        pixel.MapCode.Value = (int)(SiteVars.TimeOfLastEvent[site]);

                    }
                    else
                    {
                        //  Inactive site
                        pixel.MapCode.Value = 0;
                    }
                    outputRaster.WriteBufferPixel();
                }
            }
             * */
        }

        //---------------------------------------------------------------------

        private void LogEvent(int   currentTime,
                              Event windEvent)
        {
            //log.WriteLine("{0},\"{1}\",{2},{3},{4},{5:0.0}",
            //              currentTime,
            //              windEvent.StartLocation,
            //              windEvent.Size,
            //              windEvent.SitesDamaged,
            //              windEvent.CohortsKilled,
            //              windEvent.Severity);

            eventLog.Clear();
            EventsLog el = new EventsLog();
            el.Time = currentTime;
            el.InitRow = windEvent.StartLocation.Row;
            el.InitColumn = windEvent.StartLocation.Column;
            el.Type = windEvent.Type;
            el.TotalSites = windEvent.SitesInEvent;
            el.DamageSites = windEvent.SitesDamaged;
            el.TotalArea = windEvent.SizeHectares;
            el.DamagedArea = windEvent.SitesDamaged * (PlugIn.ModelCore.CellLength * PlugIn.ModelCore.CellLength) / 10000;
            el.CohortsKilled = windEvent.CohortsKilled;
            el.MeanSeverity = windEvent.Severity;
            el.Intensity = windEvent.Intensity;
            el.Direction = windEvent.WindDirection;
            el.Length = windEvent.Length;
            el.Width = windEvent.Width;


            eventLog.AddObject(el);
            eventLog.WriteToFile();

        }


    }
}
