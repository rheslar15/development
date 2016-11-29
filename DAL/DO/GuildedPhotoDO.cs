using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using DAL.DO;


namespace DAL
{
	[Table("GuidedImage")]
	public class GuildedPhotoDO:IDomianObject
	{

			int _GuidedImageID;
			[PrimaryKey, AutoIncrement]
			[Column("GuidedImageID")]
			public int ID
			{
			get { return _GuidedImageID; }
			set { _GuidedImageID = value; }
			}

			byte[] _Image;
			public byte[] Image
			{
			get { return _Image; }
			set { _Image = value; }
			}

			int _CheckListTransID;
			public int CheckListTransID
			{
			get { return _CheckListTransID; }
			set { _CheckListTransID = value; }
			}

		public static List<GuildedPhotoDO> getGuidedImageList(SQLiteConnection conn, int checkListTransID)
		{
			string query = "select * from GuidedImage where CheckListTransID= " + checkListTransID;
			List<GuildedPhotoDO> imageTrans = conn.Query<GuildedPhotoDO>(query);
			return imageTrans;
		}

		public static GuildedPhotoDO getGuidedImage(SQLiteConnection conn, int ID)
		{
			string query = "select * from GuidedImage where GuidedImageID= " + ID;
			GuildedPhotoDO image = conn.Query<GuildedPhotoDO>(query).FirstOrDefault();
			return image;
		}



		public static int DeleteGuidedImageList(SQLiteConnection conn, int checkListTransID)
		{
			string query = "delete from GuidedImage  where  CheckListTransID=" + checkListTransID;
			return conn.Execute(query);
		}

		public static int DeleteGuidedImage(SQLiteConnection conn, int ID)
		{
			string query = "delete from GuidedImage where GuidedImageID=" + ID;
			return conn.Execute(query);
		}

		public static void InsertGuidedImage(SQLiteConnection conn, int checkListTransID , byte[] image)
		{
			Object[] obs = new object[] { image, checkListTransID };
			SQLite.SQLiteCommand cmd = conn.CreateCommand("insert into GuidedImage(Image,CheckListTransID) values(@Image,@checkListTransID);", obs);
			cmd.ExecuteNonQuery();
		}

	}
}

