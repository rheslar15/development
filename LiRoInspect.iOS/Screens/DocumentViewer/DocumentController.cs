// This file has been autogenerated from a class added in the UI designer.

using System;
using System.Collections.Generic;
using Foundation;
using UIKit;
using System.Threading.Tasks;
using LiRoInspect.iOS.Reporting;
using BAL.Service;
using Model;
using System.IO;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Text;
using CoreGraphics;

namespace LiRoInspect.iOS
{
	public partial class DocumentController : UICollectionViewController
	{
		public List<Model.Document> documentsList=new List<Model.Document>();
		protected string cellIdentifier = "CollectionViewCell";
		public string token;

		public DocumentController (IntPtr handle) : base (handle)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			this.NavigationController.NavigationBarHidden = false;

			this.NavigationItem.SetHidesBackButton(false,true);
			this.NavigationItem.SetRightBarButtonItem(
				new UIBarButtonItem(UIBarButtonSystemItem.Action, (sender,args) => {
					this.NavigationController.PopViewController (true);
				})
				, true);
		}

		public override nint NumberOfSections (UICollectionView collectionView)
		{
			return 1;
		}

		public override UICollectionViewCell GetCell (UICollectionView collectionView, Foundation.NSIndexPath indexPath)
		{
			UICollectionViewCell cell = collectionView.DequeueReusableCell (cellIdentifier, indexPath) as UICollectionViewCell;
			cell.Layer.BorderColor = UIColor.Black.CGColor;
			cell.Layer.BorderWidth = 0.5f;

			foreach(var lbl in cell.Subviews)
			{
				if(lbl.GetType() == typeof(UILabel))
					lbl.RemoveFromSuperview();
			}
				
			UILabel label = new UILabel (new CoreGraphics.CGRect (5, 30, 90, 60));
			label.Font = UIFont.FromName("Helvetica", 10f);
			label.BackgroundColor = UIColor.Clear;
			label.TextAlignment = UITextAlignment.Left;
			label.Lines = 2;
			label.Text = documentsList [indexPath.Row].documentDisplayName;
			cell.AddSubview (label);

			UIImageView imageView = new UIImageView (new CoreGraphics.CGRect (5, 5, 25, 25));
			imageView.Image = new UIImage ("pdfImage");
			cell.AddSubview (imageView);

			return cell;
		}

		public override nint GetItemsCount (UICollectionView collectionView, nint section)
		{
			return documentsList.Count;
		}

		LoadingOverlay _loadPop;
		public void LoadOverLayPopup()
		{
			var bounds = UIScreen.MainScreen.Bounds; // portrait bounds
			if (UIApplication.SharedApplication.StatusBarOrientation == UIInterfaceOrientation.LandscapeLeft || UIApplication.SharedApplication.StatusBarOrientation == UIInterfaceOrientation.LandscapeRight) {
				bounds.Size = new CGSize(bounds.Size.Width,bounds.Size.Height);
			}
			// show the loading overlay on the UI thread using the correct orientation sizing
			this._loadPop = new LoadingOverlay (bounds);
			this.View.AddSubview ( this._loadPop );
		}

		public void HideOverLay()
		{
			this._loadPop.Hide ();
			this._loadPop.RemoveFromSuperview ();

		}
		public override async void ItemSelected (UICollectionView collectionView, NSIndexPath indexPath)
		{
			DocViewService service = new DocViewService (AppDelegate.DatabaseContext);
			this.InvokeOnMainThread (delegate {
				LoadOverLayPopup ();
			});

			Model.Document doc =await Task.Run(()=>service.GetDocument (documentsList [indexPath.Row].ID, token));

			if (doc != null) 
			{
				if (doc.documentArray == null) {
					UIAlertView alert = new UIAlertView (@"Alert", @"The document is not available", null, NSBundle.MainBundle.LocalizedString ("OK", "OK"));
					alert.Show ();
				}
				string documentPath = string.Empty;
				if (string.IsNullOrEmpty (doc.DocumentPath)) {
					documentPath = this.GenerateReport (doc);
					doc.DocumentPath = documentPath;
					service.UpdateDocumentItems (doc);
				} else {
					documentPath = doc.DocumentPath;
				}



				if (File.Exists (documentPath))
				{
					var viewer = UIDocumentInteractionController.FromUrl (NSUrl.FromFilename (documentPath));
					UIDocumentInteractionControllerDelegateDerived del = new UIDocumentInteractionControllerDelegateDerived (this);
					//del.doneWithPreview+=(informer, eventArgs)=>{btnFinalizeSave.Hidden = false;
					//	btnEdit.Hidden = false;};
					viewer.Delegate = del; 
					viewer.PresentPreview (true);

				}
				this.InvokeOnMainThread (delegate {
					HideOverLay ();
				});
			}
			else
			{
				UIAlertView alert = new UIAlertView (@"Alert", @"Entire pdf not available", null, NSBundle.MainBundle.LocalizedString ("OK", "OK"));
				alert.Show ();
			}
		}

		void button_TouchUpInside (object sender, EventArgs e)
		{
			this.NavigationController.PopViewController (true);
		}

		public string GenerateReport(Model.Document documentObject)
		{
			string filePath = string.Empty;
			if (documentObject.documentArray != null) {
				string appRootDir = Environment.GetFolderPath (Environment.SpecialFolder.MyDocuments);
				// Step 1: Creating System.IO.FileStream object
				DirectoryInfo path = Directory.CreateDirectory (appRootDir + "/LiRoInspectionDocuments");

				//iTextSharp.text.Document pdfDocument = new iTextSharp.text.Document (PageSize.A4, 20f, 20f, 60f, 60f);
				//FillDocument (documentObject.documentArray, pdfDocument);
				using (FileStream fs = new FileStream (path.FullName + "/" + documentObject.documentDisplayName, FileMode.Create, FileAccess.ReadWrite, FileShare.None)) {

					using (StreamWriter writer = new StreamWriter (fs, Encoding.UTF8)) {

						byte[] buffer = documentObject.documentArray;

						fs.Write (buffer, 0, buffer.Length);

						writer.Close ();

					}
				}
				filePath = Path.Combine (path.FullName, documentObject.documentDisplayName);
			}
			return filePath;
		}

	
		public void FillDocument(byte[] docArray, iTextSharp.text.Document document)
		{
			//byte[] all;

			using (MemoryStream ms = new MemoryStream ()) {
				

				PdfWriter writer = PdfWriter.GetInstance (document, ms);

				//document.SetPageSize (PageSize.LETTER);
				document.Open ();
				PdfContentByte cb = writer.DirectContent;
				PdfImportedPage page;

				PdfReader reader;
				//foreach (byte[] p in docArray) {
				reader = new PdfReader (docArray);
					int pages = reader.NumberOfPages;

					// loop over document pages
					for (int i = 1; i <= pages; i++) {
						//document.SetPageSize (PageSize.LETTER);
						document.NewPage ();
						page = writer.GetImportedPage (reader, i);
						cb.AddTemplate (page, 0, 0);
					}
				//}
			}

		}
	}
}