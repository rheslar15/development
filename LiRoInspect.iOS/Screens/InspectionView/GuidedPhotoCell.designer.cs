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
	[Register ("GuidedPhotoCell")]
	partial class GuidedPhotoCell
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel guidedOptionLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton guidedPhotoTkPicBtn { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UICollectionView imagesCollectionView { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (guidedOptionLabel != null) {
				guidedOptionLabel.Dispose ();
				guidedOptionLabel = null;
			}
			if (guidedPhotoTkPicBtn != null) {
				guidedPhotoTkPicBtn.Dispose ();
				guidedPhotoTkPicBtn = null;
			}
			if (imagesCollectionView != null) {
				imagesCollectionView.Dispose ();
				imagesCollectionView = null;
			}
		}
	}
}
