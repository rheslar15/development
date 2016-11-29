using System;
using iTextSharp.text;
using iTextSharp.text.pdf;
using UIKit;
using System.IO;
using Model;
using BAL;
using BAL.Service;
using System.Diagnostics;
using System.Collections.Generic;


namespace LiRoInspect.iOS.Reporting
{
	public class TempPhotologReport : IReportHandler
	{
		#region IReportHandler implementation

		Font headerFont = null;
		Font cellContentFont = null;
		Font cellPhototFont = null;
		PdfPTable MainTable = null;
		private const int RowLimit = 10;
		public TempPhotologReport()
		{
			//font style for header part
			headerFont = new Font();
			headerFont.SetFamily(BaseFont.HELVETICA);
			headerFont.Color = Color.BLACK;
			headerFont.SetStyle(1);
			headerFont.Size = 15;
			// cell Content Font
			cellContentFont = new Font();
			cellContentFont.SetFamily(BaseFont.HELVETICA);
			cellContentFont.Color = Color.BLACK;
			cellContentFont.Size = 10;
			cellContentFont.SetStyle(0);
			//for photo Text
			cellPhototFont = new Font();
			cellPhototFont.SetFamily(BaseFont.HELVETICA);
			cellPhototFont.Color = Color.BLACK;
			cellPhototFont.Size = 20;
			cellContentFont.SetStyle(0);
			cellContentFont.Size = 13;
		}

		public string GenerateReport(string fileName, Inspection inspectionObject)
		{
			string appRootDir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
			// Step 1: Creating System.IO.FileStream object
			DirectoryInfo path = Directory.CreateDirectory(appRootDir + "/LiRoReport");
			FileStream fs = new FileStream(path.FullName + "/" + fileName, FileMode.Create, FileAccess.ReadWrite, FileShare.None);
			iTextSharp.text.Document  doc1 = new iTextSharp.text.Document(PageSize.A4, 20f, 20f, 60f, 60f);
			// Step 3: Creating iTextSharp.text.pdf.PdfWriter object
			// It helps to write the Document to the Specified FileStream
			PdfWriter writer = PdfWriter.GetInstance(doc1, fs);
			writer.PageEvent = new ItextPageEvents(ReportType.PhotoLog);
			// Step 4: Openning the Document
			doc1.Open();
			PdfPTable table = GetPDFTable(inspectionObject);
			try 
			{
				if (table != null) 
				{
					doc1.ResetHeader ();
					doc1.Add (table);
					doc1.Close ();
				}
			} 
			catch (Exception ex) 
			{
				Debug.WriteLine ("Exception occured in photolog GenerateReport" + ex.Message);
			} 
			finally 
			{
				doc1.Close ();
				writer.Close ();
			}           
			return fs.Name;
		}

		#endregion

		public PdfPTable GetPDFTable(Inspection inspectionObject)
		{
			MainTable = PhotoLogView(inspectionObject);
			PdfPCell cellblank = new PdfPCell(new Phrase("", cellContentFont));
			cellblank.FixedHeight = 25;
			cellblank.BorderColor = Color.WHITE;
			MainTable.AddCell(cellblank);
			return MainTable;
		}

