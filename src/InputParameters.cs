﻿using System.Collections.Generic;
using Landis.Core;
using Landis.Utilities;

namespace Landis.Extension.RootRot
{
    public class InputParameters
       : IInputParameters
    {
        private int timestep;
        private string inputMapFileName;
        private Dictionary<ISpecies, float[]> susceptibilityTable;
        private float lethalTemp;
        private float phWet;
        private float phDry;
        private float phMax;
        private float minSoilTemp;
        private float soilTDepth;
        private float minProbID;
        private float maxProbDI;
        private string outMapNamesTemplate;
        private string toldMapNamesTemplate;
        private string lethalTempMapNameTemplate;
        private string totalBiomassRemovedMapNameTemplate;
        private string speciesBiomassRemovedMapNamesTemplate;
        private string soilTempMapNameTemplate;
        private string wetnessIndexMapNameTemplate;
        private string pSIMapNameTemplate;
        private string pIDMapNameTemplate;
        private string eventLogFileName;
        private string summaryLogFileName;

        /// <summary>
        /// Timestep (years)
        /// </summary>
        public int Timestep
        {
            get
            { return timestep; }
            set
            {
                if (value < 0)
                    throw new InputValueException(value.ToString(),
                                                      "Value must be = or > 0.");
                timestep = value;
            }
        }
        //---------------------------------------------------------------------
        /// <summary>
        /// Filename for input map of cell status. (Optional)
        /// </summary>
        public string InputMapName
        {
            get
            { return inputMapFileName; }
            set
            {
                //if (value == null)
                 //   throw new InputValueException(value.ToString(), "Value must be a file path.");
                inputMapFileName = value;
            }
        }
        //---------------------------------------------------------------------
        /// <summary>
        /// Species susceptibility values
        /// </summary>
        public Dictionary<ISpecies, float[]> SusceptibilityTable
        {
            get
            { return susceptibilityTable; }
            set
            { susceptibilityTable = value; }
        }
        //---------------------------------------------------------------------
        /// <summary>
        /// Minimum temperature below which pathogen cannot survive
        /// </summary>
        public float LethalTemp
        {
            get
            { return lethalTemp; }
            set
            { lethalTemp = value; }
        }
        //---------------------------------------------------------------------
        /// <summary>
        /// Pressurehead threshold below which the soil is considered wet
        /// </summary>
        public float PhWet
        {
            get
            { return phWet; }
            set
            { phWet = value; }
        }
        //---------------------------------------------------------------------
        /// <summary>
        /// Pressurehead threshold above which the soil is considered dry
        /// </summary>
        public float PhDry
        {
            get
            { return phDry; }
            set
            { phDry = value; }
        }
        //---------------------------------------------------------------------
        /// <summary>
        /// Pressurehead threshold above which the soil is considered dry enough for optimal progression to Diseased
        /// </summary>
        public float PhMax
        {
            get
            { return phMax; }
            set
            { phMax = value; }
        }
        //---------------------------------------------------------------------
        /// <summary>
        /// Minimum probability of infected converting to diseased.  
        /// </summary>
        public float MinProbID
        {
            get { return minProbID; }
            set
            {
                if (value < 0 || value > 1)                
                    throw new InputValueException(value.ToString(), "MinProbID must be between 0 and 1.");
                 minProbID = value;                
            }
        }
        //---------------------------------------------------------------------
        /// <summary>
        /// Maximum probability of infected converting from diseased to infected.  
        /// </summary>
        public float MaxProbDI
        {
            get { return maxProbDI; }
            set
            {
                if (value < 0 || value > 1)
                    throw new InputValueException(value.ToString(), "MaxProbDI must be between 0 and 1.");
                maxProbDI = value;
            }
        }
        //---------------------------------------------------------------------
        /// <summary>
        /// Template for the filenames for output maps.
        /// </summary>
        public string OutMapNamesTemplate
        {
            get
            { return outMapNamesTemplate; }
            set
            {
               // if (value == null)
                //    throw new InputValueException(value.ToString(), "Value must be a file path.");
                outMapNamesTemplate = value;
            }
        }
        //---------------------------------------------------------------------
        /// <summary>
        /// Template for the filenames for Time of Last Pathogen output maps.
        /// </summary>
        public string TOLDMapNamesTemplate
        {
            get
            { return toldMapNamesTemplate; }
            set
            {
                //if (value == null)
                //    throw new InputValueException(value.ToString(), "Value must be a file path.");
                toldMapNamesTemplate = value;
            }
        }
        //---------------------------------------------------------------------
        /// <summary>
        /// Template for the filenames for Lethal Temp output maps.
        /// </summary>
        public string LethalTempMapNameTemplate
        {
            get
            { return lethalTempMapNameTemplate; }
            set
            {
               // if (value == null)
                 //   throw new InputValueException(value.ToString(), "Value must be a file path.");
                lethalTempMapNameTemplate = value;
            }
        }
        //---------------------------------------------------------------------
        /// <summary>
        /// Template for the filename for SpeciesBiomassRemoved output maps.
        /// </summary>
        public string TotalBiomassRemovedMapNameTemplate
        {
            get
            { return totalBiomassRemovedMapNameTemplate; }
            set
            {
                // if (value == null)
                //   throw new InputValueException(value.ToString(), "Value must be a file path.");
                totalBiomassRemovedMapNameTemplate = value;
            }
        }
        //---------------------------------------------------------------------
        /// <summary>
        /// Template for the filename for TotalBiomassRemoved output maps.
        /// </summary>
        public string SpeciesBiomassRemovedMapNamesTemplate
        {
            get
            { return speciesBiomassRemovedMapNamesTemplate; }
            set
            {
                // if (value == null)
                //   throw new InputValueException(value.ToString(), "Value must be a file path.");
                speciesBiomassRemovedMapNamesTemplate = value;
            }
        }
        //---------------------------------------------------------------------
        /// <summary>
        /// Template for the filename for SoilTemp output maps.
        /// </summary>
        public string SoilTempMapNameTemplate
        {
            get
            { return soilTempMapNameTemplate; }
            set
            {
                // if (value == null)
                //   throw new InputValueException(value.ToString(), "Value must be a file path.");
                soilTempMapNameTemplate = value;
            }
        }
        //---------------------------------------------------------------------
        /// <summary>
        /// Template for the filename for WetnessIndex output maps.
        /// </summary>
        public string WetnessIndexMapNameTemplate
        {
            get
            { return wetnessIndexMapNameTemplate; }
            set
            {
                // if (value == null)
                //   throw new InputValueException(value.ToString(), "Value must be a file path.");
                wetnessIndexMapNameTemplate = value;
            }
        }
        //---------------------------------------------------------------------
        /// <summary>
        /// Template for the filename for pSI output maps.
        /// </summary>
        public string PSIMapNameTemplate
        {
            get
            { return pSIMapNameTemplate; }
            set
            {
                // if (value == null)
                //   throw new InputValueException(value.ToString(), "Value must be a file path.");
                pSIMapNameTemplate = value;
            }
        }
        //---------------------------------------------------------------------
        /// <summary>
        /// Template for the filename for pID output maps.
        /// </summary>
        public string PIDMapNameTemplate
        {
            get
            { return pIDMapNameTemplate; }
            set
            {
                // if (value == null)
                //   throw new InputValueException(value.ToString(), "Value must be a file path.");
                pIDMapNameTemplate = value;
            }
        }
        //---------------------------------------------------------------------
        /// <summary>
        /// Name of event log file.
        /// </summary>
        public string EventLogFileName
        {
            get
            { return eventLogFileName; }
            set
            {
                if (value == null)
                    throw new InputValueException(value.ToString(), "Value must be a file path.");
                eventLogFileName = value;
            }
        }
        //---------------------------------------------------------------------
        /// <summary>
        /// Name of summary log file.
        /// </summary>
        public string SummaryLogFileName
        {
            get
            { return summaryLogFileName; }
            set
            {
                if (value == null)
                    throw new InputValueException(value.ToString(), "Value must be a file path.");
                summaryLogFileName = value;
            }
        }
        //---------------------------------------------------------------------
        /// <summary>
        /// Minimum soil temperature below which pathogen cannot survive
        /// </summary>
        public float MinSoilTemp
        {
            get
            { return minSoilTemp; }
            set
            { minSoilTemp = value; }
        }
        //---------------------------------------------------------------------
        /// <summary>
        /// Depth at which MinSoilTemp is measured
        /// </summary>
        public float SoilTDepth
        {
            get
            { return soilTDepth; }
            set
            { soilTDepth = value; }
        }
        //---------------------------------------------------------------------
        public InputParameters()
        {
            susceptibilityTable = new Dictionary<ISpecies, float[]>();
        }
        //---------------------------------------------------------------------
    }




