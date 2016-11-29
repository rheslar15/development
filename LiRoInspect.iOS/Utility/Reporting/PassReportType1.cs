using System;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Threading.Tasks;
using System.Collections;
using UIKit;
using Model;
using Foundation;


//using PCLStorage;
using System.Collections.Generic;
using System.Diagnostics;
using DAL.DO;


namespace LiRoInspect.iOS.Reporting
{
	public class PassReportType1 : IReportHandler
	{
		#region IReportHandler implementation

		public bool isAlreadyFilled = true;
		/// <summary>
		/// Generates the report.
		/// </summary>
		/// <returns>The report.</returns>
		/// <param name="filename">Filename.</param>
		/// <param name="inspectionObject">Inspection object.</param>
		public string GenerateReport(string filename, Inspection inspectionObject)
		{
			string appRootDir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
			//Console.WriteLine (appRootDir);        
			// Step 1: Creating System.IO.FileStream object
			DirectoryInfo path = Directory.CreateDirectory(appRootDir + "/LiRoReport");
			FileStream fs = new FileStream(path.FullName + "/" + filename, FileMode.Create, FileAccess.ReadWrite, FileShare.None);

			// Step 2: Creating iTextSharp.text.pdf.Document object
			iTextSharp.text.Document doc = new iTextSharp.text.Document(PageSize.A4, 20f, 20f, 60f, 60f);

			// Step 3: Creating iTextSharp.text.pdf.PdfWriter object
			// It helps to write the Document to the Specified FileStream
			PdfWriter writer = PdfWriter.GetInstance(doc, fs);
			writer.PageEvent = new ItextPageEvents(ReportType.Pass);

			// Step 4: Openning the Document
			doc.Open();

			PdfPTable table = PassInspectionReportTable(inspectionObject);
			try
			{
				if (table != null)
				{
					doc.ResetHeader();
					doc.Add(table);
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Exception occured in Pass GenerateReport" + ex.Message);
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
		/// Preparing the inspection Pass report table.
		/// </summary>
		/// <returns>The inspection report table.</returns>
		/// <param name="inspectionObject">Inspection object.</param>
		private PdfPTable PassInspectionReportTable(Inspection inspectionObject)
		{
			PdfPCell cellPunchListHeader = null;
			// Header Font
			Font headerFont = new Font();
			headerFont.SetFamily(BaseFont.HELVETICA);
			headerFont.Color = Color.WHITE;
			headerFont.SetStyle(1);
			headerFont.Size = 15;

			//Declaration from contractor
			Font DecFont = new Font();
			DecFont.SetFamily(BaseFont.HELVETICA);
			DecFont.Color = Color.BLACK;
			DecFont.SetStyle(0);
			DecFont.Size = 15;

			// cell Content Font
			Font cellContentFont = new Font();
			cellContentFont.SetFamily(BaseFont.HELVETICA);
			cellContentFont.Color = Color.BLACK;
			cellContentFont.Size = 15;
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


			// Main Table
			PdfPTable MainTable = new PdfPTable(1);
			try
			{
				MainTable.TotalWidth = PageSize.A4.Width - 20;
				MainTable.LockedWidth = true;

				// General Info Table
				PdfPTable GeneralInfotable = new PdfPTable(6);

				// GeneralInfo Header
				PdfPCell GeneralInfoHeader = new PdfPCell(new Phrase("General Information", headerFont));
				GeneralInfoHeader.BackgroundColor = Color.RED;
				GeneralInfoHeader.FixedHeight = 30;
				GeneralInfoHeader.BorderWidthTop = 1;

				GeneralInfoHeader.BorderColorTop = Color.GRAY;
				GeneralInfoHeader.Colspan = 6;
				GeneralInfoHeader.HorizontalAlignment = PdfCell.ALIGN_CENTER;
				GeneralInfoHeader.BorderWidthLeft = 0;
				GeneralInfoHeader.BorderWidthRight = 0;
				GeneralInfotable.AddCell(GeneralInfoHeader);

				// GeneralInfo Cells Contractor
				PdfPCell cellContractor = new PdfPCell(new Phrase("Contractor", cellContentFont));
				cellContractor.FixedHeight = 25;
				cellContractor.BorderWidthTop = 1;
				cellContractor.BorderColorTop = Color.GRAY;
				cellContractor.HorizontalAlignment = PdfCell.ALIGN_CENTER;
				cellContractor.BorderWidthLeft = 0;
				cellContractor.Colspan = 6;
				cellContractor.BorderWidthRight = 1;
				cellContractor.BorderColorRight = Color.GRAY;
				GeneralInfotable.AddCell(cellContractor);

				// GeneralInfo Cells Contractor Value
				string ContractorName = "";
				if (inspectionObject.ContractorName != null)
				{
					ContractorName = inspectionObject.ContractorName;
				}

				PdfPCell cellContractorValue = new PdfPCell(new Phrase(ContractorName, cellContentValueFont));
				cellContractorValue.FixedHeight = 25;
				cellContractorValue.BorderWidthTop = 1;
				cellContractorValue.BorderColorTop = Color.GRAY;
				cellContractorValue.HorizontalAlignment = PdfCell.ALIGN_CENTER;
				cellContractorValue.BorderWidthLeft = 0;
				cellContractorValue.Colspan = 6;
				cellContractorValue.BorderWidthRight = 1;
				cellContractorValue.BorderWidthBottom = 0;
				cellContractorValue.BorderColorRight = Color.GRAY;
				GeneralInfotable.AddCell(cellContractorValue);

				// Generate Home owner name Heading
				PdfPCell cellHomeownerName = new PdfPCell(new Phrase("Homeowner Name", cellContentFont));
				cellHomeownerName.FixedHeight = 25;
				//cellHomeownerName.BorderWidth = 0;
				cellHomeownerName.Colspan = 2;
				cellHomeownerName.HorizontalAlignment = PdfCell.ALIGN_CENTER;
				cellHomeownerName.BorderWidthTop = 1;
				cellHomeownerName.BorderColorRight = Color.GRAY;
				cellHomeownerName.BorderColorTop = Color.GRAY;
				cellHomeownerName.BorderWidthLeft = 0;
				cellHomeownerName.BorderWidthBottom = 0;
				cellHomeownerName.BorderWidthRight = 1;
				GeneralInfotable.AddCell(cellHomeownerName);

				// Generate Home owner ID Heading
				PdfPCell cellHomeownerId = new PdfPCell(new Phrase("App ID ", cellContentFont));
				cellHomeownerId.BorderWidthRight = 1;
				cellHomeownerId.BorderWidthLeft = 0;
				cellHomeownerId.BorderWidthTop = 1;
				cellHomeownerId.HorizontalAlignment = PdfCell.ALIGN_CENTER;
				cellHomeownerId.Colspan = 2;
				cellHomeownerId.BorderWidthBottom = 0;
				cellHomeownerId.BorderColorRight = Color.GRAY;
				cellHomeownerId.BorderColorTop = Color.GRAY;
				int add2 = 0, add1 = 0;
				if (inspectionObject.InspectionAddress1 != null)
				{
					add1 = inspectionObject.InspectionAddress1.Length;
				}
				if (inspectionObject.InspectionAddress2 != null)
				{
					add2 = inspectionObject.InspectionAddress2.Length;
				}
				int add = add1 + add2;

				// Generate Activity Address Heading
				GeneralInfotable.AddCell(cellHomeownerId);
				PdfPCell activityAddress = new PdfPCell(new Phrase("Activity Address", cellContentFont));
				activityAddress.FixedHeight = add;
				activityAddress.HorizontalAlignment = PdfCell.ALIGN_CENTER;
				activityAddress.BorderWidthRight = 0;
				activityAddress.BorderWidthLeft = 0;
				activityAddress.BorderWidthTop = 1;
				activityAddress.BorderWidthBottom = 0;
				activityAddress.BorderColorTop = Color.GRAY;

				activityAddress.Colspan = 2;
				GeneralInfotable.AddCell(activityAddress);

				string HouseOwnerName = "";
				if (!string.IsNullOrEmpty(inspectionObject.HouseOwnerName))
				{
					HouseOwnerName = inspectionObject.HouseOwnerName;
				}
				// Generate Home owner name  value
				PdfPCell cellHomeownerNamevalue = new PdfPCell(new Phrase(HouseOwnerName, cellContentValueFont));
				cellHomeownerNamevalue.FixedHeight = 25;
				cellHomeownerNamevalue.BorderWidthLeft = 0;
				cellHomeownerNamevalue.BorderWidthRight = 1;
				cellHomeownerNamevalue.HorizontalAlignment = PdfCell.ALIGN_CENTER;
				cellHomeownerNamevalue.BorderWidthTop = 1;
				cellHomeownerNamevalue.Colspan = 2;
				cellHomeownerNamevalue.BorderColorTop = Color.GRAY;
				cellHomeownerNamevalue.BorderWidthBottom = 1;
				cellHomeownerNamevalue.BorderColorBottom = Color.GRAY;
				cellHomeownerNamevalue.BorderColorRight = Color.GRAY;

				GeneralInfotable.AddCell(cellHomeownerNamevalue);
				string AppID = "";
				if (!string.IsNullOrEmpty(inspectionObject.projectID))
				{
					AppID = inspectionObject.projectID.ToString();
				}
				// Generate Home owner ID value
				PdfPCell cellHomeownerIDvalue = new PdfPCell(new Phrase(AppID, cellContentValueFont));
				cellHomeownerIDvalue.FixedHeight = 35;
				cellHomeownerIDvalue.BorderWidthLeft = 0;
				cellHomeownerIDvalue.BorderWidthRight = 1;
				cellHomeownerIDvalue.HorizontalAlignment = PdfCell.ALIGN_CENTER;
				cellHomeownerIDvalue.BorderWidthTop = 1;
				cellHomeownerIDvalue.BorderColorTop = Color.GRAY;
				cellHomeownerIDvalue.BorderColorRight = Color.GRAY;
				cellHomeownerIDvalue.BorderWidthBottom = 1;
				cellHomeownerIDvalue.BorderColorBottom = Color.GRAY;
				cellHomeownerIDvalue.Colspan = 2;
				GeneralInfotable.AddCell(cellHomeownerIDvalue);

				string address = inspectionObject.InspectionAddress1 + " " + inspectionObject.InspectionAddress2 + " " + inspectionObject.City + " " + inspectionObject.Pincode;

				// Generate Home owner address value
				PdfPCell activityAddressValue = new PdfPCell(new Phrase(address, cellContentValueFont));
				activityAddressValue.FixedHeight = 35;
				activityAddressValue.BorderWidthTop = 1;
				activityAddressValue.Colspan = 2;
				activityAddressValue.HorizontalAlignment = PdfCell.ALIGN_CENTER;
				activityAddressValue.BorderColorTop = Color.GRAY;
				activityAddressValue.BorderWidthLeft = 0;
				activityAddressValue.BorderWidthRight = 0;
				activityAddressValue.BorderWidthBottom = 1;
				activityAddressValue.BorderColorBottom = Color.GRAY;
				GeneralInfotable.AddCell(activityAddressValue);

				// Inspection Information  Table

				// Inspection Information Header
				PdfPCell inspectionInfoHeader = new PdfPCell(new Phrase("Inspection Information", headerFont));
				inspectionInfoHeader.BackgroundColor = Color.RED;
				inspectionInfoHeader.FixedHeight = 30;
				inspectionInfoHeader.HorizontalAlignment = PdfCell.ALIGN_CENTER;
				inspectionInfoHeader.BorderWidthTop = 1;
				inspectionInfoHeader.BorderColorTop = Color.GRAY;
				inspectionInfoHeader.BorderWidthLeft = 0;
				inspectionInfoHeader.BorderWidthRight = 0;
				inspectionInfoHeader.Colspan = 6;
				GeneralInfotable.AddCell(inspectionInfoHeader);

				// Generate Type of Inspection Header
				PdfPCell cellInspectionType = new PdfPCell(new Phrase("Type of Inspection ", cellContentFont));
				cellInspectionType.FixedHeight = 25;
				cellInspectionType.HorizontalAlignment = PdfCell.ALIGN_CENTER;
				cellInspectionType.Colspan = 2;
				cellInspectionType.BorderWidthTop = 1;
				cellInspectionType.BorderColorTop = Color.GRAY;
				cellInspectionType.BorderWidthLeft = 0;
				cellInspectionType.BorderWidthRight = 0;

				// Generate Inspector Name Header
				PdfPCell cellInspectorName = new PdfPCell(new Phrase("Inspector Name", cellContentFont));
				cellInspectorName.Colspan = 2;
				cellInspectorName.HorizontalAlignment = PdfCell.ALIGN_CENTER;
				cellInspectorName.FixedHeight = 25;
				cellInspectorName.BorderColorTop = Color.GRAY;
				cellInspectorName.BorderColorRight = Color.GRAY;
				cellInspectorName.BorderWidthLeft = 0;
				cellInspectorName.BorderWidthTop = 1;
				cellInspectorName.BorderWidthRight = 1;
				GeneralInfotable.AddCell(cellInspectorName);

				// Generate Inspector Name Header
				PdfPCell cellIPathwayName = new PdfPCell(new Phrase("Pathway Type ", cellContentFont));
				cellIPathwayName.Colspan = 2;
				cellIPathwayName.HorizontalAlignment = PdfCell.ALIGN_CENTER;
				cellIPathwayName.FixedHeight = 25;
				cellIPathwayName.BorderColorTop = Color.GRAY;
				cellIPathwayName.BorderColorRight = Color.GRAY;
				cellIPathwayName.BorderWidthLeft = 0;
				cellIPathwayName.BorderWidthTop = 1;
				cellIPathwayName.BorderWidthRight = 1;
				GeneralInfotable.AddCell(cellIPathwayName);
				GeneralInfotable.AddCell(cellInspectionType);
				String inspectionType = "";
				if (inspectionObject.InspectionType != null)
				{
					inspectionType = inspectionObject.InspectionType;
				}
				PdfPCell cellInspectionTypeValue = new PdfPCell(new Phrase(inspectionType, cellContentValueFont));
				cellInspectionTypeValue.FixedHeight = 25;
				cellInspectionTypeValue.Colspan = 2;
				cellInspectionTypeValue.BorderWidthTop = 1;
				cellInspectionTypeValue.BorderColorTop = Color.GRAY;
				cellInspectionTypeValue.HorizontalAlignment = PdfCell.ALIGN_CENTER;
				cellInspectionTypeValue.BorderWidthLeft = 0;
				cellInspectionTypeValue.BorderWidthRight = 0;
				string representativeName = "";
				if (!string.IsNullOrEmpty(inspectionObject.RepresentativeName))
				{
					representativeName = inspectionObject.RepresentativeName;
				}

				PdfPCell cellInspectorNameVAlue = new PdfPCell(new Phrase(representativeName, cellContentValueFont));
				cellInspectorNameVAlue.FixedHeight = 25;
				cellInspectorNameVAlue.Colspan = 2;
				cellInspectorNameVAlue.HorizontalAlignment = PdfCell.ALIGN_CENTER;
				cellInspectorNameVAlue.BorderColorTop = Color.GRAY;
				cellInspectorNameVAlue.BorderColorRight = Color.GRAY;
				cellInspectorNameVAlue.BorderWidthLeft = 0;
				cellInspectorNameVAlue.BorderWidthTop = 1;
				cellInspectorNameVAlue.BorderWidthRight = 1;
				GeneralInfotable.AddCell(cellInspectorNameVAlue);

				//Pathway  Tyype
				string InspectionPathway = "";
				if (!string.IsNullOrEmpty(inspectionObject.Pathway.ToString()))
				{
					InspectionPathway = inspectionObject.Pathway.ToString();
				}
				PdfPCell cellIPathwayNameVAlue = new PdfPCell(new Phrase(InspectionPathway, cellContentValueFont));
				cellIPathwayNameVAlue.Colspan = 2;
				cellIPathwayNameVAlue.FixedHeight = 25;
				cellIPathwayNameVAlue.BorderColorTop = Color.GRAY;
				cellIPathwayNameVAlue.BorderColorRight = Color.GRAY;
				cellIPathwayNameVAlue.BorderWidthLeft = 0;
				cellIPathwayNameVAlue.HorizontalAlignment = PdfCell.ALIGN_CENTER;
				cellIPathwayNameVAlue.BorderWidthTop = 1;
				cellIPathwayNameVAlue.BorderWidthRight = 1;
				GeneralInfotable.AddCell(cellIPathwayNameVAlue);
				GeneralInfotable.AddCell(cellInspectionTypeValue);
				//DAte and Time Values
				// Inspection Information two
				PdfPCell cellInspectionAttempt = new PdfPCell(new Phrase("Inspection Attempt ", cellContentFont));
				// Values Added
				cellInspectionAttempt.FixedHeight = 25;
				cellInspectionAttempt.Colspan = 2;
				cellInspectionAttempt.HorizontalAlignment = PdfCell.ALIGN_CENTER;
				cellInspectionAttempt.BorderWidthTop = 1;
				cellInspectionAttempt.BorderColorTop = Color.GRAY;
				cellInspectionAttempt.BorderWidthLeft = 0;
				cellInspectionAttempt.BorderWidthRight = 0;
				PdfPCell cellInspectorDAte = new PdfPCell(new Phrase("Inspection Date ", cellContentFont));
				cellInspectorDAte.Colspan = 2;
				cellInspectorDAte.FixedHeight = 25;
				cellInspectorDAte.HorizontalAlignment = PdfCell.ALIGN_CENTER;
				cellInspectorDAte.BorderColorTop = Color.GRAY;
				cellInspectorDAte.BorderColorRight = Color.GRAY;
				cellInspectorDAte.BorderWidthLeft = 0;
				cellInspectorDAte.BorderWidthTop = 1;
				cellInspectorDAte.BorderWidthRight = 1;
				GeneralInfotable.AddCell(cellInspectorDAte);

				//Pathway  Tyype
				PdfPCell cellInsTime = new PdfPCell(new Phrase("Inspection Time", cellContentFont));
				cellInsTime.Colspan = 2;
				cellInsTime.FixedHeight = 25;
				cellInsTime.HorizontalAlignment = PdfCell.ALIGN_CENTER;
				cellInsTime.BorderColorTop = Color.GRAY;
				cellInsTime.BorderColorRight = Color.GRAY;
				cellInsTime.BorderWidthLeft = 0;
				cellInsTime.BorderWidthTop = 1;
				cellInsTime.BorderWidthRight = 1;
				GeneralInfotable.AddCell(cellInsTime);
				GeneralInfotable.AddCell(cellInspectionAttempt);

				string InspectionAttemptCount = "";
				if (!string.IsNullOrEmpty(inspectionObject.InspectionAttemptCount))
				{
					InspectionAttemptCount = inspectionObject.InspectionAttemptCount;
				}

				PdfPCell cellInspectionAttValue = new PdfPCell(new Phrase(InspectionAttemptCount, cellContentValueFont));
				cellInspectionAttValue.FixedHeight = 25;
				cellInspectionAttValue.Colspan = 2;
				cellInspectionAttValue.HorizontalAlignment = PdfCell.ALIGN_CENTER;
				cellInspectionAttValue.BorderWidthTop = 1;
				cellInspectionAttValue.BorderColorTop = Color.GRAY;
				cellInspectionAttValue.BorderWidthLeft = 0;
				cellInspectionAttValue.BorderWidthRight = 0;
				string cellDateString = "";


				if (!string.IsNullOrEmpty(inspectionObject.inspectionDateTime.ToString()))
				{
					NSDate InspectionDate = (NSDate)DateTime.SpecifyKind(inspectionObject.inspectionDateTime, DateTimeKind.Utc);
					NSDateFormatter dateformatter = new NSDateFormatter();
					dateformatter.DateFormat = @"MM/dd/yyyy";
					cellDateString = dateformatter.StringFor(InspectionDate);
				}
				//cellInspectorDAte = 
				PdfPCell cellInspectorDateVAlue = new PdfPCell(new Phrase(cellDateString, cellContentValueFont));
				cellInspectorDateVAlue.FixedHeight = 25;
				cellInspectorDateVAlue.Colspan = 2;
				cellInspectorDateVAlue.HorizontalAlignment = PdfCell.ALIGN_CENTER;
				cellInspectorDateVAlue.BorderColorTop = Color.GRAY;
				cellInspectorDateVAlue.BorderColorRight = Color.GRAY;
				cellInspectorDateVAlue.BorderWidthLeft = 0;
				cellInspectorDateVAlue.BorderWidthTop = 1;
				cellInspectorDateVAlue.BorderWidthRight = 1;
				GeneralInfotable.AddCell(cellInspectorDateVAlue);

				string InspectionTime = "";
				if (!string.IsNullOrEmpty(inspectionObject.inspectionDateTime.ToString()))
				{
					InspectionTime = inspectionObject.inspectionDateTime.ToString("hh:mm tt");
				}
				PdfPCell cellITimeVAlue = new PdfPCell(new Phrase(InspectionTime, cellContentValueFont));

				cellITimeVAlue.Colspan = 2;
				cellITimeVAlue.FixedHeight = 25;
				cellITimeVAlue.HorizontalAlignment = PdfCell.ALIGN_CENTER;
				cellITimeVAlue.BorderColorTop = Color.GRAY;
				cellITimeVAlue.BorderColorRight = Color.GRAY;
				cellITimeVAlue.BorderWidthLeft = 0;
				cellITimeVAlue.BorderWidthTop = 1;
				cellITimeVAlue.BorderWidthRight = 1;
				GeneralInfotable.AddCell(cellITimeVAlue);
				GeneralInfotable.AddCell(cellInspectionAttValue);
				//Items to be Observed

				PdfPCell cellItemsObserved = new PdfPCell(new Phrase("Observed Items ", headerFont));
				cellItemsObserved.FixedHeight = 25;
				cellItemsObserved.BorderWidthTop = 1;
				cellItemsObserved.BorderColorTop = Color.GRAY;
				cellItemsObserved.Colspan = 6;
				cellItemsObserved.BackgroundColor = Color.RED;
				cellItemsObserved.HorizontalAlignment = PdfCell.ALIGN_CENTER;
				cellItemsObserved.BorderWidthLeft = 0;
				cellItemsObserved.BorderWidthRight = 0;
				GeneralInfotable.AddCell(cellItemsObserved);

				//ITEMS TO BE ADDED HERE FROM THE INSPECTION TYPE IN APP
				List<NACheckList> naCheckList = new List<NACheckList>();
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
												var checkList = opt.checkListItems.Find(c => c.Result == ResultType.PASS);

												var NACheckList = opt.checkListItems.FindAll(c => c.Result == ResultType.NA);
												if (NACheckList != null && NACheckList.Count > 0)
												{
													naCheckList.Add(new NACheckList()
													{
														SeqName = seqLst.name,
														LevelName = lvl.name,
														SpaceName = spc.name,
														OptionName = opt.name,
														checkList = NACheckList
													});
												}

												if (checkList != null)
												{
													PdfPCell cellSeqlName = new PdfPCell(new Phrase(seqLst.name, cellContentFont));
													cellSeqlName.Colspan = 6;
													cellSeqlName.BorderWidth = 0;
													GeneralInfotable.AddCell(cellSeqlName);

													PdfPCell cellLavelName = new PdfPCell(new Phrase(lvl.name + " - " + spc.name, cellContentFont));
													cellLavelName.PaddingLeft = 15;
													cellLavelName.Colspan = 6;
													cellLavelName.BorderWidth = 0;
													GeneralInfotable.AddCell(cellLavelName);

													PdfPCell cellOptionName = new PdfPCell(new Phrase(opt.name, cellContentFont));
													cellOptionName.PaddingLeft = 25;
													cellOptionName.Colspan = 6;
													cellOptionName.BorderWidth = 0;
													GeneralInfotable.AddCell(cellOptionName);

													foreach (var chk in opt.checkListItems)
													{
														if (chk.Result == ResultType.PASS)
														{
															PdfPCell cellCheckListName = new PdfPCell(new Phrase(chk.description, cellContentValueFont));
															cellCheckListName.PaddingLeft = 35;
															cellCheckListName.Colspan = 6;
															cellCheckListName.BorderWidth = 0;
															GeneralInfotable.AddCell(cellCheckListName);

															PdfPCell cellCheckListComments = new PdfPCell(new Phrase(chk.comments, cellContentCommentValueFont));
															cellCheckListComments.PaddingLeft = 45;
															cellCheckListComments.Colspan = 6;
															cellCheckListComments.BorderWidth = 0;
															GeneralInfotable.AddCell(cellCheckListComments);
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
										var checkList = opt.checkListItems.Find(c => c.Result == ResultType.PASS);

										var NACheckList = opt.checkListItems.FindAll(c => c.Result == ResultType.NA);
										if (NACheckList != null && NACheckList.Count > 0)
										{
											naCheckList.Add(new NACheckList()
											{
												SeqName = seqLst.name,
												LevelName = lvl.name,
												SpaceName = null,
												OptionName = opt.name,
												checkList = NACheckList
											});
										}

										if (checkList != null)
										{
											PdfPCell cellSeqlName = new PdfPCell(new Phrase(seqLst.name, cellContentFont));
											cellSeqlName.Colspan = 6;
											cellSeqlName.BorderWidth = 0;
											GeneralInfotable.AddCell(cellSeqlName);

											PdfPCell cellLavelName = new PdfPCell(new Phrase(lvl.name, cellContentFont));
											cellLavelName.PaddingLeft = 15;
											cellLavelName.Colspan = 6;
											cellLavelName.BorderWidth = 0;
											GeneralInfotable.AddCell(cellLavelName);

											PdfPCell cellOptionName = new PdfPCell(new Phrase(opt.name, cellContentFont));
											cellOptionName.PaddingLeft = 25;
											cellOptionName.Colspan = 6;
											cellOptionName.BorderWidth = 0;
											GeneralInfotable.AddCell(cellOptionName);
											foreach (var chk in opt.checkListItems)
											{
												if (chk.Result == ResultType.PASS)
												{
													PdfPCell cellCheckListName = new PdfPCell(new Phrase(chk.description, cellContentValueFont));
													cellCheckListName.PaddingLeft = 35;
													cellCheckListName.Colspan = 6;
													cellCheckListName.BorderWidth = 0;
													GeneralInfotable.AddCell(cellCheckListName);

													PdfPCell cellCheckListComments = new PdfPCell(new Phrase(chk.comments, cellContentCommentValueFont));
													cellCheckListComments.PaddingLeft = 45;
													cellCheckListComments.Colspan = 6;
													cellCheckListComments.BorderWidth = 0;
													GeneralInfotable.AddCell(cellCheckListComments);

												}
											}
										}
									}
								}
							}
						}
					}
					if (seqLst.Options != null && (inspectionObject.inspectionID == Constants.FINAL_INSPECTIONID || seqLst.id != Constants.FINALPUNCH_SEQUENCEID))
					{
						foreach (var opt in seqLst.Options)
						{
							if (opt.checkListItems != null && opt.checkListItems.Count > 0)
							{
								var checkList = opt.checkListItems.Find(c => c.Result == ResultType.PASS);

								var NACheckList = opt.checkListItems.FindAll(c => c.Result == ResultType.NA);
								if (NACheckList != null && NACheckList.Count > 0)
								{
									naCheckList.Add(new NACheckList()
									{
										SeqName = seqLst.name,
										LevelName = null,
										SpaceName = null,
										OptionName = opt.name,
										checkList = NACheckList
									});
								}

								if (checkList != null)
								{
									PdfPCell cellSeqlName = new PdfPCell(new Phrase(seqLst.name, cellContentFont));
									cellSeqlName.Colspan = 6;
									cellSeqlName.BorderWidth = 0;
									GeneralInfotable.AddCell(cellSeqlName);

									PdfPCell cellOptionName = new PdfPCell(new Phrase(opt.name, cellContentFont));
									cellOptionName.PaddingLeft = 15;
									cellOptionName.Colspan = 6;
									cellOptionName.BorderWidth = 0;
									GeneralInfotable.AddCell(cellOptionName);

									foreach (var chkList in opt.checkListItems)
									{
										if (chkList.Result == ResultType.PASS)
										{
											PdfPCell cellItemValuessadded2 = new PdfPCell(new Phrase(chkList.description, cellContentValueFont));
											cellItemValuessadded2.PaddingLeft = 25;
											cellItemValuessadded2.Colspan = 6;
											cellItemValuessadded2.BorderWidth = 0;
											GeneralInfotable.AddCell(cellItemValuessadded2);

											PdfPCell cellItemValuessadded3 = new PdfPCell(new Phrase(chkList.comments, cellContentCommentValueFont));
											cellItemValuessadded3.PaddingLeft = 30;
											cellItemValuessadded3.Colspan = 6;
											cellItemValuessadded3.BorderWidth = 0;
											GeneralInfotable.AddCell(cellItemValuessadded3);
										}

									}
								}
							}
						}
					}
				}


				if (inspectionObject.inspectionID != Constants.FINAL_INSPECTIONID)
				{
					var PunchList = PunchListDO.getPunchList(AppDelegate.DatabaseContext,
						inspectionObject.inspectionID, inspectionObject.projectID);

					if (PunchList != null && PunchList.Count > 0 && (inspectionObject.inspectionID != Constants.NINTY_PERCENT_INSPECTIONID))
					{
						cellPunchListHeader = new PdfPCell(new Phrase("Non Conformance Items", headerFont));
					}
					if (PunchList != null && PunchList.Count > 0 && (inspectionObject.inspectionID == Constants.NINTY_PERCENT_INSPECTIONID))
					{
						cellPunchListHeader = new PdfPCell(new Phrase("Punch List Items", headerFont));
					}

					if (PunchList != null && PunchList.Count > 0)
					{
						cellPunchListHeader.FollowingIndent = 10;
						cellPunchListHeader.FixedHeight = 25;
						cellPunchListHeader.BorderWidthTop = 1;
						cellPunchListHeader.BorderColorTop = Color.GRAY;
						cellPunchListHeader.Colspan = 6;
						cellPunchListHeader.BackgroundColor = Color.RED;
						cellPunchListHeader.HorizontalAlignment = PdfCell.ALIGN_CENTER;
						cellPunchListHeader.BorderWidthLeft = 0;
						cellPunchListHeader.BorderWidthRight = 0;
						GeneralInfotable.AddCell(cellPunchListHeader);
					}

					if (PunchList != null && PunchList.Count > 0)
					{
						foreach (var punch in PunchList)
						{
							PdfPCell cellPunch = new PdfPCell(new Phrase(punch.punchDescription, cellContentFont));
							cellPunch.PaddingLeft = 10;
							cellPunch.Colspan = 6;
							cellPunch.BorderWidth = 0;
							cellPunch.PaddingBottom = 10;
							GeneralInfotable.AddCell(cellPunch);
						}
					}
				}
				if (naCheckList != null && naCheckList.Count > 0)
				{

					PdfPCell cellNACheckListHeader = new PdfPCell(new Phrase("Not Applicable Items", headerFont));
					cellNACheckListHeader.FollowingIndent = 10;
					cellNACheckListHeader.FixedHeight = 25;
					cellNACheckListHeader.BorderWidthTop = 1;
					cellNACheckListHeader.BorderColorTop = Color.GRAY;
					cellNACheckListHeader.Colspan = 6;
					cellNACheckListHeader.BackgroundColor = Color.RED;
					cellNACheckListHeader.HorizontalAlignment = PdfCell.ALIGN_CENTER;
					cellNACheckListHeader.BorderWidthLeft = 0;
					cellNACheckListHeader.BorderWidthRight = 0;
					GeneralInfotable.AddCell(cellNACheckListHeader);

					foreach (var chkList in naCheckList)
					{

						PdfPCell cellSeqlName = new PdfPCell(new Phrase(chkList.SeqName, cellContentFont));
						cellSeqlName.Colspan = 6;
						cellSeqlName.BorderWidth = 0;
						GeneralInfotable.AddCell(cellSeqlName);

						if (string.IsNullOrEmpty(chkList.SpaceName) && !string.IsNullOrEmpty(chkList.LevelName))
						{
							PdfPCell cellLavelName = new PdfPCell(new Phrase(chkList.LevelName, cellContentFont));
							cellLavelName.PaddingLeft = 15;
							cellLavelName.Colspan = 6;
							cellLavelName.BorderWidth = 0;
							GeneralInfotable.AddCell(cellLavelName);
						}
						else if (!string.IsNullOrEmpty(chkList.SpaceName) && !string.IsNullOrEmpty(chkList.LevelName))
						{
							PdfPCell cellLavelName = new PdfPCell(new Phrase(chkList.LevelName + " - " + chkList.SpaceName, cellContentFont));
							cellLavelName.PaddingLeft = 15;
							cellLavelName.Colspan = 6;
							cellLavelName.BorderWidth = 0;
							GeneralInfotable.AddCell(cellLavelName);
						}

						PdfPCell cellOptionName = new PdfPCell(new Phrase(chkList.OptionName, cellContentFont));
						cellOptionName.PaddingLeft = 25;
						cellOptionName.Colspan = 6;
						cellOptionName.BorderWidth = 0;
						GeneralInfotable.AddCell(cellOptionName);

						foreach (var chk in chkList.checkList)
						{
							if (chk.Result == ResultType.NA)
							{
								PdfPCell cellCheckListName = new PdfPCell(new Phrase(chk.description, cellContentValueFont));
								cellCheckListName.PaddingLeft = 35;
								cellCheckListName.Colspan = 6;
								cellCheckListName.BorderWidth = 0;
								GeneralInfotable.AddCell(cellCheckListName);

								PdfPCell cellCheckListComments = new PdfPCell(new Phrase(chk.comments, cellContentCommentValueFont));
								cellCheckListComments.PaddingLeft = 45;
								cellCheckListComments.Colspan = 6;
								cellCheckListComments.BorderWidth = 0;
								GeneralInfotable.AddCell(cellCheckListComments);
							}
						}
					}

				}
				PdfPCell cellBlank = new PdfPCell(new Phrase("", cellContentFont));
				cellBlank.FixedHeight = 30;
				cellBlank.BorderColor = Color.GRAY;
				cellBlank.BorderWidthLeft = 0;
				cellBlank.BorderWidthRight = 0;
				cellBlank.BorderWidthTop = 0;
				cellBlank.BorderWidthBottom = 0;
				cellBlank.Colspan = 6;
				GeneralInfotable.AddCell(cellBlank);

				MainTable.AddCell(GeneralInfotable);
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Exception occured in Pass Report" + ex.Message);
			}
			return MainTable;
		}
	}
	class NACheckList
	{
		public string SeqName { get; set; }
		public string LevelName { get; set; }
		public string SpaceName { get; set; }
		public string OptionName { get; set; }
		public List<CheckList> checkList { get; set; }
	}
}