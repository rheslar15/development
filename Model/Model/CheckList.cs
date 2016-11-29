using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
     public class CheckList:ServiceModel.CheckList
    {
         public int ID { get; set; }
		 public int CheckListTransID{ get; set;}
		 public ResultType Result{ get; set;}
         public ItemType itemType { get; set; }
		 public int PunchID { get; set; }
		 public List<byte[]> photos{ get; set;}

		public CheckList ()
		{
			// default result is NA
			Result = ResultType.NA;
		}
    }
}
