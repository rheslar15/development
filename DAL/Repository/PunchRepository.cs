using System;
using DAL.Repository;
using DAL.DAL;
using SQLite;
using DAL.DO;
using System.Collections.Generic;

namespace DAL
{
	public class PunchRepository:IRepository<PunchListDO>
	{
		public PunchRepository ()
		{
		}

		DBUtlity db;

		public PunchRepository(SQLiteConnection conn, string dbLocation = "")
		{
			db = new DBUtlity(conn, dbLocation);
		}
			
		public int SaveEntity(PunchListDO item)
		{
			return db.SaveItem<PunchListDO>(item);
		}

		public int InsertPunchList(PunchListDO item)
		{
			return db.UpdateItem<PunchListDO>(item);
		}
		public int DeleteEntity(int id)
		{
			return db.DeleteItem<PunchListDO>(id);
		}

		public int UpdateEntity(PunchListDO item)
		{
			return db.UpdateItem<PunchListDO>(item);
		}

		public PunchListDO GetEntity(int id)
		{
			PunchListDO entity = db.GetItem<PunchListDO>(id);
			return entity;
		}

		public IEnumerable<PunchListDO> GetEntities()
		{
			IEnumerable<PunchListDO> entities = db.GetItems<PunchListDO>();
			return entities;
		}
	}
}
	


