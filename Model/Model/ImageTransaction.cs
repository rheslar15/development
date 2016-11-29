using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class ImageTransaction
    {
        public int ID { get; set; }
        public int OptionTransID { get; set; }
        public byte[] Image { get; set; }
        public int FailID { get; set; }
		public int PunchID{ get; set;}
       
    }
}
