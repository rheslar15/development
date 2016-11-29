
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;
using SQLite;
using DAL.DO;
using Model;
using BAL;
using System.Collections.Generic;
using CoreGraphics;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace LiRoInspect.iOS
{
	partial class config : UIViewController
	{
		int success=0;
		int SuccessSegment1=0;
		int SuccessSegment2=0;

		DB_ConfigTableSource db;
		public config (IntPtr handle) : base (handle)
		{
		}
		public override void ViewDidLoad ()
		{
			SegmentProTest.SelectedSegment = 0;
			db=new DB_ConfigTableSource(ConfigType.ProductionService);
			configTableView.Source=db;
			configTableView.ReloadData();
			configTableView.ReloadData();
			btn_Use.TouchUpInside-= Btn_Use_TouchUpInside;
			btn_Use.TouchUpInside+= Btn_Use_TouchUpInside;;
			registerForKeyboardNotifications();
		}

		partial void SegmentProTest_ValueChanged (UISegmentedControl sender)
		{

			var selectedSegmentId = (sender as UISegmentedControl).SelectedSegment;
			switch(SegmentProTest.SelectedSegment) {
			case 0:
				configTableView.Hidden=false;
				btn_Use.Hidden=false;
				db=new DB_ConfigTableSource(ConfigType.ProductionService);
				configTableView.Source=db;
				configTableView.ReloadData();
				break;
			case 1:
				configTableView.Hidden=false;
				btn_Use.Hidden=false;
				db=new DB_ConfigTableSource(ConfigType.TestService);
				configTableView.Source=db;
				configTableView.ReloadData();
				break;
			case 2:
				btn_Use.Hidden=true;
				db=new DB_ConfigTableSource(ConfigType.UserSetting);
				configTableView.Source=db;
				configTableView.ReloadData();
				break;
			}
		}

		partial void btn_save_TouchUpInside (UIButton sender)
		{ 
			switch (SegmentProTest.SelectedSegment){
			case 0:
			case 1:
			if(db!=null)
			{
				foreach(var config in db.DescTableItems)
				{
					bool validURL=true;
					validURL=URLValidation(config,validURL);
					if(!validURL)
					{
						return;
					}
					success=ConfigurationDO.UpdateConfiguration(AppDelegate.DatabaseContext,config);
				}
				configTableView.ReloadData();
				if(success==1)
				{
					UIAlertView alert = new UIAlertView (@" ", @"Url's Saved Successfully", null, NSBundle.MainBundle.LocalizedString ("OK", "OK"));
					alert.Show ();
				}
			}
			break;
			case 2:
				foreach(var userSet in db.UserSettingItems)
				{
					bool validData=true;
					if(userSet.ID == 3)
					{
						validData=DBVersionValidation(userSet, validData);
					}
					else
					{
						validData=DataValidation(userSet, validData);
					}

					if(!validData)
					{
						return;
					}

					using(UserSettingService ser=new UserSettingService(AppDelegate.DatabaseContext))
					{
						success=ser.UpdateUserSetting(userSet);
					}
				}

				configTableView.ReloadData();
				if(success==1)
				{
					UIAlertView alert = new UIAlertView (@" ", @"User Setting Saved Successfully", null, NSBundle.MainBundle.LocalizedString ("OK", "OK"));
					alert.Show ();
				}
			break;
			}
		}

		bool DataValidation(UserSetting user, bool validData)
		{
			int value;
			if (user.SettingValue == string.Empty || user.SettingValue == null) {
				validData = false;
				UIAlertView alert = new UIAlertView ("Invalid Data", user.SettingName + " value should not be empty", null, NSBundle.MainBundle.LocalizedString ("OK", "OK"));
				alert.Show ();
			} else if (!int.TryParse (user.SettingValue, out value)) {
				validData = false;
				UIAlertView alert = new UIAlertView ("Invalid Data", user.SettingName + " value should be number", null, NSBundle.MainBundle.LocalizedString ("OK", "OK"));
				alert.Show ();
			} else if (int.TryParse (user.SettingValue, out value)) {
				if (value <= 0) {
					validData = false;
					UIAlertView alert = new UIAlertView ("Invalid Data", user.SettingName + " value should be greater than Zero", null, NSBundle.MainBundle.LocalizedString ("OK", "OK"));
					alert.Show ();
				}
			}
			return validData;
		}

		bool DBVersionValidation(UserSetting user, bool validData)
		{			
			if (user != null && user.SettingValue != null)
			{
				if (Regex.IsMatch (user.SettingValue, Constants.DBVersion_RegEx)) {
					validData = true;
				} else {
					validData = false;
					UIAlertView alert = new UIAlertView ("Invalid Data", user.SettingName + " Invalid Value", null, NSBundle.MainBundle.LocalizedString ("OK", "OK"));
					alert.Show ();
				}
			}
			return validData;
		}

		bool URLValidation(Configuration config, bool validURL)
		{
			Uri UriResult;
			if (config.ConfigUrl == string.Empty || config.ConfigUrl == null) {
				validURL = false;
				UIAlertView alert = new UIAlertView ("Invalid URL", config.ConfigDesc + " should not be empty", null, NSBundle.MainBundle.LocalizedString ("OK", "OK"));
				alert.Show ();
			} else if (!(Uri.TryCreate (config.ConfigUrl,UriKind.Absolute, out UriResult) && (UriResult.Scheme==Uri.UriSchemeHttp || UriResult.Scheme==Uri.UriSchemeHttps))) {
				validURL = false;
				UIAlertView alert = new UIAlertView ("Invalid Data", config.ConfigDesc + " is not valid URL", null, NSBundle.MainBundle.LocalizedString ("OK", "OK"));
				alert.Show ();
				} 
			return validURL;
		}

		partial void btn_Logout_TouchUpInside (UIButton sender)
		{
			UIAlertView alert1 = new UIAlertView (@"Alert", @"Are you sure you want to log out?", null, NSBundle.MainBundle.LocalizedString ("Cancel", "Cancel"), NSBundle.MainBundle.LocalizedString ("OK", "OK"));
			alert1.Show ();
			int buttonClicked=-1;
			alert1.Clicked += (sender1, buttonArgs) =>  {
				buttonClicked = (int)buttonArgs.ButtonIndex ;
				if(buttonClicked==1)
				{
					LoginViewController loginViewController = this.Storyboard.InstantiateViewController ("LoginViewController") as LoginViewController;
					this.NavigationController.PushViewController (loginViewController, false);
				}
			};
		}

		void Btn_Use_TouchUpInside (object sender, EventArgs e)
		{
			switch (SegmentProTest.SelectedSegment) {
			case 0:
				if (db != null) {
					var items = ConfigurationDO.getConfiguration (AppDelegate.DatabaseContext);

					foreach (var item in items) {
						item.use = false;
						ConfigurationDO.UpdateConfiguration (AppDelegate.DatabaseContext, item);

						foreach (var config in db.DescTableItems) {

							bool validURL=true;
							validURL=URLValidation(config,validURL);
							if(!validURL)
							{
								return;
							}
							if (item.ID == config.ID) {
								config.use = true;
								SuccessSegment1 = ConfigurationDO.UpdateConfiguration (AppDelegate.DatabaseContext, config);

							}
						}
					}
					configTableView.ReloadData ();
					if (SuccessSegment1 == 1) {
						UIAlertView alert = new UIAlertView (@" ", @"Flag Changed Successfully", null, NSBundle.MainBundle.LocalizedString ("OK", "OK"));
						alert.Show ();
					}
				}
				break;
			case 1:
				if (db != null) {
					var items = ConfigurationDO.getConfiguration (AppDelegate.DatabaseContext);

					foreach (var item in items) {
						item.use = false;
						ConfigurationDO.UpdateConfiguration (AppDelegate.DatabaseContext, item);

						foreach (var config in db.DescTableItems) {
							bool validURL=true;
							validURL=URLValidation(config,validURL);
							if(!validURL)
							{
								return;
							}
							config.use = true;
							SuccessSegment2 = ConfigurationDO.UpdateConfiguration (AppDelegate.DatabaseContext, config);
						}
						configTableView.ReloadData ();
					}
					if (SuccessSegment2 == 1) {
						UIAlertView alert = new UIAlertView (@" ", @"Flag Changed Successfully", null, NSBundle.MainBundle.LocalizedString ("OK", "OK"));
						alert.Show ();
					}
				}
				break;
			}
		}

		public void registerForKeyboardNotifications()
		{
			NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillHideNotification, OnKeyboardNotification);
			NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillShowNotification, OnKeyboardNotification);
		}

		private void OnKeyboardNotification(NSNotification notification)
		{
			try
			{
				if (!IsViewLoaded) return;

				//Check if the keyboard is becoming visible
				var visible = notification.Name == UIKeyboard.WillShowNotification;
				//Start an animation, using values from the keyboard
				UIView.BeginAnimations("AnimateForKeyboard");
				UIView.SetAnimationBeginsFromCurrentState(true);
				UIView.SetAnimationDuration(UIKeyboard.AnimationDurationFromNotification(notification));
				UIView.SetAnimationCurve((UIViewAnimationCurve)UIKeyboard.AnimationCurveFromNotification(notification));
				var keyboardFrame = visible ? UIKeyboard.FrameEndFromNotification(notification) : UIKeyboard.FrameBeginFromNotification(notification);

				// Move up the textfield when the keyboard comes up
				if (visible)
				{
					nfloat Offset = 0;
					var Source = (configTableView.Source as DB_ConfigTableSource);

					if (Source != null)
					{
						var Height = configTableView.ContentSize.Height / 2;


							
						var Value = Source.CurrentOffestY;

						if (Value >= Height)
						{ 
							Offset = Height / 2;


							//Source.CurrentOffestY = Offset;
						}

					}

					configscroll.ContentOffset = new CGPoint(0, (keyboardFrame.Height / 2) + Offset);
					//configTableView.ScrollRectToVisible(new CoreGraphics.CGRect(0, 1, 1, 1), true);
				}
				else
					configscroll.ContentOffset = new CGPoint(0, 0);
				//NSDictionary info = notification.UserInfo;
				//Commit the animation
				UIView.CommitAnimations();
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Exception Occured in OnKeyBoardNotification method due to " + ex.Message);
			}
		}
	}
}