using System;
using Foundation;
using UIKit;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Model;
using CoreGraphics;
using System.Diagnostics;

namespace LiRoInspect.iOS
{
	public class PunchTextViewDelegate : UITextViewDelegate
	{
		PunchCell punchCell;
		public PunchTextViewDelegate(PunchCell punchCell)
		{
			this.punchCell = punchCell;
		}

		public override void EditingStarted (UITextView textView)
		{
			UITextView commentsTextView = textView;
			CGPoint pointInTable = new CGPoint();
			CGPoint origin = new CGPoint (commentsTextView.Frame.X, commentsTextView.Frame.Y);
			pointInTable = commentsTextView.Superview.ConvertPointToView(origin, punchCell.optionsTableView);
			CGPoint contentOffset = punchCell.optionsTableView.ContentOffset;
			contentOffset.Y = pointInTable.Y - (commentsTextView.Frame.Height-50);
			punchCell.optionsTableView.SetContentOffset (contentOffset, true);
		}

		public override void EditingEnded (UITextView textView)
		{
			UITextView failureTextView = textView;
			failureTextView.ResignFirstResponder ();
		}

		public override bool ShouldChangeText (UITextView textView, NSRange range, string text)
		{
			return (textView.Text.Length + (text.Length - range.Length) <= 300);
		}

		public override void Changed (UITextView textView)
		{
			UITextView punchTextView = textView;
			punchCell.TextViewString = punchTextView.Text;
			PunchCell.IsPunchValueChanged = true;
			punchCell.punch.punchDescription = punchCell.TextViewString;
		}
	}

	public partial class PunchCell : BaseCell, ICell
	{

		public InspectionViewController parentController;
		public UITableView optionsTableView;
		NSIndexPath indexPath;
		public Model.Punch punch;
		bool shouldInsertRow;
		object locker=new object();
		PunchDataSource Source;
		public	List<UIImage> imagesList = new List<UIImage> ();
		private List<Model.Punch> punchItems;
		UITapGestureRecognizer tap;
		public static bool IsPunchValueChanged = false;
		//public static readonly UINib Nib = UINib.FromName ("PunchCell", NSBundle.MainBundle);
		//public static readonly NSString Key = new NSString ("PunchCell");

		public PunchCell (IntPtr handle) : base (handle)
		{
		}

//		public static PunchCell Create ()
//		{
//			return (PunchCell)Nib.Instantiate (null, null) [0];
//		}

		public void UpdateCell(InspectionViewController parentController, UITableView optionsTableView, NSIndexPath indexPath, List<Model.Punch> punchItems)
		{
			try
			{				
				ResetUIView();
				this.parentController = parentController;
				this.optionsTableView = optionsTableView;
				this.indexPath = indexPath;
				Model.Punch punchItem = punchItems.ElementAt(indexPath.Row);
				this.punch = punchItem;
				this.punchItems = punchItems;

				foreach(var byteImage in this.punch.punchImages)
				{
					imagesList.Add(ByteArrayToImage(byteImage));
				}

				if(Source==null)
				{
					Source = new PunchDataSource (imagesList,this);
				}
				else
				{
					Source.itemsList=imagesList;
				}
				imagesCollectionView.DataSource = Source;
				imagesCollectionView.ReloadData();

				//if the heading is addRow
				if (true == shouldInsertRow)
				{
					Model.Punch punch = new Model.Punch();
					punch.punchHeading = "Item " + indexPath.Row;
					punchItems.Add(punch);

					optionsTableView.BeginUpdates();
					optionsTableView.InsertRows(new NSIndexPath[] { NSIndexPath.FromRowSection(indexPath.Row, indexPath.Section) }, UITableViewRowAnimation.None);
					optionsTableView.EndUpdates();
				}

				punchHeadingLabel.Font = UIFont.FromName("Helvetica", 17f);
				punchHeadingLabel.TextColor = UIColor.FromRGB(186, 190, 194);


				punchTakePicBtn.Layer.CornerRadius = 5f;
				punchTakePicBtn.Layer.BackgroundColor = UIColor.FromRGB(18, 74, 143).CGColor;
				punchTakePicBtn.Layer.BorderWidth = 0.5f;
				punchTakePicBtn.SetTitleColor(UIColor.White, UIControlState.Normal);

				punchTakePicBtn.TouchUpInside -= BtnTakePic_TouchUpInside;
				punchTakePicBtn.TouchUpInside += BtnTakePic_TouchUpInside;

				punchTextView.Layer.CornerRadius = 5f;
				punchTextView.Layer.BorderColor = UIColor.FromRGB(186, 190, 194).CGColor;
				punchTextView.Layer.BorderWidth = 0.5f;

				if (tap != null)
					this.RemoveGestureRecognizer (tap);
				
				tap = new UITapGestureRecognizer ();
				tap.AddTarget (tapAction);
				this.AddGestureRecognizer (tap);

				//punchImagesScrollView.Hidden = false;
				punchTakePicBtn.Hidden = false;
				punchTextView.Hidden = false;
				punchHeadingLabel.Hidden = false;


				int showHeadingRowNumber = indexPath.Row + 1;
				string headingString = "Item " + showHeadingRowNumber;
				punchHeadingLabel.Text = headingString;

				punchTextView.Text = punchItem.punchDescription;
				var textDelegate = new PunchTextViewDelegate (this);
				punchTextView.WeakDelegate = textDelegate;
				LoadPunchView();

				shouldInsertRow = false;
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Exception Occured in UpdateCell method due to " + ex.Message);
			}
		}

