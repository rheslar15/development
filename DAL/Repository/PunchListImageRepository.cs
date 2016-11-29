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
	public class PunchListImageRepository :IRepository<PunchListImageDO>
	{
		DBUtlity db;

		public PunchListImageRepository(SQLiteConnection conn, string dbLocation = "")
		{
			db = new DBUtlity(conn, dbLocation);
		}


		public PunchListImageDO GetEntity(int id)
		{
			PunchListImageDO entity = db.GetItem<PunchListImageDO>(id);
			return entity;
		}

		public IEnumerable<PunchListImageDO> GetEntities()
		{
			IEnumerable<PunchListImageDO> entities = db.GetItems<PunchListImageDO>();
			return entities;
		}

		public int SaveEntity(PunchListImageDO item)
		{
			return db.SaveItem<PunchListImageDO>(item);
		}

		public int UpdateEntity(PunchListImageDO item)
		{
			return db.UpdateItem<PunchListImageDO>(item);
		}

		public int DeleteEntity(int id)
		{
			return db.DeleteItem<PunchListImageDO>(id);
		}
	}
}


