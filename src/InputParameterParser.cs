using Landis.Utilities;
using System.Collections.Generic;
using Landis.Core;

namespace Landis.Extension.RootRot
{
    class InputParameterParser
        : TextParser<IInputParameters>
    {
        public static ISpeciesDataset SpeciesDataset = PlugIn.ModelCore.Species;
        //---------------------------------------------------------------------
        public override string LandisDataValue
        {
            get
            {
                return PlugIn.ExtensionName;
            }
        }
        //---------------------------------------------------------------------
        public InputParameterParser()
        {
        }
        //---------------------------------------------------------------------
        protected override IInputParameters Parse()
        {
            ReadLandisDataVar();

            InputParameters parameters = new InputParameters();

            InputVar<int> timestep = new InputVar<int>("Timestep");
            ReadVar(timestep);
            parameters.Timestep = timestep.Value;

            InputVar<string> inputMapName = new InputVar<string>("InputMap");
            if (ReadOptionalVar(inputMapName))
                parameters.InputMapName = inputMapName.Value;
            else
                parameters.InputMapName = null;

            //--------- Read In Species SusceptibiityTable ---------------------------------------
            Dictionary<string, int> lineNumbers = new Dictionary<string, int>();
            PlugIn.ModelCore.UI.WriteLine("   Begin parsing SpeciesSusceptibility table.");

            const string SppParms = "SpeciesSusceptibility";
            ReadName(SppParms);
            InputVar<string> sppName = new InputVar<string>("Species");
            InputVar<float> suscept = new InputVar<float>("Susceptibility");

            while ((!AtEndOfInput) && (CurrentName != "LethalTemp"))
            {
                StringReader currentLine = new StringReader(CurrentLine);

                ReadValue(sppName, currentLine);
                ISpecies species = SpeciesDataset[sppName.Value.Actual];
                if (species == null)
                    throw new InputValueException(sppName.Value.String,
                                                  "{0} is not a species name.",
                                                  sppName.Value.String);
                int lineNumber;
                if (lineNumbers.TryGetValue(species.Name, out lineNumber))
                    throw new InputValueException(sppName.Value.String,
                                                  "The species {0} was previously used on line {1}",
                                                  sppName.Value.String, lineNumber);
                else
                    lineNumbers[species.Name] = LineNumber;

                ReadValue(suscept, currentLine);
                parameters.SusceptibilityTable[species] = suscept.Value;

                GetNextLine();
            }

            InputVar<float> lethalTemp = new InputVar<float>("LethalTemp");
            ReadVar(lethalTemp);
            parameters.LethalTemp = lethalTemp.Value;

            InputVar<float> phWet = new InputVar<float>("PhWet");
            ReadVar(phWet);
            parameters.PhWet = phWet.Value;

            InputVar<float> phDry = new InputVar<float>("PhDry");
            ReadVar(phDry);
            parameters.PhDry = phDry.Value;

            InputVar<float> phMax = new InputVar<float>("PhMax");
            ReadVar(phMax);
            parameters.PhMax = phMax.Value;

            InputVar<float> minProbID = new InputVar<float>("MinProbID");
            ReadVar(minProbID);
            parameters.MinProbID = minProbID.Value;

            InputVar<float> maxProbDI = new InputVar<float>("MaxProbDI");
            ReadVar(maxProbDI);
            parameters.MaxProbDI = maxProbDI.Value;

            InputVar<string> outputMapName = new InputVar<string>("OutputMapName");
            if (ReadOptionalVar(outputMapName))
                parameters.OutMapNamesTemplate = outputMapName.Value;
            else
                parameters.OutMapNamesTemplate = null;

            InputVar<string> tolpMapName = new InputVar<string>("TOLPMapName");
            if (ReadOptionalVar(tolpMapName))
                parameters.TOLPMapNamesTemplate = tolpMapName.Value;
            else
                parameters.TOLPMapNamesTemplate = null;

            InputVar<string> lethalTempMapName = new InputVar<string>("LethalTempMapName");
            if (ReadOptionalVar(lethalTempMapName))
                parameters.LethalTempMapNameTemplate = lethalTempMapName.Value;
            else
                parameters.LethalTempMapNameTemplate = null;

            InputVar<string> eventLogFile = new InputVar<string>("EventLog");
            if (ReadOptionalVar(eventLogFile))
                parameters.EventLogFileName = eventLogFile.Value;
            else
                parameters.EventLogFileName = null;

            InputVar<string> summaryLogFile = new InputVar<string>("SummaryLog");
            if (ReadOptionalVar(summaryLogFile))
                parameters.SummaryLogFileName = summaryLogFile.Value;
            else
                parameters.SummaryLogFileName = null;

            return parameters;
        }
    }
}
