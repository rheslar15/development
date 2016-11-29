using System;
using UIKit;
using Foundation;
using System.Collections.Generic;
using ObjCRuntime;

namespace LiRoInspect.iOS
{
	public class PunchDataSource: UICollectionViewDataSource
	{
		protected string cellIdentifier = "PunchCollectionViewCell";
		public Func<nint, nint> ItemCount;
		public List<UIImage> itemsList;
		UITableViewCell parentCell = null;
		public List<byte[]> checklistImages { get; set;} 
		public PunchDataSource (List<UIImage> itemsList, UITableViewCell cell)
		{
			this.itemsList = itemsList;
			this.parentCell = cell;

		}

		public override UICollectionViewCell GetCell (UICollectionView collectionView, Foundation.NSIndexPath indexPath)
		{
			PunchCollectionViewCell cell = collectionView.DequeueReusableCell (cellIdentifier, indexPath) as PunchCollectionViewCell;
			cell.UpdateCell (collectionView, indexPath, this.itemsList[indexPath.Row],itemsList,parentCell);
			return cell;
		}

		public override nint GetItemsCount (UICollectionView collectionView, nint section)
		{
			return itemsList.Count;
		}

		public override nint NumberOfSections (UICollectionView collectionView)
		{
			return 1;
		}


	}
}




