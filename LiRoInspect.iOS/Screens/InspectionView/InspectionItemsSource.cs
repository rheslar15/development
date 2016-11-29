using System;
using UIKit;
using System.Collections.Generic;
using CoreGraphics;
using CoreAnimation;
using Foundation;
using System.Threading.Tasks;
using System.Linq;
using Model;
using System.Diagnostics;

namespace LiRoInspect.iOS
{
	public class InspectionItemSource: UITableViewSource
	{
		public InspectionItemSource ()
		{
		}

		protected const string cellGuidedIdentifier = "GuidedPhotoCell";
		protected const string cellFinalPunchIdentifier = "FinalPunchCell";
		protected InspectionViewController parentController;
		protected const string cellIdentifier = "ItemCell";
		private List<CheckList> _checkListItems = null;

		public List<CheckList> checkListItems { 
			get{ return _checkListItems; }
				
			set { 
				_checkListItems = value;
				if (CheckListCommentsActiveItems == null) {
					CheckListCommentsActiveItems = new List<bool> ();
				}

				foreach (var item in checkListItems) {
						
					CheckListCommentsActiveItems.Add (false);

				}
					


			}
		}

		public List<bool> CheckListCommentsActiveItems { get; set; }

		public UITableView optable { get; set;}



		public InspectionItemSource (Option selectedOption, InspectionViewController parentController, UITableView optable, bool IsInSearchForCheckList)
		{
			this.parentController = parentController;
			this.optable = optable;
			if (!IsInSearchForCheckList) {
				this.checkListItems = selectedOption.getCheckListItems ();
				this.parentController.SetCheckListItems (this.checkListItems);
			} else {
				

			}
		}



		public override nint NumberOfSections (UITableView tableView)
		{ 
			return 1;
		}

		public override nint RowsInSection (UITableView tableview, nint section)
		{
			int count = checkListItems.Count;
			return count;

		}

		public override UITableViewCell GetCell (UITableView tableView, Foundation.NSIndexPath indexPath)
		{         
			// change the defualt result to "N/A"
			if (this.checkListItems [indexPath.Row].itemType == ItemType.GuidedPicture) {
				this.checkListItems [indexPath.Row].Result = ResultType.NA;
				return GetGuidedPictureCell (tableView, indexPath);
			} else if (this.checkListItems [indexPath.Row].itemType == ItemType.PunchList) {
				return GetFinalPunchListCell (tableView, indexPath);
			} else {
				


				return GetItemCell (tableView, indexPath);
			}
		}

		public override UITableViewCellEditingStyle EditingStyleForRow (UITableView tableView, NSIndexPath indexPath)
		{
			return UITableViewCellEditingStyle.None;
		}

		public UITableViewCell GetFinalPunchListCell (UITableView tableView, Foundation.NSIndexPath indexPath)
		{
			FinalPunchCell cell = tableView.DequeueReusableCell (cellFinalPunchIdentifier) as FinalPunchCell;
			cell.UpdateCell (this.parentController, tableView, indexPath, this.checkListItems [indexPath.Row]);
			return cell;
		}

		public UITableViewCell GetItemCell (UITableView tableView, Foundation.NSIndexPath indexPath)
		{
			ItemCell cell = tableView.DequeueReusableCell (cellIdentifier) as ItemCell;
			cell.UpdateCell (this.parentController, tableView, indexPath, this.checkListItems [indexPath.Row]);
			return cell;
		}

		public UITableViewCell GetGuidedPictureCell (UITableView tableView, Foundation.NSIndexPath indexPath)
		{
			GuidedPhotoCell guidedcell = tableView.DequeueReusableCell (cellGuidedIdentifier) as GuidedPhotoCell;
			try {
				guidedcell.UpdateGuidedPhotoCell (checkListItems [indexPath.Row], tableView, parentController);
			} catch (Exception ex) {
				Debug.WriteLine (ex.Message);
			}
			return guidedcell;
		}



		public override void WillDisplay (UITableView tableView, UITableViewCell cell, NSIndexPath indexPath)
		{
//			if(checkListItems[indexPath.Row].itemType==ItemType.GuidedPicture){
//				GuidedPhotoCell cellGuidedOP = cell as GuidedPhotoCell;
//				cellGuidedOP.setViewVisibilty (cellGuidedOP.inspectionItem.Result);
//			}
//			else 
//				if(checkListItems[indexPath.Row].itemType==ItemType.PunchList){
////				GuidedPhotoCell cellGuidedOP = cell as GuidedPhotoCell;
////				cellGuidedOP.setViewVisibilty (cellGuidedOP.inspectionItem.result);
//			}
//			else
//			{
//				ItemCell itemCell = cell as ItemCell;
//				if(itemCell!=null)
//					itemCell.setViewVisibilty (itemCell.inspectionItem.result);
//			}



		}

		public override nfloat GetHeightForRow (UITableView tableView, NSIndexPath indexPath)
		{
			bool CheckListItemActive = false;
			var Row = indexPath.Row;

			var Source = tableView.Source as InspectionItemSource;

			if (Source != null) {
				if (Source.CheckListCommentsActiveItems [Row] == true) {
					CheckListItemActive = true;
				}
			}

			CheckList item = checkListItems.ElementAt (indexPath.Row);

			if (item.itemType == ItemType.GuidedPicture) {
				return 180;
			} else {
				if (item.itemType == ItemType.CheckListItem) {
					// changed to 150
					if (!CheckListItemActive) {
						return 150;
					} else
						return 260;
				} else {
					return 260;
				}
			}
		}

		public override void CellDisplayingEnded (UITableView tableView, UITableViewCell cell, NSIndexPath indexPath)
		{
//			if(cell is GuidedPhotoCell)
//			{
//				(cell as GuidedPhotoCell).cleanCell ();
//			}
//			else if(cell is FinalPunchCell)
//			{
//				(cell as FinalPunchCell).cleanCell();
//			}
//			else if(cell is ItemCell)//not required
//			{
//				(cell as ItemCell).cleanCell();
//			}
//			foreach(var subCell in cell.Subviews)
//			{
//				subCell.Dispose ();
//				subCell.RemoveFromSuperview ();
//			}
		}

	}

}
