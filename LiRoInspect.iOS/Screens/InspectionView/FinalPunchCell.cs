
using System;

using Foundation;
using UIKit;
using Model;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System.Linq;
using CoreGraphics;

namespace LiRoInspect.iOS
{
	public class FinalPunchTextViewDelegate : UITextViewDelegate
	{
		FinalPunchCell finalPunchCell;
		//UITableView itemTableView;
		public WeakReference ItemTableView;
		private UITableView itemTableView
		{
			get
			{
				if (ItemTableView ==null || !ItemTableView.IsAlive)
					return null;
				return ItemTableView.Target as UITableView;
			}
		}
		public FinalPunchTextViewDelegate(FinalPunchCell punchCell, UITableView itemTableView)
		{
			this.finalPunchCell = punchCell;
			this.ItemTableView = new WeakReference(itemTableView);
		}

		public override bool ShouldChangeText (UITextView textView, NSRange range, string text)
		{
			return (textView.Text.Length + (text.Length - range.Length) <= 300);
		}

		public override void EditingStarted (UITextView textView)
		{
			UITextView commentsTextView = textView;
			CGPoint pointInTable = new CGPoint();
			CGPoint origin = new CGPoint (commentsTextView.Frame.X, commentsTextView.Frame.Y);
			if (itemTableView != null) {
				pointInTable = commentsTextView.Superview.ConvertPointToView (origin, this.itemTableView);
				CGPoint contentOffset = this.itemTableView.ContentOffset;
				contentOffset.Y = pointInTable.Y - (commentsTextView.Frame.Height - 50);
				this.itemTableView.SetContentOffset (contentOffset, true);
			}
		}

		public override void EditingEnded (UITextView textView)
		{
			UITextView failureTextView = textView;
			failureTextView.ResignFirstResponder ();
		}

		public override void Changed (UITextView textView)
		{
			this.finalPunchCell.checkListItem.comments = textView.Text;
		}			
	}

	public partial class FinalPunchCell : UITableViewCell,ICell
	{
		public static readonly UINib Nib = UINib.FromName ("FinalPunchCell", NSBundle.MainBundle);
		public static readonly NSString Key = new NSString ("FinalPunchCell");
		private object locker;
		public CheckList checkListItem;
		public WeakReference Parent;
		private InspectionViewController parentController
		{
			get
			{
				if (Parent ==null || !Parent.IsAlive)
					return null;
				return Parent.Target as InspectionViewController;
			}
		}

		public WeakReference weakUITableView;
		private UITableView itemTableView
		{
			get
			{
				if (weakUITableView ==null || !weakUITableView.IsAlive)
					return null;
				return weakUITableView.Target as UITableView;
			}
		}

		public FinalPunchCell (IntPtr handle) : base (handle)
		{
		}

		public static FinalPunchCell Create ()
		{
			return (FinalPunchCell)Nib.Instantiate (null, null) [0];
		}

		/// <summary>
		/// Updates the Final Punch cell.
		/// </summary>
		/// <param name="parentController">Parent controller.</param>
		/// <param name="itemTableView">Item table view.</param>
		/// <param name="indexPath">Index path.</param>
		/// <param name="checkLstItem">Check lst item.</param>
		public void UpdateCell(InspectionViewController parentController, UITableView itemTableView, NSIndexPath indexPath,CheckList checkLstItem)
		{
			ResetUIView ();
			UpdateUIContent (checkLstItem);
			weakUITableView = new WeakReference( itemTableView);
			Parent = new WeakReference( parentController);


			var textDelegate = new FinalPunchTextViewDelegate (this, itemTableView);
			CommentsTextView.WeakDelegate = textDelegate;

			takePictureBtn.TouchUpInside-= takePictureBtn_TouchUpInside;
			takePictureBtn.TouchUpInside+= takePictureBtn_TouchUpInside;
			punchSegmentControl.ValueChanged-= PunchSegmentControl_ValueChanged;
			punchSegmentControl.ValueChanged+= PunchSegmentControl_ValueChanged;
			LoadPunchItemImages ();

			if (checkLstItem.Result==ResultType.PASS || checkLstItem.Result==ResultType.FAIL )
			{
				takePictureBtn.Enabled = true;
				takePictureBtn.BackgroundColor = UIColor.FromRGB (19, 95, 160);
				takePictureBtn.SetTitleColor (UIColor.White, UIControlState.Disabled);
			}
			else 
			{
				takePictureBtn.Enabled = false;
				takePictureBtn.BackgroundColor = UIColor.LightGray;
				takePictureBtn.SetTitleColor(UIColor.Black, UIControlState.Disabled);
			}
			parentController.buttonStyleRefresh (null);
		}

