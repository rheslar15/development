using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;
using System.Drawing;
using System.Collections.Generic;
using CoreGraphics;

namespace LiRoInspect.iOS
{
	partial class CollectionViewCell : UICollectionViewCell
	{

		UICollectionView CollectionView;
		public UICameraController CameraController { get; set; }
		UICameraController cameraController=null ;
		InspectionViewController inspectionController=null;
		int count;
		public bool camera=false;
		public CollectionViewCell (IntPtr handle) : base (handle)
		{
			gr = new UILongPressGestureRecognizer (AddGestureEvents);
			tb = new UITapGestureRecognizer (tapGestureAction);
		}
		List<UIImage> itemsList;
		NSIndexPath indexpath;

		UILongPressGestureRecognizer gr;

		UITapGestureRecognizer tb;

		/// <summary>
		/// Updates the cell.
		/// </summary>
		/// <param name="collectionView">Collection view.</param>
		/// <param name="indexpath">Indexpath.</param>
		/// <param name="image">Image.</param>
		/// <param name="itemsList">Items list.</param>
		/// <param name="controller">Controller.</param>
		public void UpdateCell(UICollectionView collectionView,NSIndexPath indexpath,UIImage image,List<UIImage> itemsList,UIViewController controller)
		{
			ImageView.RemoveGestureRecognizer (gr);
			ImageView.RemoveGestureRecognizer (tb);

			ImageView.AddGestureRecognizer(gr);
			ImageView.AddGestureRecognizer (tb);
			ImageView.UserInteractionEnabled = true;


			if (controller.GetType () == typeof(UICameraController)) {
				this.cameraController = controller as UICameraController;
			}
			else if (controller.GetType () == typeof(InspectionViewController))
				this.inspectionController = controller as InspectionViewController;

			this.itemsList = itemsList;
			count = (itemsList.Count)-1;
			ImageView.Image = image;
			this.indexpath = indexpath;

			this.CollectionView = collectionView;
		}

		/// <summary>
		/// Gesture Action(On tap)
		/// This is used here to display the image which is selected from the collection view to imageShow View.
		/// </summary>
		private void tapGestureAction()
		{
			Action tapAction = new Action (
				delegate {
					if(ImageView!=null){
						if(cameraController!=null)
						{
							
							this.cameraController.UpdateCameraImageView(ImageView.Image);
						}
						else
						{
							this.inspectionController.UpdateLocationIDImageView(ImageView.Image);
						}
					}
				});

			UITapGestureRecognizer tb = new UITapGestureRecognizer ();
			tb.AddTarget (tapAction);
			ImageView.AddGestureRecognizer (tb);
		}

		/// <summary>
		/// Adds the gesture events.
		/// Its basically used here for deletion.
		/// </summary>
		/// <param name="senderRecognizer">Sender recognizer.</param>
		public void AddGestureEvents (UILongPressGestureRecognizer senderRecognizer)
		{

			if (senderRecognizer.State == UIGestureRecognizerState.Began) {
				int currentIndex =	(indexpath.Row);
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
					CollectionView.ReloadData ();
					if(cameraController!=null)
					{
						cameraController.cameraPictureAvailable(count);
					} 
					else 
					{
						List<UIImageView> mediaList = new List<UIImageView> ();
						foreach (UIImage image in this.itemsList) 
						{
							UIImageView imgV = new UIImageView ();
							imgV.Image = image;
							mediaList.Add (imgV);
						}
							
						inspectionController.MediaLst = mediaList;
						inspectionController.isAnyPictureAvailable (count);
					}
				}
			}
		}
	}
}