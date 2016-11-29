using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ServiceModel
{
    public class InspectionItem
    {
        public List<CheckList> checkListItems{get;set;}
        public List<byte[]> photos { get; set; }
		public bool? result { get; set; }
        public string name { get; set; }
		public string comments{ get;set;}
    }
}