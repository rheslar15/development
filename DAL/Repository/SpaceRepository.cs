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
    class SpaceRepository:IRepository<SpaceDO>
    {
         DBUtlity db;

         public SpaceRepository(SQLiteConnection conn, string dbLocation = "")
	    {
            db = new DBUtlity(conn, dbLocation);
	    }


         public SpaceDO GetEntity(int id)
        {
            SpaceDO entity = db.GetItem<SpaceDO>(id);
            return entity;
        }

         public IEnumerable<SpaceDO> GetEntities()
        {
            IEnumerable<SpaceDO> entities = db.GetItems<SpaceDO>();
            return entities;
        }

         public int SaveEntity(SpaceDO item)
        {
            return db.SaveItem<SpaceDO>(item);
        }

		public int UpdateEntity(SpaceDO item)
		{
			return db.UpdateItem<SpaceDO>(item);
		}

        public int DeleteEntity(int id)
        {
            return db.DeleteItem<SpaceDO>(id);
        }
    }
}
