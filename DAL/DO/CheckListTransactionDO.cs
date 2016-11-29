using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using SQLite;
using DAL.Utility;

namespace DAL.DO
{
	[Table("CheckListTransaction")]
	public class CheckListTransactionDO:IDomianObject
	{
		int _CheckListTransactionID;

		[PrimaryKey, AutoIncrement]
		[Column("CheckListTransactionID")]
		public int ID
		{
			get { return _CheckListTransactionID; }
			set { _CheckListTransactionID = value; }
		}

		int _OptionTransID;
		public int OptionTransID
		{
			get { return _OptionTransID; }
			set { _OptionTransID = value; }
		}
		int _CheckListID;
		public int CheckListID
		{
			get { return _CheckListID; }
			set { _CheckListID = value; }
		}
		int _ResultTypeID;
		public int ResultTypeID
		{
			get { return _ResultTypeID; }
			set { _ResultTypeID = value; }
		}

		string _Comment;
		public string Comment
		{
			get
			{
				return _Comment;
			}
			set
			{
				_Comment = value;
			}
		}

		int _PunchID;
		public int PunchID
		{
			get { return _PunchID; }
			set { _PunchID = value; }

		}

		int _ItemTypeID;
		public int ItemTypeID
		{
			get { return _ItemTypeID; }
			set { _ItemTypeID = value; }
		}

        public static int DeletecheckList(SQLiteConnection conn, int OptionTransId)
		{
            string query = "delete from CheckListTransaction where OptionTransID=" + OptionTransId;
			return conn.Execute(query);
		}

		public static List<CheckListTransaction> GetCheckListTransaction(SQLiteConnection conn, int OptionTransId)
		{
			string query = "select * from CheckListTransaction where OptionTransID=" + OptionTransId;
			List<CheckListTransactionDO> checkListTransactionDO = conn.Query<CheckListTransactionDO>(query);
			List<Model.CheckListTransaction> checkListTransaction=new List<Model.CheckListTransaction>();
			if (checkListTransactionDO != null && checkListTransactionDO.Count > 0) {
				foreach(var chkTrans in checkListTransactionDO)
				{
					checkListTransaction.Add(Converter.GetCheckListTransaction(chkTrans));
			     }
			}

			return checkListTransaction;
		}

		public static List<CheckListTransactionDO> GetCheckListTransactionID(SQLiteConnection conn, int OptionTransId, int checkListID)
		{			
			string query = "select * from CheckListTransaction where OptionTransID=" + OptionTransId+" and CheckListID="+checkListID;
			List<CheckListTransactionDO> checkListTransactionDO = conn.Query<CheckListTransactionDO>(query);
			return checkListTransactionDO;
		}
	}
}