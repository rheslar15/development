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
	[Register ("ProjectDetailCell")]
	partial class ProjectDetailCell
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel lblProjectDesc { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel lblProjectDescValue { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (lblProjectDesc != null) {
				lblProjectDesc.Dispose ();
				lblProjectDesc = null;
			}
			if (lblProjectDescValue != null) {
				lblProjectDescValue.Dispose ();
				lblProjectDescValue = null;
			}
		}
	}
}
