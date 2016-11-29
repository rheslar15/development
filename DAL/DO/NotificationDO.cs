using System;
using DAL.DO;
using SQLite;

namespace DAL
{
	[Table("Notifications")]
	public class NotificationDO : IDomianObject
	{
		int _NotificationsID;
		[PrimaryKey, AutoIncrement]
		[Column("NotificationsID")]
		public int ID
		{
			get { return _NotificationsID; }
			set { _NotificationsID = value; }
		}

		string _NotificationType;
		public string NotificationType
		{
			get { return _NotificationType; }
			set { _NotificationType = value; }
		}

		string _NotificationTypeID;
		public string NotificationTypeID
		{
			get { return _NotificationTypeID; }
			set { _NotificationTypeID = value; }
		}

		string _Notificationmessage;
		public string Notificationmessage
		{
			get { return _Notificationmessage; }
			set { _Notificationmessage = value; }
		}

		int _Count;
		public int Count
		{
			get { return _Count; }
			set { _Count = value; }
		}
		DateTime _NotificationDate;
		public DateTime NotificationDate
		{
			get { return _NotificationDate; }
			set { _NotificationDate = value; }
		}
	}
}