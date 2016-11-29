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
	[Register ("SpaceCell")]
	partial class SpaceCell
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton btnCheck { get; set; }


		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel SpaceTxt { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (SpaceTxt != null) {
				SpaceTxt.Dispose ();
				SpaceTxt = null;
			}
		}
	}
}
