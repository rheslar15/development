using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Model
{
	public class InspectionItemTransaction
	{  public int ID { get; set; }
		public int InspectionItemID{ get; set;}
		public int PunchID{ get; set;}
		public int OptionTransactionID{ get; set;}
		public List<CheckListTransaction> checkListItems{get;set;}
		public List<InspectionItemImage> photos { get; set;}
		public int result { get; set; }
		public string name { get; set; }
		public string comments{ get;set;}
		public int itemType{ get; set;}
	}
}

