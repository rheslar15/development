using System;
using DAL.Repository;
using DAL.DO;
using SQLite;
using Model;
using BAL.Service;
using System.Collections.Generic;
using System.Linq;
using DAL;
using DAL.Utility;
using System.Diagnostics;
using System.Collections;


namespace BAL
{
	public class InspectionDetailService:BaseService
	{
		InspectionService inspectionService;        
		InspectionTransactionService inspectionTransactionService;
		OptionImageService optionimgService;
		int inspectionTransactionID;
		SQLiteConnection conn;
		bool IsImageNeeded=false;
		public InspectionDetailService (SQLiteConnection conn)
		{
			inspectionService = new InspectionService (conn);
			inspectionTransactionService = new InspectionTransactionService (conn);
			optionimgService = new OptionImageService (conn);
			this.conn = conn;
		}

		/// <summary>
		/// Gets the inspection detail.
		/// </summary>
		/// <returns>The inspection detail.</returns>
		/// <param name="inspection">Inspection.</param>
		/// <param name="IsImageNeeded">If set to <c>true</c> is image needed.</param>
		public Inspection GetInspectionDetail (Inspection inspection,bool IsImageNeeded)
		{            
			using (Inspection ins =    inspection) 
			{
				try 
				{
					this.IsImageNeeded=IsImageNeeded;
					var inspectionDesc =    inspectionService.GetInspections ().Where (i => i.inspectionID == inspection.inspectionID).FirstOrDefault ();
					if(inspectionDesc!=null)
					{    
						var inspectionTraansaction = inspectionTransactionService.GetInspectionTransactions().Where(i => i.inspectionID == inspection.inspectionID && i.projectID == inspection.projectID && i.appID == inspection.appID);	
						if(inspectionTraansaction!=null && inspectionTraansaction.Count()>0)
						{
							ins.ID=inspectionTraansaction.FirstOrDefault().ID;

							//ins.InspectionStarted = inspectionTraansaction.FirstOrDefault().InspectionStarted;


						}
						inspectionTransactionID=ins.ID;
						GetInspectionDetails (int.Parse(inspection.inspectionID),ins);
						inspection.InspectionType = ins.InspectionType;

						var punchsequenceID=ins.sequences.Where(i=>i.id==BALConstant.PUNCH_SEQUENCEID);

						if(punchsequenceID == null || punchsequenceID.Count() <= 0)
						{
							if(inspection.inspectionID==BALConstant.FINAL_INSPECTIONID)
							{
								Sequence punchsequence=new Sequence();
								SequencesDO seq=SequencesDO.getPunchListSequenceForInspection(conn,BALConstant.PUNCH_SEQUENCEID).FirstOrDefault();
								if(seq != null)
								{
									punchsequence.id=seq.ID;
									punchsequence.name=seq.SequenceDesc;
									punchsequence.Options = getPunchListForSeq(BALConstant.FINAL_INSPECTIONID, inspection.projectID, ins.ID);
								}

								if (punchsequence != null && punchsequence.Options != null && punchsequence.Options.Count > 0)
								{
									if (punchsequence.Options[0].checkListItems != null && punchsequence.Options[0].checkListItems.Count > 0)
									{
										ins.sequences.Insert(0, punchsequence);
									}
								}
							}
						}
					}
					if (ins != null) 
					{
						ins.locationIDImages = new List<byte[]> ();
						var images = LocationImageDo.getImageForLocationIdentification (conn, ins.ID);
						if (images != null && images.Count > 0) 
						{
							foreach (var img in images) 
							{
								ins.locationIDImages.Add (img.Image);
							}
						}
					}
				} 
				catch (Exception ex) 
				{
					Debug.WriteLine ("Exception Occured in GetInspectionDetail method due to " + ex.Message);
				}
				return ins;
			}
		}

