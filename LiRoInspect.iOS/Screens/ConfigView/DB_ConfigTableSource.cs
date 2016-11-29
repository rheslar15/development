using System;
using UIKit;
using System.Collections.Generic;
using Foundation;
using System.Linq;
using DAL.DO;
using Model;
using BAL;

namespace LiRoInspect.iOS
{
	public enum ConfigType{ProductionService,TestService,UserSetting}
	public class DB_ConfigTableSource:UITableViewSource
	{

		public IEnumerable<Configuration> DescTableItems ;
		public IEnumerable<UserSetting> UserSettingItems ;
		private ConfigType selectedSegment;
		public UITableView urlTableView;
		nfloat rowHeight=55f;
		public nfloat CurrentOffestY;

		public DB_ConfigTableSource(IntPtr handle) : base(handle)
		{
		}


		public DB_ConfigTableSource (ConfigType configType)
		{
			var data = ConfigurationDO.getConfiguration (AppDelegate.DatabaseContext);
			selectedSegment = configType;
			switch(configType)
			{
			case ConfigType.ProductionService:
				DescTableItems = data.Where (i => i.IsDefault == true);
				break;
			case ConfigType.TestService:
				DescTableItems = data.Where (i => i.IsDefault == false);
				break;
			case ConfigType.UserSetting:
				using(UserSettingService usr=new UserSettingService(AppDelegate.DatabaseContext))
				{
					var userSetting = usr.GetUserSettings (); 
					UserSettingItems = userSetting;
				}
				break;
			}

		}
		public override nint RowsInSection (UITableView tableview, nint section)
		{
			if (selectedSegment == ConfigType.UserSetting) {
				return UserSettingItems.Count ();
			} else {
				return DescTableItems.Count ();
			}
		}

//		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
//		{
//			SelectedIndexPath = indexPath;
//			CurrentCell = (ConfigCell)tableView.CellAt (indexPath);
//		}

		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			
			var cell = (ConfigCell)tableView.DequeueReusableCell ("ConfigCell");
			urlTableView = tableView;
			cell.tableView = tableView;
			if (selectedSegment == ConfigType.UserSetting) {
				var userSetting = UserSettingItems.ElementAt (indexPath.Row);
				cell.UpdateData (userSetting.SettingName, userSetting.SettingValue, indexPath, UserSettingItems,selectedSegment);
			} else {
				var dataDictionary = DescTableItems.ElementAt (indexPath.Row);
				cell.UpdateData (dataDictionary.ConfigDesc, dataDictionary.ConfigUrl, indexPath, DescTableItems,selectedSegment);
			}
			return cell;
		}

		public override nfloat GetHeightForRow (UITableView tableView, NSIndexPath indexPath)
		{
			return rowHeight;
		}
			
	}
}

