using System;
using System.Drawing;
using CoreGraphics;
using Foundation;
using UIKit;
using LiRoInspect.iOS.Reporting;
using System.IO;
using Model;
using BAL;
using BAL.Service;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using DAL.DO;


namespace LiRoInspect.iOS
{
	public class ReportWebviewDelegate : UIWebViewDelegate
	{ 
		RootViewController rvc;
		public ReportWebviewDelegate(RootViewController rvc)
		{
			this.rvc = rvc;
		}

		public override void LoadingFinished (UIWebView webView)
		{
			rvc.updatesizes ();
		}
		
		public override bool ShouldStartLoad (UIWebView webView, NSUrlRequest request, UIWebViewNavigationType navigationType)
		{
			NSUrl url = request.Url;
			return true;	
		}
	}

	public partial class RootViewController : BaseViewController
    {    
		public nint selectedSegmentId;
		public Inspection InspectionResult;
		string reportPath="";
		string PhotoLogReportPath="";
		string TempPhotoLogReportPath="";
		bool isLogout=false;
        public RootViewController(IntPtr handle): base(handle)
        {
        }
		public RootViewController()
		{
		}

        public override void DidReceiveMemoryWarning()
        {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
			NSUrlCache.SharedCache.RemoveAllCachedResponses();
			GC.SuppressFinalize(this);
			GC.Collect();
        }

		/// <summary>
		/// Updatesizes this instance.
		/// </summary>
		/// 
        #region View lifecycle
		public void updatesizes()
		{
			WVReport.UserInteractionEnabled = true;
			WVReport.ScrollView.ScrollEnabled = true;    // Property available in iOS 5.0 and later 
			CGRect frame = WVReport.Frame;
			frame.Size = new CGSize (WVReport.Frame.Size.Width,  WVReport.ScrollView.ContentSize.Height);

			WebViewScrollView.ContentSize = new CGSize(WebViewScrollView.Frame.Width, 2000 + WebViewScrollView.Frame.Y);
			WVReport.Frame = frame;
		}

		/// <summary>
		/// Views the did load.
		/// </summary>
		public async override void ViewDidLoad()
		{
			//OptionTransaction obt = new OptionTransaction ();
			//ssssobt.
			base.ViewDidLoad();

			lblSyncNumber.Layer.CornerRadius = lblSyncNumber.Frame.Height/2;
			lblSyncNumber.ClipsToBounds = true;

			LblNotifyNbr.Layer.CornerRadius = LblNotifyNbr.Frame.Height/2;
			LblNotifyNbr.ClipsToBounds = true;

			WVReport.Delegate = new ReportWebviewDelegate (this);
//			WVReport.UserInteractionEnabled = false;

			btnHome.TouchUpInside-= BtnHome_TouchUpInside;
			btnHome.TouchUpInside+= BtnHome_TouchUpInside;
			btnEdit.TouchUpInside-= BtnEdit_TouchUpInside;
			btnEdit.TouchUpInside+= BtnEdit_TouchUpInside;

			btnNotify.TouchUpInside += BtnNotify_TouchUpInside;
			//btnSync.TouchUpInside += btnSync_TouchUpInside;
			btnSync.Enabled = false;

			btnLogOut.TouchUpInside += BtnLogout_TouchUpInside;

			//reportPhotoSegment.SelectedSegment = 1;
	//		nint selectedsegmentid=reportPhotoSegment.SelectedSegment;
				
			reportPhotoSegment.ValueChanged +=  ReportPhotoSegment_ValueChanged;

			syncInit ();
			//NotifyCount ();
			lblCalDate.Text = DateTime.Today.Date.ToString ("MMM dd");
			lblCalDay.Text = "Today";
			btnFinalizeSave.TouchUpInside-= BtnFinalizeSave_TouchUpInside;
			btnFinalizeSave.TouchUpInside+= BtnFinalizeSave_TouchUpInside;
			OptionTransactionService optionTransactionService = new OptionTransactionService (AppDelegate.DatabaseContext);
			if (InspectionResult != null) {
				this.InvokeOnMainThread (delegate {
					LoadOverLayPopup ();
				});
				///Modification needed
				int checkListResult=0;
				var optData = optionTransactionService.GetOptionTransactions ().Where(o=>o.inspectionTransID==InspectionResult.ID);
				foreach (var opt in optData) 
				{
					opt.checkListTransaction =CheckListTransactionDO.GetCheckListTransaction(AppDelegate.DatabaseContext, opt.ID);
					checkListResult=checkListResult + opt.checkListTransaction.Where(i=>i.result==(int)ResultType.FAIL).Count();
				}

				if (checkListResult > 0) 
				{
					InspectionResult.pass = "fail";
				} 
				else 
				{
					InspectionResult.pass = "pass";
				}
				UpdateView ();
				reportPath=await GenerateReport ();
				await generatePhotoReport(InspectionResult);
				this.InvokeOnMainThread (delegate {
					HideOverLay ();
				});
			}
			if (File.Exists (reportPath)) 
			{
				WVReport.LoadRequest (new Foundation.NSUrlRequest (new Foundation.NSUrl (reportPath, false)));
			}

			// Perform any additional setup after loading the view, typically from a nib.
		}

