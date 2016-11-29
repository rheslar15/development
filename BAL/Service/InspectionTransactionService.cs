using System;
using SQLite;
using DAL.Repository;
using DAL.DO;
using Model;
using System.Collections.Generic;
using DAL.Utility;
using ServiceLayer;
using System.Linq;
using ServiceLayer.Service;
using BAL.Service;
using LiroInspectServiceModel.Services;
using System.Text.RegularExpressions;
using DAL;
using System.Diagnostics;


namespace BAL
{
	public class InspectionTransactionService:BaseService
	{
		IRepository<InspectionTransactionDO> inspectionTransactionRepository;
		InspectionService  inspectionService;
		SQLiteConnection conn;
		public Model.ServiceModel.InspectionsRes ServiceResonse{ get; set;}
		public Model.ServiceModel.PunchListRes PunchLstResponse{ get; set;}
		public InspectionTransactionService(SQLiteConnection conn)
		{
			this.conn = conn;
			inspectionTransactionRepository = RepositoryFactory<InspectionTransactionDO>.GetRepository(conn);
			inspectionService = new InspectionService (conn);
		}

		public List<Inspection> GetInspectionTransactions()
		{
			List<Inspection> inspectionsTransaction= new List<Inspection>() ;
			try
			{
			    IEnumerable<InspectionTransactionDO> inspectionsTransactionDOs = inspectionTransactionRepository.GetEntities();
			    foreach (InspectionTransactionDO insptransDo in inspectionsTransactionDOs)
			    {
				    inspectionsTransaction.Add(Converter.GetInspectionTransaction(insptransDo));
			    }
			}
            catch (Exception ex)
            {
                Debug.WriteLine("Exception Occured in GetInspectionTransactions method due to " + ex.Message);
			}
			return inspectionsTransaction;
		}

		public Inspection GetInspectionTransaction(int id)
		{
			Inspection inspectionTransaction = new Inspection();
			try
            {
			    InspectionTransactionDO inspectionTransactionDO = inspectionTransactionRepository.GetEntity(id);
			    if (inspectionTransactionDO != null)
				    inspectionTransaction = Converter.GetInspectionTransaction(inspectionTransactionDO);
			}
            catch (Exception ex)
            {
                Debug.WriteLine("Exception Occured in GetInspectionTransaction method due to " + ex.Message);
			}
			return inspectionTransaction;
		}

		public int GetInspectionTransactionID(string projectID,string inspectionID)
		{
			int inspectionTransID = 0;
			try
			{
				inspectionTransID=InspectionTransactionDO.getInspectionTransID(conn,projectID,inspectionID);
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Exception Occured in GetInspectionTransactionID method due to " + ex.Message);
			}
			return inspectionTransID;
		}

		public InspectionTransactionDO GetInspectionProjectID(int inspectionTransID)
		{
			InspectionTransactionDO inspectionTrans = new InspectionTransactionDO ();
			try
			{
				inspectionTrans=InspectionTransactionDO.getInspectionProjectID(conn,inspectionTransID).FirstOrDefault();
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Exception Occured in GetInspectionProjectID method due to " + ex.Message);
			}
			return inspectionTrans;
		}

		public int SaveInspectionTransaction(Inspection inspection)
		{
			int result = 0;
			try
            {
			    InspectionTransactionDO inspectionTransactionDO = Converter.GetInspectionTransactionDO(inspection);
			    result = inspectionTransactionRepository.SaveEntity(inspectionTransactionDO);
			}
            catch (Exception ex)
            {
                Debug.WriteLine("Exception Occured in SaveInspectionTransaction method due to " + ex.Message);
			}
			return result;
		}

		public int UpdateInspectionTransaction(Inspection inspection)
		{
			int result = 0;
			try
            {
			    var inspectionID = GetInspectionTransactions().Where(i => i.inspectionID == inspection.inspectionID && i.projectID == inspection.projectID).FirstOrDefault().ID; 
				inspection.ID = inspectionID;
			    InspectionTransactionDO inspectionTransactionDO = Converter.GetInspectionTransactionDO(inspection);
			    result = inspectionTransactionRepository.UpdateEntity(inspectionTransactionDO);			
			}
            catch (Exception ex)
            {
                Debug.WriteLine("Exception Occured in UpdateInspectionTransaction method due to " + ex.Message);
			}
			return result;
		}

