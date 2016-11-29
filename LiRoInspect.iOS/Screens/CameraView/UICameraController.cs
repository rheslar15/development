using System;
using System.CodeDom.Compiler;
using System.Threading.Tasks;
using AVFoundation;
using CoreGraphics;
using CoreMedia;
using CoreVideo;
using Foundation;
using UIKit;
using Xamarin.Media;
using MediaPlayer;
using System.IO;

//using MonoTouch.Dialog;
using System.Collections.Generic;
using iTextSharp.text;
using System.Drawing;
using System.Diagnostics;


namespace LiRoInspect.iOS
{
	public delegate void CamerapictureTaken (UIImageView img);
	public delegate void PictureCollectionDelegate (List<UIImageView> img);
	public delegate void PictureDelegate (List<UIImage> img);
	public delegate void MemoryWarningDelegate();

	public partial  class UICameraController : BaseViewController
	{
		void HandleMemoryWarningDelegate()
		{
			#if DEBUG
			Debug.WriteLine("UICameraController - DidReceiveMemoryWarning - HandleMemoryWarningDelegate");
#endif
			memoryWarningDelegate = null;

			NSUrlCache.SharedCache.RemoveAllCachedResponses();
			GC.SuppressFinalize(this);
			GC.Collect();
			UIAlertView alert1 = new UIAlertView(@"Warning", @"Not enough memory to load any more photos, please Submit inspection ", null, NSBundle.MainBundle.LocalizedString("OK", "OK"));

			alert1.Show();

			FinishButton_TouchUpInside(new Object(), new EventArgs());
		}

		#region implemented abstract members of BaseViewController

		public override void updateNotifyCount (int txt, bool fromSync)
		{
			//TodO
		}

		public override void updateSyncCount (int txt)
		{
			//TodO
		}

		#endregion

		public CamerapictureTaken pictureTaken;
		public PictureCollectionDelegate pictureCollection;
		public PictureDelegate picture; 
		public UIImageView image = new UIImageView (UIImage.FromBundle ("imagePhoto2.png"));
		private UIImageView currentSelectedImageView;
		CameraDataSource Source;
		public List<UIImage> imagesList=new List<UIImage>();
		CollectionViewCell collectionViewCell;
		public UIImageView cameraImageView { get; set; }
		CameraController cameraController = null;
		public MemoryWarningDelegate memoryWarningDelegate;

		public UICameraController (IntPtr handle) : base (handle)
		{
			memoryWarningDelegate = HandleMemoryWarningDelegate;
		}
		public void UpdateCameraImageView(UIImage img)
		{
			if (cameraImageView != null) {
				cameraImageView.Image = img;
			}
			if (cameraView != null) {
				cameraView.Image = img;
			}
		}



		private UIAlertView errorAlert;

		private void ShowUnsupported ()
		{
			if (this.errorAlert != null) {
				this.errorAlert.Dispose ();
			}
			this.errorAlert = new UIAlertView ("Device unsupported", "Your device does not support this feature",
				null, "OK");
			errorAlert.Clicked += ErrorAlert_Clicked;
			this.errorAlert.Show ();
		}

		void ErrorAlert_Clicked (object sender, UIButtonEventArgs e)
		{
			this.DismissViewControllerAsync (false);
		}

		private List<UIImageView> _mediaLst = new List<UIImageView> ();

		public List<UIImageView> MediaLst {
			get {
				return _mediaLst;
			}
			set {
				_mediaLst = value;
			}
		}

		protected InspectionViewController parentController;

