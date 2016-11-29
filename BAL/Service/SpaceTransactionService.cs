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
	public class SpaceTransactionService : BaseService
	{
		IRepository<SpaceTransactionDO> spaceTransactionRepository;
		SQLiteConnection conn;

		public SpaceTransactionService (SQLiteConnection conn)
		{
			spaceTransactionRepository = RepositoryFactory<SpaceTransactionDO>.GetRepository (conn);

			this.conn = conn;

		}

		public List<SpaceTransaction> GetSpaceTransactions ()
		{
			List<SpaceTransaction> spaceTransaction = new List<SpaceTransaction> ();
			try {
				IEnumerable<SpaceTransactionDO> spaceTransactionDOs = spaceTransactionRepository.GetEntities ();
				foreach (SpaceTransactionDO spaceTransDo in spaceTransactionDOs) {
					spaceTransaction.Add (Converter.GetSpaceTransaction (spaceTransactionRepository.GetEntity (spaceTransDo.ID)));              
				}
			} catch (Exception ex) {
				Debug.WriteLine ("Exception Occured in GetSpaceTransactions method due to " + ex.Message);
			}
			return spaceTransaction;
		}

		public SpaceTransaction GetSpaceTransaction (int SpaceID)
		{
			SpaceTransaction spaceTransaction = new SpaceTransaction ();
			try {
				SpaceTransactionDO spaceTransactionDO = spaceTransactionRepository.GetEntity (SpaceID);
				if (spaceTransactionDO != null)
					spaceTransaction = Converter.GetSpaceTransaction (spaceTransactionRepository.GetEntity (spaceTransactionDO.ID));
			} catch (Exception ex) {
				Debug.WriteLine ("Exception Occured in GetSpaceTransaction method due to " + ex.Message);
			}
			return spaceTransaction;
		}

		public int SaveSpaceTransactions (SpaceTransaction spaceTransaction, List<SpaceTransaction> data, Space space)
		{
			int result = 0;

			try {
				SpaceTransactionDO spaceTransactionDO = Converter.GetSpaceTransactionDO (spaceTransaction);
				Debug.WriteLine ("fetchinf data from Space transaction table start");
				Debug.WriteLine (DateTime.Today.TimeOfDay);

				var SPACEID = -1;
				// Verify Data already Exist or not
				SpaceTransaction item = null;

				item = data.Where (i => 
					i.SpaceID == spaceTransaction.SpaceID && 
					i.LevelID == spaceTransaction.LevelID && 
					i.SeqID == spaceTransaction.SeqID 
					).FirstOrDefault ();


				Debug.WriteLine (DateTime.Today.TimeOfDay);
				Debug.WriteLine ("fetchinf data from Space transaction finish");
				int ID = 0;
				int SpaceID = 0;

				if (item != null) {
					spaceTransactionDO.ID = item.ID;
					ID = item.ID;
					SpaceID = (item.SpaceID.HasValue) ? item.SpaceID.Value : -1;
					Debug.WriteLine ("fetchinf data from Space transaction Update table start");
					result = spaceTransactionRepository.UpdateEntity (spaceTransactionDO);

					Debug.WriteLine ("fetchinf data from Space transaction table finish");
				} else {
					Debug.WriteLine ("fetchinf data from Space transaction table save data start");
					//result = conn.Execute (query);
					//Insert the new option transaction entry
					result = spaceTransactionRepository.SaveEntity (spaceTransactionDO);
					ID = spaceTransactionRepository.GetEntities ().LastOrDefault ().ID;
					SpaceID = spaceTransactionRepository.GetEntities ().LastOrDefault ().SpaceID.Value;
				}
				if (ID > 0) {
					SPACEID = ID;
				}	
				Debug.WriteLine ("fetchinf data from Space transaction table save data finish");

			} catch (Exception ex) {
				Debug.WriteLine ("Exception Occured in SaveSpaceTransactions method due to " + ex.Message);
			}
			return result;
		}

		public int DeleteSpaceTransactions (SpaceTransaction spaceTransaction)
		{
			int result = 0;
			try 
			{			
				// Remove Content from option transaction table
				SpaceTransactionDO spaceTransactionDO = Converter.GetSpaceTransactionDO (spaceTransaction);
				// Remove Image Transaction  from image transaction table
				result = spaceTransactionRepository.DeleteEntity (spaceTransactionDO.ID);
			} 
			catch (Exception ex) 
			{
				Debug.WriteLine ("Exception Occured in DeleteSpaceTransactions method due to " + ex.Message);
			}
			return result;
		}
	}
}

