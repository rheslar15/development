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
	[Register ("InspectionDataTableViewController")]
	partial class InspectionDataTableViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITableView appInfoTableView { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (appInfoTableView != null) {
				appInfoTableView.Dispose ();
				appInfoTableView = null;
			}
		}
	}
}
