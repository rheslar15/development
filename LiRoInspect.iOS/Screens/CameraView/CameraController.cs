using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;
using Xamarin.Media;
using System.Threading.Tasks;
using System.Collections.Generic;
using iTextSharp.text;
using System.Diagnostics;
using System.IO;

namespace LiRoInspect.iOS
{
	/// <summary>
	/// Camera controller.
	/// </summary>
	public partial class CameraController : UIViewController, IDisposable
	{
		//readonly MediaPicker mediaPicker = new MediaPicker();
		//readonly TaskScheduler uiScheduler = TaskScheduler.FromCurrentSynchronizationContext();
		public static MediaController mediaController = null;
		public CamerapictureTaken pictureTaken;
		public UIImageView image = new UIImageView(UIImage.FromBundle("imagePhoto2.png"));
		public PickerDelegate pickerDelegate;
		public MemoryWarningDelegate memoryWarningDelegate;

		public CameraController(IntPtr handle) : base(handle)
		{

		}
		void HandleMemoryWarningDelegate()
		{
#if DEBUG
			Debug.WriteLine("CameraController - DidReceiveMemoryWarning - HandleMemoryWarningDelegate");

#endif
			memoryWarningDelegate = null;
			NSUrlCache.SharedCache.RemoveAllCachedResponses();
			GC.SuppressFinalize(this);
			GC.Collect();
			UIAlertView alert1 = new UIAlertView(@"Warning", @"Not enough memory to load any more photos, please Submit inspection ", null, NSBundle.MainBundle.LocalizedString("OK", "OK"));

			alert1.Show();

			DismissViewControllerAsync(false);
		}


		public CameraController()
		{
			memoryWarningDelegate = HandleMemoryWarningDelegate;
			// make this a single instance to help memory leak
#if DEBUG
			Debug.WriteLine("CameraController - Constructor");

#endif

			mediaController = new MediaController();


			mediaController.Canceled += (object sender, EventArgs e) =>
			{
				this.DismissModalViewController(false);
			};


		}

		private MediaFile _media;
		/// <summary>
		/// Gets or sets the media.
		/// </summary>
		/// <value>The media.</value>
		public MediaFile Media
		{
			get
			{
				return _media;
			}
			set
			{
				_media = value;
			}
		}
		public override void ViewDidLoad()
		{
			try
			{
				base.ViewDidLoad();

				//


				if (!MediaController.IsSourceTypeAvailable(UIImagePickerControllerSourceType.Camera))
				{
					ShowUnsupported();
					this.DismissViewControllerAsync(false);
					return;
				}


				FreemediaController();

				// set our source to the camera
				mediaController.SourceType = UIImagePickerControllerSourceType.Camera;

				// set
				mediaController.MediaTypes = new string[] { "public.image" };

				// show the camera controls

				// attach the delegate
				pickerDelegate = new PickerDelegate(this);
				mediaController.Delegate = pickerDelegate;



				this.View.AddSubview(mediaController.View);




			}
			catch (Exception ex)
			{
				Debug.WriteLine("Exception Occured in ViewDidLoad method due to " + ex.Message);
			}
		}

		private UIAlertView errorAlert;
		private void ShowUnsupported()
		{
			if (this.errorAlert != null)
			{
				this.errorAlert.Dispose();
			}
			this.errorAlert = new UIAlertView("Device unsupported", "Your device does not support this feature",
				null, "OK");

			errorAlert.Clicked += ErrorAlert_Clicked;
			this.errorAlert.Show();


		}

		void ErrorAlert_Clicked(object sender, UIButtonEventArgs e)
		{
			image.Image = UIImage.FromBundle("photo2");
			pictureTaken(image);
			this.DismissViewControllerAsync(false);
		}

