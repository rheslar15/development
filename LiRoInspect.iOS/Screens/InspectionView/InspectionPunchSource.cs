using System;
using UIKit;
using System.Collections.Generic;
using CoreGraphics;
using CoreAnimation;
using Foundation;
using System.Threading.Tasks;
using System.Linq;
using Model;

namespace LiRoInspect.iOS
{
	public class InspectionPunchSource: UITableViewSource
	{
		public InspectionPunchSource ()
		{
		}

		protected InspectionViewController parentController;
		protected string cellIdentifier = "PunchCell";
		public List<Model.Punch> punchItems;

		public InspectionPunchSource(InspectionViewController parentController,UITableView optable,List<Model.Punch> punchItems)
		{
			this.parentController = parentController;
			this.punchItems = punchItems;
		}

		public override nint NumberOfSections (UITableView tableView)
		{ 
			return 1;
		}

		public override nint RowsInSection (UITableView tableview, nint section)
		{
			int count = punchItems.Count;
			return count;
		}
			

		public override void CommitEditingStyle (UITableView tableView, UITableViewCellEditingStyle editingStyle, NSIndexPath indexPath)
		{
			if(editingStyle == UITableViewCellEditingStyle.Delete)
			{
				Model.Punch punch = punchItems.ElementAt(indexPath.Row);
				this.punchItems.Remove (punch);
				tableView.ReloadData();
			}
		}

		public override UITableViewCellEditingStyle EditingStyleForRow (UITableView tableView, NSIndexPath indexPath)
		{
			return UITableViewCellEditingStyle.None;
		}

		public  override UITableViewCell GetCell (UITableView tableView, Foundation.NSIndexPath indexPath)
		{
			PunchCell cell = tableView.DequeueReusableCell (cellIdentifier) as PunchCell;
			cell.UpdateCell (this.parentController, tableView, indexPath, punchItems);
			return cell;
		}

		public override nfloat GetHeightForRow (UITableView tableView, NSIndexPath indexPath)
		{
			return 250;
		}

		public override bool CanEditRow (UITableView tableView, NSIndexPath indexPath)
		{
			return true;
		}

		public override void CellDisplayingEnded (UITableView tableView, UITableViewCell cell, NSIndexPath indexPath)
		{
			foreach(var subCell in cell.Subviews)
			{
				subCell.Dispose ();
				subCell.RemoveFromSuperview ();
			}
		}
	}
		
}

