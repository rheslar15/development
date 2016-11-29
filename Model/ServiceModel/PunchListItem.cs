using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ServiceModel
{
    public class PunchListItem
    {
        public List<byte[]> photos { get; set; }
        public string comment { get; set; }
        public int sequence { get; set; }
    }
}