
using System;

using Foundation;
using UIKit;
using System.Collections.Generic;
using Model;
using System.Linq;

namespace LiRoInspect.iOS
{
	public partial class ConfigCell : UITableViewCell
	{
		IEnumerable<IConfig> DescTableItems;
		NSIndexPath indexPath;
		ConfigType selectedSegment;
		public UITableView tableView;

		UITapGestureRecognizer tap;
		public ConfigCell (IntPtr handle) : base (handle)
		{
		}

		public ConfigCell () : base ()
		{
		}

		public void UpdateData (string name,string value, NSIndexPath indexPath,IEnumerable<IConfig> tableItem,ConfigType selectedSegment)
		{
			
			configLHSLabel.Text = name;
			if (selectedSegment == ConfigType.UserSetting) {
				if (name != null && name != string.Empty) {
					if (name.Contains (Constants.DB_Version)) {
						configRHSLabel.Enabled = false;
						configRHSLabel.BackgroundColor = UIColor.DarkGray;
						configRHSLabel.TextColor = UIColor.White;
					}
				}
			} else 
			{
				configRHSLabel.Enabled = true;
				configRHSLabel.BackgroundColor = UIColor.Clear;
				configRHSLabel.TextColor = UIColor.DarkGray;
			}
			configRHSLabel.Text = value;
			this.selectedSegment = selectedSegment;
			this.DescTableItems = tableItem;
			this.indexPath = indexPath;
			configRHSLabel.EditingChanged-= ConfigRHSLabel_ValueChanged;
			configRHSLabel.EditingChanged+= ConfigRHSLabel_ValueChanged;

			configRHSLabel.EditingDidBegin -= ConfigRHSLabel_EditingDidBegin;
			configRHSLabel.EditingDidBegin += ConfigRHSLabel_EditingDidBegin;

			//if (tap != null)
				//configRHSLabel.RemoveGestureRecognizer (tap);

			//tap = new UITapGestureRecognizer ();

			//tap.AddTarget (tapAction);
			//this.AddGestureRecognizer (tap);
		}

		void ConfigRHSLabel_EditingDidBegin(object sender, EventArgs e)
		{
			if (tableView != null) {
				var Source = (tableView.Source as DB_ConfigTableSource);
				//var Control = (sender as UITextField);
				Source.CurrentOffestY = this.Frame.Y;
			}
		}

     	void ConfigRHSLabel_ValueChanged (object sender, EventArgs e)
     	{
			if (selectedSegment == ConfigType.UserSetting) {
				(DescTableItems as IEnumerable<UserSetting>).ElementAt (indexPath.Row).SettingValue=configRHSLabel.Text;
			} else {
				(DescTableItems as IEnumerable<Configuration>).ElementAt (indexPath.Row).ConfigUrl=configRHSLabel.Text;
			}
     	}
			
	}
		
}
	


