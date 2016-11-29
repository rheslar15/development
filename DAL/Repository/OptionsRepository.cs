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
    class OptionsRepository :IRepository<OptionsDO>
    {
         DBUtlity db;

         public OptionsRepository(SQLiteConnection conn, string dbLocation = "")
	    {
            db = new DBUtlity(conn, dbLocation);
	    }


         public OptionsDO GetEntity(int id)
        {
            OptionsDO entity = db.GetItem<OptionsDO>(id);
            return entity;
        }

         public IEnumerable<OptionsDO> GetEntities()
        {
            IEnumerable<OptionsDO> entities = db.GetItems<OptionsDO>();
            return entities;
        }

         public int SaveEntity(OptionsDO item)
        {
            return db.SaveItem<OptionsDO>(item);
        }
		public int UpdateEntity(OptionsDO item)
		{
			return db.UpdateItem<OptionsDO>(item);
		}
        public int DeleteEntity(int id)
        {
            return db.DeleteItem<OptionsDO>(id);
        }
    }
}
