using System;
using Foundation;
using UIKit;
using System.Diagnostics;
using System.Collections.Generic;
using CoreGraphics;
using System.IO;
using Model;
using System.Linq;

namespace LiRoInspect.iOS
{
	public partial class GuidedPhotoCell : BaseCell,ICell
	{
		
		PunchDataSource Source;

		public GuidedPhotoCell (IntPtr handle) : base (handle)
		{
		}

		public static GuidedPhotoCell Create ()
		{
			return (GuidedPhotoCell)Nib.Instantiate (null, null) [0];
		}

		public WeakReference GuidedTableView;

		private UITableView guidedTableView {
			get {
				if (GuidedTableView == null || !GuidedTableView.IsAlive)
					return null;
				return GuidedTableView.Target as UITableView;
			}
		}

		public WeakReference CheckListItem;

		public CheckList checkListItem {
			get {
				if (CheckListItem == null || !CheckListItem.IsAlive)
					return null;
				return CheckListItem.Target as CheckList;
			}
		}

		public WeakReference ParentController;

		public InspectionViewController parentController {
			get {
				if (ParentController == null || !ParentController.IsAlive)
					return null;
				return ParentController.Target as InspectionViewController;
			}
		}

		/// <summary>
		/// Updates the guided photo cells.
		/// </summary>
		/// <param name="checkListItem">Check list item.</param>
		/// <param name="GuidedTableView">Guided table view.</param>
		/// <param name="parentController">Parent controller.</param>
		public async void UpdateGuidedPhotoCell (CheckList checkListItem, UITableView GuidedTableView, InspectionViewController parentController)
		{
			try {
				
				ResetUIView ();
				this.ParentController = new WeakReference (parentController);
				this.GuidedTableView = new WeakReference (GuidedTableView);
				this.CheckListItem = new WeakReference (checkListItem);
				LoadGuidedView ();

				guidedPhotoTkPicBtn.TouchUpInside -= guidedPhotoTkPicBtn_TouchUpInside;
				guidedPhotoTkPicBtn.TouchUpInside += guidedPhotoTkPicBtn_TouchUpInside;

				guidedPhotoTkPicBtn.SetTitleColor (UIColor.White, UIControlState.Normal);
				guidedPhotoTkPicBtn.Layer.CornerRadius = 5f;
				guidedPhotoTkPicBtn.Layer.BorderColor = UIColor.FromRGB (18, 74, 143).CGColor;
				guidedPhotoTkPicBtn.Layer.BorderWidth = 0.5f;
				guidedPhotoTkPicBtn.BackgroundColor = UIColor.FromRGB (19, 95, 160);

				parentController.buttonStyleRefresh (null);
			} catch (Exception ex) {
				Debug.WriteLine ("Exception Occured in UpdateCell method due to " + ex.Message);
			}
		}

		/// <summary>
		/// Guided photo take picture button touch up inside.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		void guidedPhotoTkPicBtn_TouchUpInside (object sender, EventArgs e)
		{
			try {
				
				ResetTheCameraImageView ();
				if (parentController != null) {
					UICameraController cameraController = parentController.Storyboard.InstantiateViewController ("UICameraController") as UICameraController;
					cameraController.ClearCamearaView ();
					cameraController.ModalPresentationStyle = UIModalPresentationStyle.FullScreen;
					cameraController.picture = this.UpdateCollectionImageView;
					if (cameraController.MediaLst != null) {
						foreach (var view in cameraController.MediaLst) {
							view.RemoveFromSuperview ();
						}	
						cameraController.MediaLst.Clear ();
					}
					if (cameraController.imagesList != null) {
						cameraController.imagesList.Clear ();
					}

					if (this.ImageLst == null) {
						this.ImageLst = new List<UIImage> ();
					}

					List<UIImageView> MediaLst = new List<UIImageView> ();
					if (this.ImageLst.Count > 0) {
					
						foreach (UIImage image in this.ImageLst) {
							UIImageView imageView = new UIImageView ();
							imageView.Image = image;
							MediaLst.Add (imageView);
						}
					}
					cameraController.MediaLst = MediaLst;
					parentController.PresentViewController (cameraController, false, null);
				}
		
			} catch (Exception ex) {
				Debug.WriteLine ("Exception Occured in BtnTakePic_TouchUpInside method due to " + ex.Message);
			}
		}

