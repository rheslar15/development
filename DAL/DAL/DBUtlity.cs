using System;
using System.Linq;
using System.Collections.Generic;
using SQLite;
using DAL.DO;

namespace DAL.DAL
{
	/// <summary>
	/// TaskDatabase builds on SQLite.Net and represents a specific database, in our case, the Task DB.
	/// It contains methods for retrieval and persistance as well as db creation, all based on the 
	/// underlying ORM.
	/// </summary>
	public class DBUtlity 
	{
		static object locker = new object ();

        SQLiteConnection database;

		/// <summary>
		/// Initializes a new instance of the <see cref="Tasky.DL.TaskDatabase"/> TaskDatabase. 
		/// if the database doesn't exist, it will create the database and all the tables.
		/// </summary>
		/// <param name='path'>
		/// Path.
		/// </param>
        public DBUtlity(SQLiteConnection conn, string path)
		{
            database = conn;
		}

        public IEnumerable<T> GetItems<T>() where T : IDomianObject, new()
		{
            try
            {
                lock (locker)
                {
                    return database.Table<T>().ToList();//.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
		}

        public T GetItem<T>(int id) where T : IDomianObject, new()
		{
            lock (locker) {
                return database.Table<T>().Where(x => x.ID == id).FirstOrDefault();
            }
		}

		public UserDO GetUser(string  token)
		{
			lock (locker) {
				return database.Table<UserDO>().Where(x => x.Token == token).FirstOrDefault();
			}
		}

        public int SaveItem<T>(T item) where T : IDomianObject
		{
            lock (locker) {
                    return database.Insert(item);
            }
		}


		public int UpdateItem<T>(T item) where T : IDomianObject
		{
			lock (locker) {
				return database.Update(item);
			}
		}

        public int DeleteItem<T>(int id) where T : IDomianObject, new()
		{
            lock (locker) {
                return database.Delete<T>(id);
            }
		}

        public void SaveItem<T>(Action t) where T : IDomianObject
        {

            lock (locker)
            {
                database.BeginTransaction();
                t.Invoke();
                database.Commit();
            }
        }
	}
}