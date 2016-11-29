using DAL.DO;
using DAL.Repository;
using DAL.Utility;
using Model;
using System;
using System.Diagnostics;

namespace BAL
{
	public class InspectionCheckListTransactionService
	{
        IRepository<CheckListTransactionDO> checkListTransactionsRepository;
		public InspectionCheckListTransactionService ()
		{
		}

        public int SaveCheckListTransaction(CheckListTransaction checkList)
        {
            CheckListTransactionDO checkListtxnDO = new CheckListTransactionDO();
            int result = 0;
            try
            {
                if (checkList != null)
                {
                    checkListtxnDO = Converter.GetCheckListTransactionDO(checkList);
                    result = checkListTransactionsRepository.SaveEntity(checkListtxnDO);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception Occured in SaveCheckListTransaction method due to " + ex.Message);
            }
            return result;
        }

        public int DeleteCheckListTransaction(CheckListTransaction checkList)
        {
            int result = 0;
            try
            {
                if (checkList != null && checkList.ID > 0)
                {
                    result = checkListTransactionsRepository.DeleteEntity(checkList.ID);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception Occured in DeleteInspectionTransactionItem method due to " + ex.Message);
            }
            return result;
        }

        public CheckListTransaction GetCheckListTransaction(int id)
        {
            CheckListTransaction checkListTransaction = new CheckListTransaction();
            try
            {
                CheckListTransactionDO checkListitems = checkListTransactionsRepository.GetEntity(id);
                checkListTransaction = Converter.GetCheckListTransaction(checkListitems);
            }
            catch (Exception ex)
            {
				Debug.WriteLine("Exception Occured in GetCheckListTransaction method due to " + ex.Message);
            }
            return checkListTransaction;
        }
	}
}