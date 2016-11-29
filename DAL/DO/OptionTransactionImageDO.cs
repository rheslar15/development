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
	[Table("OptionImage")]
	public class OptionTransactionImageDO: IDomianObject
	{
		int _OptionImageID;
		[PrimaryKey, AutoIncrement]
		[Column("OptionImageID")]
		public int ID
		{
			get { return _OptionImageID; }
			set { _OptionImageID = value; }
		}

		byte[] _ItemImage;
		public byte[] ItemImage
		{
			get { return _ItemImage; }
			set { _ItemImage = value; }
		}

		int _OptionTransactionID;
		public int OptionTransactionID
		{
			get { return _OptionTransactionID; }
			set { _OptionTransactionID= value; }
		}	

		public static int DeleteOptionImagesSync(SQLiteConnection conn,int OptionTransactionID)
		{
			string query = "delete from OptionImage where OptionTransactionID="+OptionTransactionID;
			return conn.Execute(query);
		}
	}
}