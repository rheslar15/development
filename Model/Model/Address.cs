using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using LiRoInspect.ServiceModel;
namespace Model
{
	public class Addr
    {
        public string addrline1 { get; private set; }
        public string addrline2 { get; private set; }
        public string city { get; private set; }
        public string pin { get; private set; }

		public Addr(string addrline1, string addrline2, string city, string pin)
        {
            this.addrline1 = addrline1;
            this.addrline2 = addrline2;
            this.city = city;
            this.pin = pin;
        }
    }
}
