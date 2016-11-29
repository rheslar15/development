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
	public class SpaceTransactionRepository : IRepository<SpaceTransactionDO>
	{
		DBUtlity db;

		public SpaceTransactionRepository(SQLiteConnection conn, string dbLocation = "")
		{
			db = new DBUtlity(conn, dbLocation);
		}


		public SpaceTransactionDO GetEntity(int id)
		{
			SpaceTransactionDO entity = db.GetItem<SpaceTransactionDO>(id);
			return entity;
		}

		public IEnumerable<SpaceTransactionDO> GetEntities()
		{
			IEnumerable<SpaceTransactionDO> entities = db.GetItems<SpaceTransactionDO>();
			return entities;
		}

		public int SaveEntity(SpaceTransactionDO item)
		{
			return db.SaveItem<SpaceTransactionDO>(item);
		}

		public int UpdateEntity(SpaceTransactionDO item)
		{
			return db.UpdateItem<SpaceTransactionDO>(item);
		}

		public int DeleteEntity(int id)
		{
			return db.DeleteItem<SpaceTransactionDO>(id);

		}


	}
}

