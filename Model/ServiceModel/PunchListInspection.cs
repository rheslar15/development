using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ServiceModel
{
    public class PunchListInspection
    {
        ////Inspection ID of 25, 50, 75 and 90% 
		public int inspectionTypeId { get; set; }//changed inspectionId to inspectionTypeId
        public List<string> comments { get; set; }
        public CommentType commentType { get; set; }
    }
}