		/// <summary>
		/// Gets the punch list for seq.
		/// </summary>
		/// <returns>The punch list for seq.</returns>
		/// <param name="inspectionID">Inspection I.</param>
		/// <param name="projectID">Project I.</param>
		/// <param name="inspectionTransactionID">Inspection transaction I.</param>
		public List<Option> getPunchListForSeq (string inspectionID, string projectID,int inspectionTransactionID)
		{
			List<Option> options = new List<Option> ();
			try 
			{
				PunchService punchservice = new PunchService (conn);
				List<Punch> punchs = punchservice.GetPunchList (conn, inspectionID, projectID);
				OptionsDO optPunch=OptionsDO.GetPunchOptions(conn,BALConstant.PUNCH_OPTIONID).FirstOrDefault();
				List<byte[]> imagesList=null;
				List<Model.CheckList> checkList = new List<Model.CheckList>();
				if (punchs != null && punchs.Count > 0)
				{
					List<PunchListImage> images = new List<PunchListImage>();
					foreach (var punchItem in punchs)
					{
						string comments = "";
						ResultType results = ResultType.NA;
						int checkListTransactionID = -1;
						imagesList=new List<byte[]>();
						var finalPunchTransaction = GetFinalPunchTransactionData(inspectionTransactionID, BALConstant.PUNCH_OPTIONID, punchItem.PunchID);
						if (finalPunchTransaction != null && finalPunchTransaction.Count > 0)
						{
							var finalPunch = finalPunchTransaction.FirstOrDefault();
							checkListTransactionID = finalPunch.CheckListTransactionID;
							results = (ResultType)finalPunch.CheckListResultTypeID;
							comments = finalPunch.CheckLIstComment;
							foreach (var item in finalPunchTransaction)
							{
								images.Add(new PunchListImage()
									{
										Image = item.Images,
										inspectionTransID = item.IspectionTransactionID,
										PunchID=item.PunchID
									});
								imagesList.Add(item.Images);
							}
						}
						checkList.Add(new Model.CheckList()
							{
								description = punchItem.punchDescription,
								itemType = ItemType.PunchList,
								ID = punchItem.PunchID,
								CheckListTransID=checkListTransactionID,
								comments = comments,
								Result = results,
								PunchID=punchItem.PunchID,
								photos=imagesList
							});
					}
					options.Add (new Option (){ 
						ID=optPunch.ID,
						name = optPunch.OptionsDesc,
						checkListItems = checkList,
						InspectionID=inspectionID,
						isGuidedPicture=false,

						SequenceID=BALConstant.PUNCH_SEQUENCEID
					});
				}
			} 
			catch (Exception ex) 
			{
				Debug.WriteLine ("Exception Occured in getPunchListForSeq method due to " + ex.Message);
			}
			return options;
		}

		/// <summary>
		/// Gets the final punch transaction data.
		/// </summary>
		/// <returns>The final punch transaction data.</returns>
		/// <param name="inspectionTransactionID">Inspection transaction I.</param>
		/// <param name="optionID">Option I.</param>
		/// <param name="punchID">Punch I.</param>
		private List<TransactionResult> GetFinalPunchTransactionData(int inspectionTransactionID,int optionID,int punchID)
		{
			string query = string.Format(@"select OT.InspectionTransID IspectionTransactionID,
                            CLT.CheckListTransactionID as CheckListTransactionID,
                            CLT.Comment as CheckLIstComment,
                            CLT.OptionTransID as OptionsTransactionID,
                            OT.SequenceID as SequenceID,
                            OT.LevelID as LevelID,
                            OT.SpaceID as SpaceID,
                            OT.OptionID as OptionID,
                            CLT.ResultTypeID as CheckListResultTypeID,
                            PLI.PunchID as PunchID,
                            PLI.PunchListImage as Images
                            from OptionsTrans OT                             
                            inner join CheckListTransaction CLT on CLT.OptionTransID=OT.ID
                            inner join PunchListImage PLI on PLI.PunchID=CLT.PunchID
                            where OT.InspectionTransID={0} and OT.OptionID={1} and CLT.PunchID={2}",inspectionTransactionID,optionID,punchID);
			SQLiteCommand cmd = conn.CreateCommand (query);
			var itemTransactionData = cmd.ExecuteQuery<TransactionResult> ();
			return itemTransactionData;
		}    

		/// <summary>
		/// Gets the inspection details.
		/// </summary>
		/// <param name="inspectionID">Inspection I.</param>
		/// <param name="ins">Ins.</param>
		public void GetInspectionDetails(int inspectionID,Inspection ins)
		{
			int pathWayID = 1;
			switch (ins.Pathway) {
			case PathwayType.Elevation:
				pathWayID = 1;
				break;
			case PathwayType.Rehabilitation:
				pathWayID = 2;
				break;
			case PathwayType.Reconstruction:
				pathWayID = 3;
				break;
			default:
				pathWayID = 1;
				break;
			}
			string SequenceQuery = string.Format (@"Select * from(Select * from (Select * from (select P.PathwayID as PathwayID,
                                            P.PathwayDesc as PathwayDesc,
                                            I.InspectionID as InspectionID,                                                                                                 
                                            I.InspectionDesc as InspectionDesc,
                                            SQ.SequenceID as SequenceID,
											 SQ.Priority as SeqPriority,
                                            SQ.SequenceDesc as SequenceDesc,
                                            L.LevelID as LevelID,
                                            L.LevelDesc as LevelDesc, 
                                            L.Priority as LevPriority,											
                                            S.SpaceID as SpaceID,
                                            S.SpaceDesc as SpaceDesc,
											S.Priority as SpacePriority,
                                            O.OptionsID as OptionsID,                                                                                                          
                                            O.OptionsDesc as OptionsDesc,
											O.Priority as OptionPriority,
                                            1 as IsSpace,                                                                                                      
                                            CL.CheckListID as CheckListID,
                                            CL.CheckListDesc as CheckListDesc
                                            from InspectionMapping MM inner join Pathway as P on P.PathwayID=MM.PathwayID
                                            inner join Inspection as I on I.InspectionId=MM.InspectionId
                                            inner join Sequences SQ on MM.SequenceID=SQ.SequenceID
                                            left outer join Level L on L.LevelID=MM.LevelID  
                                            left outer join Space S on S.SpaceID=MM.SpaceID
                                            inner join options O on O.OptionsID=MM.OptionID
                                            inner join CheckList CL on CL.CheckListID=MM.CheckListID
                                            where I.InspectionId={0}  AND P.PathwayID ={1}
											Order By OptionPriority)
											Order By SpacePriority)
											order By LevPriority)
											order By SeqPriority", inspectionID, pathWayID);
			SQLiteCommand cmd = conn.CreateCommand (SequenceQuery);
			var inspectionDataList = cmd.ExecuteQuery<InspectionData> ();
			FillInspectionDetails(ins,inspectionDataList);
		}

