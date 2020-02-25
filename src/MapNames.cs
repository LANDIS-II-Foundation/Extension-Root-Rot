using System;
using System.Collections.Generic;
using System.Text;
using Landis.Utilities;
using Landis.Core;

namespace Landis.Extension.RootRot
{
    /// <summary>
	/// Methods for working with the template for map filenames.
	/// </summary>
    public static class MapNames
    {
        public const string TimestepVar = "timestep";
        public const string SpeciesVar = "species";

        private static IDictionary<string, bool> knownVars;
        private static IDictionary<string, string> varValues;

        //---------------------------------------------------------------------

        static MapNames()
        {
            knownVars = new Dictionary<string, bool>();
            knownVars[TimestepVar] = true;

            varValues = new Dictionary<string, string>();
        }

        //---------------------------------------------------------------------

        public static void CheckTemplateVars(string template)
        {
            OutputPath.CheckTemplateVars(template, knownVars);
        }

        //---------------------------------------------------------------------

        public static string ReplaceTemplateVars(string template,
                                                 int timestep)
        {
            varValues[TimestepVar] = timestep.ToString();
            return OutputPath.ReplaceTemplateVars(template, varValues);
        }
        //---------------------------------------------------------------------

        public static string ReplaceTemplateVars(string template,
                                                 string species,
                                                 int timestep)
        {
            varValues[SpeciesVar] = species;
            varValues[TimestepVar] = timestep.ToString();
            return OutputPath.ReplaceTemplateVars(template, varValues);
        }

}

}
