using DAL.DAL;
using Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.DO;

namespace DAL.Repository
{
    public class UserRepository:IRepository<UserDO>
    {
       DBUtlity db;

       public UserRepository(SQLiteConnection conn, string dbLocation)
	    {
            db = new DBUtlity(conn, dbLocation);
	    }
       public UserDO GetEntity(int id)
        {
            return db.GetItem<UserDO>(id);
        }
		public UserDO GetUser(string  userID)
		{
			return db.GetUser(userID);
		}
       public IEnumerable<UserDO> GetEntities()
        {
            return db.GetItems<UserDO>();
        }

       public int SaveEntity(UserDO item)
        {
            return db.SaveItem<UserDO>(item);
        }

		public int UpdateEntity(UserDO item)
		{
			return db.UpdateItem<UserDO>(item);
		}

        public int DeleteEntity(int id)
        {
            return db.DeleteItem<UserDO>(id);
        }
    }
}
