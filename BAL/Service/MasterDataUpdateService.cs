using System;
using ServiceLayer.Service;
using Model;
using SQLite;
using System.Linq;
using System.Collections.Generic;
using DAL.DO;
using DAL.Utility;
using System.Diagnostics;
using LiroInspectServiceModel.Services;

namespace BAL
{
	public delegate void ProgressDelegate(float Progress);
	public class MasterDataUpdateService:BaseService
	{
		Model.ServiceModel.MasterDataCheckResponse checkMasterDataResponce{ get; set;}
		public ProgressDelegate progressDelegate;
		SQLiteConnection conn;
		static object locker = new object ();
		//string dbVersion="1.0";
		//static float serverDBVersion;

		/// <summary>
		/// Initializes a new instance of the <see cref="BAL.MasterDataUpdateService"/> class.
		/// </summary>
		/// <param name="conn">Conn.</param>
		public MasterDataUpdateService (SQLiteConnection conn)
		{
			checkMasterDataResponce=new Model.ServiceModel.MasterDataCheckResponse();
			this.conn = conn;
		}
			
		/// <summary>
		/// Updates the database service.
		/// Updates/Progresses the progress delegate
		/// </summary>
		/// <returns>The database service.</returns>
		/// <param name="Token">Token.</param>
		public MasterDataUpdateStatus UpdateDatabaseService(string Token)
		{
			IServices service = new Services ();
			MasterDataUpdateStatus resultMasterData = new MasterDataUpdateStatus ();



			lock (locker)
			{
				try
				{
					progressDelegate(0.05f);
					var value=service.FetchMasterData (new Model.ServiceModel.MasterDataUpdationRequest (){token=Token });
					if(value != null && value.result != null && value.result.code == 0)
					{
						conn.BeginTransaction ();
						progressDelegate(0.10f);

						if (value.pathway != null && value.pathway.Count()>0) 
						{
							UpdateDatabase (Converter.GetMasterPathwayDO(value.pathway.ToList()));

						}
						else
						{
							conn.Rollback ();
							return (new MasterDataUpdateStatus(){ Result=false,ErrorMessage="Master Data unable to update, Contact your Administrator"});
						}
						progressDelegate(0.15f);
						if (value.inspection != null && value.inspection.Count()>0) 
						{
							UpdateDatabase (Converter.GetMasterInspectionDO(value.inspection.ToList()));
						}
						else
						{
							conn.Rollback ();
							return (new MasterDataUpdateStatus(){ Result=false,ErrorMessage="Master Data unable to update, Contact your Administrator"});
						}

						if (value.sequence != null && value.sequence.Count()>0) 
						{
							UpdateDatabase (Converter.GetMasterSequenceDO(value.sequence.ToList()));
						}
						else
						{
							conn.Rollback ();
							return (new MasterDataUpdateStatus(){ Result=false,ErrorMessage="Master Data unable to update, Contact your Administrator"});

						}

						progressDelegate(0.20f);
						if (value.level != null && value.level.Count()>0) 
						{
							UpdateDatabase (Converter.GetMasterLevelDO(value.level.ToList()));
						}
						else
						{
							conn.Rollback ();
							return (new MasterDataUpdateStatus(){ Result=false,ErrorMessage="Master Data unable to update, Contact your Administrator"});

						}

						progressDelegate(0.25f);
						if (value.space != null && value.space .Count()>0) 
						{
							UpdateDatabase (Converter.GetMasterSpaceDO(value.space.ToList()));
						}
						else
						{
							conn.Rollback ();
							return (new MasterDataUpdateStatus(){ Result=false,ErrorMessage="Master Data unable to update, Contact your Administrator"});

						}

						progressDelegate(0.35f);
						if (value.option != null && value.option.Count()>0 ) 
						{
							UpdateDatabase (Converter.GetMasterOptionsDO(value.option.ToList()));
						}
						else
						{
							conn.Rollback ();
							return (new MasterDataUpdateStatus(){ Result=false,ErrorMessage="Master Data unable to update, Contact your Administrator"});

						}

						progressDelegate(0.50f);
						if (value.checkList != null && value.checkList.Count()>0) 
						{
							UpdateDatabase (Converter.GetMasterCheckListDO(value.checkList.ToList()));
						}
						else
						{
							conn.Rollback ();
							return (new MasterDataUpdateStatus(){ Result=false,ErrorMessage="Master Data unable to update, Contact your Administrator"});

						}

						progressDelegate(0.60f);
						if (value.inspectionMapping != null && value.inspectionMapping.Count()>0) 
						{
							UpdateDatabase (Converter.GetMasterInspectionMappingDO(value.inspectionMapping.ToList()));
						}
						else
						{
							conn.Rollback ();
							return (new MasterDataUpdateStatus(){ Result=false,ErrorMessage="Master Data unable to update, Contact your Administrator"});

						}
						progressDelegate(0.80f);
						DeleteTransactionData();
						progressDelegate(0.90f);
						conn.Commit();
						using(UserSettingService usr=new UserSettingService(conn))
						{
							if(value.DBVersion != string.Empty)
							{
								usr.UpdateUserSetting(new UserSetting(){ID=3,SettingName="DBVersion",SettingValue=value.DBVersion});
							}
						}
						progressDelegate(1.0f);
						resultMasterData.Result=true;
						resultMasterData.ErrorMessage="Master Data Updated Successfully";
					}
					else if(value!=null && value.result!=null)
					{
						resultMasterData.Result=false;
						resultMasterData.ErrorMessage=value.result.message;

					}
					else
					{
						resultMasterData.Result=false;
						resultMasterData.ErrorMessage="Master Data Updation failed due to network issue.";

					}

				}
				catch(Exception ex) 
				{
					Debug.WriteLine("Exception Occured in UpdateDatabaseService method due to " + ex.Message);
					conn.Rollback ();
					resultMasterData.Result=false;
					resultMasterData.ErrorMessage="Master Data Updated Failed due to error : "+ex.Message;
				}
				return resultMasterData;
			}
		}

