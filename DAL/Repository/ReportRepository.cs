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
    class ReportRepository:IRepository<ReportDO>
    {
         DBUtlity db;

         public ReportRepository(SQLiteConnection conn, string dbLocation = "")
	    {
            db = new DBUtlity(conn, dbLocation);
	    }


         public ReportDO GetEntity(int id)
        {
            ReportDO entity = db.GetItem<ReportDO>(id);
            return entity;
        }

         public IEnumerable<ReportDO> GetEntities()
        {
            IEnumerable<ReportDO> entities = db.GetItems<ReportDO>();
            return entities;
        }
         public int SaveEntity(ReportDO item)
         {
             return db.SaveItem<ReportDO>(item);
         }
         public int UpdateEntity(ReportDO item)
         {
             return db.UpdateItem<ReportDO>(item);
         }

         

        public int DeleteEntity(int id)
        {
            return db.DeleteItem<ReportDO>(id);
        }
    }
}
