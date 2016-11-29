namespace Model.ServiceModel
{
	public class InspectionReport:BaseModel
    {
		public string inspectionTypeID { get; set; }//changed inspectionID to inspectionTypeID
		public string appID { get; set; }//changed projectID to appID
		public byte[] report { get; set; }
        public string reportName { get; set; }
        public string token { get; set; }
    }
}