		void BtnTakePic_TouchUpInside (object sender, EventArgs e)
		{
			UICameraController cameraController = parentController.Storyboard.InstantiateViewController ("UICameraController") as UICameraController;
			cameraController.ClearCamearaView ();
			cameraController.ModalPresentationStyle = UIModalPresentationStyle.FullScreen;
			cameraController.picture = this.UpdateCollectionImageView;
			if (cameraController.MediaLst != null) {
				cameraController.MediaLst.Clear ();
			}

			if (MediaLst != null && MediaLst.Count > 0) 
				MediaLst.Clear ();

			foreach (UIImage image in imagesList) 
			{
				UIImageView imageView = new UIImageView ();
				imageView.Image = image;
				MediaLst.Add (imageView);
			}
			cameraController.MediaLst = MediaLst;
			IsPunchValueChanged = true;
			parentController.PresentViewController(cameraController, false, null);
		}

		private String _textViewString = @"";
		public String TextViewString
		{
			get
			{
				return _textViewString;
			}
			set{
				_textViewString = value;
			}
		}

		private List<UIImageView> _imageLst = new List<UIImageView> ();
		public List<UIImageView> ImageLst
		{
			get
			{
				return _imageLst;
			}
			set{
				_imageLst = value;
			}
		}

		private void ResetUIView ()
		{
			if (ImageLst != null) {
				ImageLst.Clear ();
			}
			if (imagesList != null) {
				imagesList.Clear ();
			}
		}

		private void tapAction()
		{
			if (punchTextView.IsFirstResponder)
				punchTextView.ResignFirstResponder ();
			
			else 
			{
				CGRect deletionAreaRect = new CGRect (0, this.Frame.Height / 2 - 25, 44, 50);
				CGPoint point = tap.LocationInView(this);
				
				if (deletionAreaRect.Contains (point)) 
				{
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
						this.punchItems.Remove (this.punch);
						this.optionsTableView.ReloadData ();
						IsPunchValueChanged = true;
					}
				}
			}
		}



		private void LoadPunchView()
		{
			try{

				if (this.punch.punchImages.Count>0)
				{
					List<UIImage> currentPassImages = new List<UIImage> ();
					foreach (var img in this.punch.punchImages) {
						var imag = ByteArrayToImage (img);
						currentPassImages.Add (imag);
					}
					if (currentPassImages.Count > 0) {

						if(null!=ImageLst)
						{
							ImageLst.Clear();
						}
					}
				}
			}
			catch (Exception ex) {
			}
		}
	
		private List<UIImageView> _mediaLst = new List<UIImageView> ();
		public List<UIImageView> MediaLst
		{
			get
			{
				return _mediaLst;
			}
			set{
				_mediaLst = value;
			}
		}

		public void UpdateCollectionImageView(List<UIImage> images)
		{
			images.Count ();
		
			if (imagesList != null) 
			{
				imagesList.Clear ();
				imagesList = null;
			}

			if (this.punch.punchImages != null) 
			{
				this.punch.punchImages.Clear ();
			}				

			foreach(UIImage image in images)
			{
				this.punch.punchImages.Add (ImageToByteArray (image));
			}
		
			imagesList = images;

			if(Source==null)
			{
				Source = new PunchDataSource (images,this);
			}
			else
			{
				Source.itemsList=images;
			}
			imagesCollectionView.DataSource = Source;
			imagesCollectionView.ReloadData();
		}

		public void cleanCell()
		{
			if (_imageLst != null) {
				_imageLst.Clear ();
			}
			if (imagesList != null) {
				imagesList.Clear ();
			}
			if (MediaLst != null) {
				MediaLst.Clear ();
			}
			locker = null;
			if(imagesCollectionView!=null){
			foreach (var imgView in imagesCollectionView.Subviews)
			{
				if(imgView is UIImageView)
				{
					(imgView as UIImageView).Image=null;
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
		}
	}
}