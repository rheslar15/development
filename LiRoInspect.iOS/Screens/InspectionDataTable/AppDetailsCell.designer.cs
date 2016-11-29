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
	[Register ("AppDetailsCell")]
	partial class AppDetailsCell
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel appIDLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel inspectionIDLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton inspectionReportBtn { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton photoLogReportBtn { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (appIDLabel != null) {
				appIDLabel.Dispose ();
				appIDLabel = null;
			}
			if (inspectionIDLabel != null) {
				inspectionIDLabel.Dispose ();
				inspectionIDLabel = null;
			}
			if (inspectionReportBtn != null) {
				inspectionReportBtn.Dispose ();
				inspectionReportBtn = null;
			}
			if (photoLogReportBtn != null) {
				photoLogReportBtn.Dispose ();
				photoLogReportBtn = null;
			}
		}
	}
}
