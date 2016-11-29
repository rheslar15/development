using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;
using Model;


namespace LiRoInspect.iOS
{
	partial class LevelSelectCell : UITableViewCell
	{
		public Model.Level Level;
		public LevelSelectCell (IntPtr handle) : base (handle)
		{
		}
		public void UpdateCell(Model.Level level)
		{
			this.Level = level;
			lblLevelSel.Text = level.name;
			btnLevelCheck.TouchUpInside -= CheckBoxTouchUpInside;
			btnLevelCheck.TouchUpInside += CheckBoxTouchUpInside;
			setButtonImage (btnLevelCheck, level.isSelected);

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
			Level.isSelected = !Level.isSelected;


			setButtonImage (button, Level.isSelected);//reverse the selection 
		}
	}
}
