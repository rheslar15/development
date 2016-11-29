using System;
using DAL.DAL;
using System.Linq;
using SQLite;
using System.Collections.Generic;
using System.Diagnostics;

namespace  DAL.Repository
{
	public class NotificationRepository:IRepository<NotificationDO>
	{
		DBUtlity db;
		IEnumerable<NotificationDO> entities;
		public NotificationRepository (SQLiteConnection conn, string dbLocation = "")
		{
			db = new DBUtlity(conn, dbLocation);
			entities = db.GetItems<NotificationDO>();
		}

		public NotificationDO GetEntity(int id)
		{
			NotificationDO entity = db.GetItem<NotificationDO>(id);
			return entity;
		}

		public IEnumerable<NotificationDO> GetEntities()
		{
			IEnumerable<NotificationDO> entities = db.GetItems<NotificationDO>();
			return entities;
		}

		public int SaveEntity(NotificationDO item)
		{
			return db.SaveItem<NotificationDO>(item);
		}

		public int UpdateEntity(NotificationDO item)
		{
			return db.UpdateItem<NotificationDO>(item);
		}
		public void Save(string type,string id,string message)
		{
			try
			{
				if(!string.IsNullOrEmpty(type) && !string.IsNullOrEmpty(id))
				{
					var entity = entities.Where (n => n.NotificationType.ToUpper().Trim() == type.ToUpper().Trim() && n.NotificationTypeID.ToUpper().Trim() == id.ToUpper().Trim()).FirstOrDefault();
					if (entity!=null)
					{
						entity.Count++;
						entity.Notificationmessage = message;
						db.UpdateItem<NotificationDO>(entity);
					} 
					else
					{
						var notification = new  NotificationDO
						{ 
							Count=1,
							NotificationDate=DateTime.Today,
							NotificationType=type,
							NotificationTypeID=id,
							Notificationmessage=message,
						};
						db.SaveItem<NotificationDO>(notification);
					}
				}
			}
			catch(Exception ex) 
			{
				Debug.WriteLine (ex);
			}
		}
		public int DeleteEntity(int id)
		{
			return db.DeleteItem<NotificationDO>(id);
		}
	}
}