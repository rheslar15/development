using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class User
    {
         int userID;

        public int ID
        {
            get { return userID; }
            set { userID = value; }
        }
        string name;

        public string FirstName
        {
            get { return name; }
            set { name = value; }
        }
		string lastName;

        public string LastName
        {
			get { return lastName; }
			set { lastName = value; }
        }
        string token;

        public string Token
        {
            get { return token; }
			set { token = value; }
        }

		DateTime _ExpiryDate;


		public DateTime ExpiryDate
		{
			get { return _ExpiryDate; }
			set { _ExpiryDate = value; }
		}


    }
    
}