		/// <summary>
		/// Fills the inspection details.
		/// </summary>
		/// <param name="ins">Ins.</param>
		/// <param name="inspectionDataList">Inspection data list.</param>
		void FillInspectionDetails(Inspection ins,List<InspectionData> inspectionDataList)
		{
			var optionTransaction = GetOptionTransactions (inspectionTransactionID);
			if (inspectionDataList != null && inspectionDataList.Count > 0) {
				foreach (var inspectionData in inspectionDataList) 
				{
					CreateUpdateSequence (ins, inspectionData,optionTransaction);
					ins.InspectionType=inspectionData.InspectionDesc;
				}
			}
		}

		/// <summary>
		/// Creates the update sequence.
		/// </summary>
		/// <param name="ins">Ins.</param>
		/// <param name="inspectionData">Inspection data.</param>
		/// <param name="optionTransaction">Option transaction.</param>
		public void CreateUpdateSequence(Inspection ins, InspectionData inspectionData,IEnumerable<TransactionResult> optionTransaction)
		{
			Sequence seq=null;
			try{
				if (inspectionData.LevelID!=null && inspectionData.LevelID>0) {
					seq = ins.sequences.Find (a => a.getSequenceID () == inspectionData.SequenceID);

					if (seq == null) {
						seq = new Sequence (){
							id = inspectionData.SequenceID,
							LevelID=(int)inspectionData.LevelID,
							name=inspectionData.SequenceDesc
						};
						ins.sequences.Add (seq);
					}
					CreateUpdateLevel (seq, inspectionData, optionTransaction);
				} 
				else {
					seq = ins.sequences.Find (a => a.getSequenceID () == inspectionData.SequenceID);

					if (seq == null) {
						seq = new Sequence (){
							id = inspectionData.SequenceID,
							//LevelID=null,
							name=inspectionData.SequenceDesc
						};
						ins.sequences.Add (seq);
					}

					CreateUpdateOption (seq, inspectionData, optionTransaction);
				}
			}catch (Exception ex) {
				Debug.WriteLine ("Exception Occured in CreateUpdateSequence method due to " + ex.Message);
			}
		}

