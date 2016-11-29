using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ServiceModel
{
    public class PunchListItemComments
    {
        public int inspectionId { get; set; }
        public List<string> comments { get; set; }
    }
}