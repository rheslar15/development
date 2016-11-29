using System;

namespace Model
{
	public class ReportView
	{
		public string AppID { get; set; }
		public string InspectionType{get;set;}
		public string ReportType { get; set; }
		public byte[] ReportDesc { get; set; }
		public string PathwayType{ get; set;}
		public int InspectionTransactionID{ get; set;}

	}
}