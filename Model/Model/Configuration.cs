using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
	public class Configuration:IConfig

    {
		int _id;
		public int ID
		{
			get { return _id; }
			set { _id = value; }
		}

        public string ConfigDesc { get; set; }
        public string ConfigUrl { get; set; }
		public bool IsDefault{ get; set;}
		public bool use{ get; set; }
    }
}
