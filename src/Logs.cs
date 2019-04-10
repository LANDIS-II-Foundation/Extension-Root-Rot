using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Landis.Library.Metadata;

namespace Landis.Extension.RootRot
{
    public class EventsLog
    {
        //log.WriteLine("Time,Initiation Site,Total Sites,Damaged Sites,Cohorts Killed,Mean Severity");

        [DataFieldAttribute(Unit = FieldUnits.Year, Desc = "...")]
        public int Time { set; get; }

        [DataFieldAttribute(Desc = "Initiation Row")]
        public int InitRow { set; get; }

        [DataFieldAttribute(Desc = "Initiation Column")]
        public int InitColumn { set; get; }

        [DataFieldAttribute(Desc = "Event Type")]
        public string Type { set; get; }

        [DataFieldAttribute(Unit = "km", Desc = "Event Length")]
        public double Length { set; get; }

        [DataFieldAttribute(Unit = "km", Desc = "Event Width")]
        public double Width { set; get; }

        [DataFieldAttribute(Desc = "Event Direction")]
        public double Direction { set; get; }

        [DataFieldAttribute(Desc = "Event Intensity")]
        public double Intensity { set; get; }

        [DataFieldAttribute(Unit = FieldUnits.Count, Desc = "Total Sites in Event")]
        public double TotalSites { set; get; }

        [DataFieldAttribute(Unit = FieldUnits.Count, Desc = "Damaged Sites in Event")]
        public double DamageSites { set; get; }

        [DataFieldAttribute(Unit = "ha", Desc = "Total Area in Event")]
        public double TotalArea { set; get; }

        [DataFieldAttribute(Unit = "ha", Desc = "Damaged Area in Event")]
        public double DamagedArea { set; get; }

        [DataFieldAttribute(Unit = FieldUnits.Count, Desc = "Number of Cohorts Killed")]
        public int CohortsKilled { set; get; }

        [DataFieldAttribute(Unit = FieldUnits.Severity_Rank, Desc = "Mean Severity (1-5)", Format = "0.00")]
        public double MeanSeverity { set; get; }

    }

    public class SummaryLog
    {
        [DataFieldAttribute(Unit = FieldUnits.Year, Desc = "...")]
        public int Time { set; get; }

        [DataFieldAttribute(Unit = FieldUnits.Count, Desc = "Sites Infected")]
        public double InfectedSites { set; get; }

        [DataFieldAttribute(Unit = FieldUnits.Count, Desc = "Sites Diseased")]
        public double DiseasedSites { set; get; }

        [DataFieldAttribute(Unit = FieldUnits.g_B_m2, Desc = "Mortality Biomass")]
        public double MortalityBiomass { set; get; }
    }
}
