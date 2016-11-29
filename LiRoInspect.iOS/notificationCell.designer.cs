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
	[Register ("notificationCell")]
	partial class notificationCell
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIActivityIndicatorView activity { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel message { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel status { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel type { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (activity != null) {
				activity.Dispose ();
				activity = null;
			}
			if (message != null) {
				message.Dispose ();
				message = null;
			}
			if (status != null) {
				status.Dispose ();
				status = null;
			}
			if (type != null) {
				type.Dispose ();
				type = null;
			}
		}
	}
}
