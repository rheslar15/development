using System;
using System.Collections.Generic;

namespace LiRoInspect.iOS
{
	/// <summary>
	/// A group that contains DashBoard table items
	/// </summary>
	public class DashBoardTableItemGroup
	{
		public string Name { get; set; }
		public List<DashBoardLeftTableItem> Items { get; set;}
	}
}

