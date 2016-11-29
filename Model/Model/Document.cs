using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Model
{
	public class Document: ServiceModel.InspectionDocument
	{
		public byte[] documentArray { get; set; }
//		public string serviceDocID { get; set; }
		public string inspectionID { get; set; }
		public string projectID { get; set; }
		public string DocumentPath{ get; set;}
		public int ID { get; set;}
	}
}