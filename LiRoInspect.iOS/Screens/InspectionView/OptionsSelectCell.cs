using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace LiRoInspect.iOS
{
	partial class OptionsSelectCell : UITableViewCell
	{
		private Model.Option Option;

		public OptionsSelectCell (IntPtr handle) : base (handle)
		{
		}

		public void UpdateCell(Model.Option option)
		{
			this.Option = option;
			lblOptionsSel.Text = option.name;
			btnOptCheck.TouchUpInside -= CheckBoxTouchUpInside;
			btnOptCheck.TouchUpInside += CheckBoxTouchUpInside;
			setButtonImage (btnOptCheck, Option.isSelected);

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
			Option.isSelected = !Option.isSelected;
			setButtonImage (button, Option.isSelected);//reverse the selection 
		}
	}
}
