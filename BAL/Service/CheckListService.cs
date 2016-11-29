using System;
using DAL.Repository;
using DAL.DO;
using SQLite;
using Model;
using System.Collections.Generic;
using DAL.Utility;
using System.Diagnostics;

namespace BAL
{
	public class CheckListService
	{
		IRepository<CheckListDO> checkListRepository;

		public CheckListService(SQLiteConnection conn)
		{
			checkListRepository = RepositoryFactory<CheckListDO>.GetRepository(conn);
		}

		public List<CheckList> GetCheckLists()
		{
			List<CheckList> checkLists = new List<CheckList>();
			try
            {
				IEnumerable<CheckListDO> checkListDOs = checkListRepository.GetEntities();
				foreach (CheckListDO checkListDO in checkListDOs)
				{
					checkLists.Add(Converter.GetCheckList(checkListRepository.GetEntity(checkListDO.ID)));
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Exception Occured in GetCheckList method due to " + ex.Message);
			}
			return checkLists;
		}

		public CheckList GetCheckList(int checkListID)
		{
			CheckList checkList = new CheckList();
			try
            {
				CheckListDO checkListDO = checkListRepository.GetEntity(checkListID);
				if (checkListDO != null)
					checkList = Converter.GetCheckList(checkListDO);
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Exception Occured in GetCheckList method due to " + ex.Message);
			}
			return checkList;
		}
	}
}