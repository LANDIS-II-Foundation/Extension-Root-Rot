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
        public List<int> BiomassRemovedList;
        public int CohortsKilled;
        public int InfectedSites;
        public int DiseasedSites;
        public float LethalTemp;

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
            this.BiomassRemovedList = new List<int>(PlugIn.ModelCore.Species.Count);
            foreach(ISpecies spc in PlugIn.ModelCore.Species)
            {
                this.BiomassRemovedList.Add(0);
            }
            this.CohortsKilled = 0;
            this.TotalSitesDamaged = 0;
            this.InfectedSites = 0;
            this.DiseasedSites = 0;
            this.LethalTemp = 0;
        }
        //---------------------------------------------------------------------
        //  A filter to determine which cohorts are removed.
        int IDisturbance.ReduceOrKillMarkedCohort(ICohort cohort)
        {
            float[] speciesSusceptList = PlugIn.Parameters.SusceptibilityTable[cohort.Species];
            float speciesSuscept = 0;
            if (cohort.Age >= PlugIn.ModelCore.CurrentTime - SiteVars.TimeOfLastDisease[this.currentSite])
            {
                // Reinfection a second (or more) time used 2nd value in speciesSusceptList
                speciesSuscept = speciesSusceptList[1];
            }
            else
            {
                // First time infected uses 1st value in speciesSusceptList
                speciesSuscept = speciesSusceptList[0];
            }
            if (speciesSuscept > 0)
            {                
                int biomassReduction = (int)Math.Round(speciesSuscept * cohort.Biomass);
                BiomassRemoved += biomassReduction;
                BiomassRemovedList[cohort.Species.Index] = BiomassRemovedList[cohort.Species.Index] + biomassReduction;
                if (SiteVars.SpeciesBiomassRemoved[this.currentSite].ContainsKey(cohort.Species))
                {
                    int prevRemoved = SiteVars.SpeciesBiomassRemoved[this.currentSite][cohort.Species];
                    SiteVars.SpeciesBiomassRemoved[this.currentSite][cohort.Species] = prevRemoved + biomassReduction;
                }
                else
                {
                    SiteVars.SpeciesBiomassRemoved[this.currentSite].Add(cohort.Species, biomassReduction);
                }
                SiteVars.TotalBiomassRemoved[this.currentSite] += biomassReduction;
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
