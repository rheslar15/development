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
	[Register ("PunchCell")]
	partial class PunchCell
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UICollectionView imagesCollectionView { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton punchDeleteBtn { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel punchHeadingLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton punchTakePicBtn { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextView punchTextView { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (imagesCollectionView != null) {
				imagesCollectionView.Dispose ();
				imagesCollectionView = null;
			}
			if (punchDeleteBtn != null) {
				punchDeleteBtn.Dispose ();
				punchDeleteBtn = null;
			}
			if (punchHeadingLabel != null) {
				punchHeadingLabel.Dispose ();
				punchHeadingLabel = null;
			}
			if (punchTakePicBtn != null) {
				punchTakePicBtn.Dispose ();
				punchTakePicBtn = null;
			}
			if (punchTextView != null) {
				punchTextView.Dispose ();
				punchTextView = null;
			}
		}
	}
}
