using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DO
{
	[Table("CheckList")]
	public class CheckListDO : IDomianObject
	{
		int _CheckListId;
		[PrimaryKey]
		[Column("CheckListID")]
		public int ID
		{
			get { return _CheckListId; }
			set { _CheckListId = value; }
		}

		string _CheckListDesc;
		public string CheckListDesc
		{
			get { return _CheckListDesc; }
			set { _CheckListDesc = value; }
		}
	}
}