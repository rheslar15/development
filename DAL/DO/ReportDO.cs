using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using System.Diagnostics;


namespace DAL.DO
{
    [Table("Report")]
  public  class ReportDO:IDomianObject
    {
        int _ReportID;

        [PrimaryKey, AutoIncrement]
        [Column("ReportID")]
        public int ID
        {
            get { return _ReportID; }
            set { _ReportID = value; }
        }

		int _InspectionTransID;
		public int InspectionTransID
        {
			get { return _InspectionTransID; }
			set { _InspectionTransID = value; }
        }
      
        string _ReportType;
        public string ReportType
        {
            get { return _ReportType; }
            set { _ReportType = value; }
        }

        byte[]  _ReportDesc;
        public byte[] ReportDesc
        {
            get { return _ReportDesc; }
            set { _ReportDesc = value; }
        }

		public static List<ReportDO> getReports(SQLiteConnection conn, int insTransID,string reportType)
		{
			List<ReportDO> reports = new List<ReportDO>();
			try
			{
				string query = "SELECT * FROM Report WHERE InspectionTransID=" + insTransID+" AND ReportType='"+reportType+"'";
				reports = conn.Query<ReportDO>(query);
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Exception Occured in getPunchList method due to " + ex.Message);
			}
			return reports;
		}

		public static int DeleteReports(SQLiteConnection conn, int insTransID)
		{
			int result = 0;
			try
			{
				string query = "Delete FROM Report WHERE InspectionTransID=" + insTransID;
				result = conn.Execute(query);
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Exception Occured in getPunchList method due to " + ex.Message);
			}
			return result;
		}
    }
}