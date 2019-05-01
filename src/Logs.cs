using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Landis.Library.Metadata;

namespace Landis.Extension.RootRot
{
    public class EventsLog
    {
        //log.WriteLine("Time, Damaged Sites, Cohorts Damaged, Cohorts Killed, Mortality Biomass");

        [DataFieldAttribute(Unit = FieldUnits.Year, Desc = "...")]
        public int Time { set; get; }

        [DataFieldAttribute(Unit = FieldUnits.Count, Desc = "Damaged Sites in Event")]
        public double DamageSites { set; get; }

        [DataFieldAttribute(Unit = FieldUnits.Count, Desc = "Number of Cohorts Damaged")]
        public int CohortsDamaged{ set; get; }

        [DataFieldAttribute(Unit = FieldUnits.Count, Desc = "Number of Cohorts Killed")]
        public int CohortsKilled { set; get; }

        [DataFieldAttribute(Unit = FieldUnits.g_B_m2, Desc = "Mortality Biomass")]
        public double MortalityBiomass { set; get; }

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
