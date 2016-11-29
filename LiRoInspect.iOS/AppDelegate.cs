using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using UIKit;
using SQLite;
using System.IO;
using BAL;
using DAL.BAL;
using System.Diagnostics;
using Model;
using System.Threading.Tasks;
using BAL.Service;
using Connectivity.Plugin;
using DAL.DO;
using Plugin.Connectivity;

namespace LiRoInspect.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : UIApplicationDelegate
    {
        // class-level declarations
        public override UIWindow Window
        {
            get;
            set;
        }
		static AppDelegate app;
		public AppDelegate()
		{
			syncLock = new object ();
			Sync.DocumentDeleted += (object sender, EventArgs e) => {
				List<Document> documents=(e as DocumentDeletionEventArgs).documentList;
				if(documents != null && documents.Count > 0)
				{
					foreach (var doc in documents) {
						if (doc.DocumentPath != null && doc.documentDisplayName != null) {
							var documentDirectory = doc.DocumentPath;
							string fileName = System.IO.Path.Combine (documentDirectory, doc.documentDisplayName);
							System.IO.File.Delete (fileName);
						}
					}
				}
			};
		}
			
		static NSTimer autoSyncTimer;
		static double  autoSyncTimeInSeconds=0;
        private static SQLiteConnection databaseContext;

		public static SQLiteConnection DatabaseContext
        {
            get { return databaseContext; }
			set { databaseContext = value; }
        }
		public static Model.AuthenticatedUser user{get;set;}
		public static void deleteSession()
		{
            try
            {
                BAL.UserService us = new BAL.UserService(DatabaseContext);
				us.DeleteUser(DatabaseContext);
				if(user!=null)
				{
					user.UserDetails=null;
					user.DBVersion=null;
					user.IsTokenActive=false;
				}
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception Occured in deleteSession method due to " + ex.Message);
            }
		}
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            try
            {

				UIApplication.SharedApplication.IdleTimerDisabled = false;


                databaseContext = CreateDB();
				app=this;
				dataSync = new Sync (databaseContext);
                //Your View Controller Identifiers defined in Interface Builder
                String firstViewControllerIdentifier = @"LoginViewController";
                String secondViewControllerIdentifier = @"DashBoardViewController";

                //check if the key exists and its value
                LoginBAL loginBal = new LoginBAL(AppDelegate.DatabaseContext);
                Model.AuthenticatedUser authenticatedUser = loginBal.GetUserDetailsFromDatabase(AppDelegate.DatabaseContext);
                AppDelegate.user = authenticatedUser;


                bool isUserDetailsAvailableInDB = false;
				if (authenticatedUser != null && authenticatedUser.UserDetails!=null && !string.IsNullOrEmpty( authenticatedUser.UserDetails.Token) )
				{
					authenticatedUser.IsTokenActive=CheckIfTokenIsActive(authenticatedUser.UserDetails.ExpiryDate);
                    isUserDetailsAvailableInDB = true;
                }

                //check which view controller identifier should be used
				String viewControllerIdentifier = (isUserDetailsAvailableInDB && authenticatedUser.IsTokenActive)? secondViewControllerIdentifier : firstViewControllerIdentifier;

                //IF THE STORYBOARD EXISTS IN YOUR INFO.PLIST FILE AND YOU USE A SINGLE STORYBOARD
                UIStoryboard storyboard = Window.RootViewController.Storyboard;
                
				if (isUserDetailsAvailableInDB && authenticatedUser.IsTokenActive)
                {					
					startAutoSync();
					viewControllerIdentifier=secondViewControllerIdentifier;
                    DashBoardViewController dashBoardViewController = storyboard.InstantiateViewController("DashBoardViewController") as DashBoardViewController;
                    dashBoardViewController.IsFirstLogin = authenticatedUser.IsFirstTimeLoggedIn;
                    DashBoardViewController.Token = authenticatedUser.UserDetails.Token;
                    DashBoardViewController.UserName = string.Concat(authenticatedUser.UserDetails.FirstName, " ", authenticatedUser.UserDetails.LastName);
                }
				else if (isUserDetailsAvailableInDB && !authenticatedUser.IsTokenActive)
				{
					viewControllerIdentifier=firstViewControllerIdentifier;
					deleteSession();
					UIAlertView alert = new UIAlertView (@"Alert", @"Token Expired", null, NSBundle.MainBundle.LocalizedString ("OK", "OK"));
					alert.Show ();
				}else{
					viewControllerIdentifier=firstViewControllerIdentifier;
					deleteSession();
				}
				//instantiate the view controller
				UIViewController presentedViewController = storyboard.InstantiateViewController(viewControllerIdentifier);
				if(presentedViewController is DashBoardViewController)
				{
					(presentedViewController as DashBoardViewController).IsIntiatedFromAppDelegate=true;
				}
				//UIApplication.SharedApplication.IdleTimerDisabled = true;
				UINavigationController navController = (UINavigationController)Window.RootViewController;
                navController.PushViewController(presentedViewController, false);

				return true;
            }
            catch (Exception ex)
            {
				return true;
            }

        }

        // This method is invoked when the application is about to move from active to inactive state.
        // OpenGL applications should use this method to pause.
        public override void OnResignActivation(UIApplication application)
        {
        }

        // This method should be used to release shared resources and it should store the application state.
        // If your application supports background exection this method is called instead of WillTerminate
        // when the user quits.
        public override void DidEnterBackground(UIApplication application)
        {
			//application.IdleTimerDisabled = true;

        }

        /// This method is called as part of the transiton from background to active state.
        public override void WillEnterForeground(UIApplication application)
        {
			UIApplication.SharedApplication.IdleTimerDisabled = false;
			if (AppDelegate.user != null && AppDelegate.user.UserDetails != null && AppDelegate.user.UserDetails.ID > 0)
			{
				bool IsActivetoken = checkTokenExpiry();
				if (!IsActivetoken)
				{
					stopAutoSync();
					deleteSession();
					UIAlertView alert = new UIAlertView("Invalid Token", "Token Expired.", null, NSBundle.MainBundle.LocalizedString("OK", "OK"));
					alert.Show();
					ClearMemory();
				}
				else {
					stopAutoSync();

					autoSync();
					startAutoSync();

				}
			}
        }

		public void ClearMemory()
		{
			UIStoryboard storyboard = Window.RootViewController.Storyboard;
			UINavigationController navController = (UINavigationController)Window.RootViewController;
			if (navController != null && navController.ViewControllers!=null && navController.ViewControllers.Count ()>0) {
				List<UIViewController> viewcontrollers= new List<UIViewController> ();
				viewcontrollers = navController.ViewControllers.ToList();
				viewcontrollers.RemoveAll (i => i is UIViewController);
				navController.ViewControllers = viewcontrollers.ToArray();
				LoginViewController loginViewController = storyboard.InstantiateViewController ("LoginViewController") as LoginViewController;
				navController.PushViewController(loginViewController, false);
			}
		}

		public static bool checkTokenExpiry()
		{
			LoginBAL loginBal = new LoginBAL(AppDelegate.DatabaseContext);
			Model.AuthenticatedUser authenticatedUser = loginBal.GetUserDetailsFromDatabase(AppDelegate.DatabaseContext);
			AppDelegate.user = authenticatedUser;

			bool isUserActiveToken = false;
			if (authenticatedUser != null && authenticatedUser.UserDetails != null && !string.IsNullOrEmpty (authenticatedUser.UserDetails.Token)) 
			{
				authenticatedUser.IsTokenActive = CheckIfTokenIsActive (authenticatedUser.UserDetails.ExpiryDate);
				if (authenticatedUser.IsTokenActive) 
				{
					isUserActiveToken = true;
				}
			}
			return isUserActiveToken;
		}

        /// This method is called when the application is about to terminate. Save data, if needed. 
        public override void WillTerminate(UIApplication application)
        {
        }

        private SQLiteConnection CreateDB()
        {            
            var sqliteFilename = "LiRo.db3";
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal); // Documents folder
            string libraryPath = Path.Combine(documentsPath, "..", "Library"); // Library folder
            var path = Path.Combine(libraryPath, sqliteFilename);

			if (!System.IO.File.Exists(path))
            {
                if (System.IO.File.Exists("./Utility/Database/LiRoDB"))
                {
                    using (var br = new BinaryReader(File.OpenRead("./Utility/Database/LiRoDB")))
                    {
                        using (var bw = new BinaryWriter(new FileStream(path, FileMode.Create)))
                        {
                            byte[] buffer = new byte[2048];
                            int length = 0;
                            while ((length = br.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                bw.Write(buffer, 0, length);
                            }
                        }
                    }
                }
            }
			databaseContext = new SQLiteConnection(path, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.FullMutex| SQLiteOpenFlags.SharedCache, false);
            return databaseContext;
        }
          
		private static bool CheckIfTokenIsActive (DateTime expiryDate)
		{
			int isTokenActive = DateTime.Compare(DateTime.Now, expiryDate);

			if (isTokenActive != 1)
				return true;
			else
				return false;
		}

		public static double getAutoSyncInterval()
		{
			double autoSyncTime;
			List<UserSetting> userSetting = new UserSettingService (AppDelegate.DatabaseContext).GetUserSettings ();
			string strAutoIntervalTime = userSetting.Where (u => u.SettingName.Trim () == "Auto Sync Interval(in Secs)").SingleOrDefault ().SettingValue;
			Double.TryParse (strAutoIntervalTime, out autoSyncTime);
			return autoSyncTime;
		}
		public static void startAutoSync()
		{
			autoSyncTimeInSeconds = getAutoSyncInterval ();
			if (autoSyncTimeInSeconds > 0) {
				UIApplication.SharedApplication.InvokeOnMainThread(delegate
					{

						UIApplication.SharedApplication.IdleTimerDisabled = true;
					});
				autoSyncTimer = NSTimer.CreateRepeatingScheduledTimer (autoSyncTimeInSeconds, delegate {
					autoSync ();
				});
			}
		}
		public static void stopAutoSync()
		{
			if (autoSyncTimer != null) {
				autoSyncTimer.Invalidate ();
				autoSyncTimer.Dispose ();
			}
		}
		public static Sync dataSync;
		static object syncLock;
		public static void  autoSync()
		{
			bool IsActivetoken=checkTokenExpiry ();
			if (!IsActivetoken) 
			{
				stopAutoSync ();
				deleteSession ();
				UIAlertView alert = new UIAlertView ("Invalid Token", "Token Expired.", null, NSBundle.MainBundle.LocalizedString ("OK", "OK"));
				alert.Show ();
				if (app != null) {
					app.ClearMemory ();
				}
			} 
			else 
			{
				
				if (CrossConnectivity.Current.IsConnected) {
					lock (syncLock) {	

						UIApplication.SharedApplication.InvokeOnMainThread(delegate
					{

						UIApplication.SharedApplication.IdleTimerDisabled = true;
					});

						Task.Run (() => dataSync.syncData ());
					}

				}

			}
		}
    }
}