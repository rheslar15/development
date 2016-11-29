using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ServiceModel
{
    public class ProjectPunchList
    {   
		public string appID { get; set; }//changed projectId to appID
        public List<PunchListInspection> punchListItems { get; set; }
    }
}