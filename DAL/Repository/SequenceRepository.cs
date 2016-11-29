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
    class SequenceRepository :IRepository<SequencesDO>
    {
         DBUtlity db;

         public SequenceRepository(SQLiteConnection conn, string dbLocation = "")
	    {
            db = new DBUtlity(conn, dbLocation);
	    }


         public SequencesDO GetEntity(int id)
        {
            SequencesDO entity = db.GetItem<SequencesDO>(id);
            return entity;
        }

         public IEnumerable<SequencesDO> GetEntities()
        {
            IEnumerable<SequencesDO> entities = db.GetItems<SequencesDO>();
            return entities;
        }

         public int SaveEntity(SequencesDO item)
        {
            return db.SaveItem<SequencesDO>(item);
        }
		public int UpdateEntity(SequencesDO item)
		{
			return db.UpdateItem<SequencesDO>(item);
		}
        public int DeleteEntity(int id)
        {
            return db.DeleteItem<SequencesDO>(id);
        }
    }
}
