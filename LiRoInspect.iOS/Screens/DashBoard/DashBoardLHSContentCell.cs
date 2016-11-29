using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;
using CoreGraphics;
using System.Diagnostics;


namespace LiRoInspect.iOS
{
	
	partial class DashBoardLHSContentCell : UITableViewCell
	{
		EventHandler dashBoardRowSelected;
		private Model.Inspection dashBoardInspectionContent;
		public DashBoardLHSContentCell (IntPtr handle) : base (handle)
		{
		}
		public DashBoardLHSContentCell (NSString reuseIdentifier): base (UITableViewCellStyle.Default, reuseIdentifier)
		{
			
		}
		/// <summary>
		/// Updates the cell.
		/// </summary>
		/// <param name="content">Content.</param>
		/// <param name="DashBoardRowSelected">Dash board row selected.</param>
		/// <param name="rowNo">Row no.</param>
        public void UpdateCell(Model.Inspection content, EventHandler DashBoardRowSelected, nint rowNo)
        {
            try
            {
                
                if (null == this.dashBoardRowSelected)
                dashBoardRowSelected = DashBoardRowSelected;
				lblOwnerNamer.Text = content.HouseOwnerName;

                lblAddress1.Text = content.InspectionAddress1;
				lblConstructionType.Text = content.Pathway.ToString();
				lblInspectionDate.Text=content.inspectionDateTime.ToString();

				lblAddress2.Text = content.City + " " + content.Pincode;

				//lblCityStateZip.Hidden = true;
				// use for debugging
				lblInspectionType.Text = content.InspectionType;


                lblContactNo.Text = content.PhoneNo;
                this.BackgroundColor = UIColor.Clear;
                btnBeginInspection.BackgroundColor = UIColor.White;
                btnBeginInspection.SetTitleColor(UIColor.FromRGB(0, 153, 204), UIControlState.Normal);
                btnBeginInspection.Layer.BorderWidth = 1;
                btnBeginInspection.Layer.BorderColor = UIColor.FromRGB(0, 105, 170).CGColor;
                btnBeginInspection.Layer.ShadowColor = UIColor.DarkGray.CGColor;
                btnBeginInspection.Layer.ShadowOpacity = 1.0f;
                btnBeginInspection.Layer.ShadowRadius = 6.0f;
                btnBeginInspection.Layer.ShadowOffset = new System.Drawing.SizeF(0f, 3f);
                btnBeginInspection.Layer.ShouldRasterize = true;
                btnBeginInspection.Layer.MasksToBounds = false;
                btnBeginInspection.Enabled = true;
                this.dashBoardInspectionContent = content;
                this.btnBeginInspection.TouchUpInside -= BtnBeginInspection_TouchUpInside;
                this.btnBeginInspection.TouchUpInside += BtnBeginInspection_TouchUpInside;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception Occured in UpadetCell method due to " + ex.Message);
            }
        }

		public override bool Selected {
			get {
				return base.Selected;
			}
			set {
				base.Selected = value;
			}
		}

		public override void Select (NSObject sender)
		{
			base.Select (sender);
		}
		public override bool RespondsToSelector (ObjCRuntime.Selector sel)
		{
			return base.RespondsToSelector (sel);
		}
		/// <summary>
		/// Sets the font colour,font name and size.
		/// </summary>
		/// <param name="selected">If set to <c>true</c> selected.</param>
		/// <param name="animated">If set to <c>true</c> animated.</param>
		public override void SetSelected (bool selected, bool animated)
		{
            try
            {
                base.SetSelected(selected, animated);
                if (selected)
                {
                    lblOwnerNamer.TextColor = UIColor.FromRGB(0, 153, 204);
                    lblConstructionType.TextColor = UIColor.Black;
                    lblAddress1.TextColor = UIColor.Black;
                    lblAddress2.TextColor = UIColor.Black;
                    lblInspectionType.TextColor = UIColor.Black;

                    lblContactNo.TextColor = UIColor.Black;
					lblInspectionDate.TextColor=UIColor.Black;
                    btnBeginInspection.BackgroundColor = UIColor.FromRGB(0, 105, 170);
                    btnBeginInspection.SetTitleColor(UIColor.White, UIControlState.Normal);
                    btnBeginInspection.Enabled = true;
                }
                else
                {
                    lblOwnerNamer.TextColor = UIColor.Gray;
                    lblConstructionType.TextColor = UIColor.Gray;
                    lblAddress1.TextColor = UIColor.Gray;
                    lblAddress2.TextColor = UIColor.Gray;
                    lblInspectionType.TextColor = UIColor.Gray;
                    lblContactNo.TextColor = UIColor.Gray;
					lblInspectionDate.TextColor=UIColor.Gray;
                    btnBeginInspection.SetTitleColor(UIColor.FromRGB(0, 153, 204), UIControlState.Normal);
                    btnBeginInspection.Enabled = true;
                    btnBeginInspection.BackgroundColor = UIColor.White;
					//lblCityStateZip.TextColor = UIColor.Gray;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception Occured in SetSelected method due to " + ex.Message);
            }

		}

		/// <summary>
		///On tap of this button the information about that inspection will be displayed on right side.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
        public void BtnBeginInspection_TouchUpInside(object sender, EventArgs e)
        {
            try
            {
                DashBoardEventArgs source = new DashBoardEventArgs();
                source.RowType = RowType.None;
                source.InspectionDetail = this.dashBoardInspectionContent;
                if (null != dashBoardRowSelected)
                {
                    dashBoardRowSelected.Invoke(this, source);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception Occured in BtnBeginInspection_TouchUpInside method due to " + ex.Message);
            }
        }
	}
}
