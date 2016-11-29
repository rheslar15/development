using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class InspectionItem : ServiceModel.InspectionItem
    {
        public int ID { get; set; }
		public int ItemTransID{ get; set;}
		public int PunchID{ get; set;}
		public ItemType itemType{ get; set;}
        public new ResultType result { get; set; }
		public new List<CheckList> checkListItems{get;set;}        
    }
}