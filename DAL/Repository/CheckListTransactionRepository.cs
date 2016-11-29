using System;
using DAL.Repository;
using DAL.DO;
using SQLite;
using DAL.DAL;
using System.Collections.Generic;

namespace DAL.Repository

{
	public class CheckListTransactionRepository:IRepository<CheckListTransactionDO>
	{
		DBUtlity db;

		public CheckListTransactionRepository(SQLiteConnection conn, string dbLocation = "")
		{
			db = new DBUtlity(conn, dbLocation);
		}


		public CheckListTransactionDO GetEntity(int id)
		{
			CheckListTransactionDO entity = db.GetItem<CheckListTransactionDO>(id);
			return entity;
		}

		public IEnumerable<CheckListTransactionDO> GetEntities()
		{
			IEnumerable<CheckListTransactionDO> entities = db.GetItems<CheckListTransactionDO>();
			return entities;
		}

		public int SaveEntity(CheckListTransactionDO item)
		{
			return db.SaveItem<CheckListTransactionDO>(item);
		}
		public int UpdateEntity(CheckListTransactionDO item)
		{
			return db.UpdateItem<CheckListTransactionDO>(item);
		}

		public int DeleteEntity(int id)
		{
			return db.DeleteItem<CheckListTransactionDO>(id);
		}
	}
}

