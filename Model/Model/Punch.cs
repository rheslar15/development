using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
	public class Punch
	{
		public Punch(){}
		public string InspectionID { get; set; }
		public int PunchID { get; set; }
		public string ProjectID { get; set; }
		public List<byte[]> punchImages = new List<byte[]> ();
		public string  punchHeading { get; set; }
		public string punchDescription { get; set; }
	}
}

