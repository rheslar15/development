using DAL.Repository;
using DAL.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Utility;
using SQLite;
using Model;
using System.Diagnostics;
using DAL;

namespace BAL.Service
{
	public class OptionTransactionService:BaseService
	{
		IRepository<OptionTransactionDO> optionTransactionRepository;        
		SQLiteConnection conn;
		CheckListTransactionRepository checkListTransactionRepository;
		public OptionTransactionService (SQLiteConnection conn)
		{
			optionTransactionRepository = RepositoryFactory<OptionTransactionDO>.GetRepository (conn);
			checkListTransactionRepository = new CheckListTransactionRepository (conn);
			this.conn = conn;
		}

		public List<OptionTransaction> GetOptionTransactions ()
		{
			List<OptionTransaction> optionTransaction = new List<OptionTransaction> ();
			try 
			{
				IEnumerable<OptionTransactionDO> optionTransactionDOs = optionTransactionRepository.GetEntities ();
				foreach (OptionTransactionDO opttransDo in optionTransactionDOs) 
				{
					optionTransaction.Add (Converter.GetOptionTransaction (optionTransactionRepository.GetEntity (opttransDo.ID)));              
				}
			} 
			catch (Exception ex) 
			{
				Debug.WriteLine ("Exception Occured in GetOptionTransactions method due to " + ex.Message);
			}
			return optionTransaction;
		}

		public List<OptionTransaction> GetOptionTransactionsForInspection (int inspectionTransID)
		{
			List<OptionTransaction> optionTransaction = new List<OptionTransaction> ();
			try 
			{				
				List<OptionTransactionDO> optionTransactionDOsforSync = OptionTransactionDO.getInsoectionOptionsforsync(conn,inspectionTransID);
				if (optionTransactionDOsforSync != null) 
				{
					foreach (OptionTransactionDO opttransDo in optionTransactionDOsforSync) 
					{
						optionTransaction.Add (Converter.GetOptionTransaction (opttransDo));
					}
				}
			} 
			catch (Exception ex) 
			{
				Debug.WriteLine ("Exception Occured in GetOptionTransactionsForInspection method due to " + ex.Message);
			}
			return optionTransaction;
		}

		public OptionTransaction GetOptionTransaction (int OptionID)
		{
			OptionTransaction optionTransaction = new OptionTransaction ();
			try 
			{
				OptionTransactionDO optionTransactionDO = optionTransactionRepository.GetEntity (OptionID);
				if (optionTransactionDO != null)
					optionTransaction = Converter.GetOptionTransaction (optionTransactionRepository.GetEntity (optionTransactionDO.ID));
			} 
			catch (Exception ex) 
			{
				Debug.WriteLine ("Exception Occured in GetOptionTransaction method due to " + ex.Message);
			}
			return optionTransaction;
		}

		public int DeleteOptionTransactionByInspectionID (int inspectionTransID)
		{
			int x = 0;
			try 
            {
				x = OptionTransactionDO.DeleteInspectionOptions (conn, inspectionTransID);
			} 
            catch (Exception ex) 
            {
				Debug.WriteLine ("Exception Occured in DeleteOptionTransactionByInspectionID method due to " + ex.Message);
			}
			return x;
		}

		public int SaveOptionTransactions (OptionTransaction optionTransaction, List<OptionTransaction> data,Option option)
		{
			int result = 0;
			try
            {
				OptionTransactionDO optionTransactionDO = Converter.GetOptionTransactionDO (optionTransaction);
				Debug.WriteLine ("fetchinf data from option transaction table start");
				Debug.WriteLine (DateTime.Today.TimeOfDay);
				var OPTID = -1;
				// Verify Data already Exist or not
				OptionTransaction item = null;

				item = data.Where (i => 
					i.inspectionTransID == optionTransaction.inspectionTransID &&
					i.SequenceID == optionTransaction.SequenceID &&
					i.LevelID == optionTransaction.LevelID &&
					i.SpaceID == optionTransaction.SpaceID &&
					i.OptionId == optionTransaction.OptionId).FirstOrDefault ();			

				Debug.WriteLine (DateTime.Today.TimeOfDay);
				Debug.WriteLine ("fetchinf data from option transaction finish");

				int optID=0;

				if (item != null) 
                {
					optionTransactionDO.ID = item.ID;
					optID = item.ID;
					Debug.WriteLine ("fetchinf data from option transaction Update table start");
					result = optionTransactionRepository.UpdateEntity (optionTransactionDO);

					Debug.WriteLine ("fetchinf data from option transaction table finish");
				} 
                else 
                {
					Debug.WriteLine ("fetchinf data from option transaction table save data start");
					//result = conn.Execute (query);
					//Insert the new option transaction entry
					result = optionTransactionRepository.SaveEntity (optionTransactionDO);
					optID = optionTransactionRepository.GetEntities ().LastOrDefault ().ID;
				}

				if (optID > 0)
				{
					OPTID = optID;
					if (option != null)
					{
						option.OptionTransactionID = optID;
						SaveInspectionCheckList(OPTID, optionTransaction, option);
					}
				}
				Debug.WriteLine ("fetchinf data from option transaction table save data finish");
			} 
            catch (Exception ex)
            {
				Debug.WriteLine ("Exception Occured in SaveOptionTransactions method due to " + ex.Message);
			}
			return result;
		}

