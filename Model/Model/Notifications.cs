using System;

namespace Model
{
	public class Notifications
	{
		public DateTime notificationDate{ get; set;}
		public string notificationType { get; set; }
		public string notificationTypeID { get; set; }
		public string notificationmessage { get; set; }
		public int count{ get; set;}
		public int notificationID { get; set;}
		public bool inProgress{ get; set;}
	}
}