using DAL.DAL;
using DAL.DO;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
	public class LevelTransactionRepository : IRepository<LevelTransactionDO>
	{
		DBUtlity db;

		public LevelTransactionRepository(SQLiteConnection conn, string dbLocation = "")
		{
			db = new DBUtlity(conn, dbLocation);
		}


		public LevelTransactionDO GetEntity(int id)
		{
			LevelTransactionDO entity = db.GetItem<LevelTransactionDO>(id);
			return entity;
		}

		public IEnumerable<LevelTransactionDO> GetEntities()
		{
			IEnumerable<LevelTransactionDO> entities = db.GetItems<LevelTransactionDO>();
			return entities;
		}

		public int SaveEntity(LevelTransactionDO item)
		{
			return db.SaveItem<LevelTransactionDO>(item);
		}

		public int UpdateEntity(LevelTransactionDO item)
		{
			return db.UpdateItem<LevelTransactionDO>(item);
		}

		public int DeleteEntity(int id)
		{
			return db.DeleteItem<LevelTransactionDO>(id);

		}
	}
}

