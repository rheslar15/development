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
	[Register ("config")]
	partial class config
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton btn_Logout { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton btn_save { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton btn_Use { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIScrollView configscroll { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITableView configTableView { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UISegmentedControl SegmentProTest { get; set; }

		[Action ("btn_Logout_TouchUpInside:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void btn_Logout_TouchUpInside (UIButton sender);

		[Action ("btn_save_TouchUpInside:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void btn_save_TouchUpInside (UIButton sender);

		[Action ("SegmentProTest_ValueChanged:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void SegmentProTest_ValueChanged (UISegmentedControl sender);

		void ReleaseDesignerOutlets ()
		{
			if (btn_Logout != null) {
				btn_Logout.Dispose ();
				btn_Logout = null;
			}
			if (btn_save != null) {
				btn_save.Dispose ();
				btn_save = null;
			}
			if (btn_Use != null) {
				btn_Use.Dispose ();
				btn_Use = null;
			}
			if (configscroll != null) {
				configscroll.Dispose ();
				configscroll = null;
			}
			if (configTableView != null) {
				configTableView.Dispose ();
				configTableView = null;
			}
			if (SegmentProTest != null) {
				SegmentProTest.Dispose ();
				SegmentProTest = null;
			}
		}
	}
}