		/// <summary>
		/// segment control value changed.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		void PunchSegmentControl_ValueChanged (object sender, EventArgs e)
		{
			UISegmentedControl segment = sender as UISegmentedControl;
			if (segment != null && segment is UISegmentedControl) {
				takePictureBtn.Enabled = true;
				takePictureBtn.BackgroundColor = UIColor.FromRGB (19, 95, 160);
				takePictureBtn.SetTitleColor (UIColor.White, UIControlState.Disabled);

				switch (segment.SelectedSegment) {
				case 0:
					checkListItem.Result = ResultType.PASS;
					break;
				case 1:
					checkListItem.Result = ResultType.FAIL;
					break;
				default:
					//checkListItem.Result = ResultType.NA;
					break;
				}
				parentController.buttonStyleRefresh (null);
			}
		}

		private void ResetUIView()
		{
			CommentsTextView.Text = "";
			punchSegmentControl.SelectedSegment = -1;
			PunchHeading.Text = "";
			if (null != ImageLst) {
				foreach (var view in ImageLst) {
					view.Image = null;
					view.RemoveFromSuperview ();
				}
				ImageLst.Clear ();
			}
		}

		private void UpdateUIContent (CheckList checkLstItem)
		{
			PunchHeading.Font = UIFont.FromName("Helvetica", 17f);
			PunchHeading.TextColor = UIColor.FromRGB(186, 190, 194);
			takePictureBtn.Layer.CornerRadius = 5f;
			takePictureBtn.Layer.BorderColor = UIColor.FromRGB(18, 74, 143).CGColor;
			takePictureBtn.Layer.BorderWidth = 0.5f;
			CommentsTextView.Layer.CornerRadius = 5f;
			CommentsTextView.Layer.BorderColor = UIColor.FromRGB (186, 190, 194).CGColor;
			CommentsTextView.Layer.BorderWidth = 0.5f;
			this.checkListItem=checkLstItem;
			PunchHeading.Text = checkLstItem.description;
			CommentsTextView.Text = checkLstItem.comments;
			UpdateSegmentControl (checkLstItem.Result);	
		}

		private void UpdateSegmentControl(ResultType result)
		{
			switch (result) {
			case ResultType.PASS:
				punchSegmentControl.SelectedSegment = 0;
				break;
			case ResultType.FAIL:
				punchSegmentControl.SelectedSegment = 1;
				break;
			default:
				//punchSegmentControl.SelectedSegment = -1;
				break;
			}
		}

		private void LoadPunchItemImages ()
		{
			try {
				locker=new object();
				///Commented the changes for new requirements
				ResetTheCameraImageView ();
				if (this.checkListItem.photos!=null &&this.checkListItem.photos.Count > 0) {
					List<UIImage> currentinspectionItemImages = new List<UIImage> ();
					foreach (var img in this.checkListItem.photos) {
						var imag = ByteArrayToImage (img);
						currentinspectionItemImages.Add (imag);
					}
					if (currentinspectionItemImages.Count > 0) {
						var scrollViewImgs = RestructureImages (currentinspectionItemImages);
						punchImgScrollView.AddSubviews (scrollViewImgs.ToArray ());
						if (scrollViewImgs != null && scrollViewImgs.Count > 0) {
							punchImgScrollView.ContentSize = new CoreGraphics.CGSize (scrollViewImgs.Count * scrollViewImgs.FirstOrDefault ().Frame.Width + scrollViewImgs.Count * 10, 100);
						}
						if (null != ImageLst) {
							foreach(var view in ImageLst)
							{
								view.Image=null;
								view.RemoveFromSuperview();
							}
							ImageLst.Clear ();
						}
						if(ImageLst == null)
						{
							ImageLst=new List<UIImageView>();
						}

						ImageLst.AddRange (scrollViewImgs);
						AddGestureEvents ();
					}
				}
			} catch (Exception ex) {
				Debug.WriteLine ("Exception Occured in LoadPassView method due to " + ex.Message);
			}
		}

