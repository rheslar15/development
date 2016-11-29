using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
	public class PunchListImage
	{
		public int inspectionTransID { get; set; }
		public byte[] Image { get; set; }
		public int PunchID{ get; set; }
	}
}


