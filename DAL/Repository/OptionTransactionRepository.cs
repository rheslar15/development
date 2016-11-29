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
    class OptionTransactionRepository : IRepository<OptionTransactionDO>
    {
         DBUtlity db;

         public OptionTransactionRepository(SQLiteConnection conn, string dbLocation = "")
	    {
            db = new DBUtlity(conn, dbLocation);
	    }


         public OptionTransactionDO GetEntity(int id)
        {
            OptionTransactionDO entity = db.GetItem<OptionTransactionDO>(id);
            return entity;
        }

         public IEnumerable<OptionTransactionDO> GetEntities()
        {
            IEnumerable<OptionTransactionDO> entities = db.GetItems<OptionTransactionDO>();
            return entities;
        }

         public int SaveEntity(OptionTransactionDO item)
        {
            return db.SaveItem<OptionTransactionDO>(item);
        }

		public int UpdateEntity(OptionTransactionDO item)
		{
			return db.UpdateItem<OptionTransactionDO>(item);
		}
        public int DeleteEntity(int id)
        {
            return db.DeleteItem<OptionTransactionDO>(id);
        }
    }
}
