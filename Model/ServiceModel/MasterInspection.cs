using System;

namespace Model.ServiceModel
{
	public class MasterInspection
	{
		public int ID {	get; set;}
		public string InspectionTypeId {get;set;}
		public string InspectionDesc {get; set;	}
		public int Priority{ get; set; }
	}
}