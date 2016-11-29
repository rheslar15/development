using System;
using SQLite;
using DAL.DAL;
using DAL.DO;
using System.Collections.Generic;
using DAL.Repository;

namespace DAL
{
	public class PathwayRepository:IRepository<PathwayDO>
	{
		DBUtlity db;

		public PathwayRepository(SQLiteConnection conn, string dbLocation = "")
		{
			db = new DBUtlity(conn, dbLocation);
		}

		public PathwayDO GetEntity(int id)
		{
			PathwayDO pathwayDO = db.GetItem<PathwayDO>(id);
			return pathwayDO;
		}

		public IEnumerable<PathwayDO> GetEntities()
		{
			IEnumerable<PathwayDO> entities = db.GetItems<PathwayDO>();
			return entities;
		}

		public int SaveEntity(PathwayDO item)
		{
			return db.SaveItem<PathwayDO>(item);
		}

		public int UpdateEntity (PathwayDO item)
		{
			return db.UpdateItem<PathwayDO>(item);
		}

		public int DeleteEntity(int id)
		{
			return db.DeleteItem<PathwayDO>(id);
		}
	}
}