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
	public class LevelTransactionService : BaseService
	{
		IRepository<LevelTransactionDO> levelTransactionRepository;
		SQLiteConnection conn;

		public LevelTransactionService (SQLiteConnection conn)
		{
			levelTransactionRepository = RepositoryFactory<LevelTransactionDO>.GetRepository (conn);

			this.conn = conn;
			
		}

		public List<LevelTransaction> GetLevelTransactions ()
		{
			List<LevelTransaction> levelTransaction = new List<LevelTransaction> ();
			try {
				IEnumerable<LevelTransactionDO> levelTransactionDOs = levelTransactionRepository.GetEntities ();
				foreach (LevelTransactionDO levelTransDo in levelTransactionDOs) {
					levelTransaction.Add (Converter.GetLevelTransaction (levelTransactionRepository.GetEntity (levelTransDo.ID)));              
				}
			} catch (Exception ex) {
				Debug.WriteLine ("Exception Occured in GetLevelTransactions method due to " + ex.Message);
			}
			return levelTransaction;
		}

		public LevelTransaction GetLevelTransaction (int LevelID)
		{
			LevelTransaction levelTransaction = new LevelTransaction ();
			try {
				LevelTransactionDO levelTransactionDO = levelTransactionRepository.GetEntity (LevelID);
				if (levelTransactionDO != null)
					levelTransaction = Converter.GetLevelTransaction (levelTransactionRepository.GetEntity (levelTransactionDO.ID));
			} catch (Exception ex) {
				Debug.WriteLine ("Exception Occured in GetLevelTransaction method due to " + ex.Message);
			}
			return levelTransaction;
		}

		public int SaveLevelTransactions (LevelTransaction levelTransaction, List<LevelTransaction> data, Level level)
		{
			int result = 0;

			try {
				LevelTransactionDO levelTransactionDO = Converter.GetLevelTransactionDO (levelTransaction);
				Debug.WriteLine ("fetchinf data from Level transaction table start");
				Debug.WriteLine (DateTime.Today.TimeOfDay);

				var LEVELID = -1;
				// Verify Data already Exist or not
				LevelTransaction item = null;

				item = data.Where (i => 
				i.LevelID == levelTransaction.LevelID && 
					i.SeqID == levelTransaction.SeqID).FirstOrDefault ();


				Debug.WriteLine (DateTime.Today.TimeOfDay);
				Debug.WriteLine ("fetchinf data from Level transaction finish");

				int ID = 0;
				int LevelID = 0;

				if (item != null) {
					levelTransactionDO.ID = item.ID;
					ID = item.ID;
					LevelID = (item.LevelID.HasValue) ? item.LevelID.Value : -1;
					Debug.WriteLine ("fetchinf data from Level transaction Update table start");
					result = levelTransactionRepository.UpdateEntity (levelTransactionDO);

					Debug.WriteLine ("fetchinf data from Level transaction table finish");
				} else {
					Debug.WriteLine ("fetchinf data from Level transaction table save data start");
					//result = conn.Execute (query);
					//Insert the new option transaction entry
					result = levelTransactionRepository.SaveEntity (levelTransactionDO);
					ID = levelTransactionRepository.GetEntities ().LastOrDefault ().ID;
					LevelID = levelTransactionRepository.GetEntities ().LastOrDefault ().LevelID.Value;
				}


				if (ID > 0) {
					LEVELID = ID;
				}	
				Debug.WriteLine ("fetchinf data from Level transaction table save data finish");
			} catch (Exception ex) {
				Debug.WriteLine ("Exception Occured in SaveLevelTransactions method due to " + ex.Message);
			}

			return result;
		}

		public int DeleteLevelTransactions (LevelTransaction levelTransaction)
		{
			int result = 0;
			try 
			{			
				// Remove Content from option transaction table
				LevelTransactionDO levelTransactionDO = Converter.GetLevelTransactionDO (levelTransaction);
				// Remove Image Transaction  from image transaction table
				result = levelTransactionRepository.DeleteEntity (levelTransactionDO.ID);
			} 
			catch (Exception ex) 
			{
				Debug.WriteLine ("Exception Occured in DeleteLevelTransactions method due to " + ex.Message);
			}
			return result;
		}

		public int DeleteLevelTransaction(LevelTransaction levelTransaction)
		{
			int result = 0;
			try
			{
				// Remove Content from option transaction table
				LevelTransactionDO levelTransactionDO = Converter.GetLevelTransactionDO(levelTransaction);
				// Remove Image Transaction  from image transaction table
				result = levelTransactionRepository.DeleteEntity(levelTransactionDO.ID);
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Exception Occured in DeleteLevelTransactions method due to " + ex.Message);
			}
			return result;
		}

	}
}

