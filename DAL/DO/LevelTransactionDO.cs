using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using System.Diagnostics;

namespace DAL.DO
{
	[Table("LevelTransaction")]
	public class LevelTransactionDO : IDomianObject
	{
		
		int _Id;
		[PrimaryKey,AutoIncrement]
		public int ID
		{
			get { return _Id; }
			set { _Id = value; }
		}

		int? _LevelID;
		public int? LevelID
		{
			get { return _LevelID; }
			set { _LevelID = value; }
		}

		int? _SeqID;
		public int? SeqID
		{
			get { return _SeqID; }
			set { _SeqID = value; }
		}


		int ? _isSelected;
		public int? isSelected 
		{
			get { return _isSelected; }
			set { _isSelected = value; }
		}

		int? _inspectionTransID;

		public int? InspectionTransID
		{
			get { return _inspectionTransID; }
			set { _inspectionTransID = value; }
		}
	}
}

