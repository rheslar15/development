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
	[Register ("UICameraController")]
	partial class UICameraController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIImageView cameraImage { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIImageView cameraView { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton finishButton { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UICollectionView imageCollectionView { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton takePictureButton { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (cameraImage != null) {
				cameraImage.Dispose ();
				cameraImage = null;
			}
			if (cameraView != null) {
				cameraView.Dispose ();
				cameraView = null;
			}
			if (finishButton != null) {
				finishButton.Dispose ();
				finishButton = null;
			}
			if (imageCollectionView != null) {
				imageCollectionView.Dispose ();
				imageCollectionView = null;
			}
			if (takePictureButton != null) {
				takePictureButton.Dispose ();
				takePictureButton = null;
			}
		}
	}
}
