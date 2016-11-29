using System;
using UIKit;
using System.Collections.Generic;

namespace LiRoInspect.iOS
{

	/// <summary>
	/// DashBoard View Left Side Table Item Class
	/// </summary>
	public class DashBoardLeftTableItem
	{
		public DashBoardLeftTableItem(){}
		public string Heading { get; set; }

		public string SubHeading { get; set; }

		public string ImageName { get; set; }

		public HeaderType HeaderType { get; set; }

		public Model.Inspection InspectionContent{ get; set;}

		public DashBoardLeftTableItem (string heading)
		{
			this.Heading = heading;
		}
	}

	public enum HeaderType{None,Today,Upcoming,TodayContent,UpcomingContent}
}