		/// <summary>
		/// Creates the update level.
		/// </summary>
		/// <param name="seq">Seq.</param>
		/// <param name="inspectionData">Inspection data.</param>
		/// <param name="itemTransaction">Item transaction.</param>
		public void CreateUpdateLevel(Sequence seq,InspectionData inspectionData,IEnumerable<TransactionResult> itemTransaction)
		{
			Level level = null;
			try
			{
				if(inspectionData.SpaceID!=null && inspectionData.SpaceID>0)
				{
					if(seq!=null && seq.Levels==null){
						seq.Levels=new List<Level>();
					}    
					level = seq.Levels.Find (a => a.ID == inspectionData.LevelID && a.seqID==inspectionData.SequenceID);
					if (level == null) {
						level = new Level () {
							ID=inspectionData.LevelID !=null ? (int)inspectionData.LevelID:-1,
							name=inspectionData.LevelDesc,
							seqID=inspectionData.SequenceID,


						};
						//level.isSelected = true;
						seq.Levels.Add (level);
					}
					CreateUpdateSpace (level, inspectionData,itemTransaction);
				}
				else
				{
					if(seq!=null && seq.Levels==null){
						seq.Levels=new List<Level>();
					}    
					level = seq.Levels.Find (a => a.ID == inspectionData.LevelID && a.seqID==inspectionData.SequenceID);
					if (level == null) {
						level = new Level () {
							ID=inspectionData.LevelID !=null ? (int)inspectionData.LevelID:-1,
							name=inspectionData.LevelDesc,
							seqID=inspectionData.SequenceID
						};
						seq.Levels.Add (level);
					}
					CreateUpdateOption (level, inspectionData,itemTransaction);
				}
			}
			catch (Exception ex) {
				Debug.WriteLine ("Exception Occured in CreateUpdateLevel method due to " + ex.Message);
			}
		}

		/// <summary>
		/// Creates the update space.
		/// </summary>
		/// <param name="levelReference">Level reference.</param>
		/// <param name="inspectionData">Inspection data.</param>
		/// <param name="itemTransaction">Item transaction.</param>
		public void CreateUpdateSpace (Level levelReference,InspectionData inspectionData,IEnumerable<TransactionResult> itemTransaction)
		{
			Space spc = null;
			try{
				if(levelReference!=null && levelReference.Spaces==null){
					levelReference.Spaces=new List<Space>();
				}    
				spc = levelReference.Spaces.Find (a => a.id == inspectionData.SpaceID);
				if (spc == null) {
					spc = new Space () {
						id=inspectionData.SpaceID!=null ? (int)inspectionData.SpaceID:-1,
						name=inspectionData.SpaceDesc,
						SpaceID = (inspectionData.SpaceID.HasValue) ? inspectionData.SpaceID.Value : 0,
						seqID = inspectionData.SequenceID,
						levelID = (inspectionData.LevelID.HasValue) ? inspectionData.LevelID.Value : 0
							
					};


						

					spc.IsEnabled = true;
					//spc.isSelected = true;
					levelReference.Spaces.Add (spc);
				}



				CreateUpdateOption (spc, inspectionData,itemTransaction);



			}
			catch (Exception ex) {
				Debug.WriteLine ("Exception Occured in CreateUpdateLevel method due to " + ex.Message);
			}
		}