        public void SaveInspectionCheckList(int OPTID, OptionTransaction optionTransaction, Option option)
		{
			try
			{
				CheckListTransactionDO checkListTxnDO = new CheckListTransactionDO ();
				OptionTransactionImageRepository optionTransImageRepository=new OptionTransactionImageRepository(conn);
				PunchListImageRepository punchListRepository=new PunchListImageRepository(conn);
				PunchListImage punchImages;
				bool PunchListItems=false;
				if (optionTransaction != null) 
				{					
					if(optionTransaction.checkListTransaction != null && optionTransaction.checkListTransaction.Count > 0)
					{
						foreach (var checkListItem in optionTransaction.checkListTransaction) 
						{	
							checkListItem.OptionTransactionID = OPTID;
							CheckListTransactionDO checkLists=CheckListTransactionDO.GetCheckListTransactionID(conn,OPTID,checkListItem.CheckListID).FirstOrDefault();
							checkListTxnDO = Converter.GetCheckListTransactionDO (checkListItem);
							if(checkLists != null)
							{
								if(checkLists.ID>0)
								{
									CheckList chkItem=option.checkListItems.Find(i=>i.ID==checkListTxnDO.CheckListID);
									checkListTxnDO.ID=checkLists.ID;
									checkListTransactionRepository.UpdateEntity(checkListTxnDO);
									checkListItem.ID=checkListTxnDO.ID;
									if(chkItem!=null)
										chkItem.CheckListTransID=checkListItem.ID;
								}
								else
								{
									CheckList chkItem=option.checkListItems.Find(i=>i.ID==checkListTxnDO.CheckListID);
									checkListTransactionRepository.SaveEntity (checkListTxnDO);
									checkListItem.ID=checkListTransactionRepository.GetEntities().LastOrDefault().ID;
									if(chkItem!=null)
										chkItem.CheckListTransID=checkListItem.ID;
								}
							}
							else
							{
								CheckList chkItem=option.checkListItems.Find(i=>i.ID==checkListTxnDO.CheckListID);
								checkListTransactionRepository.SaveEntity (checkListTxnDO);
								checkListItem.ID=checkListTransactionRepository.GetEntities().LastOrDefault().ID;
								if(chkItem!=null)
									chkItem.CheckListTransID=checkListItem.ID;
							}

							if(checkListItem.itemType==ItemType.GuidedPicture)
							{
								GuildedPhotoDO.DeleteGuidedImageList(conn,checkListItem.ID);
								if(checkListItem.GuidedPictures!=null && checkListItem.GuidedPictures.Count>0)
								{
									foreach(var img in checkListItem.GuidedPictures)
									{
										GuildedPhotoDO.InsertGuidedImage(conn,checkListItem.ID,img);
									}
								}
								continue;
							}
							if(checkListItem.PunchID > 0 && checkListItem.itemType==ItemType.PunchList)
							{
								PunchListItems=true;
								punchImages=new PunchListImage();
								PunchListImageDO.DeletePunchImageList(conn,checkListItem.PunchID);
								if(checkListItem.GuidedPictures!=null && checkListItem.GuidedPictures.Count>0)
								{									
									foreach(var img in checkListItem.GuidedPictures)
									{										
										if(img != null)
										{
											punchImages.Image=img;
										}
										else
										{
											punchImages.Image=null;
										}
										punchImages.PunchID=checkListItem.PunchID;
										punchImages.inspectionTransID=optionTransaction.inspectionTransID;
										PunchListImageDO punchListDO=Converter.GetPunchListImageDO(punchImages);
										punchListRepository.SaveEntity(punchListDO);
									}
								}
							}
						}
					}
					OptionTransactionImageDO.DeleteOptionImagesSync(conn,OPTID);
					if(optionTransaction.photos != null && optionTransaction.photos.Count > 0)
					{
						foreach(var optionimage in optionTransaction.photos)
						{
							optionimage.OptionTransID=OPTID;
							OptionTransactionImageDO optTransDO=Converter.GetOptionTransactionImageDO(optionimage);
							int Result = optionTransImageRepository.SaveEntity(optTransDO);
						}
					}
				}
			}
			catch(Exception ex) 
			{
				Debug.WriteLine ("Exception occured in method SaveInspectionCheckList" + ex.Message);
			}
		}