		/// <summary>
		/// Deletes the inspection transaction.
		/// </summary>
		/// <returns>The inspection transaction.</returns>
		/// <param name="inspection">Inspection.</param>
		public int DeleteInspectionTransaction(Inspection inspection)
		{
			int result = 0;
			try
			{
				IEnumerable<InspectionTransactionDO> inspections = inspectionTransactionRepository.GetEntities ();
				InspectionTransactionDO inspectionTransactionDO = inspections.FirstOrDefault (ins => ins.InspectionID == inspection.inspectionID && ins.ProjectID == inspection.projectID);
				if(inspectionTransactionDO!=null)
				{
				result = inspectionTransactionRepository.DeleteEntity(inspectionTransactionDO.ID);
				}
			}
			catch(Exception ex)
			{
				Debug.WriteLine("Exception Occured in DeleteInspectionTransaction method due to " + ex.Message);
			}
			return result;
		}

		public int DeleteInspectionTransactionNotSync(Inspection inspection)
		{
			int inspectionTransactionResult = 0;
			try
			{
				IEnumerable<InspectionTransactionDO> inspections = inspectionTransactionRepository.GetEntities ();

				InspectionTransactionDO inspectionTransactionDO = inspections.FirstOrDefault(ins => ins.InspectionID == inspection.inspectionID && ins.ProjectID == inspection.projectID);
				inspectionTransactionResult = inspectionTransactionRepository.DeleteEntity(inspectionTransactionDO.ID);
				LocationImageDo.DeleteImage(conn,inspectionTransactionDO.ID);
				DocumentDO.DeleteDocument(conn,inspection.inspectionID,inspection.projectID);
				PunchListDO.DeletePunchList(conn,inspection.inspectionID,inspection.projectID);
				PunchListImageDO.DeletePunchListImageList(conn,inspection.ID);
				ReportDO.DeleteReports(conn,inspection.ID);
				using(var optService=new OptionTransactionService(conn))
				{
					var optIds=optService.GetOptionTransactionsForInspection(inspection.ID).Select(s=>s.ID);
					OptionTransactionDO.DeleteInspectionOptions(conn,inspection.ID);
					if(optIds !=null && optIds.Count()>0)
					{
						foreach(var optID in optIds)
						{
							var chkTransIDs=CheckListTransactionDO.GetCheckListTransaction(conn,optID).Select(s=>s.ID);	
							CheckListTransactionDO.DeletecheckList(conn,optID);
							OptionTransactionImageDO.DeleteOptionImagesSync(conn,optID);
							if(chkTransIDs!=null && chkTransIDs.Count()>0)
							{
								foreach(var chkId in chkTransIDs)
								{
									GuildedPhotoDO.DeleteGuidedImageList(conn,chkId);

								}
							}
						}
					}
				}
			}
			catch(Exception ex)
			{
				Debug.WriteLine("Exception Occured in DeleteInspectionTransaction method due to " + ex.Message);
			}
			return inspectionTransactionResult;
		}

		/// <summary>
		/// Gets the inspections.
		/// Gets the inspections from.
		/// </summary>
		/// <returns>The service inspections.</returns>
		/// <param name="IsNetorkConnected">If set to <c>true</c> is netork connected.</param>
		/// <param name="Token">Token.</param>
		/// <param name="IsFirstLogin">If set to <c>true</c> is first login.</param>
		public List<Model.Inspection> GetServiceInspections(bool IsNetorkConnected,string Token,bool IsFirstLogin)
		{
			List<Inspection> inspections = new List<Inspection> ();

			try
            {
				
				if (IsFirstLogin && IsNetorkConnected)
				{
					InspectionService(Token);

				}

				inspections = GetInspectionTransactions ().Where(i=>i.IsFinalise<=0).ToList();
				foreach (var ins in inspections) 
				{
					var inspection = inspectionService.GetInspections ().Where (i => i.inspectionID == ins.inspectionID).FirstOrDefault ();

					ins.InspectionType = inspection != null ? inspection.InspectionType : "";

					if (inspection != null)
					{
						ins.InspectionStarted = inspections.Where(x => x.inspectionID == inspection.inspectionID).FirstOrDefault().InspectionStarted;


					}
				}
			}
            catch (Exception ex)
            {
                Debug.WriteLine("Exception Occured in GetServiceInspections method due to " + ex.Message);
			}
			return inspections;
		}

