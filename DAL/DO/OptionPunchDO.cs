using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using System.Collections.Generic;
using System.Diagnostics;

namespace DAL
{
	[Table("OptionPunch")]
	public class OptionPunchDO
	{
		int _OptionPunchID;
		[PrimaryKey, AutoIncrement]
		[Column("OptionPunchID")]
		public int OptionPunchID
		{
			get { return _OptionPunchID; }
			set { _OptionPunchID = value; }
		}

		int _OptionID;
		public int OptionID
		{
			get { return _OptionID; }
			set { _OptionID = value; }
		}

		int _PunchID;
		public int PunchID
		{
			get { return _PunchID; }
			set { _PunchID = value; }
		}

		public static List<OptionPunchDO> GetOptionPunch(SQLiteConnection conn, int optPunchID)
		{
			List<OptionPunchDO> optPunch = new List<OptionPunchDO> ();
			try
			{
				string query = "select * from OptionPunch  where  OptionPunchID=" + optPunchID;
				optPunch= conn.Query<OptionPunchDO>(query);
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Exception Occured in GetOptionPunch method due to " + ex.Message);
			}
			return optPunch;
		}

		public static List<OptionPunchDO> GetPunchOptions(SQLiteConnection conn, int optPunchID)
		{
			List<OptionPunchDO> optPunch = new List<OptionPunchDO> ();
			try
			{
				string query = "select * from OptionPunch  where  OptionID=" + optPunchID;
				optPunch= conn.Query<OptionPunchDO>(query);
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Exception Occured in GetOptionPunch method due to " + ex.Message);
			}
			return optPunch;
		}

		public static int DeleteOptionPunch(SQLiteConnection conn, int optionID)
		{
			string query = "delete from OptionPunch  where  OptionID=" + optionID;
			return conn.Execute(query);
		}

		public static int DeleteOptionPunchID(SQLiteConnection conn, int PunchID)
		{
			string query = "delete from OptionPunch  where  PunchID=" + PunchID;
			return conn.Execute(query);
		}

		public static void InsertOptionPunch(SQLiteConnection conn, int optionID, int PunchID)
		{
			string Query = "insert into OptionPunch(OptionID,PunchID) values('" + optionID + "','" + PunchID + "');";
			conn.Execute(Query);
		}

		public static void UpdateOptionPunch(SQLiteConnection conn, int optionID, int PunchID,int OptionPunchID)
		{
			string Query = "update OptionPunch  set OptionPunchID='" + OptionPunchID + "',OptionID='" + optionID + "',PunchID='" + PunchID + "' where OptionPunchID='" + OptionPunchID + "';"; ;
			conn.Execute(Query);
		}
	}
}