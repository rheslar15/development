
using System;
using System.Collections.Generic;
using Foundation;
using UIKit;
using Model;
using System.Diagnostics;
using System.IO;
using System.Linq;
using CoreGraphics;

namespace LiRoInspect.iOS
{
	public class ItemTextViewDelegate : UITextViewDelegate
	{
		ItemCell itemCell;

		public ItemTextViewDelegate (ItemCell itemCell)
		{
			this.itemCell = itemCell;
		}

		public override bool ShouldChangeText (UITextView textView, NSRange range, string text)
		{
			return (textView.Text.Length + (text.Length - range.Length) <= 300);
		}

		public override void EditingStarted (UITextView textView)
		{
			UITextView commentsTextView = textView;
			CGPoint pointInTable = new CGPoint ();
			CGPoint origin = new CGPoint (commentsTextView.Frame.X, commentsTextView.Frame.Y);
			pointInTable = commentsTextView.Superview.ConvertPointToView (origin, itemCell.checkListTableView);
			CGPoint contentOffset = itemCell.checkListTableView.ContentOffset;

			// was 50 to better align with top
			contentOffset.Y = pointInTable.Y - (commentsTextView.Frame.Height - 75);
			itemCell.checkListTableView.SetContentOffset (contentOffset, true);
		}

		public override void EditingEnded (UITextView textView)
		{
			UITextView failureTextView = textView;
			failureTextView.ResignFirstResponder ();
		}

		public override void Changed (UITextView textView)
		{
			this.itemCell.checkListItem.comments = textView.Text;
		}

	}


	public partial class ItemCell : BaseCell,ICell
	{
		public UITableView checkListTableView;
		public CheckList checkListItem;
		InspectionViewController parentController;
		UITapGestureRecognizer tap;

		public bool IsCheckListItemActive { get; set; }

		private NSIndexPath IndexPath;

		public ItemCell (IntPtr handle) : base (handle)
		{
			
		}


		public void UpdateCell (InspectionViewController parentController, UITableView checkListTableView, NSIndexPath indexPath, CheckList checkListItem)
		{
			
			this.checkListTableView = checkListTableView;
			this.parentController = parentController;
			if (this.parentController != null) {

				IsCheckListItemActive = this.parentController.GetCheckListCommentsActive (indexPath.Row);
			}
			ResetUIView ();
			UpdateUIContent (checkListItem);



			var textDelegate = new ItemTextViewDelegate (this);
			commentsTextView.WeakDelegate = textDelegate;
			ChecklistSegControl.ValueChanged -= ChecklistSegControl_ValueChanged;
			ChecklistSegControl.ValueChanged += ChecklistSegControl_ValueChanged;
			parentController.buttonStyleRefresh (null);




			if (IsCheckListItemActive) {

				commentsTextView.Hidden = false;
				commentsLabel.Hidden = false;
				htLabel.Constant = 21;
				htTextView.Constant = 87;
				wdTextView.Constant = 300;
				htViewComment.Constant = 107;

				//commentsTextView.ShouldBeginEditing ();

				//commentsTextView.BecomeFirstResponder();
			} else {
				htLabel.Constant = 0;
				htTextView.Constant = 0;
				htViewComment.Constant = 0;
				commentsTextView.Hidden = true;
				commentsLabel.Hidden = true;
			}
		}


		void ChecklistSegControl_ValueChanged (object sender, EventArgs e)
		{
			UISegmentedControl checkListSegResult = sender as UISegmentedControl;
			switch (checkListSegResult.SelectedSegment) {
			case 0:
				checkListItem.Result = ResultType.NA;
				break;
			case 1:
				checkListItem.Result = ResultType.PASS;
				break;
			case 2:
				checkListItem.Result = ResultType.FAIL;
				break;
			default:
				checkListItem.Result = ResultType.OTHER;
				break;
			}
			parentController.buttonStyleRefresh (null);
		}

		void CommentsTextView_Changed (object sender, EventArgs e)
		{
			checkListItem.comments = commentsTextView.Text;
		}

		public string GetCheckListName ()
		{
			return ChecklistName.Text;
		}


