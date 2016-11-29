using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
  public  class Report
    {
        public int ReportID { get; set; } 
		public int InspectionTransID{get;set;}
        public string ReportType { get; set; }
        public byte[] ReportDesc { get; set; }

    }
}