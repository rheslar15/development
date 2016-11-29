using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Model
{
	public class CheckListTransaction
	{
		public int ID { get; set; }
		public int OptionTransactionID { get; set; }
		public int CheckListID{ get; set;}
        public int result { get; set; }
        public string comments{ get;set;}
		public int PunchID { get; set; }
		public List<byte[]> GuidedPictures{ get; set;}
		public ItemType itemType{ get; set;}
	}
}