using System;

namespace Model
{
	public class Notification
	{
		public string message{ get; set;}
		public string inspectionDetail{ get; set;}
		public bool seen{ get; set;}
	}
	public class Notification2
	{
		public string inspectionDetail{ get; set;}
		public string message{ get; set;}
		public string retrycount{ get; set;}
		public string uploadStatus{ get; set;}
	}
	public class notificationEventArgs : EventArgs
	{
		private Notifications notifications;
		public Notifications current
		{
			set
			{
				notifications = value;
			}
			get
			{
				return this.notifications;
			}
		}
		public bool isSyncCompleted{ get; set;}
	}

}