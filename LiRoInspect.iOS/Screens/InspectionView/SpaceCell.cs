using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;
namespace LiRoInspect.iOS
{
	partial class SpaceCell : UITableViewCell
	{
		string spaceid;
		Model.Space space;

		public SpaceCell (IntPtr handle) : base (handle)
		{
		}
		public void UpdateCell(string text,bool optionChecked,string spaceid)
		{
			SpaceTxt.Text = text;
			btnCheck.TouchUpInside -= CheckBoxTouchUpInside;
			btnCheck.TouchUpInside += CheckBoxTouchUpInside;
			this.spaceid = spaceid;
		}
		public void UpdateCell(Model.Space space)
		{
			this.space = space;
			SpaceTxt.Text = space.name;
			btnCheck.TouchUpInside -= CheckBoxTouchUpInside;
			btnCheck.TouchUpInside += CheckBoxTouchUpInside;
			setButtonImage (btnCheck, space.isSelected);

		}
		void setButtonImage(UIButton btnCheckBox,bool isSelected)
		{
			if (isSelected)
			{
				btnCheckBox.SetImage(UIImage.FromBundle ("Check-box-Checked.png"), UIControlState.Normal);
			}
			else
			{
				btnCheckBox.SetImage(UIImage.FromBundle("Check-box-Unchecked.png"), UIControlState.Normal);
			}
		}
		void CheckBoxTouchUpInside (object sender, EventArgs ea)
		{
			var button = sender as UIButton;
			space.isSelected = !space.isSelected;

			if (!space.isSelected)
			{
				space.IsEnabled = false;
			}
			else {
				space.IsEnabled = true;
			}

			setButtonImage (button, space.isSelected);//reverse the selection 
		}
	}
}
