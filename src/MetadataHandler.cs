using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Landis.Library.Metadata;
using Landis.Utilities;
using Landis.Core;

namespace Landis.Extension.RootRot
{
    public static class MetadataHandler
    {
        
        public static ExtensionMetadata Extension {get; set;}

        public static void InitializeMetadata(IInputParameters parameters, ICore mCore)
        {
            ScenarioReplicationMetadata scenRep = new ScenarioReplicationMetadata() {
                RasterOutCellArea = PlugIn.ModelCore.CellArea,
                TimeMin = PlugIn.ModelCore.StartTime,
                TimeMax = PlugIn.ModelCore.EndTime//,
                //ProjectionFilePath = "Projection.?" 
            };

            Extension = new ExtensionMetadata(mCore){
                Name = PlugIn.ExtensionName,
                TimeInterval = parameters.Timestep, 
                ScenarioReplicationMetadata = scenRep
            };

            //---------------------------------------
            //          table outputs:   
            //---------------------------------------

            System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(parameters.EventLogFileName));
            PlugIn.eventLog = new MetadataTable<EventsLog>(parameters.EventLogFileName);
            PlugIn.summaryLog = new MetadataTable<SummaryLog>(parameters.SummaryLogFileName);

            OutputMetadata tblOut_events = new OutputMetadata()
            {
                Type = OutputType.Table,
                Name = "RootRotEventLog",
                FilePath = PlugIn.eventLog.FilePath,
                Visualize = false,
            };
            tblOut_events.RetriveFields(typeof(EventsLog));
            Extension.OutputMetadatas.Add(tblOut_events);

            OutputMetadata tblOut_summary = new OutputMetadata()
            {
                Type = OutputType.Table,
                Name = "RootRotSummaryLog",
                FilePath = PlugIn.summaryLog.FilePath,
                Visualize = false,
            };
            tblOut_summary.RetriveFields(typeof(SummaryLog));
            Extension.OutputMetadatas.Add(tblOut_summary);

            //---------------------------------------            
            //          map outputs:         
            //---------------------------------------

            OutputMetadata mapOut_Severity = new OutputMetadata()
            {
                Type = OutputType.Map,
                Name = "severity",
                FilePath = @parameters.OutMapNamesTemplate,
                Map_DataType = MapDataType.Ordinal,
                Map_Unit = FieldUnits.Severity_Rank,
                Visualize = true,
            };
            Extension.OutputMetadatas.Add(mapOut_Severity);

            //---------------------------------------
            MetadataProvider mp = new MetadataProvider(Extension);
            mp.WriteMetadataToXMLFile("Metadata", Extension.Name, Extension.Name);




        }
    }
}