    /// <summary>
    /// Parameters for the extension.
    /// </summary>
    public interface IInputParameters
    {
        /// <summary>
		/// Timestep (years)
		/// </summary>
		int Timestep { get; set; }
        //---------------------------------------------------------------------
        /// <summary>
        /// Filename for input map of cell status. (Optional)
        /// </summary>
        string InputMapName { get; set; }
        //---------------------------------------------------------------------
        /// <summary>
        /// Species susceptibility values
        /// </summary>
        Dictionary<ISpecies, float[]> SusceptibilityTable { get; set; }
        //---------------------------------------------------------------------
        /// <summary>
        /// Minimum temperature below which pathogen cannot survive
        /// </summary>
        float LethalTemp { get; set; }
        //---------------------------------------------------------------------
        /// <summary>
        /// Minimum soil temperature below which pathogen cannot survive
        /// </summary>
        float MinSoilTemp { get; set; }
        //---------------------------------------------------------------------
        /// <summary>
        /// Depth of minimum soil temperature measurement
        /// </summary>
         float SoilTDepth { get; set; }
        //---------------------------------------------------------------------
        /// <summary>
        /// Pressurehead threshold below which the soil is considered wet
        /// </summary>
        float PhWet { get; set; }
        //---------------------------------------------------------------------
        /// <summary>
        /// Pressurehead threshold above which the soil is considered dry
        /// </summary>
        float PhDry { get; set; }
        //---------------------------------------------------------------------
        /// <summary>
        /// Pressurehead threshold above which the soil is considered dry enough for optimal progression to Diseased
        /// </summary>
        float PhMax { get; set; }
        //---------------------------------------------------------------------
        /// <summary>
        /// Minimum probability of infected converting to diseased.  
        /// </summary>
        float MinProbID { get; set; }
        //---------------------------------------------------------------------
        /// <summary>
        /// Maximum probability of infected converting from diseased to infected.  
        /// </summary>
        float MaxProbDI { get; set; }
        //---------------------------------------------------------------------
        /// <summary>
        /// Template for the filenames for output maps.
        /// </summary>
        string OutMapNamesTemplate { get; set; }
        //---------------------------------------------------------------------
        /// <summary>
        /// Template for the filenames for Time of Last Pathogen output maps.
        /// </summary>
        string TOLDMapNamesTemplate { get; set; }
        //---------------------------------------------------------------------
        /// <summary>
        /// Template for the filenames for Lethal Temp output maps.
        /// </summary>
        string LethalTempMapNameTemplate { get; set; }
        //---------------------------------------------------------------------
        /// <summary>
        /// Template for the filenames for TotalBiomassRemoved output maps.
        /// </summary>
        string TotalBiomassRemovedMapNameTemplate { get; set; }
        //---------------------------------------------------------------------
        /// <summary>
        /// Template for the filenames for SpeciesBiomassRemoved output maps.
        /// </summary>
        string SpeciesBiomassRemovedMapNamesTemplate { get; set; }
        //---------------------------------------------------------------------
        /// <summary>
        /// Template for the filenames for SoilTemp output maps.
        /// </summary>
        string SoilTempMapNameTemplate { get; set; }
        //---------------------------------------------------------------------
        /// <summary>
        /// Template for the filenames for WetnessIndex output maps.
        /// </summary>
        string WetnessIndexMapNameTemplate { get; set; }
        //---------------------------------------------------------------------
        /// <summary>
        /// Template for the filenames for pSI output maps.
        /// </summary>
        string PSIMapNameTemplate { get; set; }
        //---------------------------------------------------------------------
        /// <summary>
        /// Template for the filenames for pID output maps.
        /// </summary>
        string PIDMapNameTemplate { get; set; }
        //---------------------------------------------------------------------
        /// <summary>
        /// Name of event log file.
        /// </summary>
        string EventLogFileName { get; set; }
        //---------------------------------------------------------------------
        /// <summary>
        /// Name of summary log file.
        /// </summary>
        string SummaryLogFileName { get; set; }
        //---------------------------------------------------------------------
    }
}
