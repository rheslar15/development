using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.ServiceModel;
namespace Model
{
	/// <summary>
	/// Model class for Authenticated user.
	/// </summary>
	public class AuthenticatedUser
	{
		User userDetails;
		public User UserDetails {
			get { return userDetails; }
			set { userDetails = value; }
		}

		public Result Result{ get; set;}

		public bool IsFirstTimeLoggedIn{ get; set;}
		public bool IsTokenActive{ get; set;}
		public string DBVersion{ get; set; }

	}
}