		/// <summary>
		/// Get Inspections from service and Save to Database.
		/// </summary>
		/// <returns>The service.</returns>
		/// <param name="Token">Token.</param>
		public List<Inspection> InspectionService(string Token)
		{
			List<Inspection> inspections = new List<Inspection> ();
			try
            {
			    // Get New assigned Inspections to the User
			    ServiceResonse = new Model.ServiceModel.InspectionsRes ();
				IServices service = new Services ();
			    ServiceResonse = service.FetchInspections (new Model.ServiceModel.InspectionsReq (){ token = Token });

                List<string> projectIDs = new List<string>();

                if (ServiceResonse != null && ServiceResonse.inspections!=null && (ServiceResonse.result.code==0 || ServiceResonse.result.code==1))
                {
					var inspectionLst=ServiceResonse.inspections.Where(id => id.inspectionTypeID == BALConstant.FINAL_INSPECTIONID);
                    if (inspectionLst != null && inspectionLst.Count() > 0)
					{	
						List<string> Ids=new List<string>();
						Ids=PunchListDO.getPunchProjectIds(conn,BALConstant.FINAL_INSPECTIONID);

						foreach(var ins in inspectionLst)
						{
							if(Ids != null && Ids.Count>0)
							{
								foreach(var id in Ids)
								{
									if (ins.appID != id)
									{
										if (!projectIDs.Contains(ins.appID))
										{
											projectIDs.Add(ins.appID);
										}
									}
								}
							}
							else
							{
								projectIDs.Add(ins.appID);
							}
						}
                        
						if( projectIDs!=null && projectIDs.Count>0)
						{
							PunchLstResponse = service.FetchPuchList(new Model.ServiceModel.PunchListReq() { token = Token, appIDs = projectIDs });
						}
						else
						{
							PunchLstResponse = null;
						}
                    }
                    else
                    {
                        PunchLstResponse = null;
                    }
                }

				var ExistingInspection=GetInspectionTransactions();
				foreach(var insTrans in ExistingInspection)
				{
					if (ServiceResonse.result!=null && (ServiceResonse.result.code == 0 || ServiceResonse.result.code ==1)) {
						var existingInspection=ServiceResonse.inspections.Find(si=>si.inspectionTypeID==insTrans.inspectionID && si.appID==insTrans.projectID && si.pathway==insTrans.Pathway);
						if(existingInspection==null)
						{
							if(insTrans.IsFinalise<=0)
							{
								DeleteInspectionTransactionNotSync(insTrans);
							}
						}
					}
				}
				int count = 0;

				foreach (var servInspect in ServiceResonse.inspections) 
				{
					if (PunchLstResponse != null && PunchLstResponse.result != null && (PunchLstResponse.result.code == 0 || PunchLstResponse.result.code == 1))
					{
						if (PunchLstResponse.projectPunchLists != null && PunchLstResponse.projectPunchLists.Count > 0)
						{
							foreach (var punchItems in PunchLstResponse.projectPunchLists)
							{
								string projectId = punchItems.appID;
								if (servInspect.inspectionTypeID == BALConstant.FINAL_INSPECTIONID && projectId == servInspect.appID)
								{
									if (punchItems.punchListItems != null && punchItems.punchListItems.Count > 0)
									{
										PunchListDO.DeletePunchList(conn, BALConstant.FINAL_INSPECTIONID, projectId);
										foreach (var punchcomments in punchItems.punchListItems)
										{
											List<Punch> punchList = new List<Punch>();

											foreach (var comment in punchcomments.comments)
											{
												punchList.Add(new Punch()
													{
														punchDescription = comment,
														ProjectID = projectId,
														InspectionID = BALConstant.FINAL_INSPECTIONID
													});
											}
											PunchListDO.InsertPunchLists(conn, punchList);
											var PunchListID = PunchListDO.getPunchList(conn, BALConstant.FINAL_INSPECTIONID, projectId);
											foreach (var punchItem in PunchListID)
											{
												if (punchItem.PunchID != -1)
												{
													OptionPunchDO.DeleteOptionPunchID (conn, punchItem.PunchID);
													OptionPunchDO.InsertOptionPunch(conn, BALConstant.PUNCH_OPTIONID, punchItem.PunchID);
												}
											}
										}
									}
								}
							}
						}
					}
					var existingIns=GetInspectionTransactions().Find(i=>i.inspectionID==servInspect.inspectionTypeID && i.projectID==servInspect.appID && i.Pathway==servInspect.pathway);
					///Punch Lists review in final Inspections of 25,50,75 and 90% created Punch Lists
					if(existingIns==null)
					{
						var inspection = inspectionService.GetInspections ().Where (i => i.inspectionID == servInspect.inspectionTypeID).FirstOrDefault ();
						if (!string.IsNullOrEmpty (servInspect.phoneNo)) 
						{
							servInspect.phoneNo=Regex.Replace (servInspect.phoneNo, @"(\d{3})(\d{3})(\d{4})", "$1-$2-$3");
						}
						string homeOwnerName = "";
						if (!string.IsNullOrEmpty(servInspect.houseOwnerName))
						{
							var Name = servInspect.houseOwnerName.Split(',');
							for (int i = Name.Length; i > 0; i--)
							{
								homeOwnerName += Name[i - 1];
								homeOwnerName += " ";
							}
						}
						List<Document> docList=new List<Document>();
						if (servInspect.inspectionDocuments != null && servInspect.inspectionDocuments.Count > 0)
						{

							List<Model.ServiceModel.InspectionDocument> insDocumnents = new List<Model.ServiceModel.InspectionDocument>();
							insDocumnents = servInspect.inspectionDocuments;

							foreach(var insDoc in insDocumnents)
							{
								Model.Document doc=new Document();
								doc.documentDisplayName=insDoc.documentDisplayName;
								doc.inspectionDocumentType=insDoc.inspectionDocumentType;
								doc.documentID=insDoc.documentID;
								doc.inspectionID=servInspect.inspectionTypeID;
								doc.projectID=servInspect.appID;

								using(DocViewService docServ=new DocViewService(conn))
								{
									var existingDoc=docServ.GetDocumentItems(servInspect.inspectionTypeID,servInspect.appID).Where(d=>d.documentID==doc.documentID);
									if(existingDoc!=null && existingDoc.Count()>0)
									{
										foreach(var docs in existingDoc)
										{
											docServ.DeleteDocumentItems(docs);
										}	
									}
									docServ.SaveDocumentItems(doc);
								}
								docList.Add(doc);
							}
						}

						inspections.Add(new Inspection()
						{
							inspectionID = servInspect.inspectionTypeID,
							inspectionDateTime = servInspect.inspectionDate,
							ProjectName = servInspect.projectName,
							projectID = servInspect.appID,
							Pathway = servInspect.pathway,
							InspectionAddress1 = servInspect.activityAdress.addrline1,
							InspectionAddress2 = servInspect.activityAdress.addrline2,
							City = servInspect.activityAdress.city,
							HouseOwnerName = homeOwnerName.Trim(),
							PhoneNo = servInspect.phoneNo,
							Pincode = servInspect.activityAdress.postalCode,
							InspectionType = inspection != null ? inspection.InspectionType : "",
							IsFinalise = 0,
							InspectionAttemptCount = (servInspect.info != null) ? servInspect.info.inspectionAttempt.ToString() : "",
							ContractorName = (servInspect.info != null) ? servInspect.info.contractorName : "",
							HouseOwnerID = (servInspect.info != null) ? servInspect.info.homeOwnerID.ToString() : "",
							inspectionDocuments = docList

						});

							SaveInspectionTransaction (inspections.LastOrDefault());						
							count++;
					}
				}			   
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception Occured in InspectionService method due to " + ex.Message);
			}
			return inspections;
		}

		/// <summary>
		/// Deletes all unfinished transaction.
		/// </summary>
		private void DeleteAllUnfinishedTransaction(){
			try
            {
			    var inspectDtls = GetInspectionTransactions ();
			    if (inspectDtls != null && inspectDtls.Count() > 0) 
				{
				    foreach (var ins in inspectDtls) 
					{
					    if (conn != null && (ins.pass == null || ins.pass.ToLower () != "pass" && ins.pass.ToLower () != "fail")) 
						{
						    DeleteInspectionTransaction (ins);
					    }
				    }
			    }
		    }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception Occured in DeleteAllUnfinishedTransaction method due to " + ex.Message);
			}
		}
	}
}