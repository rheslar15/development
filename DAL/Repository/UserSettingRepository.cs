using DAL.DAL;
using Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.DO;
using DAL.Repository;


namespace DAL
{
	public class UserSettingRepository:IRepository<UserSettingDO>
	{	
		DBUtlity db;

		public UserSettingRepository(SQLiteConnection conn, string dbLocation)
		{
			db = new DBUtlity(conn, dbLocation);
		}
		public UserSettingDO GetEntity(int id)
		{
			return db.GetItem<UserSettingDO>(id);
		}

		public IEnumerable<UserSettingDO> GetEntities()
		{
			return db.GetItems<UserSettingDO>();
		}

		public int SaveEntity(UserSettingDO item)
		{
			return db.SaveItem<UserSettingDO>(item);
		}

		public int UpdateEntity(UserSettingDO item)
		{
			return db.UpdateItem<UserSettingDO>(item);
		}

		public int DeleteEntity(int id)
		{
			return db.DeleteItem<UserSettingDO>(id);
		}
	}
}

