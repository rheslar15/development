using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ServiceModel
{
	public class PunchListResult
	{
		public string appID { get; set; }//changed projectID to appID
										 ////Inspection ID for 25, 50, 75 and 90% Inspection
		public string inspectionTypeID { get; set; }//changed inspectionID to inspectionTypeID
		public List<PunchListItem> punchList { get; set; }
		public CommentType commentType { get; set; }
		public string token { get; set; }
	}
}