		/// <summary>
		/// Creates the update option.
		/// </summary>
		/// <param name="sequence">Sequence.</param>
		/// <param name="inspectionData">Inspection data.</param>
		/// <param name="itemTransaction">Item transaction.</param>
		private void CreateUpdateOption(ISequenceOption sequence,InspectionData inspectionData,IEnumerable<TransactionResult> itemTransaction){
			Option opt = null;
			try{
				if(sequence!=null && sequence.Options==null){
					sequence.Options=new List<Option>();
				}

				opt = sequence.Options.Find (o => o.ID == inspectionData.OptionsID && o.SequenceID==inspectionData.SequenceID && o.LevelID==inspectionData.LevelID && o.SpaceID==inspectionData.SpaceID);
				if (opt == null) {
					opt = new Option () {
						ID= inspectionData.OptionsID,
						name=inspectionData.OptionsDesc
					};

					opt.InspectionID=inspectionData.InspectionID;
					opt.SequenceID=inspectionData.SequenceID;
					opt.LevelID=inspectionData.LevelID ;
					opt.SpaceID=inspectionData.SpaceID;
					opt.OptionId = inspectionData.OptionsID;

					//opt.OptionTransactionID=itemTransaction.FirstOrDefault().OptionsTransactionID;
					sequence.Options.Add (opt);
				}
				if(inspectionData.LevelID>0)
				{
					if(itemTransaction!=null && itemTransaction.Count()>0)
					{
						if(inspectionData.SpaceID>0)
						{
							itemTransaction=itemTransaction.Where(i=>i.SequenceID==inspectionData.SequenceID && 
								i.LevelID==inspectionData.LevelID &&
								i.SpaceID==inspectionData.SpaceID &&
								i.OptionID==inspectionData.OptionsID);
						}
						else
						{
							itemTransaction=itemTransaction.Where(i=>i.SequenceID==inspectionData.SequenceID && 
								i.LevelID==inspectionData.LevelID &&
								i.SpaceID<=0 &&
								i.OptionID==inspectionData.OptionsID);
						}
						if(itemTransaction!=null && itemTransaction.Count()>0)
						{
							if(sequence is ISpace)
							{
								//(sequence as Space).isSelected=true;


								//UpdateOptionSelected(itemTransaction, ref opt);
							}

							//if(this.IsImageNeeded)
							//{
								GetOptionImages(itemTransaction,opt);
							//}
						}
					}
				}else
				{
					if(itemTransaction!=null && itemTransaction.Count()>0){
						itemTransaction=itemTransaction.Where(i=>i.SequenceID==inspectionData.SequenceID && 
							i.LevelID <= 0 &&
							i.SpaceID <= 0 &&
							i.OptionID==inspectionData.OptionsID);
						if(itemTransaction!=null && itemTransaction.Count()>0){
							UpdateOptionSelected(itemTransaction, ref opt);
							//if(this.IsImageNeeded)
							//{
								GetOptionImages(itemTransaction,opt);
							//}
						}
					}
				}

				CreateUpdateCheckLists(opt, inspectionData, itemTransaction);

				if (opt.getCheckListItems() != null && opt.getCheckListItems().Count > 0)
				{
					opt.isEnabled = true;
					//opt.isSelected = true;
				}
			}
			catch (Exception ex) {
				Debug.WriteLine ("Exception Occured in CreateUpdateOption method due to " + ex.Message);
			}

		}

		private void UpdateOptionSelected(IEnumerable<TransactionResult> itemTransaction, ref Option opt)
		{
			if (itemTransaction != null) {
				foreach (var transoption in itemTransaction)
				{
					if (transoption != null)
					{
						if (transoption.OptionID == opt.ID) {
							opt.isSelected = (transoption.isSelected == 1) ? true : false;
						}
						
					}
				}

			}
		}

		/// <summary>
		/// Gets the option images.
		/// </summary>
		/// <param name="itemTransaction">Item transaction.</param>
		/// <param name="opt">Opt.</param>
		private void GetOptionImages(IEnumerable<TransactionResult> itemTransaction, Option opt)
		{
			try{
				var optionImage = optionimgService.GetOptionTransactionImage (itemTransaction.FirstOrDefault().OptionsTransactionID);

				if (optionImage != null && optionImage.Count > 0) 
				{
					opt.photos = new List<OptionImage> ();
					foreach (var img in optionImage) 
					{
						opt.photos.Add (new OptionImage () 
							{
								ID=img.ID,
								Image=img.Image,
								OptionTransID=img.OptionTransID
							});
					}
				}
			}
			catch(Exception ex) {
				Debug.WriteLine ("Exception Occured in GetOptionImages method due to " + ex.Message);
			}
		}

		/// <summary>
		/// Creates the update check lists.
		/// </summary>
		/// <param name="optReference">Opt reference.</param>
		/// <param name="inspectionData">Inspection data.</param>
		/// <param name="itemTransaction">Item transaction.</param>
		private void CreateUpdateCheckLists(Option optReference, InspectionData inspectionData, IEnumerable<TransactionResult> itemTransaction)
		{
			CheckList checkList = null;
			try {
				if (optReference != null && optReference.checkListItems == null)
				{
					optReference.checkListItems = new List<CheckList>();
				}
				checkList = optReference.checkListItems.Find(i => i.ID == inspectionData.CheckListID);
				if (checkList == null) {
					checkList = new CheckList () {
						ID = inspectionData.CheckListID,
						description = inspectionData.CheckListDesc
					};
					optReference.checkListItems.Add(checkList);
				}
				if(optReference.ID==BALConstant.GUIDEDPICTURE_OPTIONID)
				{
					checkList.itemType=ItemType.GuidedPicture;
				}
				else
				{
					checkList.itemType=ItemType.CheckListItem;
				}
				if(optReference.SequenceID==BALConstant.PUNCH_SEQUENCEID)
				{
					checkList.itemType=ItemType.PunchList;
				}
				if(itemTransaction!=null && itemTransaction.Count()>0)
				{
					if (optReference.checkListItems != null && optReference.checkListItems.Count > 0)
					{
						optReference.isEnabled = true;
					}

					optReference.OptionTransactionID=itemTransaction.FirstOrDefault().OptionsTransactionID;
					var checkListTransaction=itemTransaction.Where(it=>it.CheckListID==checkList.ID).FirstOrDefault();
					if(checkListTransaction!=null)
					{
						checkList.Result=(ResultType)checkListTransaction.CheckListResultTypeID;
						checkList.comments=checkListTransaction.CheckLIstComment;
						checkList.CheckListTransID=checkListTransaction.CheckListTransactionID;
						if(checkList.itemType==ItemType.GuidedPicture)
						{
							if(this.IsImageNeeded)
							GetCheckListImages(checkListTransaction,checkList);
						}
					}
				}

			} catch (Exception ex) {
				Debug.WriteLine ("Exception Occured in CreateUpdateCheckLists method due to " + ex.Message);
			}
		}    

