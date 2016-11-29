using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using Model;
using DAL.Utility;
using System.Diagnostics;

namespace DAL.DO
{
    [Table("PunchList")]
   public class PunchListDO:IDomianObject
    {
        int _PunchID;
        [PrimaryKey, AutoIncrement]
        [Column("PunchID")]
        public int ID
        {
            get { return _PunchID; }
            set { _PunchID = value; }
        }

        string _PunchDesc;
        public string PunchDesc
        {
            get { return _PunchDesc; }
            set { _PunchDesc = value; }
        }

        string _InspectionID;
        public string InspectionID
        {
            get { return _InspectionID; }
            set { _InspectionID = value; }
        }

        string _ProjectID;
        public string ProjectID
        {
            get { return _ProjectID; }
            set { _ProjectID = value; }
        }

        public static List<Punch> getPunchList(SQLiteConnection conn, string InspectionID, string ProjectID)
        {
            List<Punch> PunchItems = new List<Punch>();
			try{
            string query = "SELECT * FROM PunchList WHERE InspectionID='" + InspectionID + "' AND ProjectID='" + ProjectID + "'";
            List<PunchListDO> PunchTrans = conn.Query<PunchListDO>(query);
            foreach (var Abc in PunchTrans)
            {
                PunchItems.Add(Converter.GetPunchList(Abc));
            }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception Occured in getPunchList method due to " + ex.Message);
			}
            return PunchItems;
        }

		public static List<string> getPunchProjectIds(SQLiteConnection conn, string InspectionID)
		{
			List<string> projectIDs = new List<string>();
			try{
				string query = "SELECT * FROM PunchList WHERE InspectionID=" + InspectionID ;
				List<PunchListDO> PunchTrans = conn.Query<PunchListDO>(query);
				foreach (var punch in PunchTrans)
				{
					if(!projectIDs.Contains(punch.ProjectID))
					{
						projectIDs.Add(punch.ProjectID);
					}
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Exception Occured in getPunchList method due to " + ex.Message);
			}
			return projectIDs;
		}

		public static List<Punch> getPunchListPunchID(SQLiteConnection conn, int punchID)
		{
			List<Punch> PunchItems = new List<Punch>();
			try{
				string query = "SELECT * FROM PunchList WHERE PunchID="+punchID;
				List<PunchListDO> PunchTrans = conn.Query<PunchListDO>(query);
				foreach (var Abc in PunchTrans)
				{
					PunchItems.Add(Converter.GetPunchList(Abc));
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Exception Occured in getPunchList method due to " + ex.Message);
			}
			return PunchItems;
		}

        public static List<Punch> getPunchList(SQLiteConnection conn, string InspectionID)
        {
            List<Punch> PunchItems = new List<Punch>();
			try
            {
			    string query = "SELECT * FROM PunchList WHERE InspectionID='" + InspectionID + "'";
                List<PunchListDO> PunchTrans = conn.Query<PunchListDO>(query);
                foreach (var Abc in PunchTrans)
                {
                    PunchItems.Add(Converter.GetPunchList(Abc));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception Occured in getPunchList method due to " + ex.Message);
			}
            return PunchItems;
        }

        public static int DeletePunchList(SQLiteConnection conn, string InspectionID, string ProjectID)
        {
            string query = "DELETE FROM PunchList WHERE InspectionID='" + InspectionID + "' AND ProjectID='" + ProjectID + "'";
			return conn.Execute(query);
		}

		public static void InsertPunchList(SQLiteConnection conn, int PunchID, string InspectionID, int ProID, string PunchDesc)
        {
			string Query = "insert into PunchList(PunchID,InspectionID,ProjectID,PunchDesc) values('" + PunchID + "','" + InspectionID + "','" + ProID + "','" + PunchDesc + "');";
            conn.Execute(Query);
        }

        public static void InsertPunchLists(SQLiteConnection conn, List<Punch> PunchList)
        {
            foreach (var PunChitem in PunchList)
            {
                string Query = "insert into PunchList(InspectionID,ProjectID,PunchDesc) values('" + PunChitem.InspectionID + "','" + PunChitem.ProjectID + "','" + PunChitem.punchDescription + "');";
                conn.Execute(Query);
            }
        }

		public static void UpdatePunchList(SQLiteConnection conn, int PunchID, string InspectionID, int ProID, string PunchDesc)
        {
			string Query = "update PunchList  set PunchID='" + PunchID + "',InspectionID='" + InspectionID + "',ProjectID='" + ProID + "',PunchDesc='" + PunchDesc + "' where PunchID='" + PunchID + "';";
            conn.Execute(Query);
        }

        public static void UpdatePunchLists(SQLiteConnection conn, List<Punch> Punchlist)
        {
            foreach (var PunChitem in Punchlist)
            {
                string Query = "update PunchList  set PunchID='" + PunChitem.PunchID + "',InspectionID='" + PunChitem.InspectionID + "',ProjectID='" + PunChitem.ProjectID + "',PunchDesc='" + PunChitem.punchDescription + "' where PunchID='" + PunChitem.PunchID + "';";
				conn.Execute(Query);
            }
        }
    }
}