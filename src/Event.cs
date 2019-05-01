using System;
using System.Collections.Generic;
using System.Text;
using Landis.Library.BiomassCohorts;
using Landis.Core;
using Landis.SpatialModeling;

namespace Landis.Extension.RootRot
{
    public class Event
        : IDisturbance
    {
        public ActiveSite currentSite;
        public int TotalSitesDamaged;
        public int CohortsDamaged;
        public int BiomassRemoved;
        public int CohortsKilled;
        public int InfectedSites;
        public int DiseasedSites;

        //---------------------------------------------------------------------
        ExtensionType IDisturbance.Type
        {
            get
            {
                return PlugIn.ExtType;
            }
        }
        //---------------------------------------------------------------------
        ActiveSite IDisturbance.CurrentSite
        {
            get
            {
                return currentSite;
            }
        }
        //---------------------------------------------------------------------
        // Constructor function
        public Event()
        {
            this.CohortsDamaged = 0;
            this.BiomassRemoved = 0;
            this.CohortsKilled = 0;
            this.TotalSitesDamaged = 0;
            this.InfectedSites = 0;
            this.DiseasedSites = 0;
        }
        //---------------------------------------------------------------------
        //  A filter to determine which cohorts are removed.
        int IDisturbance.ReduceOrKillMarkedCohort(ICohort cohort)
        {
            float speciesSuscept = PlugIn.Parameters.SusceptibilityTable[cohort.Species];
            if (speciesSuscept > 0)
            {
                int biomassReduction = (int)Math.Round(speciesSuscept * cohort.Biomass);
                BiomassRemoved += biomassReduction;
                if (biomassReduction == cohort.Biomass)
                    CohortsKilled += 1;
                else
                    CohortsDamaged += 1;
                return biomassReduction;
            }
            else
                return 0;
        }
    }
}
