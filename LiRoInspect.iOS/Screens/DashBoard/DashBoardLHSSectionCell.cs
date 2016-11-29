using Foundation;
using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using UIKit;

namespace LiRoInspect.iOS
{
	partial class DashBoardLHSSectionCell : UITableViewCell
	{
		public DashBoardLHSSectionCell (IntPtr handle) : base (handle)
		{
		}
		public DashBoardLHSSectionCell (NSString reuseIdentifier): base (UITableViewCellStyle.Default, reuseIdentifier)
		{
		}


        public void UpdateCell(string header, string day, string count, UIImage image, string CalDate)
        {
            try
            {                
                lblInspectionDayHeader.Text = day;
                lblInpectionCount.Text = count;
                lblCalDate.Text = CalDate;
                if (lblInspectionDayHeader.Text == "Today")
                {                   
                    lblInspectionDayHeader.TextColor = UIColor.FromRGB(121, 171, 51);
                    lblInpectionCount.TextColor = UIColor.FromRGB(121, 171, 51);
                    lblInspectionDay.Text = "Today";
                }
                else
                {
                    lblInspectionDayHeader.TextColor = UIColor.FromRGB(0, 105, 170);
                    lblInpectionCount.TextColor = UIColor.FromRGB(0, 105, 170);
                    lblInspectionDay.Text = "Upcoming";
                }
                imgAccesary.Image = null;
                this.AccessoryView = new UIImageView(image);
                this.BackgroundColor = UIColor.Clear;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception Occured in UpdateCell method due to " + ex.Message);
            }
        }
	}
}
