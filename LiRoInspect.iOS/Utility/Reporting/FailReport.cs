using System;
using iTextSharp.text;
using iTextSharp.text.pdf;
using UIKit;
using System.IO;
using Model;
using Foundation;
using System.Diagnostics;
using System.Linq;

namespace LiRoInspect.iOS.Reporting
{
	public class FailReport : IReportHandler
	{
		#region IReportHandler implementation

		/// <summary>
		/// Generates the report.
		/// </summary>
		/// <returns>The report.</returns>
		/// <param name="fileName">File name.</param>
		/// <param name="inspectionObject">Inspection object.</param>
		public string GenerateReport(string fileName, Inspection inspectionObject)
		{
			string appRootDir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
			//Console.WriteLine (appRootDir);		
			// Step 1: Creating System.IO.FileStream object
			DirectoryInfo path = Directory.CreateDirectory(appRootDir + "/LiRoReport");
			FileStream fs = new FileStream(path.FullName + "/" + fileName, FileMode.Create, FileAccess.ReadWrite, FileShare.None);

			iTextSharp.text.Document doc = new iTextSharp.text.Document(PageSize.A4, 20f, 20f, 60f, 60f);

			// Step 3: Creating iTextSharp.text.pdf.PdfWriter object
			// It helps to write the Document to the Specified FileStream
			PdfWriter writer = PdfWriter.GetInstance(doc, fs);
			writer.PageEvent = new ItextPageEvents(ReportType.Fail);
			try
			{
				doc.Open();
				PdfPTable table = FailReportView(inspectionObject);
				if (table != null)
				{
					doc.ResetHeader();
					doc.Add(table);
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Exception occured in Fail GenerateReport" + ex.Message);
			}
			finally
			{
				doc.Close();
				writer.Close();
			}
			return fs.Name;
		}
		#endregion



		/// <summary>
		/// Generates fail report view
		/// </summary>
		/// <returns>The report view.</returns>
		/// <param name="inspectionObject">Inspection object.</param>

		public PdfPTable FailReportView(Inspection inspectionObject)
		{
			// Header Font
			Font headerFont = new Font();
			headerFont.SetFamily(BaseFont.HELVETICA);
			headerFont.Color = Color.WHITE;
			headerFont.SetStyle(1);
			headerFont.Size = 15;

			//Declaration from contractor
			Font DecFont = new Font();
			DecFont.SetFamily(BaseFont.HELVETICA);
			DecFont.Color = Color.WHITE;
			DecFont.SetStyle(1);
			DecFont.Size = 10;

			//Footer
			Font FootFont = new Font();
			FootFont.SetFamily(BaseFont.HELVETICA);
			FootFont.Color = Color.BLACK;
			FootFont.SetStyle(1);

			FootFont.Size = 10;
			//Failed Footer Header
			Font FailFont = new Font();
			FailFont.SetFamily(BaseFont.HELVETICA);
			FailFont.Color = Color.BLACK;
			FailFont.SetStyle(1);
			FailFont.Size = 13;

			//Report Content
			Font RepFont = new Font();
			RepFont.SetFamily(BaseFont.HELVETICA);
			RepFont.Color = Color.BLACK;
			RepFont.SetStyle(0);
			RepFont.Size = 10;

			// cell Content Font
			Font cellContentFont = new Font();
			cellContentFont.SetFamily(BaseFont.HELVETICA);
			cellContentFont.Color = Color.BLACK;
			cellContentFont.Size = 13;
			cellContentFont.SetStyle(1);

			// cell Content Value Font
			Font cellContentValueFont = new Font();
			cellContentFont.SetFamily(BaseFont.HELVETICA);
			cellContentFont.Color = Color.BLACK;
			cellContentFont.Size = 13;

			//cell Content Comment Value Font

			Font cellContentCommentValueFont = new Font();

			cellContentCommentValueFont.SetFamily(BaseFont.HELVETICA);
			cellContentCommentValueFont.SetStyle(Font.ITALIC);
			cellContentCommentValueFont.Color = Color.BLACK;
			cellContentCommentValueFont.Size = 13;



			//Main Table
			PdfPTable MainTable = new PdfPTable(1);
			MainTable.SplitLate = false;
			MainTable.SplitRows = true;
			MainTable.TotalWidth = PageSize.A4.Width - 20;
			MainTable.LockedWidth = true;

			try
			{
				// General Info Table
				PdfPTable GeneralInfotable = new PdfPTable(3);
				GeneralInfotable.TotalWidth = PageSize.A4.Width - 20;
				GeneralInfotable.LockedWidth = true;

				// GeneralInfo Header
				PdfPCell GeneralInfoHeader = new PdfPCell(new Phrase("General Information", headerFont));
				GeneralInfoHeader.BackgroundColor = Color.RED;
				GeneralInfoHeader.FixedHeight = 30;
				GeneralInfoHeader.BorderWidth = 1;
				GeneralInfoHeader.BorderColor = Color.GRAY;
				GeneralInfoHeader.Colspan = 3;
				GeneralInfoHeader.HorizontalAlignment = PdfCell.ALIGN_CENTER;
				GeneralInfotable.AddCell(GeneralInfoHeader);

				// GeneralInfo Cells Contractor
				PdfPCell cellContractor = new PdfPCell(new Phrase("Contractor", cellContentFont));
				cellContractor.FixedHeight = 25;
				cellContractor.BorderWidthRight = 1;
				cellContractor.BorderWidthBottom = 1;
				cellContractor.BorderWidthLeft = 0;
				cellContractor.BorderWidthTop = 0;
				cellContractor.Colspan = 3;
				cellContractor.HorizontalAlignment = PdfCell.ALIGN_CENTER;
				cellContractor.BorderColorRight = Color.GRAY;
				cellContractor.BorderColorBottom = Color.GRAY;
				cellContractor.BorderColor = Color.GRAY;
				cellContractor.HorizontalAlignment = PdfCell.ALIGN_CENTER;
				GeneralInfotable.AddCell(cellContractor);

				string ContractorName = "";
				if (inspectionObject.ContractorName != null)
				{
					ContractorName = inspectionObject.ContractorName;
				}

				// GeneralInfo Cells Contractor Value
				PdfPCell cellContractorValue = new PdfPCell(new Phrase(ContractorName, cellContentValueFont));
				cellContractorValue.FixedHeight = 30;
				cellContractorValue.BorderWidthRight = 1;
				cellContractorValue.BorderWidthBottom = 0;
				cellContractorValue.BorderWidthLeft = 0;
				cellContractorValue.BorderWidthTop = 0;
				cellContractorValue.Colspan = 3;
				cellContractorValue.HorizontalAlignment = PdfCell.ALIGN_CENTER;
				cellContractorValue.BorderColorRight = Color.GRAY;
				cellContractorValue.HorizontalAlignment = PdfCell.ALIGN_CENTER;
				GeneralInfotable.AddCell(cellContractorValue);

				// GeneralInfo Home ownername
				PdfPCell cellHomwOwnerName = new PdfPCell(new Phrase("Homeowner Name", cellContentFont));
				cellHomwOwnerName.FixedHeight = 25;
				cellHomwOwnerName.BorderWidthRight = 1;
				cellHomwOwnerName.BorderWidthBottom = 1;
				cellHomwOwnerName.BorderWidthLeft = 0;
				cellHomwOwnerName.BorderWidthTop = 1;
				cellHomwOwnerName.BorderColorBottom = Color.GRAY;
				cellHomwOwnerName.BorderColorRight = Color.GRAY;
				cellHomwOwnerName.BorderColorTop = Color.GRAY;
				cellHomwOwnerName.HorizontalAlignment = PdfCell.ALIGN_CENTER;
				GeneralInfotable.AddCell(cellHomwOwnerName);

				// GeneralInfo Home owner ID
				PdfPCell cellHomeOwnerID = new PdfPCell(new Phrase("App ID", cellContentFont));
				cellHomeOwnerID.FixedHeight = 25;
				cellHomeOwnerID.BorderWidthRight = 1;
				cellHomeOwnerID.BorderWidthBottom = 1;
				cellHomeOwnerID.BorderWidthLeft = 0;
				cellHomeOwnerID.BorderWidthTop = 1;
				cellHomeOwnerID.BorderColorBottom = Color.GRAY;
				cellHomeOwnerID.BorderColorRight = Color.GRAY;
				cellHomeOwnerID.BorderColorTop = Color.GRAY;
				cellHomeOwnerID.HorizontalAlignment = PdfCell.ALIGN_CENTER;

				GeneralInfotable.AddCell(cellHomeOwnerID);

				int inspectionAddLength1 = 0, inspectionAddLength2 = 0;
				if (inspectionObject.InspectionAddress1 != null)
				{
					inspectionAddLength1 = inspectionObject.InspectionAddress1.Length;
				}
				if (inspectionObject.InspectionAddress2 != null)
				{
					inspectionAddLength2 = inspectionObject.InspectionAddress2.Length;
				}
				int inspectionAddLength = inspectionAddLength1 + inspectionAddLength1;
				// GeneralInfo Activity Address
				PdfPCell cellActivityAddress = new PdfPCell(new Phrase("Activity Address", cellContentFont));
				cellActivityAddress.FixedHeight = inspectionAddLength;
				cellActivityAddress.BorderWidthRight = 0;
				cellActivityAddress.BorderWidthBottom = 1;
				cellActivityAddress.BorderWidthLeft = 0;
				cellActivityAddress.BorderWidthTop = 1;
				cellActivityAddress.BorderColorBottom = Color.GRAY;
				cellActivityAddress.BorderColorTop = Color.GRAY;
				cellActivityAddress.HorizontalAlignment = PdfCell.ALIGN_CENTER;
				GeneralInfotable.AddCell(cellActivityAddress);

				// GeneralInfo Cells Home Owner Name Value
				string HouseOwnerName = "";
				if (!string.IsNullOrEmpty(inspectionObject.HouseOwnerName))
				{
					HouseOwnerName = inspectionObject.HouseOwnerName;
				}
				PdfPCell cellHomeOwnerNameValue = new PdfPCell(new Phrase(HouseOwnerName, cellContentValueFont));
				cellHomeOwnerNameValue.FixedHeight = 25;
				cellHomeOwnerNameValue.BorderWidthRight = 1;
				cellHomeOwnerNameValue.BorderWidthBottom = 0;
				cellHomeOwnerNameValue.BorderWidthLeft = 0;
				cellHomeOwnerNameValue.BorderWidthTop = 0;
				cellHomeOwnerNameValue.BorderColorRight = Color.GRAY;
				cellHomeOwnerNameValue.HorizontalAlignment = PdfCell.ALIGN_CENTER;
				GeneralInfotable.AddCell(cellHomeOwnerNameValue);

				// GeneralInfo Cells HomeOwnerIDValue
				string AppID = "";
				if (!string.IsNullOrEmpty(inspectionObject.projectID))
				{
					AppID = inspectionObject.projectID.ToString();
				}

				PdfPCell cellHomeOwnerIDValue = new PdfPCell(new Phrase(AppID, cellContentValueFont));
				cellHomeOwnerIDValue.FixedHeight = 30;
				cellHomeOwnerIDValue.BorderWidthRight = 1;
				cellHomeOwnerIDValue.BorderWidthBottom = 0;
				cellHomeOwnerIDValue.BorderWidthLeft = 0;
				cellHomeOwnerIDValue.BorderWidthTop = 0;
				cellHomeOwnerIDValue.BorderColorRight = Color.GRAY;
				cellHomeOwnerIDValue.HorizontalAlignment = PdfCell.ALIGN_CENTER;
				GeneralInfotable.AddCell(cellHomeOwnerIDValue);

				// GeneralInfo Cells Activity Address value
				string address = inspectionObject.InspectionAddress1 + Environment.NewLine + inspectionObject.InspectionAddress2 + Environment.NewLine + inspectionObject.City + " " + inspectionObject.Pincode;
				PdfPCell cellActivityAddressvalue = new PdfPCell(new Phrase(address, cellContentValueFont));
				cellActivityAddressvalue.FixedHeight = 30;
				cellActivityAddressvalue.BorderWidth = 0;
				cellActivityAddressvalue.HorizontalAlignment = PdfCell.ALIGN_CENTER;
				GeneralInfotable.AddCell(cellActivityAddressvalue);

				//Header For Inspection Info
				PdfPCell InsInfoHeader = new PdfPCell(new Phrase("Inspection Information", headerFont));
				InsInfoHeader.BackgroundColor = Color.RED;
				InsInfoHeader.FixedHeight = 30;
				InsInfoHeader.BorderWidth = 1;
				InsInfoHeader.BorderColor = Color.GRAY;
				InsInfoHeader.Colspan = 3;
				InsInfoHeader.HorizontalAlignment = PdfCell.ALIGN_CENTER;
				GeneralInfotable.AddCell(InsInfoHeader);
				MainTable.AddCell(GeneralInfotable);

				PdfPTable nestedContractor = new PdfPTable(3);
				// GeneralInfo Pathway Type
				PdfPCell cellPathwayType = new PdfPCell(new Phrase("Pathway Type", cellContentFont));
				cellPathwayType.FixedHeight = 25;
				cellPathwayType.HorizontalAlignment = PdfCell.ALIGN_CENTER;
				cellPathwayType.BorderWidthRight = 1;
				cellPathwayType.BorderWidthBottom = 1;
				cellPathwayType.BorderWidthLeft = 0;
				cellPathwayType.BorderWidthTop = 0;
				cellPathwayType.BorderColorRight = Color.GRAY;
				cellPathwayType.BorderColorBottom = Color.GRAY;
				cellPathwayType.HorizontalAlignment = PdfCell.ALIGN_CENTER;

				// GeneralInfo Type of Inspection
				PdfPCell cellInspectionType = new PdfPCell(new Phrase("Type of Inspection", cellContentFont));
				cellInspectionType.FixedHeight = 25;
				cellInspectionType.BorderWidthRight = 1;
				cellInspectionType.BorderWidthBottom = 1;
				cellInspectionType.HorizontalAlignment = PdfCell.ALIGN_CENTER;
				cellInspectionType.BorderWidthLeft = 0;
				cellInspectionType.BorderWidthTop = 0;
				cellInspectionType.BorderColorBottom = Color.GRAY;
				cellInspectionType.BorderColorRight = Color.GRAY;
				cellInspectionType.HorizontalAlignment = PdfCell.ALIGN_CENTER;

				// GeneralInfo Inspection Attempt
				PdfPCell cellInspectionAttempt = new PdfPCell(new Phrase("Inspection Attempt", cellContentFont));
				cellInspectionAttempt.FixedHeight = 25;
				cellInspectionAttempt.BorderWidthRight = 1;
				cellInspectionAttempt.BorderWidthBottom = 1;
				cellInspectionAttempt.BorderWidthLeft = 0;
				cellInspectionAttempt.BorderWidthTop = 1;
				cellInspectionAttempt.BorderColorBottom = Color.GRAY;
				cellInspectionAttempt.BorderColorTop = Color.GRAY;
				cellInspectionAttempt.BorderColorRight = Color.GRAY;
				cellInspectionAttempt.HorizontalAlignment = PdfCell.ALIGN_CENTER;

				// GeneralInfo Inspection Date
				PdfPCell cellInspectionDate = new PdfPCell(new Phrase("Inspection Date", cellContentFont));
				cellInspectionDate.FixedHeight = 25;
				cellInspectionDate.BorderWidthRight = 1;
				cellInspectionDate.BorderWidthBottom = 1;
				cellInspectionDate.BorderWidthLeft = 0;
				cellInspectionDate.BorderWidthTop = 0;
				cellInspectionDate.BorderColorBottom = Color.GRAY;
				cellInspectionDate.BorderColorRight = Color.GRAY;
				cellInspectionDate.HorizontalAlignment = PdfCell.ALIGN_CENTER;
				// GeneralInfo Cells Pathway type Value
				string Pathway = "";
				if (!string.IsNullOrEmpty(inspectionObject.Pathway.ToString()))
				{
					Pathway = inspectionObject.Pathway.ToString();
				}

				PdfPCell cellPathwayTypeValue = new PdfPCell(new Phrase(Pathway, cellContentValueFont));
				cellPathwayTypeValue.FixedHeight = 30;
				cellPathwayTypeValue.BorderWidthRight = 1;
				cellPathwayTypeValue.BorderWidthBottom = 1;
				cellPathwayTypeValue.BorderWidthLeft = 0;
				cellPathwayTypeValue.BorderWidthTop = 0;
				cellPathwayTypeValue.BorderColorBottom = Color.GRAY;
				cellPathwayTypeValue.BorderColorRight = Color.GRAY;
				cellPathwayTypeValue.HorizontalAlignment = PdfCell.ALIGN_CENTER;
				//nestedContractor.AddCell(cellPathwayTypeValue);

				// GeneralInfo Type of Inspection value
				string InspectionType = "";
				if (!string.IsNullOrEmpty(inspectionObject.InspectionType))
				{
					InspectionType = inspectionObject.InspectionType;
				}
				PdfPCell cellInspectionTypeValue = new PdfPCell(new Phrase(InspectionType, cellContentValueFont));
				cellInspectionTypeValue.FixedHeight = 30;
				cellInspectionTypeValue.BorderWidthRight = 1;
				cellInspectionTypeValue.BorderWidthBottom = 0;
				cellInspectionTypeValue.BorderWidthLeft = 0;
				cellInspectionTypeValue.BorderWidthTop = 0;
				cellInspectionTypeValue.BorderColorBottom = Color.GRAY;
				cellInspectionTypeValue.BorderColorRight = Color.GRAY;
				cellInspectionTypeValue.HorizontalAlignment = PdfCell.ALIGN_CENTER;

				// GeneralInfo Inspection Attempt value
				string InspectionAttemptCount = "";
				if (!string.IsNullOrEmpty(inspectionObject.InspectionAttemptCount))
				{
					InspectionAttemptCount = inspectionObject.InspectionAttemptCount;
				}

				PdfPCell cellInspectionAttemptValue = new PdfPCell(new Phrase(InspectionAttemptCount, cellContentFont));
				cellInspectionAttemptValue.FixedHeight = 30;
				cellInspectionAttemptValue.BorderWidthRight = 1;
				cellInspectionAttemptValue.BorderWidthBottom = 0;
				cellInspectionAttemptValue.BorderWidthLeft = 0;
				cellInspectionAttemptValue.BorderWidthTop = 0;
				cellInspectionAttemptValue.BorderColorTop = Color.GRAY;
				cellInspectionAttemptValue.BorderColorRight = Color.GRAY;
				cellInspectionAttemptValue.HorizontalAlignment = PdfCell.ALIGN_CENTER;
				string cellDateString = "";
				if (!string.IsNullOrEmpty(inspectionObject.inspectionDateTime.ToString()))
				{
					NSDate InspectionDate = (NSDate)DateTime.SpecifyKind(inspectionObject.inspectionDateTime, DateTimeKind.Utc);
					NSDateFormatter dateformatter = new NSDateFormatter();
					dateformatter.DateFormat = @"MM/dd/yyyy";
					cellDateString = dateformatter.StringFor(InspectionDate);
				}
				// GeneralInfo Inspection Date value
				PdfPCell cellInspectionDateValue = new PdfPCell(new Phrase(cellDateString, cellContentValueFont));
				cellInspectionDateValue.FixedHeight = 30;
				cellInspectionDateValue.BorderWidthRight = 1;
				cellInspectionDateValue.BorderWidthBottom = 0;
				cellInspectionDateValue.BorderWidthLeft = 0;
				cellInspectionDateValue.BorderWidthTop = 0;
				cellInspectionDateValue.BorderColorRight = Color.GRAY;
				cellInspectionDateValue.HorizontalAlignment = PdfCell.ALIGN_CENTER;

				// GeneralInfo Inspector Name
				PdfPCell cellInspectorName = new PdfPCell(new Phrase("Inspector Name", cellContentFont));
				cellInspectorName.FixedHeight = 25;
				cellInspectorName.BorderWidthRight = 1;
				cellInspectorName.BorderWidthBottom = 0;
				cellInspectorName.BorderWidthLeft = 0;
				cellInspectorName.BorderWidthTop = 0;
				cellInspectorName.BorderColorRight = Color.GRAY;
				cellInspectorName.HorizontalAlignment = PdfCell.ALIGN_CENTER;
				nestedContractor.AddCell(cellInspectorName);
				nestedContractor.AddCell(cellPathwayType);
				nestedContractor.AddCell(cellInspectionType);

				// GeneralInfo Inspection Time
				PdfPCell cellInspectionTime = new PdfPCell(new Phrase("Inspection Time", cellContentFont));
				cellInspectionTime.BorderWidthRight = 1;
				cellInspectionTime.BorderWidthBottom = 0;
				cellInspectionTime.BorderWidthLeft = 0;
				cellInspectionTime.BorderWidthTop = 0;
				cellInspectionTime.BorderColorTop = Color.GRAY;
				cellInspectionTime.BorderColorRight = Color.GRAY;
				cellInspectionTime.FixedHeight = 25;
				cellInspectionTime.HorizontalAlignment = PdfCell.ALIGN_CENTER;
				// GeneralInfo Inspection blank value
				PdfPCell cellInspectionblank = new PdfPCell(new Phrase("", cellContentFont));
				cellInspectionblank.BorderWidthRight = 1;
				cellInspectionblank.BorderWidthBottom = 0;
				cellInspectionblank.BorderWidthLeft = 0;
				cellInspectionblank.BorderWidthTop = 1;
				cellInspectionblank.BorderColorTop = Color.GRAY;
				cellInspectionblank.BorderColorRight = Color.GRAY;
				cellInspectionblank.FixedHeight = 30;

				// GeneralInfo Inspection blank value
				PdfPCell cellInspectionblank1 = new PdfPCell(new Phrase("", cellContentFont));
				cellInspectionblank1.FixedHeight = 30;
				cellInspectionblank1.BorderWidthRight = 0;
				cellInspectionblank1.BorderWidthBottom = 0;
				cellInspectionblank1.BorderWidthLeft = 0;
				cellInspectionblank1.BorderWidthTop = 1;
				cellInspectionblank1.BorderColorTop = Color.GRAY;

				// GeneralInfo Inspector Name value
				string Represtativename = "";
				if (!string.IsNullOrEmpty(inspectionObject.RepresentativeName))
				{
					Represtativename = inspectionObject.RepresentativeName;
				}
				PdfPCell cellInspectorNamevalue = new PdfPCell(new Phrase(Represtativename, cellContentValueFont));
				cellInspectorNamevalue.FixedHeight = 30;
				cellInspectorNamevalue.BorderWidthRight = 1;
				cellInspectorNamevalue.BorderWidthBottom = 1;
				cellInspectorNamevalue.BorderWidthLeft = 0;
				cellInspectorNamevalue.BorderWidthTop = 1;
				cellInspectorNamevalue.BorderColorBottom = Color.GRAY;
				cellInspectorNamevalue.BorderColorTop = Color.GRAY;
				cellInspectorNamevalue.BorderColorRight = Color.GRAY;
				cellInspectorNamevalue.HorizontalAlignment = PdfCell.ALIGN_CENTER;

				nestedContractor.AddCell(cellInspectorNamevalue);
				nestedContractor.AddCell(cellPathwayTypeValue);
				nestedContractor.AddCell(cellInspectionTypeValue);
				nestedContractor.AddCell(cellInspectionDate);
				nestedContractor.AddCell(cellInspectionTime);
				nestedContractor.AddCell(cellInspectionAttempt);

				// GeneralInfo Inspection Time value
				string Instime = "";
				if (!string.IsNullOrEmpty(inspectionObject.inspectionDateTime.ToString()))
				{
					Instime = inspectionObject.inspectionDateTime.ToString("hh:mm tt");
				}
				PdfPCell cellInspectionTimeValue = new PdfPCell(new Phrase(Instime, cellContentValueFont));
				cellInspectionTimeValue.FixedHeight = 30;
				cellInspectionTimeValue.BorderWidthRight = 1;
				cellInspectionTimeValue.BorderWidthBottom = 0;
				cellInspectionTimeValue.BorderWidthLeft = 0;
				cellInspectionTimeValue.BorderWidthTop = 1;
				cellInspectionTimeValue.BorderColorTop = Color.GRAY;
				cellInspectionTimeValue.BorderColorRight = Color.GRAY;
				cellInspectionTimeValue.HorizontalAlignment = PdfCell.ALIGN_CENTER;

				nestedContractor.AddCell(cellInspectionDateValue);
				nestedContractor.AddCell(cellInspectionTimeValue);
				nestedContractor.AddCell(cellInspectionAttemptValue);

				MainTable.AddCell(nestedContractor);

				//adding a blank cell
				PdfPCell cellblank = new PdfPCell(new Phrase("", cellContentFont));
				cellblank.FixedHeight = 25;
				cellblank.BorderColor = Color.WHITE;
				MainTable.AddCell(cellblank);

				PdfPTable Inspectiontable = new PdfPTable(5);
				// InspectionInfo Header
				PdfPCell InspectionInfoHeader = new PdfPCell(new Phrase("Items Observed", headerFont));
				InspectionInfoHeader.BackgroundColor = Color.RED;
				InspectionInfoHeader.FixedHeight = 30;
				InspectionInfoHeader.BorderWidth = 1;
				InspectionInfoHeader.Colspan = 5;
				InspectionInfoHeader.BorderColor = Color.GRAY;
				InspectionInfoHeader.HorizontalAlignment = PdfCell.ALIGN_CENTER;
				Inspectiontable.AddCell(InspectionInfoHeader);

				foreach (var seqLst in inspectionObject.sequences)
				{
					if (seqLst.Levels != null)
					{
						foreach (var lvl in seqLst.Levels)
						{
							if (lvl.Spaces != null)
							{
								foreach (var spc in lvl.Spaces)
								{
									if (spc.Options != null && spc.isSelected)
									{
										foreach (var opt in spc.Options)
										{
											if (opt.checkListItems != null && opt.checkListItems.Count > 0)
											{
												var checkList = opt.checkListItems.Find(c => c.Result == ResultType.FAIL);
												if (checkList != null)
												{
													PdfPCell cellSeqlName = new PdfPCell(new Phrase(seqLst.name, cellContentFont));
													cellSeqlName.Colspan = 6;
													cellSeqlName.BorderWidth = 0;
													Inspectiontable.AddCell(cellSeqlName);

													PdfPCell cellLavelName = new PdfPCell(new Phrase(lvl.name + " - " + spc.name, cellContentFont));
													cellLavelName.PaddingLeft = 15;
													cellLavelName.Colspan = 6;
													cellLavelName.BorderWidth = 0;
													Inspectiontable.AddCell(cellLavelName);

													PdfPCell cellOptionName = new PdfPCell(new Phrase(opt.name, cellContentFont));
													cellOptionName.PaddingLeft = 25;
													cellOptionName.Colspan = 6;
													cellOptionName.BorderWidth = 0;
													Inspectiontable.AddCell(cellOptionName);

													foreach (var chk in opt.checkListItems)
													{
														if (chk.Result == ResultType.FAIL)
														{
															PdfPCell cellCheckListName = new PdfPCell(new Phrase(chk.description, cellContentValueFont));
															cellCheckListName.PaddingLeft = 35;
															cellCheckListName.Colspan = 6;
															cellCheckListName.BorderWidth = 0;
															Inspectiontable.AddCell(cellCheckListName);

															PdfPCell cellCheckListComments = new PdfPCell(new Phrase(chk.comments, cellContentCommentValueFont));
															cellCheckListComments.PaddingLeft = 45;

															cellCheckListComments.Colspan = 6;
															cellCheckListComments.BorderWidth = 0;
															Inspectiontable.AddCell(cellCheckListComments);
														}
													}
												}
											}
										}
									}
								}
							}

							if (lvl.Options != null)
							{
								foreach (var opt in lvl.Options)
								{
									if (opt.checkListItems != null && opt.checkListItems.Count > 0)
									{
										var checkList = opt.checkListItems.Find(c => c.Result == ResultType.FAIL);
										if (checkList != null)
										{
											PdfPCell cellSeqlName = new PdfPCell(new Phrase(seqLst.name, cellContentFont));
											cellSeqlName.Colspan = 6;
											cellSeqlName.BorderWidth = 0;
											Inspectiontable.AddCell(cellSeqlName);

											PdfPCell cellLavelName = new PdfPCell(new Phrase(lvl.name, cellContentFont));
											cellLavelName.PaddingLeft = 15;
											cellLavelName.Colspan = 6;
											cellLavelName.BorderWidth = 0;
											Inspectiontable.AddCell(cellLavelName);

											PdfPCell cellOptionName = new PdfPCell(new Phrase(opt.name, cellContentFont));
											cellOptionName.PaddingLeft = 25;
											cellOptionName.Colspan = 6;
											cellOptionName.BorderWidth = 0;
											Inspectiontable.AddCell(cellOptionName);
											foreach (var chk in opt.checkListItems)
											{
												if (chk.Result == ResultType.FAIL)
												{
													PdfPCell cellCheckListName = new PdfPCell(new Phrase(chk.description, cellContentValueFont));
													cellCheckListName.PaddingLeft = 35;
													cellCheckListName.Colspan = 6;
													cellCheckListName.BorderWidth = 0;
													Inspectiontable.AddCell(cellCheckListName);

													PdfPCell cellCheckListComments = new PdfPCell(new Phrase(chk.comments, cellContentCommentValueFont));
													cellCheckListComments.PaddingLeft = 45;
													cellCheckListComments.Colspan = 6;
													cellCheckListComments.BorderWidth = 0;
													Inspectiontable.AddCell(cellCheckListComments);

												}
											}
										}
									}
								}
							}
						}
					}
					if (seqLst.Options != null)
					{
						foreach (var opt in seqLst.Options)
						{
							if (opt.checkListItems != null && opt.checkListItems.Count > 0)
							{
								var checkList = opt.checkListItems.Find(c => c.Result == ResultType.FAIL);
								if (checkList != null)
								{
									PdfPCell cellSeqlName = new PdfPCell(new Phrase(seqLst.name, cellContentFont));
									cellSeqlName.Colspan = 6;
									cellSeqlName.BorderWidth = 0;
									Inspectiontable.AddCell(cellSeqlName);

									PdfPCell cellOptionName = new PdfPCell(new Phrase(opt.name, cellContentFont));
									cellOptionName.PaddingLeft = 15;
									cellOptionName.Colspan = 6;
									cellOptionName.BorderWidth = 0;
									Inspectiontable.AddCell(cellOptionName);

									foreach (var chkList in opt.checkListItems)
									{
										string Comments = "";
										if (chkList.Result == ResultType.FAIL)
										{
											PdfPCell cellItemValuessadded2 = new PdfPCell(new Phrase(chkList.description, cellContentValueFont));
											cellItemValuessadded2.PaddingLeft = 25;
											cellItemValuessadded2.Colspan = 6;
											cellItemValuessadded2.BorderWidth = 0;
											Inspectiontable.AddCell(cellItemValuessadded2);

											if (Comments != chkList.comments)
											{
												PdfPCell cellItemValuessadded3 = new PdfPCell(new Phrase(chkList.comments, cellContentCommentValueFont));
												cellItemValuessadded3.PaddingLeft = 30;
												cellItemValuessadded3.Colspan = 6;
												cellItemValuessadded3.BorderWidth = 0;
												Inspectiontable.AddCell(cellItemValuessadded3);
												Comments = chkList.comments;
											}

										}

									}

								}
							}
						}
					}
				}

				//adding it ti main table
				MainTable.AddCell(Inspectiontable);
				PdfPTable tableCloseandDesc = new PdfPTable(12);
				tableCloseandDesc.TotalWidth = PageSize.A4.Width - 20;
				tableCloseandDesc.LockedWidth = true;

				string str = "This report is documented proof that the home has FAILED to meet the acceptable conditions as shown listed. Prime Contractor is responsible for any and all corrective measures required and scheduling a new inspection";

				System.IO.Stream stream = UIImage.FromBundle("Cross").AsPNG().AsStream();
				byte[] array = ToByteArray(stream);
				iTextSharp.text.Image image = Image.GetInstance(array);

				image.ScaleToFit(10f, 10f);
				image.Alignment = Element.ALIGN_RIGHT;
				PdfPCell cellImage = new PdfPCell();
				cellImage.AddElement(new Chunk(image, 0, 0));
				cellImage.PaddingLeft = 15f;
				cellImage.PaddingTop = 25f;
				cellImage.BorderColor = Color.RED;
				cellImage.BackgroundColor = Color.RED;
				cellImage.Rowspan = 2;
				tableCloseandDesc.AddCell(cellImage);
				//Adding Failed header
				PdfPCell InspectionFailed = new PdfPCell(new Phrase("Failed", FailFont));
				InspectionFailed.FixedHeight = 20;
				InspectionFailed.BackgroundColor = Color.RED;

				InspectionFailed.BorderWidthBottom = 2;
				InspectionFailed.BorderWidthLeft = 0;
				InspectionFailed.BorderWidthRight = 0;
				InspectionFailed.BorderWidthTop = 0;
				InspectionFailed.BorderColorBottom = Color.BLACK;
				tableCloseandDesc.AddCell(InspectionFailed);

				//Adding blank column
				//Adding Failed header
				PdfPCell InspectionBlank = new PdfPCell(new Phrase("", FailFont));
				InspectionBlank.FixedHeight = 20;
				InspectionBlank.BorderColor = Color.RED;
				InspectionBlank.BorderWidth = 0;
				InspectionBlank.Colspan = 10;
				InspectionBlank.BackgroundColor = Color.RED;

				tableCloseandDesc.AddCell(InspectionBlank);

				// InspectionInfo Failed Message
				PdfPCell InspectionFailedHeader = new PdfPCell(new Phrase(str, FootFont));
				InspectionFailedHeader.FixedHeight = 40;
				InspectionFailedHeader.BorderWidth = 1;
				InspectionFailedHeader.Colspan = 11;
				InspectionFailedHeader.BorderColor = Color.RED;
				InspectionFailedHeader.BackgroundColor = Color.RED;
				tableCloseandDesc.AddCell(InspectionFailedHeader);

				MainTable.AddCell(tableCloseandDesc);
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Exception Occured in FailReportView method due to " + ex.Message);
			}
			return MainTable;
		}

		private byte[] ToByteArray(Stream stream)
		{
			stream.Position = 0;
			byte[] buffer = new byte[stream.Length];
			for (int totalBytesCopied = 0; totalBytesCopied < stream.Length;)
				totalBytesCopied += stream.Read(buffer, totalBytesCopied, Convert.ToInt32(stream.Length) - totalBytesCopied);
			return buffer;
		}
	}
}