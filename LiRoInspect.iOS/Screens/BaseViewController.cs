using System;
using System.Collections.Generic;
using Foundation;
using UIKit;
using CoreGraphics;
using BAL;
using System.Threading.Tasks;
using Model;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using Connectivity.Plugin;
using System.Diagnostics;
using System.IO;
using Plugin.Connectivity;

namespace LiRoInspect.iOS
{
	//public delegate void UpdateTextInCtrl(int text);
	public abstract partial class BaseViewController : UIViewController,IDisposable
	{
		LoadingOverlay _loadPop;
		Sync syn = new Sync (AppDelegate.DatabaseContext);
		object locker = new object ();
		public static List<Notification> syncNotifications = new List<Notification> ();
		protected bool IsNetworkConnected=CrossConnectivity.Current.IsConnected;
		public BaseViewController (IntPtr handle) : base (handle)
		{if (syncNotifications.Count < 1) {
			
				syncNotifications.Add (new Notification (){ message = "No Notifications", inspectionDetail = "", seen = true });
			}
		}

		public BaseViewController () : base ("BaseViewController", null)
		{
		}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
		}

		public bool LogOut()
		{
			int buttonClicked = -1;
			UIAlertView alert1 = new UIAlertView (@"Alert", @"Are you sure you want to log out?", null, NSBundle.MainBundle.LocalizedString ("Cancel", "Cancel"), NSBundle.MainBundle.LocalizedString ("OK", "OK"));
			alert1.Show ();

			alert1.Clicked += (sender, buttonArgs) =>  {buttonClicked = (int)buttonArgs.ButtonIndex ;};

			// Wait for a button press.
			while (buttonClicked == -1)
			{
				NSRunLoop.Current.RunUntil(NSDate.FromTimeIntervalSinceNow (0.5));
			}

			if (buttonClicked == 1)
			{
				AppDelegate.deleteSession ();
				ClearMemory ();
				AppDelegate.stopAutoSync ();
				LoginViewController loginViewController = this.Storyboard.InstantiateViewController ("LoginViewController") as LoginViewController;
				this.NavigationController.PushViewController (loginViewController, false);
				return true;
			}
			return false;

		}
		public void ClearMemory()
		{
			if (this.NavigationController != null && this.NavigationController.ViewControllers!=null && this.NavigationController.ViewControllers.Count ()>0) {
				List<UIViewController> viewcontrollers= new List<UIViewController> ();
				viewcontrollers = this.NavigationController.ViewControllers.ToList();
				viewcontrollers.RemoveAll (i => i is UIViewController);
				viewcontrollers.Add (this);
				this.NavigationController.ViewControllers = viewcontrollers.ToArray ();
			}
		}

		public void LoadOverLayPopup()
		{
			var bounds = UIScreen.MainScreen.Bounds; // portrait bounds
			if (UIApplication.SharedApplication.StatusBarOrientation == UIInterfaceOrientation.LandscapeLeft || UIApplication.SharedApplication.StatusBarOrientation == UIInterfaceOrientation.LandscapeRight) {
				bounds.Size = new CGSize(bounds.Size.Width,bounds.Size.Height);
			}
			// show the loading overlay on the UI thread using the correct orientation sizing
			this._loadPop = new LoadingOverlay (bounds);
			this.View.AddSubview ( this._loadPop );
		}
		public void HideOverLay()
		{
			this._loadPop.Hide ();
			this._loadPop.RemoveFromSuperview ();
		
		}
		#region sync & Notify
		public abstract void updateNotifyCount (int txt, bool fromSync=false);
		public abstract void updateSyncCount (int txt);

		public async Task syncData(bool updateUI=true)
		{
			if (CrossConnectivity.Current.IsConnected) {
				Task task = Task.Run (() => triggerSyncService ());
				await task;
				if (updateUI) {
					syncInit ();
				}
			}
		}
		public void triggerSyncService()
		{			
			updateSyncNotificationn notDel = updateStatusSync;
			UpdateTextInCtrl txtUp = updateNotifyCount;
			//syn.syncData (notDel,txtUp);
		}
		public void updateStatusSync(string message,string inspectinDetailTxt)
		{
			if (syncNotifications.Count>0 && syncNotifications [0].message == "No Notifications") {
				syncNotifications.RemoveAt (0);
			}
			syncNotifications.Add (new Notification(){message=message,inspectionDetail=inspectinDetailTxt,seen=false});
		}
		public async  void syncInit()
		{
			Task<int> task = Task.Run (()=>getSyncCountFromService());
			int count=await task;
			updateSyncCount(count);
		}
		public int getSyncCountFromService()
		{
			return syn.getPendingSyncCount ();
		}
		public void clearSeen()
		{
			syncNotifications.ForEach (n => n.seen = true);
			updateNotifyCount (0, false);
		}
		public void NotifyCount()
		{
			var countOfUnseen=syncNotifications.Where (n => n.seen == false);
			if (countOfUnseen.Any ()) {
				updateNotifyCount (countOfUnseen.Count());
			} else {
				updateNotifyCount (0);
			}

		}

