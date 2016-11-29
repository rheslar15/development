// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace LiRoInspect.iOS
{
	[Register ("DashBoardViewController")]
	partial class DashBoardViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton btnHome { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton btnLogout { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton btnNotify { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton btnSync { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton btnViewLog { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton reportBtn { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIView DashboardLHSView { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITableView DashboardView { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIView headerView { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel lblDate { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextField LblNotifyNbr { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextField lblSyncNumber { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel lblUserName { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITableView ProjectDetailView { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (btnHome != null) {
				btnHome.Dispose ();
				btnHome = null;
			}
			if (btnLogout != null) {
				btnLogout.Dispose ();
				btnLogout = null;
			}
			if (btnNotify != null) {
				btnNotify.Dispose ();
				btnNotify = null;
			}
			if (btnSync != null) {
				btnSync.Dispose ();
				btnSync = null;
			}
			if (btnViewLog != null) {
				btnViewLog.Dispose ();
				btnViewLog = null;
			}
			if (reportBtn != null) {
				reportBtn.Dispose ();
				reportBtn = null;
			}
			if (DashboardLHSView != null) {
				DashboardLHSView.Dispose ();
				DashboardLHSView = null;
			}
			if (DashboardView != null) {
				DashboardView.Dispose ();
				DashboardView = null;
			}
			if (headerView != null) {
				headerView.Dispose ();
				headerView = null;
			}
			if (lblDate != null) {
				lblDate.Dispose ();
				lblDate = null;
			}
			if (LblNotifyNbr != null) {
				LblNotifyNbr.Dispose ();
				LblNotifyNbr = null;
			}
			if (lblSyncNumber != null) {
				lblSyncNumber.Dispose ();
				lblSyncNumber = null;
			}
			if (lblUserName != null) {
				lblUserName.Dispose ();
				lblUserName = null;
			}
			if (ProjectDetailView != null) {
				ProjectDetailView.Dispose ();
				ProjectDetailView = null;
			}
		}
	}
}
