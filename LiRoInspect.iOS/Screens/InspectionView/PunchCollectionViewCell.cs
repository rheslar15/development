using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;
using System.Drawing;
using System.Collections.Generic;
using CoreGraphics;

namespace LiRoInspect.iOS
{
	partial class PunchCollectionViewCell : UICollectionViewCell
	{
		UICollectionView CollectionView;
		public UICameraController CameraController { get; set; }
		public bool camera=false;
		public PunchCollectionViewCell (IntPtr handle) : base (handle)
		{
			gr = new UILongPressGestureRecognizer (AddGestureEvents);
			tb = new UITapGestureRecognizer (tapGestureAction);
		}
		List<UIImage> itemsList;
		NSIndexPath indexpath;
		UITableViewCell parentCell;
		UILongPressGestureRecognizer gr;

		UITapGestureRecognizer tb;

		public void UpdateCell(UICollectionView collectionView,NSIndexPath indexpath,UIImage image,List<UIImage> itemsList,UITableViewCell cell)
		{

			ImageView.AddGestureRecognizer(gr);
			ImageView.AddGestureRecognizer (tb);
			ImageView.UserInteractionEnabled = true;


			this.itemsList = itemsList;

			ImageView.Image = image;
			this.indexpath = indexpath;
			this.parentCell = cell;
			this.CollectionView = collectionView;

		}

		private void tapGestureAction()
		{
			Action tapAction = new Action (
				delegate {
					{
						if(this.parentCell is PunchCell)
						{
							ImagePeviewViewController imagePreviewController = (this.parentCell as PunchCell).parentController.Storyboard.InstantiateViewController ("ImagePeviewViewController") as ImagePeviewViewController;
						imagePreviewController.ModalPresentationStyle = UIModalPresentationStyle.FullScreen;
						imagePreviewController.previewImage = ImageView;

							(this.parentCell as PunchCell).parentController.PresentViewController (imagePreviewController, false, null);
						}
						else if(this.parentCell is GuidedPhotoCell)
						{
							ImagePeviewViewController imagePreviewController = (this.parentCell as GuidedPhotoCell).parentController.Storyboard.InstantiateViewController ("ImagePeviewViewController") as ImagePeviewViewController;
							imagePreviewController.ModalPresentationStyle = UIModalPresentationStyle.FullScreen;
							imagePreviewController.previewImage = ImageView;
							(this.parentCell as GuidedPhotoCell).parentController.PresentViewController (imagePreviewController, false, null);
						}
					}
							
				});

			UITapGestureRecognizer tb = new UITapGestureRecognizer ();
			tb.AddTarget (tapAction);
			ImageView.AddGestureRecognizer (tb);

		}

		public void AddGestureEvents (UILongPressGestureRecognizer senderRecognizer)
		{

			if (senderRecognizer.State == UIGestureRecognizerState.Began) {
				int currentIndex =	indexpath.Row;
				int buttonClicked = -1;
				UIAlertView alert1 = new UIAlertView (@"Alert", @"Are you sure you want to delete?", null, NSBundle.MainBundle.LocalizedString ("Cancel", "Cancel"), NSBundle.MainBundle.LocalizedString ("OK", "OK"));
				alert1.Show ();

				alert1.Clicked += (sender, buttonArgs) => {
					buttonClicked = (int)buttonArgs.ButtonIndex;
				};

				//Wait for a button press.
				while (buttonClicked == -1) {
					NSRunLoop.Current.RunUntil (NSDate.FromTimeIntervalSinceNow (0.5));
				}

				if (buttonClicked == 1) {
					this.itemsList.RemoveAt (currentIndex);
					if (parentCell is GuidedPhotoCell) {
						(parentCell as GuidedPhotoCell).checkListItem.photos.RemoveAt (currentIndex);
						if ((parentCell as GuidedPhotoCell).parentController != null) {
							(parentCell as GuidedPhotoCell).parentController.buttonStyleRefresh (null);
						}
					}
					else if (parentCell is PunchCell) {
						(parentCell as PunchCell).punch.punchImages.RemoveAt(currentIndex);
						PunchCell.IsPunchValueChanged=true;
					}
					CollectionView.ReloadData ();
				}

			}

		}

	}

}

