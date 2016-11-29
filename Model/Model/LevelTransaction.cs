using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
	public class LevelTransaction
	{
		public int ID { get; set; }
		public int? LevelID { get; set; }
		public int? SeqID { get; set; }
		public int? isSelected { get; set; }
		public int? InspectionTransID { get; set; }
	}
}

