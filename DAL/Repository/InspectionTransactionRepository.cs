using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.DAL;
using DAL.DO;
using SQLite;

namespace DAL.Repository
{
    class InspectionTransactionRepository:IRepository<InspectionTransactionDO>
    {
          DBUtlity db;

          public InspectionTransactionRepository(SQLiteConnection conn, string dbLocation = "")
	    {
            db = new DBUtlity(conn, dbLocation);
	    }


          public InspectionTransactionDO GetEntity(int id)
        {
            InspectionTransactionDO entity = db.GetItem<InspectionTransactionDO>(id);
            return entity;
        }

          public IEnumerable<InspectionTransactionDO> GetEntities()
        {
            IEnumerable<InspectionTransactionDO> entities = db.GetItems<InspectionTransactionDO>();
            return entities;
        }

          public int SaveEntity(InspectionTransactionDO item)
        {
            return db.SaveItem<InspectionTransactionDO>(item);
        }

		public int UpdateEntity(InspectionTransactionDO item)
		{
			return db.UpdateItem<InspectionTransactionDO>(item);
		}

        public int DeleteEntity(int id)
        {
            return db.DeleteItem<InspectionTransactionDO>(id);
        }
    }
}
