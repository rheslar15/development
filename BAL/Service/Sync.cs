using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BAL.Service;
using DAL.DO;
using DAL.Repository;
using LiroInspectServiceModel.Services;
using Model;
using ServiceLayer.Service;
using SQLite;
using System.Diagnostics;
using DAL;
using UIKit;
using Foundation;
using System.CodeDom.Compiler;
using CoreGraphics;
using System.IO;
using System.Drawing;


namespace BAL
{
	public class Sync
	{
		SQLiteConnection conn;
		public event notificationHandler notifiy;
		public static event EventHandler DocumentDeleted;
		public delegate void notificationHandler(notificationEventArgs e);
		public event syncProgressHandler syncProgress;
		public delegate void syncProgressHandler(bool e);
		public Sync(SQLiteConnection conn)
		{
			this.conn = conn;
		}

		/// <summary>
		/// Gets the pending sync count.
		/// </summary>
		/// <returns>The pending sync count.</returns>
		public int getPendingSyncCount()
		{
			InspectionTransactionService transSer = new InspectionTransactionService(conn);
			int count = 0;
			var finishedInspectionsQry = transSer.GetInspectionTransactions().Where(i => i.IsFinalise > 0).ToList();
			if (finishedInspectionsQry != null && finishedInspectionsQry.Count() > 0)
			{
				foreach (var inspection in finishedInspectionsQry)
				{
					count++;
				}
			}
			return count;
		}

