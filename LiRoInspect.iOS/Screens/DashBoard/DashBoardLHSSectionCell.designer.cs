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
	[Register ("DashBoardLHSSectionCell")]
	partial class DashBoardLHSSectionCell
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIImageView calInspectionDate { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIImageView imgAccesary { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel lblCalDate { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel lblInpectionCount { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel lblInspectionDay { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel lblInspectionDayHeader { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (calInspectionDate != null) {
				calInspectionDate.Dispose ();
				calInspectionDate = null;
			}
			if (imgAccesary != null) {
				imgAccesary.Dispose ();
				imgAccesary = null;
			}
			if (lblCalDate != null) {
				lblCalDate.Dispose ();
				lblCalDate = null;
			}
			if (lblInpectionCount != null) {
				lblInpectionCount.Dispose ();
				lblInpectionCount = null;
			}
			if (lblInspectionDay != null) {
				lblInspectionDay.Dispose ();
				lblInspectionDay = null;
			}
			if (lblInspectionDayHeader != null) {
				lblInspectionDayHeader.Dispose ();
				lblInspectionDayHeader = null;
			}
		}
	}
}
