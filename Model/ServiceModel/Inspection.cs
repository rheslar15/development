
using System;
using System.Collections.Generic;


namespace Model.ServiceModel
{
	public class Inspection
	{
		public string inspectionTypeID { get; set; }//Changed inspectionID to inspectionTypeID
		public DateTime inspectionDate { get; set; }
		public string projectName { get; set; }
		public string appID { get; set; }//Changed projectID to appID
		public PathwayType pathway { get; set; }
		public Address activityAdress { get; set; }
		public string houseOwnerName { get; set; }
		public string phoneNo { get; set; }//Changed PhoneNo to phoneNo
		public InspectionInfo info { get; set; }
		public List<InspectionDocument> inspectionDocuments { get; set; }
	}
}