		async void  HandleTouchUpInside (object sender, EventArgs ea)
		{
			try
			{
				if (imagesList.Count <= ImageLimit)
				{
					
					#if DEBUG
				var Process1 = Process.GetCurrentProcess();

					if (Process1 != null)
					{
						Debug.WriteLine(string.Format("memory allocated by HandleTouchUpInside before PresentViewController = {0}", Process1.WorkingSet64));
					}
#endif

						cameraController = new CameraController();
						cameraController.ModalPresentationStyle = UIModalPresentationStyle.OverCurrentContext;
						cameraController.pictureTaken = this.CamerapictureTaken;
						this.PresentViewController(cameraController, false, null);




				}
				else {
					UIAlertView alert = new UIAlertView("Maximum", "Maximum Images were added to the list", null, "OK");
					alert.Show();
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Exception Occured in HandleTouchUpInside method due to " + ex.Message);
			}
		}
	
		public void cameraPictureAvailable(int count)
		{
			try
			{
				if (count == 0)
				{
					cameraView.Image = null;
					cameraImage.Hidden = false;
				}
				else
					cameraView.Image = imagesList[imagesList.Count - 1];
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Exception Occured in cameraPictureAvailable method due to " + ex.Message);
			}
		}
	
		public async void CamerapictureTaken (UIImageView image)
		{
			try
			{
				Debug.WriteLine("camera picture taken");
				cameraView.Image = image.Image;

				var Data = cameraView.Image.AsJPEG(0.0f);

				if (Data != null)
				{

					UIImage resizedImage = UIImage.LoadFromData(Data);

					image.Image.Dispose();
					image.Image = null;
					//cameraView.Image = resizedImage.Scale(new CGSize() { Height = 120, Width = 120 });
					image.Image = resizedImage;

					cameraView.Image.Dispose();
					cameraView.Image = null;
					cameraView.Image = resizedImage;
					Data.Dispose();
					Data = null;
				}

				cameraImage.Hidden = true;
				imagesList.Add(image.Image);
				imageCollectionView.ReloadData();

	
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Exception Occured in CamerapictureTaken method due to " + ex.Message);
			}

		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();


			if (Source != null)
			{
				Source.Dispose();
				Source = null;
			}
			imageCollectionView.DataSource = null;
			if (cameraView.Image != null)
			{
				cameraView.Image.Dispose();
				cameraView.Image = null;
			}
			
			cameraView.Layer.CornerRadius = 5f;
			cameraView.Layer.BorderColor = UIColor.LightGray.CGColor;
			cameraView.Layer.BorderWidth = 0.5f;
			cameraView.Layer.ShadowColor = UIColor.LightGray.CGColor;
			cameraView.Layer.ShadowOpacity = 0.5f;
			cameraView.Layer.ShadowRadius = 1.0f;
			cameraView.Layer.ShadowOffset = new System.Drawing.SizeF (1f, 1f);
			cameraView.Layer.MasksToBounds = false;
			takePictureButton.TouchUpInside -= HandleTouchUpInside;
			takePictureButton.TouchUpInside += HandleTouchUpInside;
			finishButton.TouchUpInside -= FinishButton_TouchUpInside;
			finishButton.TouchUpInside += FinishButton_TouchUpInside;
			imageCollectionView.BackgroundColor = UIColor.Clear;
			try
			{
				Debug.WriteLine("UICameraController - line 226");
				if (MediaLst != null && MediaLst.Count > 0)
				{
					cameraView.Image = MediaLst[MediaLst.Count - 1].Image;

					cameraImage.Hidden = true;
					if (imagesList != null)
					{
						imagesList.Clear();
					}

					foreach (var img in MediaLst)
					{
						imagesList.Add(img.Image);
					}
					MediaLst.Clear();
				}
				Source = new CameraDataSource(imagesList, this);
				imageCollectionView.DataSource = Source;                                                                                                 
				this.cameraImageView = cameraView;
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Exception Occured in ViewDidLoad method due to " + ex.Message);
	
			}
		}

		async void FinishButton_TouchUpInside (object sender, EventArgs e)
		{
			try
			{
				
				if (currentSelectedImageView != null) 
				{
					currentSelectedImageView.Layer.CornerRadius = 0.0f;
					currentSelectedImageView.Layer.BorderColor = UIColor.FromRGB (0, 0, 0).CGColor;
					currentSelectedImageView.Layer.BorderWidth = 0.0f;
					currentSelectedImageView.BackgroundColor = UIColor.Clear;
				}
				if (picture != null)
				{
					Debug.WriteLine("storing picture - picture taken");
					picture(imagesList);
				}
				this.DismissViewControllerAsync (false);
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Exception Occured in FinishButton_TouchUpInside method due to " + ex.Message);
								
			}
		}

		async void SegTakePic_ValueChanged (object sender, EventArgs e)
		{
		}

		void BtnCloseWindow_TouchUpInside (object sender, EventArgs e)
		{
			this.DismissViewControllerAsync (false);
		}

		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);
		}

