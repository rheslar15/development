using System;
using System.Collections.Generic;

namespace Model.ServiceModel
{
	public class Level
	{
		public string name { get;  set; }//changed Name to name
		public List<Space> spaces { get; set; }//changed Spaces to spaces
		public List<Option> options{ get; set; }//changed Options to options
		public int ID { get; set;}
		//public bool isSelected { get; set; }
	}
}