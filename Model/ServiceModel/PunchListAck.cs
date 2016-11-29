using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ServiceModel
{
    public class PunchListAck : IResult
    {
        public Result result { get; set; }
    }
}
