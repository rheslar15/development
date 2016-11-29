using System;
using UIKit;
using System.Collections.Generic;
using CoreGraphics;
using CoreAnimation;
using Foundation;
//using LiRoInspect.SharedInterfaces.Model;
using System.Linq;
using Model;

namespace LiRoInspect.iOS
{
	public class InspectionSpaceSource: UITableViewSource
	{
		ITraversible selSequence;
		protected string cellIdentifier = "SpaceCell";
		List<Model.Space> spaces;

		public WeakReference Parent;
		private UIViewController parentController
		{
			get
			{
				if (Parent ==null || !Parent.IsAlive)
					return null;
				return Parent.Target as UIViewController;
			}
		}

		public WeakReference weakUITableView;
		private UITableView optable
		{
			get
			{
				if (weakUITableView ==null || !weakUITableView.IsAlive)
					return null;
				return weakUITableView.Target as UITableView;
			}
		}

		public InspectionSpaceSource(ITraversible selSequence, UIViewController parentController,UITableView optable)
		{
			this.selSequence = selSequence;
			if (selSequence is ILevel) {
				spaces = (selSequence as ILevel).getSpaces ();
			} else {
				
			}
			Parent = new WeakReference( parentController);
			weakUITableView = new WeakReference( optable);
			//this.optable = optable;
		}

		public override UITableViewCellEditingStyle EditingStyleForRow (UITableView tableView, NSIndexPath indexPath)
		{
			return UITableViewCellEditingStyle.None;
		}

		public override nint NumberOfSections (UITableView tableView)
		{  
			return 1;
		}

		public override nint RowsInSection (UITableView tableview, nint section)
		{
			if (spaces != null) {
				return spaces.Count;
			}
			return 0;
		}

		public override UITableViewCell GetCell (UITableView tableView, Foundation.NSIndexPath indexPath)
		{
			SpaceCell cell = tableView.DequeueReusableCell (cellIdentifier) as SpaceCell;
			cell.UpdateCell (spaces.ElementAt(indexPath.Row));//.name,spaces[indexPath.Row].isSelected,spaces[indexPath.Row].id);
			return cell;
		}
		 
		public override void RowSelected (UITableView tableView, Foundation.NSIndexPath indexPath)
		{


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

