using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using Model;
using DAL.DO;

namespace DAL
{
    [Table("LocationImage")]
	/// <summary>
	/// Location image do.
	/// </summary>
	public class LocationImageDo : IDomianObject
	{
		int _LocationImageID;
		[PrimaryKey, AutoIncrement]
        [Column("LocationImageID")]
		public int ID
		{
			get { return _LocationImageID; }
			set { _LocationImageID = value; }
		}

		byte[] _Image;
        public byte[] Image
		{
			get { return _Image; }
			set { _Image = value; }
		}

        int _InspectionTransID;
        public int InspectionTransID
		{
            get { return _InspectionTransID; }
            set { _InspectionTransID = value; }
		}	

		/// <summary>
		/// Gets the image for location identification.
		/// </summary>
		/// <returns>The image for location identification.</returns>
		/// <param name="conn">Conn.</param>
		/// <param name="inspectionTransID">Inspection trans I.</param>
		public static List<LocationImageDo> getImageForLocationIdentification(SQLiteConnection conn, int inspectionTransID)
		{
            string query = "SELECT * FROM LocationImage WHERE InspectionTransID=" + inspectionTransID;
			List<LocationImageDo> image = conn.Query<LocationImageDo>(query);
			return image;
		}

		/// <summary>
		/// Deletes the image location identification.
		/// </summary>
		/// <returns>The image.</returns>
		/// <param name="conn">Conn.</param>
		/// <param name="inspectionTransID">Inspection trans I.</param>
        public static int DeleteImage(SQLiteConnection conn, int inspectionTransID)
		{
            string query = "delete from LocationImage where InspectionTransID=" + inspectionTransID;
			return conn.Execute(query);
		}

		/// <summary>
		/// Inserts the image for inspection location identification.
		/// </summary>
		/// <param name="conn">Conn.</param>
		/// <param name="inspectionTransID">Inspection trans I.</param>
		/// <param name="Image">Image.</param>
        public static void InsertImageForInspection(SQLiteConnection conn, int inspectionTransID, byte[] Image)
		{
            Object[] obs = new object[] { Image, inspectionTransID };
            SQLite.SQLiteCommand cmd = conn.CreateCommand("insert into LocationImage(Image,InspectionTransID) values(@Image,@inspectionTransID);", obs);
			cmd.ExecuteNonQuery();
		}
	}
}