		public  void UpdateCollectionImageView (List<UIImage> images)
		{
			try {
				this.ImageLst = images;
				if (checkListItem != null) {
					checkListItem.photos = new List<byte[]> ();
					List<byte[]> cameraImages = new List<byte[]> ();
					foreach (var img in images) {
						byte[] task = ImageToByteArray (img);
						var imgArr = task;
						cameraImages.Add (imgArr);
					}
					foreach (var img in cameraImages) {
						checkListItem.photos.Add (img);
					}
				}
				if (Source == null) {

					Source = new PunchDataSource (images, this);
				} else {
					Source.itemsList = images;
				}
				imagesCollectionView.DataSource = Source;
				imagesCollectionView.ReloadData ();
				if (parentController != null) {
					parentController.buttonStyleRefresh (null);
				}
			} catch (Exception ex) {
				Debug.WriteLine ("Exception Occured in UpdateCollectionImageView method due to " + ex.Message);
			}
		}

		private void ResetTheCameraImageView ()
		{
			try {
				foreach (var view in imagesCollectionView.Subviews) {
					view.RemoveFromSuperview ();
				}
			} catch (Exception ex) {
				Debug.WriteLine ("Exception Occured in ResetTheCameraImageView method due to " + ex.Message);
			}
		}


		public void setViewVisibilty (ResultType result)
		{
			try {
				if (result == ResultType.OTHER) {
					guidedPhotoTkPicBtn.Hidden = false;
					guidedPhotoTkPicBtn.BackgroundColor = UIColor.FromRGB (18, 74, 143);
					guidedPhotoTkPicBtn.SetTitleColor (UIColor.White, UIControlState.Normal);
					guidedPhotoTkPicBtn.Enabled = true;
				} else {
					guidedPhotoTkPicBtn.Hidden = true;
				}
			} catch (Exception ex) {
				Debug.WriteLine ("Exception Occured in setViewVisibilty method due to " + ex.Message);
			}
		}

		private void ResetUIView ()
		{
			guidedOptionLabel.Text = "";
			if (imagesCollectionView != null) {
				foreach (var view in imagesCollectionView.Subviews) { 
					if (view != null) {
						if (view is UIImageView) {
							(view as UIImageView).Image = null;
						}
						view.RemoveFromSuperview ();
						view.Dispose ();
					}
				}
			}
			if (null != ImageLst) {
				ImageLst.Clear ();
				_imageLst.Clear ();
			}
		}

		private List<UIImage> _imageLst = new List<UIImage> ();

		public List<UIImage> ImageLst {
			get {
				return _imageLst;
			}
			set {
				_imageLst = value;
			}
		}

		private void LoadGuidedView ()
		{
			try {
				if (parentController != null) {
					parentController.HideTakePicture ();
					if (this.checkListItem != null) {
						guidedOptionLabel.Text = checkListItem.description;
						if (this.checkListItem.photos != null && this.checkListItem.photos.Count > 0) {
							List<UIImage> currentPassImages = new List<UIImage> ();
							foreach (var img in this.checkListItem.photos) {
								var imag = ByteArrayToImage (img);
								if (imag != null)
									currentPassImages.Add (imag);
							}
							this.ImageLst = currentPassImages;
							if (Source == null) {
								Source = new PunchDataSource (currentPassImages, this);
							} else {
								Source.itemsList = currentPassImages;
							}
							imagesCollectionView.DataSource = Source;
							imagesCollectionView.ReloadData ();
							parentController.buttonStyleRefresh (null);
						}
					}
				}

			} catch (Exception ex) {
				Debug.WriteLine ("Exception Occured in LoadGuidedView method due to " + ex.Message);
			}
		}

		/// <summary>
		/// Memory clean up code for cell.
		/// </summary>
		public void cleanCell ()
		{
			_imageLst = null;
			CheckListItem = null;
			ParentController = null;
			GuidedTableView = null;
			if (imagesCollectionView != null) {
				foreach (var imgView in imagesCollectionView.Subviews) {
					if (imgView is UIImageView) {
						(imgView as UIImageView).Image = null;
					}
					imgView.Dispose ();
					imgView.RemoveFromSuperview ();
				}

				imagesCollectionView.Source = null;
				imagesCollectionView.Delegate = null;
				imagesCollectionView.DataSource = null;
				imagesCollectionView.WeakDelegate = null;
				imagesCollectionView.WeakDataSource = null;
			}
			if (ImageLst != null) {
				ImageLst.Clear ();
			}
			if (Source != null) {
				Source.Dispose ();
				Source = null;
			}
		}
	}
}