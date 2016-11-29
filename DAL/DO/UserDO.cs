using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using System.Diagnostics;


namespace DAL.DO
{
    [Table("User")]
    public class UserDO : IDomianObject
    {
        int id;
        [PrimaryKey, AutoIncrement]
        [Column("UserID")]
        public int ID
        {
			get { return id; }
			set { id = value; }
        }

        string _Token;
        public string Token
		{
            get { return _Token; }
            set { _Token = value; }
		}

        string _FirstName;
        public string FirstName
        {
            get { return _FirstName; }
            set { _FirstName = value; }
        }

        string _LastName;
        public string LastName 
        {
            get { return _LastName; }
            set { _LastName = value; }
        }

        DateTime _ExpiryDate;
        public DateTime ExpiryDate
        {
            get { return _ExpiryDate; }
            set { _ExpiryDate = value; }
        }
        
		public static UserDO GetLastUser(SQLiteConnection conn)
		{
			DateTime today = DateTime.Today;
			UserDO Lastuser = new UserDO ();
			string Query = "select * from user where ExpiryDate>'"+today+"'";
			List<UserDO> user=conn.Query<UserDO>(Query);
			if (user != null && user.Count>0) {
				Lastuser = user [0];
			} else {
				Lastuser = null;
			}
			return Lastuser;
		}

		public static void DeleteAllUser(SQLiteConnection conn)
		{
            try
            {
			    string Query = "delete from user";
				conn.Execute(Query);
			}
			catch(Exception ex)
            {
				Debug.WriteLine (ex.Message);
			}
		}        
    }
}