		/// <summary>
		/// Deletes the transaction data.
		/// </summary>
		private void DeleteTransactionData()
		{
			string InspectionTransQuery= "Delete From InspectionTrans";
			conn.Execute (InspectionTransQuery);

			string OptionTransQuery= "Delete From OptionsTrans";
			conn.Execute (OptionTransQuery);

			string SpaceTransQuery= "Delete From SpaceTransaction";
			conn.Execute (SpaceTransQuery);

			string LevelTransQuery= "Delete From LevelTransaction";
			conn.Execute (LevelTransQuery);

			string CheckListTransQuery= "Delete From CheckListTransaction";
			conn.Execute (CheckListTransQuery);

			string DocumentTransQuery= "Delete From Document";
			conn.Execute (DocumentTransQuery);

			string GuidedImageTransQuery= "Delete From GuidedImage";
			conn.Execute (GuidedImageTransQuery);

			string LocationImageTransQuery= "Delete From LocationImage";
			conn.Execute (LocationImageTransQuery);

			string NotificationsTransQuery= "Delete From Notifications";
			conn.Execute (NotificationsTransQuery);

			string OptionImageTransQuery= "Delete From OptionImage";
			conn.Execute (OptionImageTransQuery);

			string OptionPunchTransQuery= "Delete From OptionPunch";
			conn.Execute (OptionPunchTransQuery);

			string PunchListTransQuery= "Delete From PunchList";
			conn.Execute (PunchListTransQuery);

			string PunchListImageTransQuery= "Delete From PunchListImage";
			conn.Execute (PunchListImageTransQuery);

			string ReportTransQuery= "Delete From Report";
			conn.Execute (ReportTransQuery);
				
			//string InspectionTransQuery2= "Delete From Inspection";
			//conn.Execute (InspectionTransQuery2);

			//string InspectionTransQuery1 = "Select * from  Inspection";
			//ar Data = conn.Execute (InspectionTransQuery1);

		}

		/// <summary>
		/// Updates the database.
		/// </summary>
		/// <param name="itemvalues">Itemvalues.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public void UpdateDatabase<T>(IEnumerable<T> itemvalues) where T : IDomianObject
		{
			conn.DeleteAll<T> ();
			conn.InsertAll (itemvalues);
		}
	}

	/// <summary>
	/// Master data update status.
	/// </summary>
	public class MasterDataUpdateStatus
	{
		public bool Result{ get; set; }
		public string ErrorMessage{ get; set; }
	}
}