		void CommentsTextView_Changed (object sender, EventArgs e)
		{
			checkListItem.comments=CommentsTextView.Text;
		}

		void takePictureBtn_TouchUpInside (object sender, EventArgs e)
		{
			try {
				ResetTheCameraImageView ();
				UICameraController cameraController = parentController.Storyboard.InstantiateViewController ("UICameraController") as UICameraController;
				cameraController.ClearCamearaView();
				cameraController.ModalPresentationStyle = UIModalPresentationStyle.OverCurrentContext;
				cameraController.picture = this.UpdateCollectionImageView;
				if (cameraController.MediaLst != null) {
					cameraController.MediaLst.Clear ();
				}
				cameraController.MediaLst = ImageLst;
				parentController.PresentViewController (cameraController, false, null);
			} catch (Exception ex) {
				Debug.WriteLine ("Exception Occured in BtnTakePic_TouchUpInside method due to " + ex.Message);
			}
		}

		public  void UpdateCollectionImageView (List<UIImage> images)
		{
			try {
				List<byte[]> cameraImages = new List<byte[]> ();
				///Commented the changes for new requirements
				checkListItem.photos=new List<byte[]>();
				foreach (var img in images) {
					byte[] task = ImageToByteArray (img);
					var imgArr = task;
					cameraImages.Add (imgArr);
				}

				if (cameraImages.Count > 0) {
					foreach(var imag in cameraImages)
					{
						///Commented the changes for new requirements
						checkListItem.photos.Add(imag);
					}
					itemTableView.ReloadData ();
				}
				parentController.buttonStyleRefresh(null);
				if(ImageLst!=null && ImageLst.Count>0)
				{
					foreach(var view in ImageLst)
					{
						view.Image=null;
						view.RemoveFromSuperview();
					}
					ImageLst.Clear ();
				}
			} catch (Exception ex) {
				Debug.WriteLine ("Exception Occured in UpdateCollectionImageView method due to " + ex.Message);
			}
		}

		public byte[] ImageToByteArray (UIImage image)
		{
			Stream stream = image.AsJPEG ().AsStream ();
			byte[] array = ToByteArray (stream);
			return array;
		}

		private  byte[] ToByteArray (Stream stream)
		{
			stream.Position = 0;
			byte[] buffer = new byte[stream.Length];
			for (int totalBytesCopied = 0; totalBytesCopied < stream.Length;)
				totalBytesCopied += stream.Read (buffer, totalBytesCopied, Convert.ToInt32 (stream.Length) - totalBytesCopied);
			return buffer;
		}

		public UIImage ByteArrayToImage (byte[] imageArray)
		{
			lock (locker) {
				UIImage img = new UIImage ();
				img = UIImage.LoadFromData (new NSData (Convert.ToBase64String (imageArray), NSDataBase64DecodingOptions.IgnoreUnknownCharacters));
				if (img != null) {
					UIImage resizedImage = UIImage.LoadFromData (img.AsJPEG (0.0f));
					return resizedImage;
				}
				return img;
			}
		}

		private void ResetTheCameraImageView ()
		{
			try 
			{
				foreach(var view in punchImgScrollView.Subviews)
				{
					view.RemoveFromSuperview();
				}
				parentController.buttonStyleRefresh (null);
			} 
			catch (Exception ex) 
			{
				Debug.WriteLine ("Exception Occured in ResetTheCameraImageView method due to " + ex.Message);
			}
		}
		private List<UIImageView> _imageLst = new List<UIImageView> ();
		public List<UIImageView> ImageLst 
		{
			get 
			{
				return _imageLst;
			}
			set 
			{
				_imageLst = value;
			}
		}