		/// <summary>
		/// Gets the check list images.
		/// </summary>
		/// <param name="checkListTransaction">Check list transaction.</param>
		/// <param name="checkList">Check list.</param>
		private void GetCheckListImages(TransactionResult checkListTransaction, CheckList checkList)
		{
			try{
				var images = GuildedPhotoDO.getGuidedImageList(conn,checkListTransaction.CheckListTransactionID);

				if (images != null && images.Count > 0) 
				{
					checkList.photos = new List<byte[]> ();
					foreach (var img in images) 
					{
						checkList.photos.Add (img.Image);
					}
				}
			}
			catch(Exception ex) {
				Debug.WriteLine ("Exception Occured in GetCheckListImages method due to " + ex.Message);
			}
		}

		/// <summary>
		/// Gets the option transactions.
		/// </summary>
		/// <returns>The option transactions.</returns>
		/// <param name="InspectionTransID">Inspection trans I.</param>
		private List<TransactionResult> GetOptionTransactions(int InspectionTransID)
		{
			List<TransactionResult> optionTransactionData = new List<TransactionResult> ();
			var transactionQuery=@"select OT.InspectionTransID  as IspectionTransactionID,
                                   OT.ID as OptionsTransactionID,
                                   CLT.ChecKListTransactionID as CheckListTransactionID,
                                   OT.SequenceID as SequenceID,
                                   OT.LevelID as LevelID,
                                   OT.SpaceID as SpaceID,
                                   OT.OptionID as OptionID,
								   OT.isSelected as isSelected,
                                   CLT.Comment as CheckLIstComment,
                                   CLT.ChecklIstID as CheckListID,
                                   CLT.ResultTypeID as CheckListResultTypeID
                                   from OptionsTrans OT
                                   inner join CheckListTransaction CLT on CLT.OptionTransID=OT.ID
                                   inner join ResultType RT on RT.ResultTypeID=CLT.ResultTypeID
                                   where OT.InspectionTransID="+InspectionTransID;
			SQLiteCommand cmd = conn.CreateCommand (transactionQuery);
			optionTransactionData = cmd.ExecuteQuery<TransactionResult> ();
			return optionTransactionData;
		}
	}
	public class InspectionData
	{    
		public string PathwayID{ get; set; }
		public string PathwayDesc{ get; set; }
		public string InspectionID{ get; set;}
		public string InspectionDesc{ get; set;}
		public int SequenceID{ get; set;}
		public string SequenceDesc{ get; set;}
		public int? LevelID{ get; set;}
		public string LevelDesc{ get; set;}
		public int? SpaceID{ get; set;}
		public string SpaceDesc{ get; set;}
		public int OptionsID{ get; set;}
		public string OptionsDesc{ get; set;}
		public int IsSpace{ get; set;}
		public int CheckListID{ get; set;}
		public string CheckListDesc{ get; set;}
	}

	public class TransactionResult
	{    
		public int IspectionTransactionID{ get; set;}
		public int CheckListTransactionID{ get; set;}
		public string CheckLIstComment{ get; set;}
		public int OptionsTransactionID{ get; set;}
		public int SequenceID{ get; set;}
		public int LevelID{ get; set;}
		public int SpaceID{ get; set;}
		public int OptionID{ get; set;}
		public int PunchID{ get; set;}
		public int ItemTypeID{ get; set;}
		public int ItemResultTypeID{ get; set;}
		public string ItemComment{ get; set;}
		public int CheckListID{ get; set;}
		public int CheckListResultTypeID{ get; set;}
		public byte[] Images{ get; set;}
		public int isSelected { get; set;}
	}
}