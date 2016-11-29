using System;
using SQLite;
using System.Collections.Generic;
using ServiceLayer.Service;
using Model.ServiceModel;
using BAL;
using System.Linq;
using LiroInspectServiceModel.Services;

/// <summary>
/// The class verifies the login details of the inspector
/// </summary>
using DAL.DO;
using System.Diagnostics;

namespace DAL.BAL
{
	public class LoginBAL
	{
		public bool canGotoDasboard;
		SQLiteConnection conn;
		public LoginBAL (SQLiteConnection conn)
		{
			this.conn = conn;
			UpdateServiceURL ();
		}

		/// <summary>
		/// Gets the user details from database.
		/// </summary>
		/// <returns>The user details from database.</returns>
		/// <param name="conn">Conn.</param>
		public Model.AuthenticatedUser GetUserDetailsFromDatabase(SQLiteConnection conn)
		{
			UserService userServiceObject = new UserService (conn);
			Model.AuthenticatedUser authenticatedUser = new Model.AuthenticatedUser ();
			try
            {
				var user=userServiceObject.GetUser();
				if(user!=null)
				{
					authenticatedUser.UserDetails = user;
					authenticatedUser.IsFirstTimeLoggedIn = false;
					if(authenticatedUser.UserDetails!=null && authenticatedUser.UserDetails.ExpiryDate!=null)
                    {
						authenticatedUser.IsTokenActive = this.CheckIfTokenIsActive(authenticatedUser.UserDetails.ExpiryDate);
					}
				}
                else
                {
					authenticatedUser.UserDetails = null;
					authenticatedUser.IsFirstTimeLoggedIn = true;
					authenticatedUser.IsTokenActive = false;
				}
			}
			catch(Exception ex)
            {
				Debug.WriteLine ("Exception occured at GetUserDetailsFromDatabase Method : "+ex.Message);
			}
			return authenticatedUser;
		}

		/// <summary>
		/// Gets the user details from service.
		/// </summary>
		/// <returns>The user details from service.</returns>
		/// <param name="userName">User name.</param>
		/// <param name="password">Password.</param>
		public Model.AuthenticatedUser GetUserDetailsFromService(String userName, String password)
		{
			IServices servicesObject = new Services();
			AuthenticationReq request = new AuthenticationReq();
			request.userName = userName;
			request.password = password;
			AuthenticationRes authenticationResult = null;
			Model.AuthenticatedUser authenticatedUser = new Model.AuthenticatedUser ();
			try
            {
			        UserService userServiceObject = new UserService (conn);
				    authenticationResult = servicesObject.authenticate (request);
				
				    bool isTokenActive = this.CheckIfTokenIsActive(authenticationResult.expiryTime);
				    authenticatedUser.IsFirstTimeLoggedIn = true;

			    if (authenticationResult.result != null) 
				    authenticatedUser.Result = authenticationResult.result;

				    if (!string.IsNullOrEmpty (authenticationResult.token))
				    {
						    authenticatedUser.UserDetails = new Model.User ();
						    authenticatedUser.UserDetails.Token = authenticationResult.token;
						    authenticatedUser.UserDetails.FirstName = authenticationResult.firstName;
						    authenticatedUser.UserDetails.LastName = authenticationResult.lastName;
						    authenticatedUser.UserDetails.ExpiryDate = authenticationResult.expiryTime;
						    authenticatedUser.IsTokenActive = isTokenActive;
							authenticatedUser.DBVersion=authenticationResult.DBVersion;
						    //Delete the previous exist users
						    userServiceObject.DeleteUser(conn);
						    //save the details of the user in the database
						    userServiceObject.SaveUser (authenticatedUser.UserDetails);
					    try 
                        {
				                var user=userServiceObject.GetUsers ().FirstOrDefault ();
						        if (user != null && authenticatedUser!=null)
                                {
							        authenticatedUser.UserDetails.ID = user.ID;
						        }
				        }
                        catch(Exception ex)
                        {
					    // To DO
				        }			
				    } 
			}
            catch(Exception ex) 
            {
			}
			return authenticatedUser;
		}

		/// <summary>
		/// Checks if token is active.
		/// </summary>
		/// <returns><c>true</c>, if if token is active was checked, <c>false</c> otherwise.</returns>
		/// <param name="expiryDate">Expiry date.</param>
		private bool CheckIfTokenIsActive (DateTime expiryDate)
		{
			int isTokenActive = DateTime.Compare(DateTime.Now, expiryDate);

			if (isTokenActive != 1)
				return true;
			else
				return false;
		}

		/// <summary>
		/// Updates the service URL, Config page
		/// </summary>
		private void UpdateServiceURL()
		{
			if (conn != null) 
            {
				var items = ConfigurationDO.getConfiguration (conn);
				if (items != null && items.Count > 0) 
                {
					var configURLs = items.Where (i => i.use == true);
					foreach (var configURL in configURLs) 
                    {
						switch (configURL.ConfigDesc) 
                        {
						case "UrlAuth":
							    if (configURL.ConfigUrl != null && configURL.ConfigUrl!=string.Empty)
                                {
								    ServiceURL.UrlAuth = configURL.ConfigUrl;
							    }
							    break;
						case "UrlGetInspection":
							    if (configURL.ConfigUrl != null && configURL.ConfigUrl!=string.Empty)
                                {
								    ServiceURL.UrlGetInspection = configURL.ConfigUrl;
                                }
							    break;
						case "UrlInspectionResults":
								    if (configURL.ConfigUrl != null && configURL.ConfigUrl!=string.Empty)
                                    {
									    ServiceURL.UrlInspectionResults = configURL.ConfigUrl;
                                    }
							    break;
						case "UrlINspectionReport":
									    if (configURL.ConfigUrl != null && configURL.ConfigUrl!=string.Empty) 
                                        {
									    ServiceURL.UrlINspectionReport = configURL.ConfigUrl;
                                        }
								    break;
						case "UrlGetPunchList":
										if (configURL.ConfigUrl != null && configURL.ConfigUrl!=string.Empty) 
                                        {
											ServiceURL.UrlGetPunchList = configURL.ConfigUrl;
                                        }
									break;
						case "UrlPunchListResults":
						if (configURL.ConfigUrl != null && configURL.ConfigUrl!=string.Empty) 
                        {
								ServiceURL.UrlPunchListResults = configURL.ConfigUrl;
                        }
						break;

						case "UrlGetInspectionDocuments":
							if (configURL.ConfigUrl != null && configURL.ConfigUrl!=string.Empty) 
							{
								ServiceURL.UrlGetInspectionDocuments = configURL.ConfigUrl;
							}
							break;

						case "UrlMasterDataUpdate":
							if (configURL.ConfigUrl != null && configURL.ConfigUrl!=string.Empty) 
							{
								ServiceURL.UrlMasterDataUpdate = configURL.ConfigUrl;
							}
							break;
						}
					}
				}
			}
		}
	}
}