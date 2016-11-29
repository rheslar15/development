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
	[Register ("ConfigCell")]
	partial class ConfigCell
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel configLHSLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextField configRHSLabel { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (configLHSLabel != null) {
				configLHSLabel.Dispose ();
				configLHSLabel = null;
			}
			if (configRHSLabel != null) {
				configRHSLabel.Dispose ();
				configRHSLabel = null;
			}
		}
	}
}
