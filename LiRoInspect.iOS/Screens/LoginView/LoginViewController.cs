using System;
using Foundation;
using UIKit;
using BAL;
using DAL.BAL;
using Connectivity.Plugin;
using CoreGraphics;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Linq;
using Plugin.Connectivity;

namespace LiRoInspect.iOS
{

	public class LoginTextFieldDelegate : UITextFieldDelegate
	{
		LoginViewController loginController;
		public LoginTextFieldDelegate(LoginViewController loginController)
		{
			this.loginController = loginController;
		}
		//        string username = txtUserName.Text;
		//        string password = txtPassword.Text;
		public EventHandler<UITextField> OnShouldReturn;

		/// <summary>
		/// Called when return button is clicked on keyboard
		/// </summary>
		/// <returns><c>true</c>, if return was shoulded, <c>false</c> otherwise.</returns>
		/// <param name="textField">Text field.</param>
		public override bool ShouldReturn(UITextField textField)
		{
			if (OnShouldReturn != null) 
				OnShouldReturn (this, textField);

			if(textField.SecureTextEntry == true)
				loginController.callButtonMethod ();

			return true;
		}
	}

	public partial class LoginViewController : BaseViewController
	{
		Model.AuthenticatedUser authenticatedUser;

		public LoginViewController (IntPtr handle) : base (handle)
		{

		}
			
		/// <summary>
		/// login button method is called from here 
		/// </summary>
		public void callButtonMethod()
		{
			btnLogin_TouchUpInside (null);
		}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();

			// Release any cached data, images, etc that aren't in use.
		}
			
		/// <summary>
		/// View load starts. Date label is set
		/// </summary>
		public override void LoadView()
		{
			base.LoadView ();

			lblDate.Text = DateTime.Now.ToString("MMM dd, yyyy");

		}
			 
		/// <summary>
		///  View loading is done.
		///  Keyboard notification registration to hide and show keyboard when tap on textfield
		/// </summary>
		public override void ViewDidLoad()
		{
			try
			{
				base.ViewDidLoad();
				//UIApplication.SharedApplication.IdleTimerDisabled = true;
				this.NavigationController.NavigationBarHidden = true;
				progressView.Hidden=true;

				registerForKeyboardNotifications();

				var textDelegate = new LoginTextFieldDelegate(this)
				{
					OnShouldReturn = (sender, textField) =>
					{
						if (textField.Tag == txtUserName.Tag)
							txtPassword.BecomeFirstResponder();
						else if (textField.Tag == txtPassword.Tag)
							txtPassword.ResignFirstResponder();
					}
				};

				txtUserName.Delegate = textDelegate;
				txtPassword.Delegate = textDelegate;
			}

			catch (Exception ex)
			{
				Debug.WriteLine("Exception Occured in LoginView ViewDidLoad method due to " + ex.Message);
			}
		}
			
