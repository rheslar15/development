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
	public class InspectionLevelSource : UITableViewSource
	{
		List<Model.Level> Levels;
		ITraversible selSequence;
		protected string cellIdentifier = "LevelSelectCell";

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

		public InspectionLevelSource(ITraversible selSequence, UIViewController parentController,UITableView optable)
		{
			this.selSequence = selSequence;
			if (selSequence is ISequence) {
				Levels = (selSequence as ISequence).Levels;
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
			if (Levels != null) {
				return Levels.Count;
			}
			return 0;
		}

		public override UITableViewCell GetCell (UITableView tableView, Foundation.NSIndexPath indexPath)
		{
			LevelSelectCell cell = tableView.DequeueReusableCell (cellIdentifier) as LevelSelectCell;


			cell.UpdateCell (Levels.ElementAt(indexPath.Row));//.name,spaces[indexPath.Row].isSelected,spaces[indexPath.Row].id);
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

