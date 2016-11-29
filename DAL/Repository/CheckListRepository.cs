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
	class CheckListRepository:IRepository<CheckListDO>
	{
		DBUtlity db;

		public CheckListRepository(SQLiteConnection conn, string dbLocation = "")
		{
			db = new DBUtlity(conn, dbLocation);
		}

		public CheckListDO GetEntity(int id)
		{
			CheckListDO entity = db.GetItem<CheckListDO>(id);
			return entity;
		}

		public IEnumerable<CheckListDO> GetEntities()
		{
			IEnumerable<CheckListDO> entities = db.GetItems<CheckListDO>();
			return entities;
		}

		public int SaveEntity(CheckListDO item)
		{
			return db.SaveItem<CheckListDO>(item);
		}

		public int UpdateEntity(CheckListDO item)
		{
			return db.UpdateItem<CheckListDO>(item);
		}

		public int DeleteEntity(int id)
		{
			return db.DeleteItem<CheckListDO>(id);
		}
	}
}