		public override void ViewDidAppear (bool animated)
		{
			// When we're done viewing the media, we should clean it up
			if (Media != null) {
				Media.Dispose ();
				Media = null;
			}
			base.ViewDidAppear (animated);
		}

		private MediaFile _media;
		public MediaFile Media {
			get {
				return _media;
			}
			set {
				_media = value;
			}
		}



		public override Task DismissViewControllerAsync (bool animated)
		{
			ClearCollectionView ();
			try
			{
				this.Media = null;
				//if (MediaLst != null) {
				//	foreach (var view in MediaLst) {
				//		if (view is UIImageView) {
				//			(view as UIImageView).Image = null;
				//		}
				//		view.RemoveFromSuperview ();
				//		view.Dispose ();
				//	}

				//	MediaLst.Clear ();
				//	MediaLst = null;
				//}
				if (image != null)
				{
					image.Image = null;
					image = null;
				}
				if (cameraView != null)
				{
					cameraView.Image = null;
					cameraView.Dispose();
				}
				if (currentSelectedImageView != null)
				{
					currentSelectedImageView.Image = null;
					currentSelectedImageView = null;
				}
				if (Source != null)
				{
					Source.Dispose();
					Source = null;
				}
				this.Dispose();

				GC.Collect();
		
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Exception Occured in DismissViewControllerAsync method due to " + ex.Message);
			}
	
			return base.DismissViewControllerAsync (animated);
		}

		private void ClearCollectionView()
		{

			try
			{
				if (imageCollectionView != null)
				{
					if (imageCollectionView.Subviews != null)
					{
						foreach (var view in imageCollectionView.Subviews)
						{

							if (view is UIImageView)
							{
								(view as UIImageView).Image = null;
							}
							view.RemoveFromSuperview();
							view.Dispose();

						}
					}

					imageCollectionView.Source = null;
					imageCollectionView.Delegate = null;
					imageCollectionView.DataSource = null;
					imageCollectionView.WeakDelegate = null;
					imageCollectionView.WeakDataSource = null;
					imageCollectionView.Dispose();

				}
				if (this.View != null)
				{
					if (this.View.Subviews != null)
					{
						foreach (var view in this.View.Subviews)
						{


							view.RemoveFromSuperview();
							view.Dispose();

						}
					}
				}

				pictureTaken = null;
				pictureCollection = null;
				picture = null;
				if (image != null)
				{
					image.Image = null;
					image.RemoveFromSuperview();
					image.Dispose();
				}

				Source = null;
				imagesList = null;
				collectionViewCell = null;

				if (cameraImageView != null)
				{
					cameraImageView.Image = null;
					cameraImageView.RemoveFromSuperview();
					cameraImageView.Dispose();
				}

				if (cameraImage != null)
				{
					cameraImage.Image = null;
					cameraImage.RemoveFromSuperview();
					cameraImage.Dispose();
				}

				if (cameraView != null)
				{
					cameraView.Image = null;
					cameraView.RemoveFromSuperview();
					cameraView.Dispose();
				}

				GC.Collect();
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Exception Occured in ClearCollectionView method due to " + ex.Message);
			}
		}

		public override void DidReceiveMemoryWarning ()
		{
			#if DEBUG
			Debug.WriteLine("DidReceiveMemoryWarning - UICameraController");
#endif
			if (memoryWarningDelegate != null)
			{
				memoryWarningDelegate();
			}

			base.DidReceiveMemoryWarning();


					


			NSUrlCache.SharedCache.RemoveAllCachedResponses ();
			GC.SuppressFinalize (this);
			GC.Collect ();
		}

		public void ClearCamearaView()
		{
			if (cameraImageView != null) {
				cameraImageView.Image = null;
			}
			if(imagesList != null)
			{
				imagesList.Clear();

			}

			if(MediaLst != null)
			{
				foreach (var view in MediaLst) {
					view.RemoveFromSuperview ();
				}
				MediaLst.Clear();
			}

			if (imageCollectionView != null) {
				foreach (var view in imageCollectionView.Subviews) {
					if (view is UIImageView) {
						(view as UIImageView).Image = null;
					}
					view.RemoveFromSuperview ();
				}
				if (imageCollectionView.Source != null) {
					imageCollectionView.Source.Dispose ();
					imageCollectionView.Source = null;
				}
			}

			GC.Collect();
		}
	}
}