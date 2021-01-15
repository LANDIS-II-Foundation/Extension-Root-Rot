using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Landis.Library.Metadata;
using Landis.Core;
using System.Data;

namespace Landis.Extension.RootRot
{
    public class EventsLog
    {
        [DataFieldAttribute(Unit = FieldUnits.Year, Desc = "...")]
        public int Time { set; get; }

        [DataFieldAttribute(Unit = FieldUnits.None, Desc = "Species")]
        public string Species { set; get; }

        [DataFieldAttribute(Unit = FieldUnits.g_B_m2, Desc = "Biomass Removed")]
        public int MortalityBiomass { set; get; }
    }

    public class SummaryLog
    {
        [DataFieldAttribute(Unit = FieldUnits.Year, Desc = "...")]
        public int Time { set; get; }

        [DataFieldAttribute(Unit = FieldUnits.Count, Desc = "Sites UnInfected")]
        public float UninfectedSites { set; get; }

        [DataFieldAttribute(Unit = FieldUnits.Count, Desc = "Sites Infected")]
        public float InfectedSites { set; get; }
        
        [DataFieldAttribute(Unit = FieldUnits.Count, Desc = "Sites Diseased")]
        public float DiseasedSites { set; get; }

        [DataFieldAttribute(Unit = FieldUnits.Count, Desc = "Sites Damaged")]
        public float DamageSites { set; get; }

        [DataFieldAttribute(Unit = FieldUnits.Count, Desc = "Sites Cold Killed")]
        public float Absent { set; get; }

        [DataFieldAttribute(Unit = FieldUnits.Count, Desc = "Number of Cohorts Damaged")]
        public int CohortsDamaged { set; get; }

        [DataFieldAttribute(Unit = FieldUnits.Count, Desc = "Number of Cohorts Killed")]
        public int CohortsKilled { set; get; }

        [DataFieldAttribute(Unit = FieldUnits.g_B_m2, Desc = "Biomass Removed")]
        public int MortalityBiomass { set; get; }

        
    }

    /*public class EventTable<EventLog>
    {
        private DataTable tbl = new DataTable();
        private List<EventLog> list = new List<EventLog>();
        private string filePath;

        public List<EventLog> ObjectsList { get { return this.list; } }
        public string FilePath { get { return this.filePath; } }

        //------
        public EventTable(string filePath)
        {
            this.filePath = filePath;
            this.tbl.SetColumns<EventLog>();
            this.tbl.WriteToFile(this.filePath, false);
        }
        //

        //------
        public void AddObject(EventLog obj)
        {
            this.list.Add(obj);
        }

        //------
        public EventLog GetObjectAt(int index)
        {
            return this.list[index];
        }

        //------
        public void RemoveObjectAt(int index)
        {
            this.list.RemoveAt(index);
        }

        //------
        public void RemoveObject(EventLog obj)
        {
            this.list.Remove(obj);
        }

        //------
        public void WriteToFile()
        {
            this.tbl.AppendDataObjects(this.list);
            this.tbl.WriteToFile(this.filePath, true);
        }

        //------
        public void Clear()
        {
            ObjectsList.Clear();
            tbl.Clear();
        }



    }*/
}
