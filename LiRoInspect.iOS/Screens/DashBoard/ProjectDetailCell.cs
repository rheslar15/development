using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace LiRoInspect.iOS
{
	partial class ProjectDetailCell : UITableViewCell
	{
		public ProjectDetailCell (IntPtr handle) : base (handle)
		{
		}
		public void UpdateData (string name,string value)
		{
			lblProjectDesc.Text = name;
			lblProjectDescValue.Text = value;
		}
	}
}