		/// <summary>
		/// Reports the photo segment value changed.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		void ReportPhotoSegment_ValueChanged (object sender, EventArgs e)
		{
			var selectedSegmentId = (sender as UISegmentedControl).SelectedSegment;
			switch(reportPhotoSegment.SelectedSegment) {

			case 0:

				if (File.Exists (reportPath)) 
				{
					WVReport.ScrollView.UserInteractionEnabled = true;
					WVReport.LoadRequest (new Foundation.NSUrlRequest (new Foundation.NSUrl (reportPath, false)));
				}

				break;
			case 1:
				if (File.Exists (TempPhotoLogReportPath)) 
				{
					WVReport.ScrollView.UserInteractionEnabled = false;
					WVReport.LoadRequest (new Foundation.NSUrlRequest (new Foundation.NSUrl (TempPhotoLogReportPath, false)));
				}
				break;
			}
		}
			
		/// <summary>
		/// Generates the report.
		/// </summary>
		/// <returns>The report.</returns>
		private async Task<string> GenerateReport(){
		
			reportPath=new ReportUtility().GenerateReport("Report.pdf",InspectionResult);
			return reportPath;
		}

		/// <summary>
		/// Generates the photo report.
		/// </summary>
		/// <returns>The photo report.</returns>
		/// <param name="inspection">Inspection.</param>
		private async Task generatePhotoReport(Inspection inspection)
		{
			InspectionDetailService inspectionDetailService = new InspectionDetailService (AppDelegate.DatabaseContext);
			Inspection insObj= inspectionDetailService.GetInspectionDetail (inspection,true);
			IReportHandler reportHandler = ReportFactory.GetReportHandler (ReportType.TempPhotolog);
			TempPhotoLogReportPath=reportHandler.GenerateReport ("TempPhotoLogReport.pdf", insObj);
			reportHandler = ReportFactory.GetReportHandler (ReportType.PhotoLog);
			PhotoLogReportPath =reportHandler.GenerateReport ("PhotoLogReport.pdf", insObj);
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear(animated);
			this.InvokeOnMainThread (delegate {
				headerView.BackgroundColor = UIColor.FromPatternImage (UIImage.FromFile ("HeaderViewBackground.png"));
			});

			headerView.Layer.ShadowColor = UIColor.FromRGB(142,187,223).CGColor;
			headerView.Layer.ShadowOpacity = 0.8f;
			headerView.Layer.ShadowRadius = 2.0f;
			headerView.Layer.ShadowOffset = new System.Drawing.SizeF(0f, 3f);
			headerView.Layer.MasksToBounds = false;
			AppDelegate.dataSync.syncProgress +=  syncProgressChange;
			//IReportHandler reportHandler = ReportFactory.GetReportHandler (ReportType.Fail);

		}
	