		private void ResetUIView ()
		{
			//


			commentsTextView.Layer.CornerRadius = 5f;
			commentsTextView.Layer.BorderColor = UIColor.FromRGB (186, 190, 194).CGColor;
			commentsTextView.Layer.BorderWidth = 0.5f;
			//spComments.Constant = 0;
			if (!IsCheckListItemActive) {
				htLabel.Constant = 0;
				htTextView.Constant = 0;
				htViewComment.Constant = 0;
			}


			btnEnComment.Layer.CornerRadius = 5f;
			btnEnComment.BackgroundColor = UIColor.FromRGB (19, 95, 160);
			btnEnComment.SetTitleColor (UIColor.White, UIControlState.Normal);

			commentsTextView.Text = "";
			commentsTextView.Editable = true;
			ChecklistName.Text = "";
			// default button to n/a
			ChecklistSegControl.SelectedSegment = 0;
			btnEnComment.TouchUpInside -= btnEnComment_clicked; 
			btnEnComment.TouchUpInside += btnEnComment_clicked;

			commentsTextView.Layer.BorderColor = UIColor.Black.CGColor;
			commentsTextView.Layer.BorderWidth = 0.5f;

			if (tap != null)
				this.RemoveGestureRecognizer (tap);

			tap = new UITapGestureRecognizer ();

			tap.AddTarget (tapAction);
			this.AddGestureRecognizer (tap);



		}

//		public override void TouchesBegan (NSSet touches, UIEvent evt)
//		{
//			
//
//		}

		void btnEnComment_clicked (object sender, EventArgs e)
		{
			
			IsCheckListItemActive = true;
			// set SetCheckListCommentsActive flag
			this.parentController.SetCheckListCommentsActive (this, IsCheckListItemActive);
			// refresh option table
			this.parentController.RefreshOptionsTable ();

		}

		private void tapAction ()
		{
			
			var View = tap.View;

			if (View != null && tap != null) {
				var Location = tap.LocationInView (commentsTextView);


				var Bounds = commentsTextView.Bounds;

				if (Bounds.Contains (Location)) {
					if (!commentsTextView.IsFirstResponder) {
						// fire commentsTextView only if location is withing the bounds of the text view
						commentsTextView.BecomeFirstResponder ();
					}
				}


			}

		}

		private void UpdateUIContent (CheckList checkListItem)
		{
			this.checkListItem = checkListItem;
			ChecklistName.Text = checkListItem.description;
			commentsTextView.Text = checkListItem.comments;
			UpdateSegmentResult (checkListItem.Result);
			
		}

		public void ChangeChecklistNameColor (UITableView table, CheckList checkListItem, UIColor color)
		{
			var numRows = table.NumberOfRowsInSection (0);



			for (int i = 0; i < numRows; i++) {
				NSIndexPath indexpath1 = NSIndexPath.FromRowSection (i, 0);

				var cell = table.CellAt (indexpath1);
				if (cell is ItemCell) {
					//(cell as ItemCell).cleanCell ();
					var Item = (cell as ItemCell).ChecklistName.Text;
					if (Item != null && Item == checkListItem.description) {
						ChecklistName.BackgroundColor = color;
						return;
					} else {
						if (ChecklistName.BackgroundColor != UIColor.White) {
							if (Item != checkListItem.description) {
								ChecklistName.BackgroundColor = UIColor.White;
							}
						}
					}
				}
			}


		



		}

		public void ClearBackgroundColor (ItemCell cell)
		{
			


		}

		void UpdateSegmentResult (ResultType result)
		{
			switch (result) {
			case ResultType.NA:
				ChecklistSegControl.SelectedSegment = 0;
				break;
			case ResultType.PASS:
				ChecklistSegControl.SelectedSegment = 1;
				break;
			case ResultType.FAIL:
				ChecklistSegControl.SelectedSegment = 2;
				break;
			case ResultType.OTHER:
				ChecklistSegControl.SelectedSegment = -1;
				break;

			}

		}

		#region ICell implementation

		public void cleanCell ()
		{
			commentsTextView.WeakDelegate = null;

		}

		#endregion
	}
}