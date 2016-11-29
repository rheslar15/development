using System;
using DAL.Repository;
using DAL;
using SQLite;
using Model;
using DAL.DO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Utility;
using System.Diagnostics;

namespace BAL
{
	public class NotificationService
	{
		IRepository<NotificationDO> notificationRepository;
		public NotificationService (SQLiteConnection conn)
		{
			notificationRepository = RepositoryFactory<NotificationDO>.GetRepository(conn);
		}

		public List<Notifications> GetNotifications()
		{
			List<Notifications> notifications = new List<Notifications>();
			try
			{
				IEnumerable<NotificationDO> notificationsDOs = notificationRepository.GetEntities();
				foreach (NotificationDO notifiDo in notificationsDOs)
				{
					notifications.Add(Converter.GetNotifications(notificationRepository.GetEntity(notifiDo.ID)));
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Exception Occured in GetOptions method due to " + ex.Message);
			}
			return notifications;
		}

		public int SaveNotifications(Notifications notification)
		{
			int result = 0;
			try
			{
				NotificationDO optionsDO = Converter.GetNotificationsDO(notification);
				result = notificationRepository.SaveEntity(optionsDO);
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Exception Occured in SaveOptions method due to " + ex.Message);
			}
			return result;
		}

		public int DeleteNotifications(Notifications notification)
		{
			int result = 0;
			try
			{
				NotificationDO optionsDO = Converter.GetNotificationsDO(notification);
				result = notificationRepository.DeleteEntity(optionsDO.ID);
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Exception Occured in DeleteOptions method due to " + ex.Message);
			}
			return result;
		}
	}
}