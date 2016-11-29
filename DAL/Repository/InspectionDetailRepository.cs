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
	class InspectionDetailRepository : IRepository<InspectionDO>
    {

         DBUtlity db;

         public InspectionDetailRepository(SQLiteConnection conn, string dbLocation = "")
	    {
            db = new DBUtlity(conn, dbLocation);
	    }


        public InspectionDO GetEntity(int id)
        {
            InspectionDO inspectionrDetailDO = db.GetItem<InspectionDO>(id);
            return inspectionrDetailDO;
        }

        public IEnumerable<InspectionDO> GetEntities()
        {
            IEnumerable<InspectionDO> entities = db.GetItems<InspectionDO>();
            return entities;
        }

        public int SaveEntity(InspectionDO item)
        {
            return db.SaveItem<InspectionDO>(item);
        }


		public int UpdateEntity (InspectionDO item)
		{
			return db.UpdateItem<InspectionDO>(item);
		}

        public int DeleteEntity(int id)
        {
            return db.DeleteItem<InspectionDO>(id);
        }
    }
}
