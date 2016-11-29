using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Helper
{
    public static class ConnectionHelper
    {
        /// <summary>
        /// Method to establish connection with database
        /// </summary>
        /// <returns></returns>
		public static SQLiteConnection conn;
        public static SQLiteConnection GetConnection()
        {
            try
            {
				if(conn==null)
				{
                	return new SQLiteConnection(@"LiRoDB", SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.FullMutex, false);
				}
				else
				{
					return conn;
				}
            }
            catch (SQLiteException ex)
            {
                throw new Exception(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
