using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ServiceModel
{
	public class PunchListReq
	{
		public string token { get; set; }
		public List<string> appIDs { get; set; }//changed ProjectIDs to appIDs
	}
}