		/// <summary>
		/// Shows the photo.
		/// </summary>
		/// <param name="resizedImage">Resized image.</param>
		public async void ShowPhoto(UIImage resizedImage)
		{
			try
			{
				Debug.WriteLine("show photo - line 171");
				if (resizedImage != null)
				{
					image.Image = resizedImage;
					if (resizedImage != null)
					{
						resizedImage.Dispose();
						resizedImage = null;
					}
					pictureTaken(image);

				}
				this.DismissViewControllerAsync(false);

				//FreemediaController();
				GC.SuppressFinalize(this);
				GC.Collect();

			}
			catch (Exception ex)
			{
				Debug.WriteLine("Exception Occured in Showphoto method due to " + ex.Message);
			}
		}

		/// <summary>
		/// Clears the media.
		/// </summary>
		/// <param name="mediaPath">Media path.</param>
		public void ClearMedia(string mediaPath)
		{
			try
			{
				Debug.WriteLine("clearing media");
				FileInfo fileInfo = new FileInfo(mediaPath);
				if (fileInfo != null)
				{
					var directoryInfo = fileInfo.Directory;
					try
					{
						foreach (var file in directoryInfo.GetFiles())
						{
							file.Delete();
						}
					}
					catch (Exception ex)
					{
						Debug.WriteLine("Exception Occured in ClearMedia method due to " + ex.Message);
					}
				}
				if (this.Media != null)
				{
					this.Media.Dispose();
					this.Media = null;
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Exception Occured in ClearMedia method due to " + ex.Message);
			}

		}

		private void FreemediaController()
		{
			if (Media != null)
			{
				ClearMedia(Media.Path);
			}

			if (mediaController != null && mediaController.Delegate != null)
			{
				if (mediaController.View != null)
				{
					mediaController.clearZoomSliderDelegate(mediaController.View.Subviews);
				}
				mediaController.Delegate.Dispose();
				mediaController.Delegate = null;
			}

			if (this.View != null)
			{
				foreach (var view in View.Subviews)
				{
					view.RemoveFromSuperview();
					view.Dispose();
				}
			}

			if (this.Media != null)
			{
				this.Media.Dispose();
			}


			if (pickerDelegate != null)
			{
				pickerDelegate.Dispose();
				pickerDelegate = null;
			}

			//mediaController.Delegate = null;
		}

		public override void ViewDidUnload()
		{
			base.ViewDidUnload();

			this.Dispose();
		}
		/// <summary>
		/// Dismisses the view controller async.
		/// </summary>
		/// <returns>The view controller async.</returns>
		/// <param name="animated">If set to <c>true</c> animated.</param>
		public override Task DismissViewControllerAsync(bool animated)
		{
			try
			{

				if (Media != null)
				{
					ClearMedia(Media.Path);
				}

				if (mediaController != null && mediaController.Delegate != null)
				{
					if (mediaController.View != null)
					{
						mediaController.clearZoomSliderDelegate(mediaController.View.Subviews);
					}
					mediaController.Delegate.Dispose();
					mediaController.Delegate = null;
				}
				if (this.View != null)
				{
					foreach (var view in View.Subviews)
					{
						view.RemoveFromSuperview();
						view.Dispose();
					}
				}



				if (this.Media != null)
				{
					this.Media.Dispose();
				}


				if (pickerDelegate != null)
				{
					pickerDelegate.Dispose();
					pickerDelegate = null;
				}
#if DEBUG
				var Process2 = Process.GetCurrentProcess();

				if (Process2 != null)
				{
					Debug.WriteLine(string.Format("memory allocated by DismissViewControllerAsync before memory release = {0}", Process2.WorkingSet64));
				}
#endif

				if (mediaController != null)
				{

					mediaController.DismissModalViewController(true);
					mediaController.Dispose();
					mediaController = null;
				}

				GC.Collect();
				this.Dispose();
#if DEBUG
				var Process1 = Process.GetCurrentProcess();

				if (Process1 != null)
				{
					Debug.WriteLine(string.Format("memory allocated by DismissViewControllerAsync = {0}", Process1.WorkingSet64));
				}
#endif
				return base.DismissViewControllerAsync(animated);
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Exception Occured in DismissViewControllerAsync method due to " + ex.Message);
			}

			return base.DismissViewControllerAsync(animated);

		}

		public override void DidReceiveMemoryWarning()
		{

			if (memoryWarningDelegate != null)
			{
				memoryWarningDelegate();
			}

#if DEBUG
			Debug.WriteLine("DidReceiveMemoryWarning - CameraController");
#endif


			base.DidReceiveMemoryWarning();

			NSUrlCache.SharedCache.RemoveAllCachedResponses();
			GC.SuppressFinalize(this);
			GC.Collect();
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
			GC.Collect();
		}
	}


