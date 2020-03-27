//  Copyright 2005-2010 Portland State University, University of Wisconsin
//  Authors:  Robert M. Scheller, James B. Domingo

using Landis.SpatialModeling;
using Landis.Core;
using Landis.Library.Metadata;
using System.Collections.Generic;
using System.IO;
using System;
using Landis.Utilities;
//using Landis.Library.Climate;
using System.Linq;
//using Landis.Library.Cohorts;
using Landis.Library.BiomassCohorts;

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
        private string lethalTempMapNameTemplate;
        private string totalBiomassRemovedMapNameTemplate;
        private string speciesBiomassRemovedMapNameTemplate;

        //private StreamWriter log;
        public static IInputParameters Parameters;
        private static ICore modelCore;
        public static int ActualYear;

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
            Parameters = Landis.Data.Load<IInputParameters>(dataFile, parser);
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
            MetadataHandler.InitializeMetadata(Parameters, ModelCore);

            Timestep = Parameters.Timestep;
            outMapNameTemplate = Parameters.OutMapNamesTemplate;
            tolpMapNameTemplate = Parameters.TOLPMapNamesTemplate;
            lethalTempMapNameTemplate = Parameters.LethalTempMapNameTemplate;
            totalBiomassRemovedMapNameTemplate = Parameters.TotalBiomassRemovedMapNameTemplate;
            speciesBiomassRemovedMapNameTemplate = Parameters.SpeciesBiomassRemovedMapNamesTemplate;

            SiteVars.Initialize(Parameters.InputMapName);
        }

        //---------------------------------------------------------------------

        ///<summary>
        /// Run the plug-in at a particular timestep.
        ///</summary>
        public override void Run()
        {
            ModelCore.UI.WriteLine("Processing landscape for root rot ...");

            ActualYear = 0;

            // new Event for each timestep
            Event newEvent = new Event();
            int lethalTempSites = 0;

            foreach (ActiveSite site in ModelCore.Landscape.ActiveSites)
            {
                newEvent.currentSite = site;
                int status = SiteVars.Status[site];
                float pressureHead = SiteVars.PressureHead[site];
                SiteVars.SpeciesBiomassRemoved[site] = new Dictionary<ISpecies, int>();
                SiteVars.TotalBiomassRemoved[site] = 0;

                int newStatus = status;
                if (status > 0) // Status 0 = Nonactive, not processed
                {
                    IEcoregion ecoregion = PlugIn.ModelCore.Ecoregion[site];
                    //float tmin = ecoMinTemp[ecoregion];
                    float tmin = SiteVars.ExtremeMinTemp[site];
                    float dTemp = (tmin - Parameters.LethalTemp) / Math.Abs(Parameters.LethalTemp);
                    dTemp = Math.Min(dTemp, 1);
                    dTemp = Math.Max(dTemp, 0);
                    
                    bool presence = (status > 1);
                    if (dTemp < PlugIn.ModelCore.GenerateUniform())
                    {
                        presence = false;
                        lethalTempSites += 1;
                        newStatus = 1;  // If Presence == false, site transitions to Susceptible (S) regardless of current state 
                        SiteVars.LethalTemp[site] = (int) Math.Round(tmin);
                    }
                    else  // If Presence == true, other transitions are possible based on Conducive Environment
                    {
                        SiteVars.LethalTemp[site] = 99;
                        presence = true;
                        float pSI = 0;
                        //float pSD = 0;
                        float pID = 0;
                        //float pIS = 0;
                        float pDI = 0;
                        //float pDS = 0;
                        if (status == 3) // Site currently Diseased (D) can transition to Susceptible (S) or Infected (I)
                        {
                            // probability of D converting to S only based on presenece - handled above

                            // probability of D converting to I (pDI)
                            float maxSusceptibility = 0;

                            foreach (ISpeciesCohorts speciesCohorts in SiteVars.Cohorts[site])
                            {
                                foreach (ICohort cohort in speciesCohorts)
                                {
                                    float speciesSuscept = Parameters.SusceptibilityTable[cohort.Species][0];
                                    if (speciesSuscept > maxSusceptibility)
                                    {
                                        maxSusceptibility = speciesSuscept;
                                        break;
                                    }
                                }
                            }
                            
                            if (maxSusceptibility == 0)
                                pDI = 1;
                            else
                            {
                                float m2 = (float) (1.0 / (((Parameters.PhDry - Parameters.PhWet)/ 2.0) - Parameters.PhWet));
                                float b2 = (float) -1.0 * Parameters.PhWet * m2;
                                float m3 = (float) (1.0 / ((Parameters.PhDry - Parameters.PhWet) / 2.0 - Parameters.PhDry));
                                float b3 = (float) -1.0 * Parameters.PhDry * m3;
                                if (pressureHead < Parameters.PhWet)
                                    pDI = 0;
                                else if (pressureHead > Parameters.PhDry)
                                    pDI = 0;
                                else if (pressureHead <= (Parameters.PhDry - Parameters.PhWet) / 2)
                                    pDI = m2 * pressureHead + b2;
                                else
                                    pDI = m3 * pressureHead + b3;
                                pDI = Math.Min(pDI, Parameters.MaxProbDI);
                            }
                            if(pDI >= PlugIn.ModelCore.GenerateUniform())
                            {
                                newStatus = 2;
                            }
                        }
                        else if (status == 2)  // Site currently Infected (I) can transition to Diseased (D) or Susceptible (S)
                        {
                            // probability of I converting to S only based on presenece - handled above

                            // probability of I converting to D
                            pID = Calc_pID(Parameters, pressureHead);
                            if (pID >= PlugIn.ModelCore.GenerateUniform())
                            {
                                newStatus = 3;
                            }
                        }
                        else if (status == 1)  // Site currently Susceptible (S) can transition to Infected (I) or Diseased (D)
                        {
                            // probability of S converting to I
                            if (pressureHead < Parameters.PhWet)
                                pSI = (float)-1.0 / Parameters.PhWet * pressureHead + 1;
                            else
                                pSI = 0;
                            if (pSI >= PlugIn.ModelCore.GenerateUniform())
                            {
                                // probability of S converting to D is contingent on S converting to I
                                pID = Calc_pID(Parameters, pressureHead);
                                if (pID >= PlugIn.ModelCore.GenerateUniform())                             
                                    newStatus = 3;                                
                                else
                                    newStatus = 2;
                            }
                        }
                    }
                    if (newStatus == 2) // Infected
                        newEvent.InfectedSites += 1;
                    if(newStatus == 3) // Diseased - can cause damage
                    {
                        newEvent.DiseasedSites += 1;
                        int damage = SiteVars.Cohorts[site].ReduceOrKillBiomassCohorts(newEvent);
                        if(damage > 0)
                        {
                            newEvent.TotalSitesDamaged += 1;
                        }
                        SiteVars.TimeOfLastDisease[site] = PlugIn.ModelCore.CurrentTime;
                    }
                }
                SiteVars.Status[site] = newStatus;
            }
            newEvent.LethalTemp = (float) lethalTempSites / (float) PlugIn.ModelCore.Landscape.ActiveSiteCount;
            // Write logs
            LogEvent(PlugIn.ModelCore.CurrentTime, newEvent);

            // Write output maps
            string path = "";
            Dimensions dimensions = new Dimensions(ModelCore.Landscape.Rows, ModelCore.Landscape.Columns);
            //  Write Pathogen Status map
            if (outMapNameTemplate != null)
            {
                path = MapNames.ReplaceTemplateVars(outMapNameTemplate, ModelCore.CurrentTime);
                using (IOutputRaster<IntPixel> outputRaster = ModelCore.CreateRaster<IntPixel>(path, dimensions))
                {
                    IntPixel pixel = outputRaster.BufferPixel;
                    foreach (Site site in ModelCore.Landscape.AllSites)
                    {
                        if (site.IsActive)
                        {
                                pixel.MapCode.Value = (int)(SiteVars.Status[site]);                            
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
            //  Write Time of Last Pathogen map
            if (tolpMapNameTemplate != null)
            {             
                path = MapNames.ReplaceTemplateVars(tolpMapNameTemplate, ModelCore.CurrentTime);
                using (IOutputRaster<IntPixel> outputRaster = ModelCore.CreateRaster<IntPixel>(path, dimensions))
                {
                    IntPixel pixel = outputRaster.BufferPixel;
                    foreach (Site site in ModelCore.Landscape.AllSites)
                    {
                        if (site.IsActive)
                        {
                            pixel.MapCode.Value = (int)(SiteVars.TimeOfLastDisease[site]);

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
            //  Write Lethal Temp map
            if (lethalTempMapNameTemplate != null)
            {
                path = MapNames.ReplaceTemplateVars(lethalTempMapNameTemplate, ModelCore.CurrentTime);
                using (IOutputRaster<IntPixel> outputRaster = ModelCore.CreateRaster<IntPixel>(path, dimensions))
                {
                    IntPixel pixel = outputRaster.BufferPixel;
                    foreach (Site site in ModelCore.Landscape.AllSites)
                    {
                        if (site.IsActive)
                        {
                            pixel.MapCode.Value = (int)(SiteVars.LethalTemp[site]);

                        }
                        else
                        {
                            //  Inactive site
                            pixel.MapCode.Value = 99;
                        }
                        outputRaster.WriteBufferPixel();
                    }
                }
            }
            //  Write TotalBiomassRemoved map
            if (totalBiomassRemovedMapNameTemplate != null)
            {
                path = MapNames.ReplaceTemplateVars(totalBiomassRemovedMapNameTemplate, ModelCore.CurrentTime);
                using (IOutputRaster<IntPixel> outputRaster = ModelCore.CreateRaster<IntPixel>(path, dimensions))
                {
                    IntPixel pixel = outputRaster.BufferPixel;
                    foreach (Site site in ModelCore.Landscape.AllSites)
                    {
                        if (site.IsActive)
                        {
                            pixel.MapCode.Value = (int)(SiteVars.TotalBiomassRemoved[site]);
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
            //  Write SpeciesBiomassRemoved maps
            if (speciesBiomassRemovedMapNameTemplate != null)
            {
                foreach (ISpecies spc in PlugIn.ModelCore.Species)
                {
                    path = MapNames.ReplaceTemplateVars(speciesBiomassRemovedMapNameTemplate, spc.Name, ModelCore.CurrentTime);
                    using (IOutputRaster<IntPixel> outputRaster = ModelCore.CreateRaster<IntPixel>(path, dimensions))
                    {
                        IntPixel pixel = outputRaster.BufferPixel;
                        foreach (Site site in ModelCore.Landscape.AllSites)
                        {
                            if ((site.IsActive) && (SiteVars.SpeciesBiomassRemoved[site].ContainsKey(spc)))
                            {
                                pixel.MapCode.Value = (int)(SiteVars.SpeciesBiomassRemoved[site][spc]);
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
            }


        }
        private static float Calc_pID(IInputParameters parameters, float pressureHead)
        {
            float pID = 0;
            float m1 = (float)(1.0 - parameters.MinProbID) / (parameters.PhMax - parameters.PhDry);
            float b1 = (float)(parameters.MinProbID - (1.0 * parameters.PhDry * m1));
            if (pressureHead < parameters.PhWet)
                pID = (float)((parameters.MinProbID - 1.0) / parameters.PhWet * pressureHead + 1.0);
            else if (pressureHead > parameters.PhDry)
                if (pressureHead > parameters.PhMax)
                    pID = 1;
                else
                    pID = m1 * pressureHead + b1;
            else
                pID = parameters.MinProbID;
            return pID;

        }
        //---------------------------------------------------------------------

        private void LogEvent(int currentTime,
                              Event diseaseEvent)
        {
            eventLog.Clear();
            foreach(int i in Enumerable.Range(0, PlugIn.ModelCore.Species.Count))
            {
                EventsLog el = new EventsLog();
                el.Time = currentTime;
                el.Species = PlugIn.ModelCore.Species[i].Name;
                el.MortalityBiomass = diseaseEvent.BiomassRemovedList[i];
                eventLog.AddObject(el);
            }
            eventLog.WriteToFile();
            
            summaryLog.Clear();
            SummaryLog sl = new SummaryLog();
            sl.Time = currentTime;
            sl.InfectedSites = diseaseEvent.InfectedSites;
            sl.DiseasedSites = diseaseEvent.DiseasedSites;
            sl.DamageSites = diseaseEvent.TotalSitesDamaged;
            sl.CohortsDamaged = diseaseEvent.CohortsDamaged;
            sl.CohortsKilled = diseaseEvent.CohortsKilled;
            sl.MortalityBiomass = diseaseEvent.BiomassRemoved;
            sl.LethalTemp = diseaseEvent.LethalTemp;

            summaryLog.AddObject(sl);
            summaryLog.WriteToFile();
        }


        }
}