		public override Task DismissViewControllerAsync (bool animated)
		{
			return base.DismissViewControllerAsync (animated);
		}
		#endregion


		#region Idispose implementation

		// Flag: Has Dispose already been called?
		bool disposed = false;

		// Public implementation of Dispose pattern callable by consumers.
		public void Dispose()
		{ 
			Dispose(true);
			GC.SuppressFinalize(this);           
		}

		// Protected implementation of Dispose pattern.
		protected virtual void Dispose(bool disposing)
		{
			if (disposed)
				return; 

			if (disposing) {
				// Free any other managed objects here.
				//
			}

			// Free any unmanaged objects here.
			//
			disposed = true;
		}

		~BaseViewController()
		{
			Dispose(false);
		}

		#endregion


		#region Camera Common Functions
		public List<UIImageView> BaseRestructureImages (List<UIImage> scrollImages)
		{
			try {
				nfloat h = 100.0f;
				nfloat w = 100.0f;
				nfloat padding = 10.0f;
				List<UIImageView> scrollImageCollection = new List<UIImageView> ();
				for (int i = 0; i < scrollImages.Count; i++) {
					UIImageView imageView = new UIImageView ();
					imageView.Frame = new CGRect (padding * (i + 1) + (i * w), 0, w, h);
					imageView.Image = scrollImages [i];

					Action tapAction = new Action (
						                   delegate {
							//imageView = HighlightSelectedImage (imageView);
							ImagePeviewViewController imagePreviewController = this.Storyboard.InstantiateViewController ("ImagePeviewViewController") as ImagePeviewViewController;
							imagePreviewController.ModalPresentationStyle = UIModalPresentationStyle.FullScreen;
							imagePreviewController.previewImage = imageView;
							this.PresentViewController (imagePreviewController, false, null);
						});

					UITapGestureRecognizer tap = new UITapGestureRecognizer ();
					tap.AddTarget (tapAction);
					imageView.AddGestureRecognizer (tap);
					imageView.UserInteractionEnabled = true;

					scrollImageCollection.Add (imageView);

				}

				return scrollImageCollection;
			} catch (Exception ex) {
				Debug.WriteLine ("Exception Occured in BaseRestructureImages method due to " + ex.Message);
				return new List<UIImageView> ();
			}
		}


		/// <summary>
		///  Conversion from Images to byte array.
		/// </summary>
		/// <returns>The to byte array.</returns>
		/// <param name="image">Image.</param>
			public byte[] ImageToByteArray (UIImage image)
			{
				Stream stream = image.AsJPEG ().AsStream ();
				byte[] array = ToByteArray (stream);
				return array;
			}

			public  byte[] ToByteArray (Stream stream)
			{
				stream.Position = 0;
				byte[] buffer = new byte[stream.Length];
				for (int totalBytesCopied = 0; totalBytesCopied < stream.Length;)
					totalBytesCopied += stream.Read (buffer, totalBytesCopied, Convert.ToInt32 (stream.Length) - totalBytesCopied);
				return buffer;
			}

		/// <summary>
		/// Conversion from byte array to image
		/// </summary>
		/// <returns>The array to image.</returns>
		/// <param name="imageArray">Image array.</param>
			public UIImage ByteArrayToImage (byte[] imageArray)
			{
				lock (locker) {
					UIImage img = new UIImage ();
					img = UIImage.LoadFromData (new NSData (Convert.ToBase64String (imageArray), NSDataBase64DecodingOptions.IgnoreUnknownCharacters));
					if (img != null) {
						UIImage resizedImage = UIImage.LoadFromData (img.AsJPEG (0.0f));
						return resizedImage;
					}
					return img;
				}
			}

		/// <summary>
		/// The image limit.
		/// </summary>
		private int _imageLimit = Constants.INSPECTION_MAXIMAGE;
			public int ImageLimit
			{
				get{
					return _imageLimit;
				}set{
					_imageLimit = value;
				}
			}
		}
	 


		#endregion

}

