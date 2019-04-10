using Landis.SpatialModeling;
using Landis.Library.AgeOnlyCohorts;

namespace Landis.Extension.RootRot
{
    public static class SiteVars
    {
        private static ISiteVar<int> status;
        private static ISiteVar<int> timeOfLastDisease;
        private static ISiteVar<ISiteCohorts> cohorts;

        //---------------------------------------------------------------------
        public static void Initialize()
        {
            status = PlugIn.ModelCore.Landscape.NewSiteVar<int>();
            cohorts = PlugIn.ModelCore.GetSiteVar<ISiteCohorts>("Succession.AgeCohorts");
            timeOfLastDisease = PlugIn.ModelCore.GetSiteVar<int>("Pathogen.TimeOfLastDisease");  // If other wind disturbance extension is active, use the registered site var from it
            if (timeOfLastDisease == null)
            {
                timeOfLastDisease = PlugIn.ModelCore.Landscape.NewSiteVar<int>();
                PlugIn.ModelCore.RegisterSiteVar(SiteVars.TimeOfLastDisease, "Pathogen.TimeOfLastDisease");
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
        public static ISiteVar<int> Status
        {
            get
            {
                return status;
            }
        }
        //---------------------------------------------------------------------
    }
}
