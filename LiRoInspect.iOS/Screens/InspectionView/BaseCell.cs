using System;

using Foundation;
using UIKit;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using CoreGraphics;

namespace LiRoInspect.iOS
{
	public partial class BaseCell : UITableViewCell
	{
		public static readonly NSString Key = new NSString ("BaseCell");
		public static readonly UINib Nib;
		object locker = new object ();
		static BaseCell ()
		{
			Nib = UINib.FromName ("BaseCell", NSBundle.MainBundle);
		}

		public BaseCell (IntPtr handle) : base (handle)
		{
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
			lock (locker) 
			{
				UIImage img = new UIImage ();
				img = UIImage.LoadFromData (new NSData (Convert.ToBase64String (imageArray), NSDataBase64DecodingOptions.IgnoreUnknownCharacters));
				if (img != null) 
				{
					UIImage resizedImage = UIImage.LoadFromData (img.AsJPEG (0.0f));
					return resizedImage;
				}
				return img;
			}
		}

		public List<UIImageView> RestructureImages (List<UIImage> scrollImages,InspectionViewController parentController)
		{
			try 
			{
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
							//imageView = HighlightSelectedImage (imageView);
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
			} 
			catch (Exception ex) 
			{
				Debug.WriteLine ("Exception Occured in RestructureImages method due to " + ex.Message);
				return new List<UIImageView> ();
			}
		}

		private void AddGestureEvents ()
		{
//						try {
//							if (this.ImageLst    != null) {
//									foreach (var view in ImageLst) {
//										Action action = new Action (
//											delegate {
//												var answer = new UIActionSheet ("Do you want to Delete the Image?", null, "Cancel", "Ok", new string[1]{ "Cancel" });
//			
//												answer.Clicked += delegate (object sender, UIButtonEventArgs args) {
//													var btnarg = args.ButtonIndex;
//													if (btnarg == 0) {
//														view.RemoveFromSuperview ();
//														int index = -1;
//														for (int i = 0; i < ImageLst.Count; i++) {
//															if (view == ImageLst [i]) {
//																index = i;
//																break;
//															}
//														}
//														ImageLst.Remove (view);
//														if (index >= 0) {
//															if (this.inspectionItem != null && this.inspectionItem.photos != null && this.inspectionItem.photos.Count > 0) {
//																if (index < this.inspectionItem.photos.Count)
//																	this.inspectionItem.photos.RemoveAt (index);
//															}
//														}
//													
//														itemTableView.ReloadData ();
//													}
//												};
//												answer.ShowInView (this);
//											});
//										UILongPressGestureRecognizer gr = new UILongPressGestureRecognizer ();
//										gr.AddTarget (action);
//										if (view is UIImageView) {
//											view.AddGestureRecognizer (gr);
//											view.UserInteractionEnabled = true;
//										}
//			
//									}
//							}
//						} catch (Exception ex) {
//							Debug.WriteLine ("Exception Occured in AddGestureEvents method due to " + ex.Message);
//						}
		}
	}
}