		/// <summary>
		/// Updates the view.
		/// </summary>
		private void UpdateView()
		{
			lblOwnerName.Text = InspectionResult.HouseOwnerName;
			leftlblInspectionDate.Text = InspectionResult.inspectionDateTime.ToString();
			//lblPathWay.Text = InspectionResult.Pathway.ToString();
			lblInspectionType.Text = InspectionResult.InspectionType;
			lblAddress1.Text = InspectionResult.InspectionAddress1;
			string address = !string.IsNullOrEmpty(InspectionResult.InspectionAddress2) ? InspectionResult.InspectionAddress2 + "," + InspectionResult.City + " " + InspectionResult.Pincode : InspectionResult.City + " " + InspectionResult.Pincode;
			lblAddress2.Text = address.Trim();
			lblPhoneNumber.Text = InspectionResult.PhoneNo;
			lblInspectorName.Text = InspectionResult.RepresentativeName;
			//lblInspectionDate.Text =InspectionResult.inspectionDateTime!=null? InspectionResult.inspectionDateTime.Date.ToString ("dd MMM yyyy"):DateTime.Today.Date.ToString ("dd MMMMM yyyy");
			//			lblInspectionDate.Text=DateTime.Today.Date.ToString ("MM/dd/yyyy");

			NSDate todayDate = NSDate.Now;
			NSDateFormatter dateFormatter = new NSDateFormatter();
			dateFormatter.DateFormat = @"MM/dd/yyyy";
			lblInspectionDate.Text = dateFormatter.StringFor(todayDate);

			//			lblInspectionDate.TextAlignment = UITextAlignment.Right;
			lblFailMessage.Text = "";
		}

		/// <summary>
		/// Saves the Reports
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
        void BtnFinalizeSave_TouchUpInside (object sender, EventArgs e)
        {
			if (InspectionResult != null) {
				InspectionTransactionService inspectionTransactionService = new InspectionTransactionService (AppDelegate.DatabaseContext);
				if (InspectionResult != null && !string.IsNullOrEmpty (InspectionResult.pass)) {
					InspectionResult.IsFinalise = 1;
				}
				inspectionTransactionService.UpdateInspectionTransaction (InspectionResult);
				SaveAllReports ();
			}
//			btnLogOut.Enabled = false;

			btnSync.Enabled = false;
			AppDelegate.stopAutoSync();
			UIApplication.SharedApplication.IdleTimerDisabled = true;

			syncData (false);

			AppDelegate.startAutoSync();

			ClearMemory ();
			DashBoardViewController dashBoardViewController = this.Storyboard.InstantiateViewController ("DashBoardViewController") as DashBoardViewController;
			this.NavigationController.PushViewController (dashBoardViewController, false);
        }

		/// <summary>
		/// Edit button event.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
        void BtnEdit_TouchUpInside (object sender, EventArgs e)
        {
			var controller = this.NavigationController.ViewControllers.Where(i=>i is InspectionViewController).FirstOrDefault();
			//var controller = this.NavigationController.ViewControllers.ElementAt(index);
			if (controller != null && controller is InspectionViewController) {
				(controller as InspectionViewController).IsEdit = true;
				//(controller as InspectionViewController).clearPhotoBuffer ();
				this.NavigationController.PopViewController (false);
			}
        }

		/// <summary>
		/// logout button method.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		void BtnLogout_TouchUpInside(object sender,EventArgs e)
		{
			isLogout=LogOut ();
			if (isLogout) {
				this.DismissViewControllerAsync (false);
				isLogout = false;
			}

		}

		/// <summary>
		/// home touch up inside method.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		void BtnHome_TouchUpInside (object sender, EventArgs e)
		{
			DashBoardViewController dashBoardViewController = this.Storyboard.InstantiateViewController ("DashBoardViewController") as DashBoardViewController;
			ClearMemory ();
			this.NavigationController.PushViewController (dashBoardViewController, false);
		}


        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
			AppDelegate.dataSync.syncProgress -=  syncProgressChange;
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
        }

		/// <summary>
		/// Print preview of the report.
		/// </summary>
		/// <param name="sender">Sender.</param>
		partial void btnPrintPreview_TouchUpInside (UIButton sender)
		{
			//Object inspectionObject=new Object();
			if(reportPhotoSegment.SelectedSegment==0)
			{
			if(File.Exists(reportPath)){
				var viewer = UIDocumentInteractionController.FromUrl(NSUrl.FromFilename(reportPath));
				UIDocumentInteractionControllerDelegateDerived del = new UIDocumentInteractionControllerDelegateDerived(this);
				del.doneWithPreview+=(informer, eventArgs)=>{btnFinalizeSave.Hidden = false;
				btnEdit.Hidden = false;};
				viewer.Delegate = del; 
				viewer.PresentPreview(true);

			}
			}
			else
			{
				if(File.Exists(PhotoLogReportPath)){
					var viewer = UIDocumentInteractionController.FromUrl(NSUrl.FromFilename(PhotoLogReportPath));
					UIDocumentInteractionControllerDelegateDerived del = new UIDocumentInteractionControllerDelegateDerived(this);
					del.doneWithPreview+=(informer, eventArgs)=>{btnFinalizeSave.Hidden = false;
						btnEdit.Hidden = false;};
					viewer.Delegate = del; 
					viewer.PresentPreview(true);

		}
		}
		}

