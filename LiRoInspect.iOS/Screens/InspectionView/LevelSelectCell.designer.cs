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
	[Register ("LevelSelectCell")]
	partial class LevelSelectCell
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton btnLevelCheck { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel lblLevelSel { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (btnLevelCheck != null) {
				btnLevelCheck.Dispose ();
				btnLevelCheck = null;
			}
			if (lblLevelSel != null) {
				lblLevelSel.Dispose ();
				lblLevelSel = null;
			}
		}
	}
}