		public PdfPTable PhotoLogView(Inspection inspectionObject)
		{
			PdfPTable ParentTable = new PdfPTable (4);
			ParentTable.TotalWidth = PageSize.A4.Width - 20;
			ParentTable.LockedWidth = true;
			//Header of Report
			PdfPCell PhotoReportHeader = new PdfPCell (new Phrase ("Location Identification", headerFont));
			PhotoReportHeader.BackgroundColor = Color.RED;
			PhotoReportHeader.FixedHeight = 30;
			PhotoReportHeader.BorderWidth = 1;
			PhotoReportHeader.HorizontalAlignment=PdfCell.ALIGN_CENTER;
			PhotoReportHeader.BorderColor = Color.GRAY;
			PhotoReportHeader.Colspan = 4;
			ParentTable.AddCell (PhotoReportHeader);
			foreach (var LocationImage in inspectionObject.locationIDImages) 
			{
				if (LocationImage != null) 
				{
					iTextSharp.text.Image image = Image.GetInstance (LocationImage);
					if (null != image) 
					{						
						image.ScaleAbsolute (574.5f, 300f);
						PdfPCell cellPhotoAdd1;
						cellPhotoAdd1 = new PdfPCell (image);
						cellPhotoAdd1.Colspan = 4;
						cellPhotoAdd1.BorderWidthLeft = 0;
						cellPhotoAdd1.FixedHeight = 300;
						cellPhotoAdd1.BorderColor = Color.GRAY;
						ParentTable.AddCell (cellPhotoAdd1);
					}
				}
				PdfPCell Blankcell1 = new PdfPCell (new Phrase (" ", headerFont));
				Blankcell1.FixedHeight = 10;
				Blankcell1.BorderWidth = 0;
				Blankcell1.Colspan = 4;
				ParentTable.AddCell (Blankcell1);

				if (ParentTable.Rows.Count > RowLimit) {
					break;
				}
			}
			PdfPCell Blankcell = new PdfPCell (new Phrase ("Inspection Photos", headerFont));
			Blankcell.FixedHeight = 30;
			Blankcell.BorderWidth = 0;
			Blankcell.Colspan = 4;
			ParentTable.AddCell (Blankcell);

			foreach (var seq in inspectionObject.sequences) 
			{
				if (ParentTable.Rows.Count > RowLimit) {
					break;
				}

				if (seq.Levels != null) 
				{
					foreach (var lvl in seq.Levels) 
					{
						if (ParentTable.Rows.Count > RowLimit) {
							break;
						}

						if (lvl.Spaces != null && lvl.Spaces.Count > 0) {
							foreach (var spc in lvl.Spaces) {
								if (ParentTable.Rows.Count > RowLimit) {
									break;
								}
								if (spc.Options != null && spc.isSelected) {
									foreach (var opt in spc.Options) {
										
										if (opt.ID == Constants.GUIDED_OPTIONID) 
										{
											if (opt.checkListItems != null && opt.checkListItems.Count > 0) 
											{
												bool RepeatHeading = false;
												foreach (var checkList in opt.checkListItems) 
												{
													if (ParentTable.Rows.Count > RowLimit) {
														break;
													}
													if (checkList.photos!= null && checkList.photos.Count > 0 )
													{
														if (!RepeatHeading) 
														{
															PdfPCell cellSequence = new PdfPCell (
																new Phrase (""+seq.name +" - " + lvl.name + " - " + spc.name + " - " + opt.name, headerFont));
															cellSequence.HorizontalAlignment = PdfCell.ALIGN_CENTER;
															cellSequence.BorderWidthTop = 1;
															cellSequence.BorderWidthRight = 1;
															cellSequence.BorderWidthBottom = 0;
															cellSequence.BorderWidthLeft = 0;
															cellSequence.BackgroundColor = Color.RED;
															cellSequence.Colspan = 4;
															ParentTable.AddCell (cellSequence);
															RepeatHeading = true;
														}

														//PdfPTable guidedDetails = new PdfPTable (1);
														PdfPCell guidedItem = new PdfPCell (
															new Phrase ("" + checkList.description, headerFont));
														//guidedItem.BorderWidth = 0;
														guidedItem.BorderWidthTop = 1;
														guidedItem.BorderWidthRight = 0;
														guidedItem.BorderWidthBottom = 1;
														guidedItem.BorderWidthLeft = 1;

														guidedItem.Colspan =1;
														//guidedDetails.AddCell (guidedItem);
														PdfPCell cellPhotoAdd;
														foreach (var photo in checkList.photos) 
														{	
															if (ParentTable.Rows.Count > RowLimit) {
																break;
															}														
															iTextSharp.text.Image image = Image.GetInstance (photo);
															if (null != image) 
															{
																image.ScaleAbsolute (430f, 370f);
																cellPhotoAdd = new PdfPCell (image);
																cellPhotoAdd.PaddingLeft = 1;
																cellPhotoAdd.PaddingTop = 1;
																cellPhotoAdd.Colspan = 3;
																cellPhotoAdd.PaddingBottom = 1;
																cellPhotoAdd.BorderWidthTop = 1;
																cellPhotoAdd.BorderWidthRight = 1;
																cellPhotoAdd.BorderWidthBottom = 1;
																cellPhotoAdd.BorderWidthLeft = 1;
																cellPhotoAdd.BorderColor = Color.GRAY;
																ParentTable.AddCell (guidedItem);
																ParentTable.AddCell (cellPhotoAdd);

															}
														}
													}
												}
											}
										}
										else
											if (opt.photos != null && opt.photos.Count > 0) 
											{	
												bool RepeatHeader = false;
												foreach (var photo in opt.photos) 
												{
													if (ParentTable.Rows.Count > RowLimit) {
														break;
													}
													iTextSharp.text.Image image = Image.GetInstance (photo.Image);
													if (null != image) 
													{
														if (!RepeatHeader) 
														{
															PdfPCell cellSequence = new PdfPCell (
																new Phrase (""+seq.name + " - " + lvl.name + " - " + spc.name + " - " + opt.name, headerFont));
															cellSequence.HorizontalAlignment = PdfCell.ALIGN_CENTER;
															cellSequence.BorderWidth = 0;
															cellSequence.BackgroundColor = Color.RED;
															cellSequence.Colspan = 4;
															ParentTable.AddCell (cellSequence);
															RepeatHeader = true;
														}

														PdfPCell cellPhotoAdd;
														image.ScaleAbsolute (574.5f, 300f);
														cellPhotoAdd = new PdfPCell (image);
														cellPhotoAdd.BorderWidthLeft = 0;
														cellPhotoAdd.Colspan = 4;
														cellPhotoAdd.BorderColor = Color.GRAY;
														cellPhotoAdd.FixedHeight = 300;
														ParentTable.AddCell (cellPhotoAdd);
													}
												}
											}
									}
								}

							}
						} 
						else if (lvl.Options != null) {
							if (ParentTable.Rows.Count > RowLimit) {
								break;
							}
							foreach (var opt in lvl.Options) {
								if (ParentTable.Rows.Count > RowLimit) {
									break;
								}
								if (opt.photos != null && opt.photos.Count > 0) 
								{
									bool RepeatHeader = false;
									if (opt.photos != null)
									{
										foreach (var photo in opt.photos) 
										{
											if (ParentTable.Rows.Count > RowLimit) {
												break;
											}
											iTextSharp.text.Image image = Image.GetInstance (photo.Image);
											if (null != image) 
											{
												if(!RepeatHeader)
												{
													PdfPCell cellSequence = new PdfPCell (
														new Phrase (""+seq.name+" - " +"" + lvl.name+" - "+opt.name, headerFont));
													cellSequence.HorizontalAlignment=PdfCell.ALIGN_CENTER;
													cellSequence.BorderWidth = 0;
													cellSequence.BackgroundColor = Color.RED;
													cellSequence.Colspan = 4;
													ParentTable.AddCell (cellSequence);
													RepeatHeader = true;
												}
												PdfPCell cellPhotoAdd;

												image.ScaleAbsolute (574.5f, 300f);
												cellPhotoAdd = new PdfPCell (image);
												cellPhotoAdd.BorderWidthLeft = 0;
												cellPhotoAdd.Colspan = 4;
												cellPhotoAdd.BorderColor = Color.GRAY;
												cellPhotoAdd.FixedHeight = 300;
												ParentTable.AddCell (cellPhotoAdd);
											}
										}
									}
								}
							}

						}
					}
				}
				else if(seq.Options != null)
				{
					if (ParentTable.Rows.Count > RowLimit) {
						break;
					}
					foreach (var opt in seq.Options) 
					{
						if (ParentTable.Rows.Count > 10) {
							break;
						}	
						if (opt.photos != null && opt.photos.Count > RowLimit) 
						{
							bool RepeatHeader = false;
							if (opt.photos != null)
							{
								foreach (var photo in opt.photos) 
								{
									if (ParentTable.Rows.Count > RowLimit) {
										break;
									}	
									iTextSharp.text.Image image = Image.GetInstance (photo.Image);
									if (null != image) 
									{
										if (!RepeatHeader)
										{
											PdfPCell cellSequence = new PdfPCell (
												new Phrase (""+seq.name +" - " + opt.name, headerFont));
											cellSequence.HorizontalAlignment = PdfCell.ALIGN_CENTER;
											cellSequence.BorderWidth = 0;
											cellSequence.BackgroundColor = Color.RED;
											cellSequence.Colspan = 4;
											ParentTable.AddCell (cellSequence);
											RepeatHeader = true;
										}
										PdfPCell cellPhotoAdd;
										image.ScaleAbsolute (574.5f, 300f);
										cellPhotoAdd = new PdfPCell (image);
										cellPhotoAdd.BorderWidthLeft = 0;
										cellPhotoAdd.Colspan = 4;
										cellPhotoAdd.BorderColor = Color.GRAY;
										cellPhotoAdd.FixedHeight = 300;
										ParentTable.AddCell (cellPhotoAdd);
									}
								}
							}
						}
					}

				}
			}

			PdfPCell Blankcel1 = new PdfPCell(new Phrase(" ", headerFont));
			Blankcel1.FixedHeight = 30;
			Blankcel1.BorderWidth = 0;
			Blankcel1.Colspan = 4;
			ParentTable.AddCell (Blankcel1);

			PunchService PunchTab=new PunchService(AppDelegate.DatabaseContext);
			var punchItems = PunchTab.GetPunchList (AppDelegate.DatabaseContext,inspectionObject.inspectionID,inspectionObject.projectID);
			if (punchItems != null) 
			{
				int count = 0;
				if (punchItems != null && punchItems.Count > 0) 
				{
					bool RepeatHeader = false;
					foreach (var Punchpic in punchItems) 
					{	
						if (ParentTable.Rows.Count > RowLimit) {
							break;
						}					
						count++;
						var	PunchImage = DAL.DO.PunchListImageDO.getPunchImageList (AppDelegate.DatabaseContext, Punchpic.PunchID);
						if (PunchImage != null && PunchImage.Count > 0) 
						{
							foreach (var imagecollection in PunchImage) 
							{	
								if (ParentTable.Rows.Count > RowLimit) {
									break;
								}
								PdfPCell PunchItem = new PdfPCell ();

								PunchItem.AddElement(new Phrase ("Item " + count, headerFont));
								PunchItem.AddElement(new Phrase (Punchpic.punchDescription, cellContentFont));
								//PunchItem.BorderWidth = 0;
								PunchItem.BorderWidthTop = 1;
								PunchItem.BorderWidthRight = 0;
								PunchItem.BorderWidthBottom = 1;
								PunchItem.BorderWidthLeft = 1;
								PdfPCell punchHeading = new PdfPCell (
									new Phrase (Punchpic.punchDescription, cellContentFont));						
								punchHeading.FixedHeight = 80;
								punchHeading.BorderWidth = 0;
								PunchItem.Colspan = 1;
								punchHeading.BorderColor = Color.GRAY;

								//adding the cell for adding photo
								PdfPCell cellPhotoAdd;
								if (imagecollection != null) 
								{
									if (imagecollection.PunchListImage != null) 
									{
										if (!RepeatHeader)
										{
											RepeatHeader = true;
											string punchListHeading = "Non Conformance Items";
											if (inspectionObject.inspectionID == Constants.FINAL_INSPECTIONID) {
												punchListHeading = "Punch List Items";
											}

											PdfPCell HeadingPunch = new PdfPCell (new Phrase (punchListHeading, headerFont));
											HeadingPunch.BackgroundColor = Color.RED;
											HeadingPunch.FixedHeight = 30;
											HeadingPunch.BorderWidth = 0;
											HeadingPunch.HorizontalAlignment = PdfCell.ALIGN_CENTER;
											HeadingPunch.Colspan = 4;
											ParentTable.AddCell (HeadingPunch);	

										}
										ParentTable.AddCell (PunchItem);


										iTextSharp.text.Image image1 = Image.GetInstance (imagecollection.PunchListImage);
										if (null != imagecollection) 
										{    image1.ScaleAbsolute (430f, 370f);

											cellPhotoAdd = new PdfPCell (image1);
											cellPhotoAdd.BorderWidthLeft = 0;


											cellPhotoAdd.PaddingLeft = 1;
											cellPhotoAdd.PaddingTop = 1;
											cellPhotoAdd.Colspan = 3;
											//cellPhotoAdd.Rowspan = 2;
											cellPhotoAdd.PaddingBottom = 1;

											cellPhotoAdd.BorderColor = Color.GRAY;
											ParentTable.AddCell (cellPhotoAdd);
										}

									}
								}

								//ParentTable.AddCell (photoLogTable);
							}
						}
					}
				}
			}
			return ParentTable;
		}

		private byte[] ToByteArray(Stream stream)
		{
			byte[] buffer = new byte[stream.Length - 1];
			stream.Read(buffer, 0, buffer.Length);
			return buffer;
		}

	}
}