		/// <summary>
		/// Saves all reports.
		/// </summary>
		private void SaveAllReports()
		{
			ReportService reportService = new ReportService (AppDelegate.DatabaseContext);
			InspectionTransactionService InsTransservice = new InspectionTransactionService (AppDelegate.DatabaseContext);
			int inspectionTransactionID = InsTransservice.GetInspectionTransactionID (InspectionResult.projectID,InspectionResult.inspectionID);
			if(!string.IsNullOrEmpty(reportPath) && File.Exists(reportPath))
			{
				byte[] buffer;
				using (Stream stream = new FileStream(reportPath,FileMode.Open))
				{
					buffer = new byte[stream.Length - 1];
					stream.Read(buffer, 0, buffer.Length);
				}
				Model.Report report = new Model.Report (){
					InspectionTransID=inspectionTransactionID,
					ReportType =InspectionResult.pass,
					ReportDesc=buffer
				};
				reportService.SaveReport (report);
			}

			if (!string.IsNullOrEmpty (PhotoLogReportPath) && File.Exists (PhotoLogReportPath)) {
				byte[] photoLogbuffer;
				using (Stream stream = new FileStream (PhotoLogReportPath, FileMode.Open)) {
					photoLogbuffer = new byte[stream.Length - 1];
					stream.Read (photoLogbuffer, 0, photoLogbuffer.Length);
				}

				Model.Report photoLogReport = new Model.Report (){
					InspectionTransID=inspectionTransactionID,
					ReportType =Constants.REPORTTYPE_PHOTOLOG,
					ReportDesc=photoLogbuffer
				};
				reportService.SaveReport (photoLogReport);
			}
		}
        #endregion

		#region sync & Notify 

		/// <summary>
		/// Updates the sync count.
		/// </summary>
		/// <param name="count">Count.</param>
		public override void updateSyncCount(int count)
		{
			string countStr="";
			if (count > 0) {
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

				btnSync.Enabled = true;
				btnLogOut.Enabled = true;
			});
		}

		/// <summary>
		/// Updates the notify count.
		/// </summary>
		/// <param name="count">Count.</param>
		/// <param name="fromSync">If set to <c>true</c> from sync.</param>
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
			});

		}

		/// <summary>
		/// Notify button event
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		void BtnNotify_TouchUpInside(object sender,EventArgs e)
		{
			SyncNotification SyncNotification = this.Storyboard.InstantiateViewController ("SyncNotification") as SyncNotification;
			UIPopoverController cl = new UIPopoverController (SyncNotification);
			cl.PresentFromRect(btnNotify.Frame,View,UIPopoverArrowDirection.Any, true);
			clearSeen ();
		}

		/// <summary>
		/// Syncs progress change.
		/// </summary>
		/// <param name="inProgress">If set to <c>true</c> in progress.</param>
		void syncProgressChange(bool inProgress)
		{
			this.InvokeOnMainThread (delegate 
			{
				btnLogOut.Enabled = !inProgress;
			});
			syncInit ();
		}

		#endregion
    
		/// <summary>
		/// Clears the memory.
		/// </summary>
		private void ClearMemory()
		{
			if (!isLogout) {
				base.ClearMemory ();
			}
			if (this.InspectionResult != null) {
				this.InspectionResult.Dispose ();
				this.InspectionResult = null;
			}
			try{
			if (File.Exists (reportPath)) {
				File.Delete (reportPath);
			}
			if(File.Exists(PhotoLogReportPath)){

				File.Delete(PhotoLogReportPath);
			}
			if(File.Exists(TempPhotoLogReportPath)){

					File.Delete(TempPhotoLogReportPath);
			}
			}
			catch(Exception ex) {
				Debug.WriteLine ("Exception occured in ClearMemory "+ex.Message);
			}
		}

	}
}