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
	[Register ("OptionsSelectCell")]
	partial class OptionsSelectCell
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton btnOptCheck { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel lblOptionsSel { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (btnOptCheck != null) {
				btnOptCheck.Dispose ();
				btnOptCheck = null;
			}
			if (lblOptionsSel != null) {
				lblOptionsSel.Dispose ();
				lblOptionsSel = null;
			}
		}
	}
}