		static bool IsSyncProgress = false;
		static object syncLock = new object();
		/// <summary>
		/// Syncs the data.
		/// </summary>
		public void syncData()
		{
			lock (syncLock)
			{
				try
				{
					if (!IsSyncProgress)
					{
						IsSyncProgress = true;

						if (syncProgress != null)
						{
							syncProgress(true);
						}
						//Check if there are finished inspections
						InspectionTransactionService transSer = new InspectionTransactionService(conn);
						OptionTransactionService OTS = new OptionTransactionService(conn);
						//int count = 0;
						var finishedInspectionsQry = transSer.GetInspectionTransactions().Where(i => i.IsFinalise > 0);// && (i.pass.ToLower () == "pass" || i.pass.ToLower () == "fail"));
						UserService userService = new UserService(conn);
						User user = userService.GetUser();
						IServices service = new Services();
						InspectionDetailService ids = new InspectionDetailService(conn);
						OptionImageService optImageservice = new OptionImageService(conn);
						PunchService pushservice = new PunchService(conn);
						DocViewService docService = new DocViewService(conn);
						NotificationRepository notificationRep = new NotificationRepository(conn);

						if (finishedInspectionsQry.Any())
						{
							List<Inspection> finishedInspections = finishedInspectionsQry.Where(i => i.IsFinalise > 0).ToList();
							if (finishedInspections.Count > 0)
							{
								int reportCount = 0;
								int punchListCount = 0;

								Debug.WriteLine("sync thread started");
					
								foreach (var inspection in finishedInspections)
								{
									if (inspection == null)
									{
										Debug.WriteLine(string.Format("inspection is null"));
									}

									if (!inspection.isInspectionSynced)
									{
										if (notifiy != null)
										{
											notifiy(new notificationEventArgs
											{
												current = new Notifications
												{
													notificationTypeID = inspection.projectID,
													notificationType = "Inspection",
												},
												isSyncCompleted = false,
											});
										}

										Model.ServiceModel.InspectionResults req = new Model.ServiceModel.InspectionResults();
										req.token = user.Token;
										req.inspections = new List<Model.ServiceModel.InspectionResult>();
										Model.Inspection inspectionSer = ids.GetInspectionDetail(inspection, true);


										if (inspectionSer != null)
										{
											var insp = inspectionSer.getServiceModel();



											req.inspections.Add(insp);

											Model.ServiceModel.InspectionResultsAck inspectinPushRes = service.PushInspections(req);


											if (inspectinPushRes.result != null && inspectinPushRes.result.code == 0)
											{
												

												int inspectionTransactionID = 0;
												InspectionTransactionService insTransService = new InspectionTransactionService(conn);
												inspectionTransactionID = insTransService.GetInspectionTransactionID(inspection.projectID, inspection.inspectionID);

												//Loaction Image Delete after sync
												DAL.LocationImageDo.DeleteImage(conn, inspectionTransactionID);

												//Inspection Document Delete after sync
												var documents = docService.GetDocumentsForSync(inspection.inspectionID, inspection.projectID);
												if (documents != null && documents.Count > 0)
												{
													if (DocumentDeleted != null)
													{
														DocumentDeleted(this, new DocumentDeletionEventArgs() { documentList = documents });
													}
													docService.DeleteDocumentItemsForSync(inspection.inspectionID, inspection.projectID);
												}

												/// Punch List Delete for only final Inspection
												if (inspection.inspectionID == BALConstant.FINAL_INSPECTIONID)
												{
													List<Punch> punchList = pushservice.getPunchList(inspection.inspectionID, inspection.projectID);
													foreach (var punch in punchList)
													{
														PunchListImageDO.DeletePunchImageList(conn, punch.PunchID);
														OptionPunchDO.DeleteOptionPunchID(conn, punch.PunchID);
													}
													PunchListDO.DeletePunchList(conn, BALConstant.FINAL_INSPECTIONID, inspection.projectID);
												}

												using (LevelTransactionService levelTransactionService = new LevelTransactionService(conn))
												{
													var levelTransactions = levelTransactionService.GetLevelTransactions();

													if (levelTransactions != null)
													{
														foreach (var levelTrans in levelTransactions)
														{
															levelTransactionService.DeleteLevelTransactions(levelTrans);
														}
													}
												}


												using (SpaceTransactionService spaceTransactionService = new SpaceTransactionService(conn))
												{
													var spaceTransactions = spaceTransactionService.GetSpaceTransactions();

													if (spaceTransactions != null)
													{
														foreach (var spaceTrans in spaceTransactions
														)
														{
															spaceTransactionService.DeleteSpaceTransactions(spaceTrans);
														}
													}
												}


												//Get Option transactions for inspection(Not including punch list)
												List<OptionTransaction> optiontransactionLst = null;
												optiontransactionLst = OTS.GetOptionTransactionsForInspection(inspectionTransactionID);

												//Delete Item Transaction
												if (optiontransactionLst != null && optiontransactionLst.Count > 0)
												{
													foreach (var optiontransaction in optiontransactionLst)
													{

														//Option Image Deletion
														optImageservice.DeleteOptionImagesForSync(conn, optiontransaction.ID);

														//Guided picture deletion
														if (optiontransaction.OptionId == BALConstant.GUIDEDPICTURE_OPTIONID)
														{
															var chkTransIDs = CheckListTransactionDO.GetCheckListTransaction(conn, optiontransaction.ID).Select(s => s.ID);
															if (chkTransIDs != null && chkTransIDs.Count() > 0)
															{
																foreach (var chkId in chkTransIDs)
																{
																	GuildedPhotoDO.DeleteGuidedImageList(conn, chkId);
																}
															}
														}
														// Checklist transaction Deletion
														CheckListTransactionDO.DeletecheckList(conn, optiontransaction.ID);

														//Option Transaction Row deletion
														OTS.DeleteOptionTransactions(optiontransaction);


													}
												}

												inspection.isInspectionSynced = true;
												inspection.InspectionStarted = 0;

												transSer.UpdateInspectionTransaction(inspection);
												//update notification table with successfully uploaded insÏpection
												notificationRep.Save("Inspection", inspection.projectID, "Inspection Results for App ID  : " + inspection.projectID + "  successfully synced");
											}
											else
											{
												
												//update notification table with retry count for inspection
												notificationRep.Save("Inspection", inspection.projectID, "Inspection Results for App ID : " + inspection.projectID + " not synced");
											}
										}
									}

									if (inspection.isInspectionSynced)
									{ ///If inspection sync is success
										ReportService repservice = new ReportService(conn);
										IEnumerable<Report> reports = repservice.GetReports().Where(r => r.InspectionTransID == inspection.ID);
										if (reports != null)
										{

											foreach (var report in reports)
											{
												
												if (notifiy != null)
												{
													notifiy(new notificationEventArgs
													{
														current = new Notifications
														{
															notificationTypeID = report.ReportID + "-" + inspection.projectID,
															notificationType = "Report",
														},
														isSyncCompleted = false,
													});
												}

												Model.ServiceModel.InspectionReportAck inspectionReoprtRes = service.PushReport(new Model.ServiceModel.InspectionReport()
												{
													inspectionTypeID = inspection.inspectionID,
													appID = inspection.projectID,
													report = report.ReportDesc,
													reportName = report.ReportType,
													token = user.Token,
												});
												if (inspectionReoprtRes.result != null && inspectionReoprtRes.result.code == 0)
												{
													notificationRep.Save(report.ReportType.ToUpper(), inspection.projectID, report.ReportType.ToUpper() + " Report for App ID : " + inspection.projectID + " successfully synced");
													repservice.DeleteReport(report);
												}
												else
												{
													notificationRep.Save(report.ReportType.ToUpper(), inspection.projectID, report.ReportType.ToUpper() + " Report for App ID  : " + inspection.projectID + " not synced");
												}
											}
										}


										List<Model.ServiceModel.PunchListItem> PunchItemList = new List<Model.ServiceModel.PunchListItem>();
										if (inspection.inspectionID != BALConstant.FINAL_INSPECTIONID)
										{
											if (notifiy != null)
											{
												notifiy(new notificationEventArgs
												{
													current = new Notifications
													{
														notificationTypeID = inspection.projectID,
														notificationType = "Punchlist",
													},
													isSyncCompleted = false,
												});
											}

											List<Punch> punchList = pushservice.getPunchList(inspection.inspectionID, inspection.projectID);
											if (punchList != null && punchList.Count > 0)
											{
												foreach (var punch in punchList)
												{
													List<PunchListImageDO> ImageTransLst = PunchListImageDO.getPunchImageList(conn, punch.PunchID);
													List<byte[]> images = new List<byte[]>();

													if (ImageTransLst != null)
													{
														foreach (var ImageTrans in ImageTransLst)
														{
															images.Add(ImageTrans.PunchListImage);
														}
													}

													PunchItemList.Add(new Model.ServiceModel.PunchListItem()
													{
														photos = images,
														comment = punch.punchDescription,
														sequence = -1
													});
												}

												Model.ServiceModel.PunchListAck inspectionPunchListRes = service.PushPunchList(new Model.ServiceModel.PunchListResult()
												{
													inspectionTypeID = inspection.inspectionID,
													appID = inspection.projectID,
													punchList = PunchItemList,
													token = user.Token
												});
												if (inspectionPunchListRes.result != null && inspectionPunchListRes.result.code == 0)
												{
													//update notification table with successfully uploaded punch list
													notificationRep.Save("Punchlist", inspection.projectID, "Punch List for App ID : " + inspection.projectID + " successfully synced");
													foreach (Punch item in punchList)
													{
														pushservice.DeletePunchItem(item);
														PunchListImageDO.DeletePunchImageList(conn, item.PunchID);
													}
													OptionTransactionDO.DeleteInspectionOptions(conn, inspection.ID);
												}
												else
												{
													//update notification table with retry count for punch list
													notificationRep.Save("Punchlist", inspection.projectID, "Punch List for App ID : " + inspection.projectID + " not synced");
												}
											}
										}
									}

									if (inspection.isInspectionSynced)
									{
										using (ReportService repservice = new ReportService(conn))
										{
											IEnumerable<Report> reports = repservice.GetReports().Where(r => r.InspectionTransID == inspection.ID);
											if (reports != null && reports.Count() > 0)
											{
												reportCount = reports.Count();
											}
										}
										if (inspection.inspectionID != BALConstant.FINAL_INSPECTIONID)
										{
											List<Punch> punchList = pushservice.getPunchList(inspection.inspectionID, inspection.projectID);
											if (punchList != null && punchList.Count > 0)
											{
												punchListCount = punchList.Count;
											}
										}
										if (reportCount == 0 && punchListCount == 0)
										{
											transSer.DeleteInspectionTransaction(inspection);
											continue;
										}
									}
								}
							}
						}
					}
				}
				catch (Exception ex)
				{
					Debug.WriteLine("Exception Occured in Syncdata method due to " + ex.Message);
				}
				finally
				{
					if (notifiy != null)
					{
						notifiy(new notificationEventArgs
						{
							current = new Notifications(),
							isSyncCompleted = true,
						});
					}
					if (syncProgress != null)
					{
						syncProgress(false);
					}
					IsSyncProgress = false;
					UIApplication.SharedApplication.InvokeOnMainThread(delegate
					{

						UIApplication.SharedApplication.IdleTimerDisabled = false;
						Debug.WriteLine("sync thread finished");
					});
				}
			}
		}
	}
}