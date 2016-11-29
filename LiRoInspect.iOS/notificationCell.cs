using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace LiRoInspect.iOS
{
	partial class notificationCell : UITableViewCell
	{
		public notificationCell (IntPtr handle) : base (handle)
		{
		}
		public void UpdateCell(string messageTxt,bool inProgress, int retrycount)
		{
			message.Text = messageTxt;
			if (inProgress)
			{
				activity.StartAnimating ();
				activity.Hidden = false;
				status.Text = "Uploading...("+retrycount.ToString()+")";
			} else
			{
				activity.StopAnimating ();
				activity.Hidden = true;
				status.Text = retrycount.ToString();
			}
		}
	}
}