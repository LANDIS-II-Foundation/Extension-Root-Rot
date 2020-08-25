using Landis.SpatialModeling;
//using Landis.Library.AgeOnlyCohorts;
using System.IO;
using Landis.Library.BiomassCohorts;
using System.Collections.Generic;
using Landis.Core;

namespace Landis.Extension.RootRot
{
    public static class SiteVars
    {
        private static ISiteVar<int> status;
        private static ISiteVar<int> timeOfLastDisease;
        private static ISiteVar<ISiteCohorts> cohorts;
        private static ISiteVar<float[]> monthlyPressureHead;
        private static ISiteVar<float> extremeMinTemp;
        private static ISiteVar<int> lethalTemp;
        private static ISiteVar<int> totalBiomassRemoved;
        private static ISiteVar<Dictionary<ISpecies,int>> speciesBiomassRemoved;
        private static ISiteVar<SortedList<float,float>[]> monthlySoilTemp;
        private static ISiteVar<float> fieldCapacity;

        //---------------------------------------------------------------------
        public static void Initialize(string inputMapName)
        {
            status = PlugIn.ModelCore.Landscape.NewSiteVar<int>(0);
            lethalTemp = PlugIn.ModelCore.Landscape.NewSiteVar<int>();
            totalBiomassRemoved = PlugIn.ModelCore.Landscape.NewSiteVar<int>(0);
            speciesBiomassRemoved = PlugIn.ModelCore.Landscape.NewSiteVar < Dictionary<ISpecies, int>>();
            foreach (ActiveSite site in PlugIn.ModelCore.Landscape.ActiveSites)
                speciesBiomassRemoved[site] = new Dictionary<ISpecies, int>();

            cohorts = PlugIn.ModelCore.GetSiteVar<ISiteCohorts>("Succession.BiomassCohorts");
            timeOfLastDisease = PlugIn.ModelCore.GetSiteVar<int>("Pathogen.TimeOfLastDisease");  // If other pathogen disturbance extension is active, use the registered site var from it
            if (timeOfLastDisease == null)
            {
                timeOfLastDisease = PlugIn.ModelCore.Landscape.NewSiteVar<int>();
                foreach (Site site in PlugIn.ModelCore.Landscape.AllSites)
                {
                    if (site.IsActive)
                        timeOfLastDisease[site] = -9999;
                    else
                        timeOfLastDisease[site] = 0;
                }
                PlugIn.ModelCore.RegisterSiteVar(SiteVars.TimeOfLastDisease, "Pathogen.TimeOfLastDisease");
            }
            monthlyPressureHead = PlugIn.ModelCore.GetSiteVar<float[]>("Succession.MonthlyPressureHead");
            extremeMinTemp = PlugIn.ModelCore.GetSiteVar<float>("Succession.ExtremeMinTemp");
            monthlySoilTemp = PlugIn.ModelCore.GetSiteVar<SortedList<float, float>[]>("Succession.MonthlySoilTemp");
            fieldCapacity = PlugIn.ModelCore.GetSiteVar<float>("Succession.SoilFieldCapacity");

            if (inputMapName == null)
            {
                foreach(Site site in PlugIn.ModelCore.Landscape.AllSites)
                {
                    if (site.IsActive)
                        status[site] = 1;
                    else
                        status[site] = 0;
                }
            }
            else
            {                
                IInputRaster<IntPixel> map;

                try
                {
                    map = PlugIn.ModelCore.OpenRaster<IntPixel>(inputMapName);
                }
                catch (FileNotFoundException)
                {
                    string message = string.Format("Error: The file {0} does not exist", inputMapName);
                    throw new System.ApplicationException(message);
                }

                if (map.Dimensions != PlugIn.ModelCore.Landscape.Dimensions)
                {
                    string message = string.Format("Error: The input map {0} does not have the same dimension (row, column) as the ecoregions map", inputMapName);
                    throw new System.ApplicationException(message);
                }

                using (map)
                {
                    IntPixel pixel = map.BufferPixel;
                    foreach (Site site in PlugIn.ModelCore.Landscape.AllSites)
                    {
                        map.ReadBufferPixel();
                        int mapCode = (int)pixel.MapCode.Value;

                        if (mapCode < 0 || mapCode > 3)
                        {
                            string message = string.Format("Error: The input map {0} has values outside the range of 0-3", inputMapName);
                            throw new System.ApplicationException(message);
                        }
                        if (site.IsActive)
                        {
                            status[site] = mapCode;
                        }
                    }
                }
                
            }
        }
        //---------------------------------------------------------------------
        public static ISiteVar<ISiteCohorts> Cohorts
        {
            get
            {
                return cohorts;
            }
        }
        //---------------------------------------------------------------------
        public static ISiteVar<int> TimeOfLastDisease
        {
            get
            {
                return timeOfLastDisease;
            }
        }
        //---------------------------------------------------------------------
        public static ISiteVar<int> LethalTemp
        {
            get
            {
                return lethalTemp;
            }
        }
        //---------------------------------------------------------------------
        public static ISiteVar<int> Status
        {
            get
            {
                return status;
            }
        }
        //---------------------------------------------------------------------
        public static ISiteVar<float[]> MonthlyPressureHead
        {
            get
            {
                return monthlyPressureHead;
            }
        }
        //---------------------------------------------------------------------
        public static ISiteVar<SortedList<float,float>[]> MonthlySoilTemp
        {
            get
            {
                return monthlySoilTemp;
            }
        }
        //---------------------------------------------------------------------
        public static ISiteVar<float> ExtremeMinTemp
        {
            get
            {
                return extremeMinTemp;
            }
        }
        //---------------------------------------------------------------------
        public static ISiteVar<float> FieldCapacity
        {
            get
            {
                return fieldCapacity;
            }
        }
        //---------------------------------------------------------------------
        public static ISiteVar<int> TotalBiomassRemoved
        {
            get
            {
                return totalBiomassRemoved;
            }
        }
        //---------------------------------------------------------------------
        public static ISiteVar<Dictionary<ISpecies,int>> SpeciesBiomassRemoved
        {
            get
            {
                return speciesBiomassRemoved;
            }
        }
        //---------------------------------------------------------------------
    }
}
