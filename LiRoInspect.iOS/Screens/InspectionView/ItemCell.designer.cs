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
	[Register ("ItemCell")]
	partial class ItemCell
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton btnEnComment { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextView ChecklistName { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UISegmentedControl ChecklistSegControl { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel commentsLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextView commentsTextView { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIView commentsView { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		NSLayoutConstraint htChecklistName { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		NSLayoutConstraint htLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		NSLayoutConstraint htTextView { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		NSLayoutConstraint htViewComment { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		NSLayoutConstraint spButton { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		NSLayoutConstraint spComments { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		NSLayoutConstraint wdCommentLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		NSLayoutConstraint wdTextView { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (btnEnComment != null) {
				btnEnComment.Dispose ();
				btnEnComment = null;
			}
			if (ChecklistName != null) {
				ChecklistName.Dispose ();
				ChecklistName = null;
			}
			if (ChecklistSegControl != null) {
				ChecklistSegControl.Dispose ();
				ChecklistSegControl = null;
			}
			if (commentsLabel != null) {
				commentsLabel.Dispose ();
				commentsLabel = null;
			}
			if (commentsTextView != null) {
				commentsTextView.Dispose ();
				commentsTextView = null;
			}
			if (commentsView != null) {
				commentsView.Dispose ();
				commentsView = null;
			}
			if (htChecklistName != null) {
				htChecklistName.Dispose ();
				htChecklistName = null;
			}
			if (htLabel != null) {
				htLabel.Dispose ();
				htLabel = null;
			}
			if (htTextView != null) {
				htTextView.Dispose ();
				htTextView = null;
			}
			if (htViewComment != null) {
				htViewComment.Dispose ();
				htViewComment = null;
			}
			if (spButton != null) {
				spButton.Dispose ();
				spButton = null;
			}
			if (spComments != null) {
				spComments.Dispose ();
				spComments = null;
			}
			if (wdCommentLabel != null) {
				wdCommentLabel.Dispose ();
				wdCommentLabel = null;
			}
			if (wdTextView != null) {
				wdTextView.Dispose ();
				wdTextView = null;
			}
		}
	}
}
