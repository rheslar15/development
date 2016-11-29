using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;
using Model;
using BAL.Service;
using DAL.DO;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Diagnostics;
using BAL;

namespace LiRoInspect.iOS
{
	public partial class AppDetailsCell : UITableViewCell
	{
		public AppDetailsCell (IntPtr handle) : base (handle)
		{
		}
		ReportService reportService;
		ReportView reportView;
		UIViewController appDetailsController;
		DashBoardViewController dashViewController;
		string pathname="";
		public static bool previewdone = true;

		public AppDetailsCell()
		{
			
		}

		public void UpdateCell(UITableView itemTableView, NSIndexPath indexPath,ReportView reports, UIViewController appDetailsTableController, DashBoardViewController dashViewCntlr)
		{
			try
			{
				ResetUIView ();
				reportView = reports;
				if (reports != null) {
					appIDLabel.Text = reports.AppID;
					inspectionIDLabel.Text = reports.InspectionType;
				}
				appDetailsController = appDetailsTableController;
				dashViewController = dashViewCntlr;
			}
			catch(Exception ex) {
				Debug.WriteLine ("Exception occured method UpdateCell due to : "+ex.Message);
			}
		}

		void ResetUIView ()
		{
			photoLogReportBtn.Layer.CornerRadius = 5.0f;
			photoLogReportBtn.Layer.BackgroundColor = UIColor.White.CGColor;
			photoLogReportBtn.Layer.BorderWidth = 0.5f;
			photoLogReportBtn.TouchUpInside -= photoLogReportBtn_TouchUpInside;
			photoLogReportBtn.TouchUpInside += photoLogReportBtn_TouchUpInside;

			inspectionReportBtn.Layer.CornerRadius = 5.0f;
			inspectionReportBtn.Layer.BackgroundColor = UIColor.White.CGColor;
			inspectionReportBtn.Layer.BorderWidth = 0.5f;
			inspectionReportBtn.TouchUpInside -= inspectionReportBtn_TouchUpInside;
			inspectionReportBtn.TouchUpInside += inspectionReportBtn_TouchUpInside;
		}

		void inspectionReportBtn_TouchUpInside (object sender, EventArgs e)
		{
			//Button color changes on inspection Report click
			photoLogReportBtn.Layer.BackgroundColor = UIColor.White.CGColor;
			photoLogReportBtn.SetTitleColor(UIColor.FromRGB(18, 74, 143), UIControlState.Normal);
			inspectionReportBtn.Layer.BackgroundColor = UIColor.FromRGB(18, 74, 143).CGColor;
			inspectionReportBtn.SetTitleColor(UIColor.White, UIControlState.Normal);
			if (reportView.ReportType != string.Empty) {
				PdfViewer (reportView.ReportType);
			} else {
				UIAlertView alert = new UIAlertView ("Alert", "Selected report is uploaded, hence not available to display", null, NSBundle.MainBundle.LocalizedString ("OK", "OK"));
				alert.Show ();
			}
		}

		void PdfViewer(string reportType)
		{
			try
			{
				reportService = new ReportService (AppDelegate.DatabaseContext);
				ReportDO reports=reportService.GetReportOnInspectionTransactionID (AppDelegate.DatabaseContext,reportView.InspectionTransactionID,reportType);

				if(reports != null)
				{
					string appRootDir = Environment.GetFolderPath (Environment.SpecialFolder.MyDocuments);
					DirectoryInfo path = Directory.CreateDirectory (appRootDir + "/LiRoSyncReports");
					string pathname = path.FullName + "/" + reportType.ToUpper()+" REPORT"+".pdf";
					FileStream fs = new FileStream (pathname, FileMode.Create, FileAccess.ReadWrite, FileShare.None);
					foreach (byte fileByte in reports.ReportDesc) 
					{
						fs.WriteByte (fileByte);
					}
					fs.Close ();

					if (File.Exists (pathname)) 
					{
						var viewer = UIDocumentInteractionController.FromUrl (NSUrl.FromFilename (pathname));
						InspectionDataTableViewController.FilePath=pathname;
						UIDocumentInteractionControllerDelegateDerived del = new UIDocumentInteractionControllerDelegateDerived (appDetailsController);
						del.doneWithPreview+= Del_doneWithPreview;

						viewer.Delegate = del; 
						previewdone=false;
						viewer.PresentPreview (true);
					}
				}
				else
				{
					UIAlertView alert = new UIAlertView ("Alert", "Selected report is uploaded, hence not available to display", null, NSBundle.MainBundle.LocalizedString ("OK", "OK"));
					alert.Show ();
				}
			}
			catch(Exception ex) {
				Debug.WriteLine ("Exception occured method PdfViewer due to : "+ex.Message);
			}
		}

		void Del_doneWithPreview (object sender, EventArgs e)
		{
			try
			{				
				if (InspectionDataTableViewController.FilePath != null) {
					if (File.Exists (InspectionDataTableViewController.FilePath)) 
					{
						File.Delete (InspectionDataTableViewController.FilePath);
						Sync syn = new Sync (AppDelegate.DatabaseContext);
						int syncCount=syn.getPendingSyncCount();
						dashViewController.updateSyncCount(syncCount);
						dashViewController.Dispose();
					}
				}
			}
			catch(Exception ex) {
				Debug.WriteLine ("Exception occured method Del_doneWithPreview due to : "+ex.Message);
			}
		}

		void photoLogReportBtn_TouchUpInside (object sender, EventArgs e)
		{
			//Button color changes on photoLog Report click
			inspectionReportBtn.Layer.BackgroundColor = UIColor.White.CGColor;
			inspectionReportBtn.SetTitleColor(UIColor.FromRGB(18, 74, 143), UIControlState.Normal);
			photoLogReportBtn.Layer.BackgroundColor = UIColor.FromRGB(18, 74, 143).CGColor;
			photoLogReportBtn.SetTitleColor(UIColor.White, UIControlState.Normal);
			PdfViewer (Constants.REPORTTYPE_PHOTOLOG);
		}
	}
}