	public class PickerDelegate : UIImagePickerControllerDelegate
	{
		private CameraController camController { get; set; }
		public PickerDelegate(CameraController cameraController)
		{
			camController = cameraController;

		}
		public override void Canceled(UIImagePickerController picker)
		{

			if (picker != null)
			{

				picker.DismissModalViewController(true);
				picker.View.RemoveFromSuperview();
				picker.Dispose();
				picker = null;
			}
			camController.DismissViewControllerAsync(false);
		}

		public byte[] ImageToByteArray(UIImage image)
		{
			Stream stream = image.AsJPEG().AsStream();
			byte[] array = ToByteArray(stream);
			return array;
		}

		public byte[] ToByteArray(Stream stream)
		{
			stream.Position = 0;
			byte[] buffer = new byte[stream.Length];
			for (int totalBytesCopied = 0; totalBytesCopied < stream.Length;)
				totalBytesCopied += stream.Read(buffer, totalBytesCopied, Convert.ToInt32(stream.Length) - totalBytesCopied);
			return buffer;
		}

		/// <summary>
		/// Checks if media is video or Image.
		/// If it was an image, get the other image info.
		/// </summary>
		/// <param name="picker">Picker.</param>
		/// <param name="info">Info.</param>
		public async override void FinishedPickingMedia(UIImagePickerController picker, NSDictionary info)
		{
			try
			{
				// determine what was selected, video or image
				bool isImage = false;
				switch (info[UIImagePickerController.MediaType].ToString())
				{
					case "public.image":
						isImage = true;
						break;
					case "public.video":
						break;
				}

				if (isImage)
				{
					

					UIImage originalImage = info[UIImagePickerController.OriginalImage] as UIImage;
					if (originalImage != null)
					{
						
						UIImageOrientation OrIn = originalImage.Orientation;
						Debug.WriteLine("scaling image");
						var originalImage1 = await Task.Run(() => ScaleAndRotateImage.ScaleAndRotateImageView(originalImage, OrIn));

						if (originalImage1 != null)
						{

							var Data = originalImage1.AsJPEG(0.0f);

							if (Data != null)
							{


								UIImage resizedImage = UIImage.LoadFromData(Data);

								if (originalImage != null)
								{
									originalImage.Dispose();

									originalImage = null;
								}

								originalImage1.Dispose();

								originalImage1 = null;
								Data.Dispose();
								Data = null;
								GC.Collect();




								camController.ShowPhoto(resizedImage);


							}


							if (info != null)
							{



								(info[UIImagePickerController.OriginalImage] as UIImage).CGImage.Dispose();
								(info[UIImagePickerController.OriginalImage] as UIImage).Dispose();

								info[UIImagePickerController.OriginalImage].Dispose();

								info[UIImagePickerController.OriginalImage] = new UIImage();

								info.Dispose();



							}


							GC.Collect();

						}
					}


				}



			}
			catch (Exception ex)
			{
				Debug.WriteLine("Exception Occured in FinishedPickingMedia method due to " + ex.Message);


			}
			finally
			{
				// dismiss the picker
				if (picker != null)
				{


					picker.DismissModalViewController(true);




					picker.Dispose();
					picker = null;



					GC.Collect();


				}


#if DEBUG
				var Process1 = Process.GetCurrentProcess();

				if (Process1 != null)
				{
					Debug.WriteLine(string.Format("memory allocated by FinishedPickingMedia after DismissModalViewController = {0}", Process1.WorkingSet64));
				}
#endif



			}

		}
	}
}

