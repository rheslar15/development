using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;
using System.Collections.Generic;
using CoreGraphics;

using BAL;
using Connectivity.Plugin;
using System.Threading.Tasks;
using System.Diagnostics;
using BAL.Service;
using Model;
using Plugin.Connectivity;

namespace LiRoInspect.iOS
{ 
	
	public partial class DashBoardViewController : BaseViewController
	{
		InspectionScheduleSource inspectionScheduleSource;
		static List<Model.Inspection> inspectionDetailInfo;
		InspectionTransactionService inspectionTransactionService;
		public static string Token{ get; set;}
		public bool IsFirstLogin{ get; set;}
		public bool IsIntiatedFromAppDelegate{ get; set; }
		public DashBoardViewController (IntPtr handle) : base (handle)
		{
			inspectionDetailInfo = new List<Model.Inspection> ();
		}

		/// <summary>
		/// Views the did load.
		/// </summary>
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
		
			InitialiseDashBoard();
		}

		/// <summary>
		/// Initialises the dash board.
		/// </summary>
		private async void InitialiseDashBoard ()
		{
			try{
				//UIApplication.SharedApplication.IdleTimerDisabled = true;
				this.NavigationController.NavigationBarHidden = true;
				inspectionTransactionService = new InspectionTransactionService (AppDelegate.DatabaseContext);
				inspectionDetailInfo=await Task.Run(()=> inspectionTransactionService.GetServiceInspections (CrossConnectivity.Current.IsConnected ,Token, IsFirstLogin));
				ShowErrorResponse();

				if(IsIntiatedFromAppDelegate)
				{
					IsIntiatedFromAppDelegate = false;
					//sync and notify
					syncInit ();
					NotifyCount ();

					//btnLogout.Enabled=false;

					syncData();
				}
				else
				{
					syncInit ();
				}	
				if(!CrossConnectivity.Current.IsConnected && IsFirstLogin)
				{
					UIAlertView alert = new UIAlertView(@"Alert", @"To get new assigned inspections please connect to the network!", null,NSBundle.MainBundle.LocalizedString("OK", "OK"));
					alert.Show();
				}



				List<DashBoardLeftTableItem> tableItems = new List<DashBoardLeftTableItem> () {
					new DashBoardLeftTableItem () { Heading = "Inspections", 
						SubHeading = "Today", 
						ImageName = "FolderIcon.png", 
						HeaderType =HeaderType.Today
					},
					new DashBoardLeftTableItem () {	Heading = "Inspections",
						SubHeading = "Upcoming",
						ImageName = "FolderIcon.png",
						HeaderType = HeaderType.Upcoming
					}
				};

				lblSyncNumber.Layer.CornerRadius = lblSyncNumber.Frame.Height/2;
				lblSyncNumber.ClipsToBounds = true;

				LblNotifyNbr.Layer.CornerRadius = LblNotifyNbr.Frame.Height/2;
				LblNotifyNbr.ClipsToBounds = true;
				inspectionScheduleSource= new InspectionScheduleSource (tableItems,inspectionDetailInfo);
				DashboardView.Source = inspectionScheduleSource;
				DashboardView.ReloadData();
				// Header View Events
				inspectionScheduleSource.DashBoardRowSelected += UpdateRightTableView;
				btnHome.TouchUpInside-= BtnHome_TouchUpInside;
				btnHome.TouchUpInside+= BtnHome_TouchUpInside;
				btnLogout.TouchUpInside -= BtnLogout_TouchUpInside;
				btnLogout.TouchUpInside += BtnLogout_TouchUpInside;
				btnSync.TouchUpInside -= BtnSync_TouchUpInside;
				btnSync.TouchUpInside+= BtnSync_TouchUpInside;
				reportBtn.TouchUpInside -= ReportBtn_TouchUpInside;
				reportBtn.TouchUpInside += ReportBtn_TouchUpInside;

				btnNotify.TouchUpInside -= BtnNotify_TouchUpInside;
				btnNotify.TouchUpInside += BtnNotify_TouchUpInside;

				ProjectDetailView.Hidden = true;
				ProjectDetailView.TableFooterView = new UIView (new CGRect (0, 0, 0, 0));
				lblUserName.Text = UserName;
				lblDate.Text = DateTime.Today.Date.ToString ("MMM dd, yyyy");
			
			}catch(Exception ex) {
				HideOverLay();
				ShowErrorResponse();
			}
		}


		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear(animated);
			this.NavigationController.NavigationBarHidden = true;
			AppDelegate.dataSync.syncProgress +=  syncProgressChange;
			LoadOverLayPopup();
			SetLHSStyle ();
			//HideOverLay();
		}

	 public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);
			HideOverLay();
		}
		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);
			AppDelegate.dataSync.syncProgress -=  syncProgressChange;
		}
		 void BtnLogout_TouchUpInside(object sender,EventArgs e)
		{
			LogOut ();
		}

		void BtnHome_TouchUpInside (object sender, EventArgs e)
		{
			IsFirstLogin = false;
			this.ViewDidLoad ();
		}

		// Update the Project Detail right View

		public void UpdateRightTableView(object sender, EventArgs e)
		{
            try
            {
                var arg = e as DashBoardEventArgs;

                if (arg != null && arg.RowType == RowType.Content)
                {
                    ProjectDetailView.Source = new DB_DetailTableSource(arg.InspectionDetail);
                    ProjectDetailView.Hidden = false;
                    ProjectDetailView.ReloadData();

                }
                else if (arg != null && arg.RowType == RowType.Header)
                {
                    ProjectDetailView.Hidden = true;
                }
                else if (arg != null && arg.RowType == RowType.None)
                {
					if(arg.InspectionDetail.IsFinalise>0)
					{
						UIAlertView alert = new UIAlertView (@"Alert", @"Inspection is Finalise", null, NSBundle.MainBundle.LocalizedString ("OK", "OK"));
						alert.Show ();
					}
					else{
                    arg.InspectionDetail.RepresentativeName = UserName;

						if (!IsFirstLogin && arg.InspectionDetail.IsFinalise == 0)
						{
							arg.InspectionDetail.InspectionStarted = 1;
						}

					InspectionViewController inspectionViewController = this.Storyboard.InstantiateViewController("InspectionViewController") as InspectionViewController;
					inspectionViewController.InspectionData = arg.InspectionDetail;
                    this.NavigationController.PushViewController(inspectionViewController, true);
					}
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception Occured in UpadteRightTableView method due to " + ex.Message);
            }
		}

		private static string userName="";
		public static string UserName{
			get{
				return userName;
			}
			set {
				userName = value;
			}
			}

		private void SetLHSStyle()
		{
			//Set Header View Style
			this.InvokeOnMainThread (delegate {
				var backgroundImage=new UIImage ("HeaderViewBackground.png");
				headerView.BackgroundColor = UIColor.FromPatternImage (backgroundImage);
			});
			headerView.ContentMode = UIViewContentMode.ScaleToFill;
			headerView.Layer.ShadowColor = UIColor.FromRGB(142,187,223).CGColor;
			headerView.Layer.ShadowOpacity = 0.8f;
			headerView.Layer.ShadowRadius = 2.0f;
			headerView.Layer.ShadowOffset = new System.Drawing.SizeF(0f, 3f);
			headerView.Layer.MasksToBounds = false;

			//Set Dashboard Left View Style

			DashboardView.BackgroundColor=UIColor.Clear;
			UIImage backgroundImageLHS = new UIImage ("left-bg-1.png");
			DashboardLHSView.BackgroundColor=UIColor.FromPatternImage (backgroundImageLHS);


			//Set Dashboard Left Table View Style
			DashboardLHSView.Layer.ShadowColor = UIColor.DarkGray.CGColor;
			DashboardLHSView.Layer.ShadowOpacity = 0.8f;
			DashboardLHSView.Layer.ShadowRadius = 2.0f;
			DashboardLHSView.Layer.ShadowOffset = new System.Drawing.SizeF(0f, 3f);
			//DashboardLHSView.Layer.ShouldRasterize = true;
			DashboardLHSView.Layer.MasksToBounds = false;

	}

		private void ShowErrorResponse()
		{
			if (inspectionTransactionService.ServiceResonse != null && inspectionTransactionService.ServiceResonse.result != null) {
				var res = inspectionTransactionService.ServiceResonse;
				switch (res.result.code) {
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
					UIAlertView alert7 = new UIAlertView (@"Alert", @"Http Error", null, NSBundle.MainBundle.LocalizedString ("OK", "OK"));
					alert7.Show ();
					break;
				case -2:
					UIAlertView alert8 = new UIAlertView (@"Alert", @"Timed Out", null, NSBundle.MainBundle.LocalizedString ("OK", "OK"));
					alert8.Show ();
					break;
				case -3:
					UIAlertView alert9 = new UIAlertView (@"Alert", @"HTTP Exception occurred", null, NSBundle.MainBundle.LocalizedString ("OK", "OK"));
					alert9.Show ();
					break;
				default:
					UIAlertView alert10 = new UIAlertView (@"Alert", res.result.message, null, NSBundle.MainBundle.LocalizedString ("OK", "OK"));
					alert10.Show ();
					break;
				}
			}
		}


		private List<Model.Inspection> GetInspectionInfo (){
			
			//inspectionDetailInfo=new Fill ().inspection;
			return inspectionDetailInfo;
		}

		public virtual void Clicked (UIAlertView alertview, nint buttonIndex)
		{
			
		}

		void ReportBtn_TouchUpInside(object sender, EventArgs e)
		{
			ReportService reportService = new ReportService (AppDelegate.DatabaseContext);
			List<Report> reports=reportService.GetReports ();
			if (reports == null || reports.Count <= 0) {
				syncProgressMessage ();
			} 
			else 
			{
				InspectionDataTableViewController inspectionDataController = this.Storyboard.InstantiateViewController("InspectionDataTableViewController") as InspectionDataTableViewController;
				inspectionDataController.dashboardViewCntrlr = this;
				this.NavigationController.PushViewController (inspectionDataController, true);
			}
		}

		public void syncProgressMessage()
		{
			try
			{
				UIAlertView alert = new UIAlertView ("Alert", "No reports available", null, NSBundle.MainBundle.LocalizedString ("OK", "OK"));
				alert.Show ();
			}
			catch(Exception ex) {
				Debug.WriteLine ("Exception occured method PdfViewer due to : "+ex.Message);
			}
		}

		#region sync & Notify 

		async void BtnSync_TouchUpInside (object sender, EventArgs e)
		{
			InvokeOnMainThread (delegate { 
//				btnSync.Enabled = false;
//				btnLogout.Enabled = false;
			});
			//await syncData ();
			IsFirstLogin = true;
			this.ViewDidLoad ();
		}



		public override void updateSyncCount(int count)
		{
			string countStr="";
			if (count > 0) 
			{
				countStr = count.ToString ();
			}
			//run on ui thread
			InvokeOnMainThread(delegate {  
				if (count > 0){
				lblSyncNumber.Text = countStr; 
					lblSyncNumber.Hidden=false;
				}
				else
				{
					lblSyncNumber.Hidden=true;
				}
				//btnLogout.Enabled=true;
				//btnSync.Enabled=true;
			});
		}
		public override void  updateNotifyCount(int count, bool fromSync)
		{
			
			string countStr="";
			if (count > 0) {
				countStr = count.ToString ();
			}
			//run on ui thread
			InvokeOnMainThread(delegate {  
				if (count > 0){
					LblNotifyNbr.Text = countStr; 
					LblNotifyNbr.Hidden=false;
				}
				else
				{
					LblNotifyNbr.Hidden=true;
				}
				btnSync.Enabled=true;
				if(fromSync)
				{
					btnLogout.Enabled=true;
				}
			});

		}
		void BtnNotify_TouchUpInside(object sender,EventArgs e)
		{
			SyncNotification SyncNotification = this.Storyboard.InstantiateViewController ("SyncNotification") as SyncNotification;
			UIPopoverController cl = new UIPopoverController (SyncNotification);
			cl.SetPopoverContentSize (new CGSize (500, 600), true);
			cl.PresentFromRect(btnNotify.Frame,View,UIPopoverArrowDirection.Any, true);
			clearSeen ();

		}
		void syncProgressChange(bool inProgress)
		{
			this.InvokeOnMainThread (delegate 
			{
				btnLogout.Enabled = !inProgress;
			});
			syncInit ();
		}
		#endregion
	}

	public enum RowType{None,Header,Content}
	public class DashBoardEventArgs:EventArgs
	{
		public RowType RowType{ get; set;}
		public Model.Inspection InspectionDetail{ get; set;}
	}


}