		public List<UIImageView> RestructureImages (List<UIImage> scrollImages)
		{
			try {
				nfloat h = 100.0f;
				nfloat w = 100.0f;
				nfloat padding = 10.0f;
				List<UIImageView> scrollImageCollection = new List<UIImageView> ();
				for (int i = 0; i < scrollImages.Count; i++) {
					UIImageView imageView = new UIImageView ();
					imageView.Frame = new CGRect (padding * (i + 1) + (i * w), 0, w, h);
					imageView.Image = scrollImages [i];

					Action tapAction = new Action (
						delegate {
							imageView = HighlightSelectedImage (imageView);
							ImagePeviewViewController imagePreviewController = parentController.Storyboard.InstantiateViewController ("ImagePeviewViewController") as ImagePeviewViewController;
							imagePreviewController.ModalPresentationStyle = UIModalPresentationStyle.FullScreen;
							imagePreviewController.previewImage = imageView;
							parentController.PresentViewController (imagePreviewController, false, null);
						});

					UITapGestureRecognizer tap = new UITapGestureRecognizer ();
					tap.AddTarget (tapAction);
					imageView.AddGestureRecognizer (tap);
					imageView.UserInteractionEnabled = true;
					scrollImageCollection.Add (imageView);

				}

				return scrollImageCollection;
			} catch (Exception ex) {
				Debug.WriteLine ("Exception Occured in RestructureImages method due to " + ex.Message);
				return new List<UIImageView> ();
			}
		}
		private void AddGestureEvents ()
		{
			try {
				if (this.checkListItem != null) {
					foreach (var view in ImageLst) {

						Action action = new Action (
							delegate {
								var answer = new UIActionSheet ("Do you want to Delete the Image?", null, "Cancel", "Ok", new string[1]{ "Cancel" });

								answer.Clicked += delegate (object sender, UIButtonEventArgs args) {
									var btnarg = args.ButtonIndex;
									if (btnarg == 0) {
										view.RemoveFromSuperview ();
										int index = -1;
										for (int i = 0; i < ImageLst.Count; i++) {
											if (view == ImageLst [i]) {
												index = i;
												break;
											}
										}

										ImageLst.Remove (view);

										if (index >= 0) {
											if (this.checkListItem != null && this.checkListItem.photos != null && this.checkListItem.photos.Count > 0) {
												if (index < this.checkListItem.photos.Count)
													this.checkListItem.photos.RemoveAt (index);
											}
											///Commented the changes for new requirements
//											if (this.inspectionItem != null && this.inspectionItem.photos != null && this.inspectionItem.photos.Count > 0) {
//												if (index < this.inspectionItem.photos.Count)
//													this.inspectionItem.photos.RemoveAt (index);
//											}												
										}
										itemTableView.ReloadData ();
									}
								};
								answer.ShowInView (this);
							});
						UILongPressGestureRecognizer gr = new UILongPressGestureRecognizer ();
						gr.AddTarget (action);
						if (view is UIImageView) {
							view.AddGestureRecognizer (gr);
							view.UserInteractionEnabled = true;
						}
					} 
				}
			} catch (Exception ex) {
				Debug.WriteLine ("Exception Occured in AddGestureEvents method due to " + ex.Message);
			}
		}

		private UIImageView currentSelectedImageView;

		public UIImageView HighlightSelectedImage (UIImageView imageView)
		{
			if (currentSelectedImageView != null && currentSelectedImageView.Layer!=null) 
			{
				currentSelectedImageView.Layer.CornerRadius = 0.0f;
				currentSelectedImageView.Layer.BorderColor = UIColor.FromRGB (0, 0, 0).CGColor;
				currentSelectedImageView.Layer.BorderWidth = 0.0f;
				currentSelectedImageView.BackgroundColor = UIColor.Clear;
			}

			if (imageView.Layer != null) 
			{
				imageView.Layer.CornerRadius = 5f;
				imageView.Layer.BorderColor = UIColor.FromRGB (18, 74, 143).CGColor;
				imageView.Layer.BorderWidth = 0.5f;
			}
			imageView.BackgroundColor = UIColor.White;
			currentSelectedImageView = imageView;
			return imageView;
		}

		public void cleanCell()
		{
			try{
			locker = null;
			takePictureBtn.TouchUpInside-= takePictureBtn_TouchUpInside;
			punchSegmentControl.ValueChanged-= PunchSegmentControl_ValueChanged;
			if (null != ImageLst) {
				foreach(var view in ImageLst)
				{
					view.RemoveFromSuperview();
				}
				ImageLst.Clear ();
			}
			if (punchImgScrollView.Subviews != null && punchImgScrollView.Subviews.Count() > 0) 
			{
				foreach (var imgView in punchImgScrollView.Subviews) {
					if (imgView is UIImageView) {
						(imgView as UIImageView).Image = null;
					}
					imgView.Dispose ();
					imgView.RemoveFromSuperview ();
				}
			}
			}
			catch(Exception ex) {
				Debug.WriteLine ("Exception Occured in Final punch Clean cell method due to " + ex.Message);
			}
		}
	}
}