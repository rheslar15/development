using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
	public class InspectionItemImage
	{
		public int ID { get; set; }
		public int ItemTransID { get; set; }
		public byte[] Image { get; set; }			
	}
}