		public void clearPhotoBuffer (Inspection ins, int seqID)
		{
			try 
			{
				if (ins.locationIDImages != null && ins.locationIDImages.Count > 0)
					ins.locationIDImages.Clear ();
				List<Option> options =new List<Option>();

				var seq=ins.sequences.Where(s=>s.getSequenceID()==seqID).FirstOrDefault();
				if(seq!=null && seq.Levels!=null &&  seq.Levels.Count>0)
				{
					foreach(var lvl in seq.Levels)
					{
						if(lvl.Spaces!=null && lvl.Spaces.Count>0)
						{
							foreach(var spc in lvl.Spaces)
							{
								if(spc.isSelected)
								{
									if(spc.Options!=null && spc.Options.Count>0)
									{
									options.AddRange(spc.Options);
									}
								}
							}
						}
						else
						{
							if(lvl.Options!=null && lvl.Options.Count>0)
							{
							options.AddRange(lvl.Options);
							}
						}
					}
				}
				else
				{
					if(seq!=null && seq.Options!=null && seq.Options.Count>0)
					{
						options.AddRange(seq.Options);
					}
				}

				if(options!=null && options.Count>0)
				{
					foreach(var option in options)
					{
						if(option.photos!=null)
						{
							option.photos.Clear();
						}

						if(option!=null )
						{
							if(option.checkListItems!=null && (option.ID==BALConstant.GUIDEDPICTURE_OPTIONID || option.ID==BALConstant.PUNCH_OPTIONID))
							{
								foreach(var checkListItem in option.checkListItems)
								{
									if(checkListItem.itemType==ItemType.GuidedPicture || checkListItem.itemType==ItemType.PunchList)
									{
										if(checkListItem.photos!=null)
										{
											checkListItem.photos.Clear();
										}
									}
								}
							}
						}
					}
				}							
			} 
			catch (Exception ex) 
			{
				Debug.WriteLine ("Exception Occured in clearPhotoBuffer method due to " + ex.Message);
			}
		}

		public int DeleteOptionTransactions (OptionTransaction optionTransaction)
		{
			int result = 0;
			try 
			{			
				// Remove Content from option transaction table
				OptionTransactionDO optionTransactionDO = Converter.GetOptionTransactionDO (optionTransaction);
				if (optionTransactionDO != null)
				{
					// Remove Image Transaction  from image transaction table
					result = optionTransactionRepository.DeleteEntity (optionTransactionDO.ID);
				}
			} 
			catch (Exception ex) 
			{
				Debug.WriteLine ("Exception Occured in DeleteOptionTransactions method due to " + ex.Message);
			}
			return result;
		}

		public List<TransactionResult> GetOptionTransactionsImage(int InspectionTransID)
		{
			List<TransactionResult> optionTransactionData = new List<TransactionResult> ();

			var	transactionQuery = string.Format (@"select OT.InspectionTransID  as IspectionTransactionID,
                                   OT.ID as OptionsTransactionID,
                                   CLT.ChecKListTransactionID as CheckListTransactionID,
                                   OT.SequenceID as SequenceID,
                                   OT.LevelID as LevelID,
                                   OT.SpaceID as SpaceID,
                                   OT.OptionID as OptionID,
                                   CLT.Comment as CheckLIstComment,
                                   CLT.ChecklIstID as CheckListID,
                                   CLT.ResultTypeID as CheckListResultTypeID,
								   OI.ItemImage as Images
                                   from OptionsTrans OT
                                   inner join CheckListTransaction CLT on CLT.OptionTransID=OT.ID
                                   inner join ResultType RT on RT.ResultTypeID=CLT.ResultTypeID
								   inner join OptionImage OI on OI.OptionTransactionID= OT.ID
                                   where OT.InspectionTransID={0}", InspectionTransID);
			
			SQLiteCommand cmd = conn.CreateCommand (transactionQuery);
			optionTransactionData = cmd.ExecuteQuery<TransactionResult> ();
			return optionTransactionData;
		}
	}
}