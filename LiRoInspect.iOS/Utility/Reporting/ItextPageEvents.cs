using System;
using iTextSharp.text.pdf;
using iTextSharp.text;
using UIKit;
using System.IO;
using Foundation;

namespace LiRoInspect.iOS
{
	public class ItextPageEvents: PdfPageEventHelper
	{
		String text = "";
		string headerText="";
		PdfPTable tableHeader = new PdfPTable(12);
		public ItextPageEvents(ReportType reportType)
		{
			switch (reportType) 
			{
			case ReportType.Pass:
				headerText = "Inspection Report";
				break;
			case ReportType.Fail:
				headerText = "Failed Inspection Report";
				break;
			case ReportType.PhotoLog:
				headerText = "Inspection Photo Log";
				NSDate InspectionDate=NSDate.Now;
				NSDateFormatter dateformatter = new NSDateFormatter ();
				dateformatter.DateFormat = @"MM/dd/yyyy";
				text= dateformatter.StringFor (InspectionDate);
				break;
			}			
		}

		// This is the contentbyte object of the writer
		PdfContentByte cb;
		// this is the BaseFont we are going to use for the header / footer
		BaseFont bf = null;
		// This keeps track of the creation time
		public override void OnOpenDocument(PdfWriter writer, Document document)
		{
			try
			{
				bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
				cb = writer.DirectContent;
				//Create PdfTable object
				tableHeader.TotalWidth=PageSize.A4.Width - 20;
				tableHeader.LockedWidth = true;
				PdfPCell cell=new PdfPCell(new Phrase(headerText,FontFactory.GetFont(Font.HELVETICA.ToString(),14,Font.BOLD,Color.BLACK)));
				cell.Colspan=11;
				cell.FixedHeight = 35f;
				cell.PaddingTop = 10f;
				cell.BorderWidth = 0f;
				cell.HorizontalAlignment=Element.ALIGN_RIGHT;
				tableHeader.AddCell(cell);
				System.IO.Stream stream = UIImage.FromBundle("Logo 1").AsPNG().AsStream();
				byte[] array = ToByteArray (stream);
				iTextSharp.text.Image image = Image.GetInstance(array);
				image.ScaleToFit(30f, 30f);
				image.Alignment = Element.ALIGN_RIGHT;
				PdfPCell cellImage=new PdfPCell();
				cellImage.AddElement (new Chunk (image, 0, 0));
				cellImage.PaddingLeft = 20f;
				cellImage.BorderWidth = 0f;
				cellImage.HorizontalAlignment=Element.ALIGN_RIGHT;
				tableHeader.AddCell(cellImage);
			}
			catch (DocumentException de)
			{

			}
			catch (System.IO.IOException ioe)
			{

			}
		}

		public override void OnEndPage(PdfWriter writer,Document document)
		{
			base.OnEndPage (writer, document);
			//Add paging to footer
			{
				cb.BeginText();
				cb.SetFontAndSize(bf, 12);
				cb.SetTextMatrix(document.PageSize.Width-(100), document.PageSize.GetBottom(20));
				cb.ShowText(text);
				cb.EndText();

				cb.BeginText();
				cb.SetFontAndSize(bf, 12);
				cb.EndText();
			}
			
			//call WriteSelectedRows of PdfTable. This writes rows from PdfWriter in PdfTable
			//first param is start row. -1 indicates there is no end row and all the rows to be included to write
			//Third and fourth param is x and y position to start writing
			tableHeader.WriteSelectedRows(0, -1, 0, document.PageSize.Height - 20, writer.DirectContent);
			//set pdfContent value
			//Move the pointer and draw line to separate header section from rest of page
			cb.MoveTo(20, document.PageSize.GetBottom(40) );
			cb.LineTo(document.PageSize.Width-20, document.PageSize.GetBottom(40));
			cb.Stroke();
		}

		public override void OnCloseDocument(PdfWriter writer, Document document)
		{
			base.OnCloseDocument(writer, document);
		}

		private  byte[] ToByteArray( Stream stream)
		{
			stream.Position = 0;
			byte[] buffer = new byte[stream.Length];
			for (int totalBytesCopied = 0; totalBytesCopied < stream.Length; )
				totalBytesCopied += stream.Read(buffer, totalBytesCopied, Convert.ToInt32(stream.Length) - totalBytesCopied);
			return buffer;
		}
	}
}