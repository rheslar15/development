using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ServiceModel
{
    public class PunchListRes : IResult
    {
        public Result result { get; set; }
        //for multiple 100% Inspections
        public List<ProjectPunchList> projectPunchLists { get; set; }
    }
}