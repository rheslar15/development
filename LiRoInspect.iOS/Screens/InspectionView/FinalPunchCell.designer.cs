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
	[Register ("FinalPunchCell")]
	partial class FinalPunchCell
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel CommentsLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextView CommentsTextView { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIView CommentsView { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		NSLayoutConstraint imgScrollViewHeightCnstrnt { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel PunchHeading { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIScrollView punchImgScrollView { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UISegmentedControl punchSegmentControl { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton takePictureBtn { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (CommentsLabel != null) {
				CommentsLabel.Dispose ();
				CommentsLabel = null;
			}
			if (CommentsTextView != null) {
				CommentsTextView.Dispose ();
				CommentsTextView = null;
			}
			if (CommentsView != null) {
				CommentsView.Dispose ();
				CommentsView = null;
			}
			if (imgScrollViewHeightCnstrnt != null) {
				imgScrollViewHeightCnstrnt.Dispose ();
				imgScrollViewHeightCnstrnt = null;
			}
			if (PunchHeading != null) {
				PunchHeading.Dispose ();
				PunchHeading = null;
			}
			if (punchImgScrollView != null) {
				punchImgScrollView.Dispose ();
				punchImgScrollView = null;
			}
			if (punchSegmentControl != null) {
				punchSegmentControl.Dispose ();
				punchSegmentControl = null;
			}
			if (takePictureBtn != null) {
				takePictureBtn.Dispose ();
				takePictureBtn = null;
			}
		}
	}
}
