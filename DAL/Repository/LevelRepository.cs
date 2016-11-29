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
    class LevelRepository:IRepository<LevelDO>
    {
         DBUtlity db;

         public LevelRepository(SQLiteConnection conn, string dbLocation = "")
	    {
            db = new DBUtlity(conn, dbLocation);
	    }


         public LevelDO GetEntity(int id)
        {
            LevelDO entity = db.GetItem<LevelDO>(id);
            return entity;
        }

         public IEnumerable<LevelDO> GetEntities()
        {
            IEnumerable<LevelDO> entities = db.GetItems<LevelDO>();
            return entities;
        }

         public int SaveEntity(LevelDO item)
        {
            return db.SaveItem<LevelDO>(item);
        }

		public int UpdateEntity(LevelDO item)
		{
			return db.UpdateItem<LevelDO>(item);
		}

        public int DeleteEntity(int id)
        {
            return db.DeleteItem<LevelDO>(id);

        }
    }
}
