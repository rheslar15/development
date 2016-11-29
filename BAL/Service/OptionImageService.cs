using System;
using DAL.Repository;
using DAL.DO;
using SQLite;
using Model;
using System.Collections.Generic;
using DAL.Utility;
using System.Diagnostics;
using DAL;
using System.Linq;

namespace BAL
{
	public class OptionImageService:BaseService
	{
		IRepository<OptionTransactionImageDO> optionTransactionRepository;
		public OptionImageService(SQLiteConnection conn)
		{
			optionTransactionRepository = RepositoryFactory<OptionTransactionImageDO>.GetRepository(conn);
		}

		public List<OptionImage> GetOptionImages()
		{
			List<OptionImage> OptionImages = new List<OptionImage>();
			try
			{
				IEnumerable<OptionTransactionImageDO> checkListDOs = optionTransactionRepository.GetEntities();
				foreach (OptionTransactionImageDO optionTransactionImageDO in checkListDOs)
				{
					OptionImages.Add(Converter.GetOptionImage(optionTransactionRepository.GetEntity(optionTransactionImageDO.ID)));
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Exception Occured in GetCheckList method due to " + ex.Message);
			}
			return OptionImages;
		}

		public OptionImage GetOptionImage(int optionImageID)
		{
			OptionImage optionImage = new OptionImage();
			try
			{
				OptionTransactionImageDO optionTransactionImageDO = optionTransactionRepository.GetEntity(optionImageID);
				if (optionTransactionImageDO != null)
					optionImage = Converter.GetOptionImage(optionTransactionImageDO);
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Exception Occured in GetCheckList method due to " + ex.Message);
			}
			return optionImage;
		}

		public List<OptionImage> GetOptionTransactionImage(int optionTransactionID)
		{
			List<OptionImage> optionImage = new List<OptionImage>();
			try
			{
				IEnumerable<OptionTransactionImageDO> optionTransactionImagesDO = optionTransactionRepository.GetEntities().Where(i=>i.OptionTransactionID==optionTransactionID);
				if (optionTransactionImagesDO != null)
				{
					foreach(var optionTransactionImage in optionTransactionImagesDO)
					{
						optionImage.Add(Converter.GetOptionImage(optionTransactionImage));
					}
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Exception Occured in GetCheckList method due to " + ex.Message);
			}
			return optionImage;
		}

		public int SaveOptionImage(OptionImage optionImage)
		{
			int result = 0;
			try{
				OptionTransactionImageDO OptionTransactionImageDO = Converter.GetOptionTransactionImageDO(optionImage);

				result = optionTransactionRepository.SaveEntity(OptionTransactionImageDO);
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Exception Occured in SaveSpaceOption method due to " + ex.Message);
			}
			return result;
		}

		public int DeleteOptionImage(OptionImage optionImage)
		{
			int result = 0;
			try{
				OptionTransactionImageDO optionTransactionImageDO = Converter.GetOptionTransactionImageDO(optionImage);

				result = optionTransactionRepository.DeleteEntity(optionTransactionImageDO.ID);
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Exception Occured in DeleteSpaceOption method due to " + ex.Message);
			}
			return result;
		}

		public int DeleteOptionImagesForSync(SQLiteConnection conn,int OptionTransactionID)
		{
			int result = 0;
			try
			{				
				result = OptionTransactionImageDO.DeleteOptionImagesSync(conn,OptionTransactionID);
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Exception Occured in DeleteSpaceOption method due to " + ex.Message);
			}
			return result;
		}
	}
}