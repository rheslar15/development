using System;
using DAL.Repository;
using DAL.DO;
using SQLite;
using DAL.DAL;
using System.Collections.Generic;

namespace DAL.Repository

{
	public class OptionTransactionImageRepository:IRepository<OptionTransactionImageDO>
	{
		DBUtlity db;

		public OptionTransactionImageRepository(SQLiteConnection conn, string dbLocation = "")
		{
			db = new DBUtlity(conn, dbLocation);
		}


		public OptionTransactionImageDO GetEntity(int id)
		{
			OptionTransactionImageDO entity = db.GetItem<OptionTransactionImageDO>(id);
			return entity;
		}

		public IEnumerable<OptionTransactionImageDO> GetEntities()
		{
			IEnumerable<OptionTransactionImageDO> entities = db.GetItems<OptionTransactionImageDO>();
			return entities;
		}

		public int SaveEntity(OptionTransactionImageDO item)
		{
			return db.SaveItem<OptionTransactionImageDO>(item);
		}
		public int UpdateEntity(OptionTransactionImageDO item)
		{
			return db.UpdateItem<OptionTransactionImageDO>(item);
		}

		public int DeleteEntity(int id)
		{
			return db.DeleteItem<OptionTransactionImageDO>(id);
		}
	}
}

