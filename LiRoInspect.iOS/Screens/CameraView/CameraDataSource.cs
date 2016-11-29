//using System;
//using UIKit;
//using Foundation;
//using System.Collections.Generic;
//using ObjCRuntime;
//
//namespace LiRoInspect.iOS
//{
//	public class CameraDataSource: UICollectionViewDataSource
//	{
//		protected string cellIdentifier = "CollectionViewCell";
//		public Func<nint, nint> ItemCount;
//		List<UIImage> itemsList;
//		UICameraController cameraController = null;
//
//		public CameraDataSource (List<UIImage> itemsList, UICameraController cameraController)
//		{
//			this.itemsList = itemsList;
//			this.cameraController = cameraController;
//		}
//
//		NSString cellId;
//
//		public override UICollectionViewCell GetCell (UICollectionView collectionView, Foundation.NSIndexPath indexPath)
//		{
//			CollectionViewCell cell = collectionView.DequeueReusableCell (cellIdentifier, indexPath) as CollectionViewCell;
//			cell.CameraController = this.cameraController;
//			cell.UpdateCell (collectionView, indexPath, this.itemsList[indexPath.Row],itemsList);
//			return cell;
//		}
//
//		public override nint GetItemsCount (UICollectionView collectionView, nint section)
//		{
//			return itemsList.Count;
//		}
//
//		public override nint NumberOfSections (UICollectionView collectionView)
//		{
//			return 1;
//		}
//			
//
//	}
//}
//
//
//
//



using System;
using UIKit;
using Foundation;
using System.Collections.Generic;
using ObjCRuntime;

namespace LiRoInspect.iOS
{
	public class CameraDataSource: UICollectionViewDataSource
	{
		protected string cellIdentifier = "CollectionViewCell";
		public Func<nint, nint> ItemCount;
		public List<UIImage> itemsList;

		public WeakReference _weakCameraController;
		private UICameraController cameraController
		{
			get
			{
				if (_weakCameraController ==null || !_weakCameraController.IsAlive)
					return null;
				return _weakCameraController.Target as UICameraController;
			}
		}

		public WeakReference _weakInspectionController;
		private InspectionViewController inspectionController
		{
			get
			{
				if (_weakInspectionController ==null || !_weakInspectionController.IsAlive)
					return null;
				return _weakInspectionController.Target as InspectionViewController;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="LiRoInspect.iOS.CameraDataSource"/> class.
		/// </summary>
		/// <param name="itemsList">Items list.</param>
		/// <param name="cameraController">Camera controller.</param>
		public CameraDataSource (List<UIImage> itemsList, UIViewController cameraController)
		{
			this.itemsList = itemsList;
			if (cameraController.GetType () == typeof(UICameraController)) {
				_weakCameraController = new WeakReference( cameraController);

			} else if (cameraController.GetType () == typeof(InspectionViewController)) {
				_weakInspectionController = new WeakReference( cameraController);
			}
		}


		//NSString cellId;

		/// <summary>
		/// Gets the cell.
		/// </summary>
		/// <returns>The cell.</returns>
		/// <param name="collectionView">Collection view.</param>
		/// <param name="indexPath">Index path.</param>
		public override UICollectionViewCell GetCell (UICollectionView collectionView, Foundation.NSIndexPath indexPath)
		{
			CollectionViewCell cell = collectionView.DequeueReusableCell (cellIdentifier, indexPath) as CollectionViewCell;
			//	cell.CameraController = this.cameraController;
			if(this.cameraController != null)
				cell.UpdateCell (collectionView, indexPath, this.itemsList[indexPath.Row],itemsList,this.cameraController);

			else if (this.inspectionController != null)
				cell.UpdateCell (collectionView, indexPath, this.itemsList[indexPath.Row],itemsList,this.inspectionController);
			return cell;
		}

		/// <summary>
		/// Gets the items count.
		/// </summary>
		/// <returns>The items count.</returns>
		/// <param name="collectionView">Collection view.</param>
		/// <param name="section">Section.</param>
		public override nint GetItemsCount (UICollectionView collectionView, nint section)
		{
			return itemsList.Count;
		}

		/// <summary>
		/// Numbers the of sections.
		/// </summary>
		/// <returns>The of sections.</returns>
		/// <param name="collectionView">Collection view.</param>
		public override nint NumberOfSections (UICollectionView collectionView)
		{
			return 1;
		}
		/// <summary>
		/// Dispose the specified disposing.
		/// </summary>
		/// <param name="disposing">If set to <c>true</c> disposing.</param>
		protected override void Dispose (bool disposing)
		{
			base.Dispose (disposing);
		}
	}
}