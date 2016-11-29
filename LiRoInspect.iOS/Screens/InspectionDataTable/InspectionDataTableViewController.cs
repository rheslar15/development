// This file has been autogenerated from a class added in the UI designer.

using System;
using Foundation;
using UIKit;
using BAL.Service;
using Model;
using System.Collections.Generic;
using BAL;
using System.Linq;
using System.Diagnostics;

namespace LiRoInspect.iOS
{
	public partial class InspectionDataTableViewController : UITableViewController
	{
		InspectionTransactionService inspectionTransactionService;
		PathwayService pathwayService;
		InspectionService inspectionService;
		List<ReportView> reportDetails;
		public DashBoardViewController dashboardViewCntrlr;
		Sync syn = new Sync (AppDelegate.DatabaseContext);
		public static string FilePath{ get; set; }
		bool isSyncProgress = false;
		public InspectionDataTableViewController (IntPtr handle) : base (handle)
		{
			
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			ReportService reportService = new ReportService (AppDelegate.DatabaseContext);
			List<Report> reports=reportService.GetReports ();

			if (reports == null || reports.Count <= 0) {
				syncProgressMessage ();
			}
			AppDelegate.dataSync.notifiy -= syncProgressHandler;
			AppDelegate.dataSync.notifiy += syncProgressHandler;
			reportDetails = ConvertToReportView (reports);

			this.NavigationController.NavigationBarHidden = false;
			this.NavigationItem.SetHidesBackButton(false,true);
			this.NavigationItem.SetRightBarButtonItem(
				new UIBarButtonItem(UIBarButtonSystemItem.Action, (sender,args) => {
					if(this.NavigationController.ViewControllers.Contains(this))
					{
						this.NavigationController.PopViewController (true);
					}

				})
				, true);
		}

		void syncProgressHandler(notificationEventArgs e)
		{
			this.InvokeOnMainThread (delegate 
				{
					if (!e.isSyncCompleted)
					{
						syncProgressMessage();
					} 
				});
		}

		public void syncProgressMessage()
		{
			try
			{
				AppDelegate.dataSync.notifiy -= syncProgressHandler;

				UIAlertView alert = new UIAlertView ("Alert", "Sync is in progress", null, NSBundle.MainBundle.LocalizedString ("OK", "OK"));
				alert.Show ();

				if(this.NavigationController != null)
				{
					if(this.NavigationController.ViewControllers[this.NavigationController.ViewControllers.Count() - 2] != null)
					{
						DashBoardViewController dashBoardViewController = (DashBoardViewController) (this.NavigationController.ViewControllers[this.NavigationController.ViewControllers.Count() - 2]);
						int syncCount=syn.getPendingSyncCount();
						dashBoardViewController.updateSyncCount(syncCount);
					}

					if(this.NavigationController.ViewControllers.Contains(this))
					{
						this.NavigationController.PopViewController (true);
					}
				}
			}
			catch(Exception ex) {
				Debug.WriteLine ("Exception occured method PdfViewer due to : "+ex.Message);
			}
		}

		public override nint NumberOfSections (UITableView tableView)
		{
			return 1;
		}

		public override nint RowsInSection (UITableView tableview, nint section)
		{
			int count = reportDetails.Count;
			return count;
		}

		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			AppDetailsCell cell = tableView.DequeueReusableCell (@"AppDetailsCell") as AppDetailsCell;				
			cell.UpdateCell (tableView, indexPath, reportDetails[indexPath.Row], this, dashboardViewCntrlr);
			return cell;
		}

		private List<ReportView> ConvertToReportView(List<Report> reports)
		{
			List<ReportView> reportView = new List<ReportView> ();
			if (reports != null && reports.Count > 0) {
				inspectionTransactionService = new InspectionTransactionService (AppDelegate.DatabaseContext);
				pathwayService=new PathwayService(AppDelegate.DatabaseContext);
				inspectionService = new InspectionService (AppDelegate.DatabaseContext);
				foreach (var report in reports) {
					if (reportView.Where (rv => rv.InspectionTransactionID == report.InspectionTransID).Count () <= 0) {							
						var inspection = inspectionTransactionService.GetInspectionProjectID (report.InspectionTransID);
						if (inspection != null) {
							switch (inspection.PathwayTypeID) {
							case(0):
								inspection.PathwayTypeID = 1;
								break;
							case(1):
								inspection.PathwayTypeID = 2;
								break;
							case(2):
								inspection.PathwayTypeID = 3;
								break;
							default:
								
								break;
							}
						

							var pathway = pathwayService.GetPathway (inspection.PathwayTypeID);
							var inspectionType = inspectionService.GetInspection (Convert.ToInt32 (inspection.InspectionID));

							if ((report.ReportType.ToUpper () == ReportType.Pass.ToString ().ToUpper ()) || (report.ReportType.ToUpper () == ReportType.Fail.ToString ().ToUpper ())) {

							} else {
								report.ReportType = string.Empty;
							}

							reportView.Add (new ReportView () {  AppID = inspection.ProjectID.ToString(),
								InspectionType = inspectionType.InspectionType,
								PathwayType = (pathway != null) ? pathway.PathwayDesc : "",
								ReportDesc = null,
								ReportType = report.ReportType,
								InspectionTransactionID = report.InspectionTransID,

							});
						}
					}
				}
			}
			return reportView;
		}

		public override nfloat GetHeightForRow (UITableView tableView, NSIndexPath indexPath)
		{
			return 44;
		}

		public override void ViewDidDisappear (bool animated)
		{			
			if (AppDetailsCell.previewdone) {
				AppDelegate.dataSync.notifiy -= syncProgressHandler;
			}

			base.ViewDidDisappear (animated);
		}

		public override void ViewDidAppear (bool animated)
		{
			AppDetailsCell.previewdone = true;
			base.ViewDidAppear (animated);
		}
	}		
}