using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace DAL.DO
{
	[Table("PunchListImage")]
	public class PunchListImageDO:IDomianObject
	{
		int _PunchListImageID;
		[PrimaryKey, AutoIncrement]
		[Column("PunchListImageID")]
		public int ID
		{
			get { return _PunchListImageID; }
			set { _PunchListImageID = value; }
		}

		byte[] _PunchListImage;
		public byte[] PunchListImage
		{
			get { return _PunchListImage; }
			set { _PunchListImage = value; }
		}

		int _inspectionTransID;
		public int InspectionTransID
		{
			get { return _inspectionTransID; }
			set { _inspectionTransID = value; }
		}

		int _punchID;
		public int PunchID
		{
			get { return _punchID; }
			set { _punchID = value; }
		}


		public static List<PunchListImageDO> getPunchListImageList(SQLiteConnection conn, int inspectionTransID,int PunchID)
		{
			string query = "select * from PunchListImage where inspectionTransID= " + inspectionTransID + " AND PunchID= " + PunchID;
			List<PunchListImageDO> imageTrans = conn.Query<PunchListImageDO>(query);
			return imageTrans;
		}

        public static List<PunchListImageDO> getPunchImageList(SQLiteConnection conn, int PunchID)
        {
            string query = "select * from PunchListImage where PunchID= " + PunchID;
            List<PunchListImageDO> imageTrans = conn.Query<PunchListImageDO>(query);
            return imageTrans;
        }

		public static List<PunchListImageDO> getAllPunchImagesForInspection(SQLiteConnection conn, int inspectionTransID)
		{
			string query = "select * from PunchListImage where inspectionTransID= " + inspectionTransID;
			List<PunchListImageDO> imageTrans = conn.Query<PunchListImageDO>(query);
			return imageTrans;
		}

		public static int DeletePunchListImageList(SQLiteConnection conn, int inspectionTransID)
		{
			string query = "delete from PunchListImage  where  inspectionTransID=" + inspectionTransID;
			return conn.Execute(query);
		}

        public static int DeletePunchImageList(SQLiteConnection conn, int punchID)
        {
            string query = "delete from PunchListImage where PunchID=" + punchID;
            return conn.Execute(query);
        }
			
		public static void InsertPunchListImageList(SQLiteConnection conn, int inspectionTransID , byte[] image)
		{
			string Query = "insert into PunchListImage(PunchListImage,inspectionTransID) values('" + image + "','" + inspectionTransID +  "');";
			conn.Execute(Query);
		}
	}
}