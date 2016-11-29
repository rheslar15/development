using System;
using DAL.Repository;
using DAL.DO;
using System.Collections.Generic;
using Model;
using DAL.Utility;
using SQLite;
using System.Diagnostics;
using System.Linq;

namespace BAL
{
	public class UserService
	{ 
		IRepository<UserDO> userRepository;
		public UserService(SQLiteConnection conn)
		{
			userRepository = RepositoryFactory<UserDO>.GetRepository(conn);
		}

		/// <summary>
		/// Gets the users.
		/// </summary>
		/// <returns>The users.</returns>
		public List<User> GetUsers()
		{
			List<User> users = new List<User>() ;
			try
            {
			    IEnumerable<UserDO> usersDOs = userRepository.GetEntities();
			    foreach (UserDO userDo in usersDOs)
			    {
				    users.Add(Converter.GetUser(userDo));
			    }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception Occured in GetUsers method due to " + ex.Message);
			}
			return users;
		}

		/// <summary>
		/// Gets the user.
		/// </summary>
		/// <returns>The user.</returns>
		public User GetUser()
		{
			User user = null;
			try
			{
				UserDO userDO = new UserDO();
				userDO = userRepository.GetEntities().FirstOrDefault();
				if (userDO != null)
					user = Converter.GetUser(userDO);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception Occured in GetUser method due to " + ex.Message);
			}
			return user;
		}

		/// <summary>
		/// Saves the user.
		/// </summary>
		/// <returns>The user.</returns>
		/// <param name="user">User.</param>
		public int SaveUser(User user)
		{
			int result = 0;
			try
            {
			    UserDO userDO = Converter.GetUserDO(user);
			    result = userRepository.SaveEntity(userDO);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception Occured in SaveUser method due to " + ex.Message);
			}
			return result;
		}

		/// <summary>
		/// Deletes the user.
		/// </summary>
		/// <param name="conn">Conn.</param>
		public void DeleteUser(SQLiteConnection conn)
		{
//			int result = 0;
			try
            {
    //			UserDO userDO = Converter.GetUserDO(user);
    //			result = userRepository.DeleteEntity(userDO.ID);
				    UserDO.DeleteAllUser(conn);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception Occured in DeleteUser method due to " + ex.Message);
			}
//			return result;
		}

		/// <summary>
		/// Verifies the user.
		/// </summary>
		/// <returns><c>true</c>, if user was verifyed, <c>false</c> otherwise.</returns>
		/// <param name="user">User.</param>
		public bool VerifyUser(User user)
		{
			return true;
		}
	}
}