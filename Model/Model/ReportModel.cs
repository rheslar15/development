using System;
using System.Collections.Generic;

namespace Model
{
	public class ReportModel
	{
		public string ContractorName{ get; set;}
		public string ReprasentativeName{ get; set;}
		public string HomeOwnerName{ get; set;}
		public string HomeOwnerID{ get; set;}
		public string ActAdd{ get; set;}
		public string Pathway{ get; set;}
		public string TypeofIns{ get; set;}
		public string InspectionAttempt{ get; set;}
		public DateTime InspectionDate{ get; set;}
		public String InspectorName{ get; set;}
		public string InspectionTime{ get; set;}

		public byte[] Image { get; set;}
		public string SequenceType{ get; set;}
		public string InspectionOptDesc{ get; set;}
		public string SequenceLocation{ get;set;}

		public string utilites{ get; set;}
		private List<string> spacelst = new List<string> ();
		public List<string> spaceLst{ get {return spacelst; }set{spacelst = value; }}

		public string Option{ get; set;}
		public string Category{ get; set;}
		public string Standard{ get; set;}
		public string Reason{ get; set;}


		
	}
}

