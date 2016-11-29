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
	public class UserSettingService:BaseService
	{
		/// <summary>
		/// The user setting repository.
		/// </summary>
		IRepository<UserSettingDO> userSettingRepository;
		public UserSettingService(SQLiteConnection conn)
		{
			userSettingRepository = RepositoryFactory<UserSettingDO>.GetRepository(conn);
		}

		/// <summary>
		/// Gets the user settings.
		/// </summary>
		/// <returns>The user settings.</returns>
		public List<UserSetting> GetUserSettings()
		{
			List<UserSetting> userSettings = new List<UserSetting>() ;
			try
            {
				IEnumerable<UserSettingDO> userSettingDOs = userSettingRepository.GetEntities();
				foreach (UserSettingDO userSettingDO in userSettingDOs)
				{				
					userSettings.Add(Converter.GetUserSetting(userSettingDO));
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Exception Occured in GetUserSettings method due to " + ex.Message);
			}
			return userSettings;
		}

		/// <summary>
		/// Gets the user setting.
		/// </summary>
		/// <returns>The user setting.</returns>
		/// <param name="id">Identifier.</param>
		public UserSetting GetUserSetting(int id)
		{
			UserSetting userSetting = null;
			try
			{
				UserSettingDO userSettingDO = new UserSettingDO();
				userSettingDO = userSettingRepository.GetEntity(id);
				if (userSettingDO != null)
					userSetting = Converter.GetUserSetting(userSettingDO);
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Exception Occured in GetUserSetting method due to " + ex.Message);
			}
			return userSetting;
		}

		/// <summary>
		/// Saves the user setting.
		/// </summary>
		/// <returns>The user setting.</returns>
		/// <param name="userSetting">User setting.</param>
		public int SaveUserSetting(UserSetting userSetting)
		{
			int result = 0;
			try
            {
				UserSettingDO userSettingDO = Converter.GetUserSettingDO(userSetting);
				result = userSettingRepository.SaveEntity(userSettingDO);
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Exception Occured in SaveUserSetting method due to " + ex.Message);
			}
			return result;
		}

		/// <summary>
		/// Updates the user setting.
		/// </summary>
		/// <returns>The user setting.</returns>
		/// <param name="userSetting">User setting.</param>
		public int UpdateUserSetting(UserSetting userSetting)
		{
			int result = 0;
			try
            {
				UserSettingDO userSettingDO = Converter.GetUserSettingDO(userSetting);
				result = userSettingRepository.UpdateEntity(userSettingDO);
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Exception Occured in UpdateUserSetting method due to " + ex.Message);
			}
			return result;
		}

		/// <summary>
		/// Deletes the user setting.
		/// </summary>
		/// <returns>The user setting.</returns>
		/// <param name="userSetting">User setting.</param>
		public int DeleteUserSetting(UserSetting userSetting)
		{
			int result = 0;
			try
            {
				UserSettingDO userSettingDO = Converter.GetUserSettingDO(userSetting);
				result = userSettingRepository.DeleteEntity(userSettingDO.ID);
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Exception Occured in DeleteUserSetting method due to " + ex.Message);
			}
			return result;
		}
	}
}