		/// <summary>
		/// Called when view appears. Color and shadow of headerView is set
		/// </summary>
		/// <param name="animated">If set to <c>true</c> animated.</param>
		public override void ViewWillAppear(bool animated)
		{
			try
			{
				base.ViewWillAppear(animated);
				this.NavigationController.NavigationBarHidden = true;
				this.InvokeOnMainThread(delegate
					{
						headerView.BackgroundColor = UIColor.FromPatternImage(UIImage.FromFile("HeaderViewBackground.png"));
					});

				headerView.Layer.ShadowColor = UIColor.FromRGB(142, 187, 223).CGColor;
				headerView.Layer.ShadowOpacity = 0.8f;
				headerView.Layer.ShadowRadius = 2.0f;
				headerView.Layer.ShadowOffset = new System.Drawing.SizeF(0f, 3f);
				headerView.Layer.MasksToBounds = false;
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Exception Occured in ViewWillAppear method due to " + ex.Message);
			}
		}

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);

		}


		/// <summary>
		/// Error response method to show error messages as per error code.
		/// </summary>
		/// <param name="code">Code.</param>
		private void ShowErrorResponse(int code)
		{
			switch (code) {
			case 0:
			case 1:
				break;
			case 999:
				UIAlertView alert1 = new UIAlertView (@"Alert", @"Invalid Login", null, NSBundle.MainBundle.LocalizedString ("OK", "OK"));
				alert1.Show ();
				break;
			case 888:
				UIAlertView alert11 = new UIAlertView (@"Alert", @"Session Timeout", null, NSBundle.MainBundle.LocalizedString ("OK", "OK"));
				alert11.Show ();
				break;
			case 777:
				UIAlertView alert2 = new UIAlertView (@"Alert", @"Invalid Data", null, NSBundle.MainBundle.LocalizedString ("OK", "OK"));
				alert2.Show ();
				break;
			case 666:
				UIAlertView alert3 = new UIAlertView (@"Alert", @"Invalid Token", null, NSBundle.MainBundle.LocalizedString ("OK", "OK"));
				alert3.Show ();
				break;
			case 555:
				UIAlertView alert4 = new UIAlertView (@"Alert", @"User Not Found", null, NSBundle.MainBundle.LocalizedString ("OK", "OK"));
				alert4.Show ();
				break;
			case 444:
				UIAlertView alert5 = new UIAlertView (@"Alert", @"Not Found", null, NSBundle.MainBundle.LocalizedString ("OK", "OK"));
				alert5.Show ();
				break;
			case 333:
				UIAlertView alert6 = new UIAlertView (@"Alert", @"Validation Failed", null, NSBundle.MainBundle.LocalizedString ("OK", "OK"));
				alert6.Show ();
				break;
			case -1:
				UIAlertView alert7 = new UIAlertView(@"Alert", @"Http Error", null,NSBundle.MainBundle.LocalizedString("OK", "OK"));
				alert7.Show();
				break;
			case -2:
				UIAlertView alert8 = new UIAlertView(@"Alert", @"Timed Out", null,NSBundle.MainBundle.LocalizedString("OK", "OK"));
				alert8.Show();
				break;
			case -3:
				UIAlertView alert9 = new UIAlertView(@"Alert", @"HTTP Exception occurred", null,NSBundle.MainBundle.LocalizedString("OK", "OK"));
				alert9.Show();
				break;
			default:
				UIAlertView alert10 = new UIAlertView(@"Alert", authenticatedUser.Result.message, null,NSBundle.MainBundle.LocalizedString("OK", "OK"));
				alert10.Show();
				break;
			}
		}

		/// <summary>
		/// Updates the progress.
		/// </summary>
		/// <param name="progress">Progress.</param>
		private void UpdateProgress(float progress)
		{
			
			this.InvokeOnMainThread(()=>{progressView.Progress = progress;dataProgressLabel.Text=(progress*100) +"%";});
		}
			
		/// <summary>
		/// Login button clicked method.
		/// Network Connectivity is checked.
		/// Authentication
		/// DB version is checked with the server and db updation happens according
		/// </summary>
		/// <param name="sender">Sender.</param>

		async partial void btnLogin_TouchUpInside (UIButton sender)
		{
			this.ResignFirstResponder();

			this.txtUserName.ResignFirstResponder();
			this.txtPassword.ResignFirstResponder();

			try
			{
				this.ResignFirstResponder();

				if (CrossConnectivity.Current.IsConnected == false)
				{
					if (CrossConnectivity.Current.IsConnected == false)
					{
						UIAlertView alert = new UIAlertView(@"Alert", @"You are not connected to the network", null, NSBundle.MainBundle.LocalizedString("OK", "OK"));
						alert.Show();
						return;
					}
				}

				if (txtUserName.Text.Length <= 0 || txtPassword.Text.Length <= 0)
				{
					UIAlertView alert = new UIAlertView(@"Alert", @"Please fill in both username and password", null, NSBundle.MainBundle.LocalizedString("OK", "OK"));
					alert.Show();
					return;
				}

				if(txtUserName.Text.ToUpper()==Constants.AdminUserID && txtPassword.Text.ToUpper()==Constants.AdminPassword)
				{
					config con = this.Storyboard.InstantiateViewController ("config") as config;
					this.NavigationController.PushViewController(con,true);
				}
				else if(txtUserName.Text.ToUpper()==Constants.AdminUserID && txtPassword.Text.ToUpper()!=Constants.AdminPassword)
				{
					UIAlertView alert = new UIAlertView (@"Alert", @"Invalid Username Or Password", null, NSBundle.MainBundle.LocalizedString ("OK", "OK"));
					alert.Show ();
				}
				else
				{
					if (AppDelegate.DatabaseContext != null)
					{
						//AppDelegate del=new AppDelegate();
						loginScrollView.ContentOffset = new CGPoint(0, 0);


						LoadOverLayPopup();
						//getter method should return if we should allow going to dashboard or not

						string username=txtUserName.Text;
						string password=txtPassword.Text;
						authenticatedUser=await Task.Run(()=>LoginToService(username,password));
						HideOverLay();

						    
							if (authenticatedUser.Result!=null && (authenticatedUser.Result.code == 0 || authenticatedUser.Result.code == 1) && authenticatedUser.UserDetails != null && authenticatedUser.IsTokenActive)
							{
								AppDelegate.user = authenticatedUser;
								using(MasterDataUpdateService masterDataUpdateService=new MasterDataUpdateService(AppDelegate.DatabaseContext))
								{
									string dbVersion="1.0";
									using (UserSettingService usrSetting = new UserSettingService (AppDelegate.DatabaseContext)) 
										{
											var dbVersions =usrSetting.GetUserSettings ().Where (u => u.SettingName == "DBVersion");
											if(dbVersions!=null && dbVersions.Count()>0)
											{
												dbVersion =dbVersions.FirstOrDefault().SettingValue;
											}
										}
										var AppDbVersion = string.IsNullOrEmpty(dbVersion) ?1:float.Parse (dbVersion);
										var serverDBVersion =string.IsNullOrEmpty(authenticatedUser.DBVersion) ?1:float.Parse (authenticatedUser.DBVersion);       

									if(AppDbVersion < serverDBVersion)
									{
										UIAlertView alert1 = new UIAlertView (@"Alert", @"Master Data update available,DB update is going to start.", null, NSBundle.MainBundle.LocalizedString ("Ok", "Ok"));
										alert1.Show ();

										alert1.Clicked += async (DBsender, buttonArgs) =>  {
											masterDataUpdateService.progressDelegate=this.UpdateProgress;
												this.ResignFirstResponder();
												this.View.UserInteractionEnabled=false;
												progressView.Hidden=false;
											dataProgressLabel.Hidden=false;
												CGAffineTransform transform =  CGAffineTransform.MakeScale(1.0f,6.0f);
												progressView.Transform = transform;
												MasterDataUpdateStatus resultMasterData=await Task.Run(()=>masterDataUpdateService.UpdateDatabaseService(authenticatedUser.UserDetails.Token));
												progressView.Hidden=true;
												dataProgressLabel.Hidden=true;
												if(resultMasterData != null){
													UIAlertView alert = new UIAlertView (@"Alert",resultMasterData.ErrorMessage , null, NSBundle.MainBundle.LocalizedString ("OK", "OK"));
													alert.Show ();
												 if(resultMasterData.Result ==false)
													{
														this.View.UserInteractionEnabled=true;
														AppDelegate.deleteSession();
													}
												else
													{
														this.View.UserInteractionEnabled=true;
														MoveToDashBoard(authenticatedUser);
													}
										}
										else
										{
											this.View.UserInteractionEnabled=true;
										}
										};
									}
									else
									{
										MoveToDashBoard(authenticatedUser);
									}
								}

							}
						else if(authenticatedUser.Result!=null && (authenticatedUser.Result.code == 0 || authenticatedUser.Result.code == 1) && authenticatedUser.UserDetails != null && !authenticatedUser.IsTokenActive)
						{
							UIAlertView alert1 = new UIAlertView (@"Invalid Token", @"Token Expired", null, NSBundle.MainBundle.LocalizedString ("OK", "OK"));
							alert1.Show ();
						}
						else
						{
							this.ShowErrorResponse (authenticatedUser.Result.code);
						}
					}
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Exception Occured in btnLogin_TouchUpInside  method due to " + ex.Message);

			}
			finally
			{
				progressView.Hidden=true;
				dataProgressLabel.Hidden=true;
				this.View.UserInteractionEnabled=true;
			}
		}

		/// <summary>
		/// On authentication completion, user moves to Dashboard
		/// </summary>
		/// <param name="authenticatedUser">Authenticated user.</param>
		private void MoveToDashBoard(Model.AuthenticatedUser  authenticatedUser)
		{
			
			if (authenticatedUser.Result.code == 0 || authenticatedUser.Result.code == 1)
			{
				AppDelegate.startAutoSync();
				DashBoardViewController dashBoardViewController = this.Storyboard.InstantiateViewController("DashBoardViewController") as DashBoardViewController;
				dashBoardViewController.IsIntiatedFromAppDelegate=true;
				dashBoardViewController.IsFirstLogin = authenticatedUser.IsFirstTimeLoggedIn;
				DashBoardViewController.Token = authenticatedUser.UserDetails.Token;
				DashBoardViewController.UserName = string.Concat(authenticatedUser.UserDetails.FirstName, " ", authenticatedUser.UserDetails.LastName);

				this.NavigationController.PushViewController(dashBoardViewController, true);
			}
			else if (authenticatedUser.UserDetails != null && !authenticatedUser.IsTokenActive)
			{
				UIAlertView alert = new UIAlertView (@"Alert", @"Invalid Token", null, NSBundle.MainBundle.LocalizedString ("OK", "OK"));
				alert.Show ();
			}
		}

		/// <summary>
		/// Connects to the BAL layer to get the authenticated user name  
		/// </summary>
		/// <returns>The service.</returns>
		/// <param name="username">Username.</param>
		/// <param name="password">Password.</param>
	
		Model.AuthenticatedUser LoginToService(string username,string password)
		{
			LoginBAL loginBal = new LoginBAL (AppDelegate.DatabaseContext);
			Model.AuthenticatedUser user=loginBal.GetUserDetailsFromService( username, password);
			return user;
		}

		/// <summary>
		/// Registers for keyboard notifications.
		/// </summary>
		public void registerForKeyboardNotifications()
		{
			NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillHideNotification, OnKeyboardNotification);
			NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillShowNotification, OnKeyboardNotification);
		}

		/// <summary>
		/// Raises the keyboard notification event.
		/// </summary>
		/// <param name="notification">Notification.</param>
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
					loginScrollView.ContentOffset = new CGPoint(0, keyboardFrame.Height / 2);
				}
				else
					loginScrollView.ContentOffset = new CGPoint(0, 0);

				//Commit the animation
				UIView.CommitAnimations();
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Exception Occured in OnKeyBoardNotification method due to " + ex.Message);
			}
		}


		#region implemented abstract members of BaseViewController
		public override void updateNotifyCount (int txt, bool fromSync)
		{
//			throw new NotImplementedException ();
		}
		public override void updateSyncCount (int txt)
		{
//			throw new NotImplementedException ();
		}
		#endregion
	}
}