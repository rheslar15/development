using System;
using DAL.Repository;
using DAL.DO;
using DAL;
using SQLite;
using Model;
using System.Collections.Generic;
using DAL.Utility;
using System.Linq;
using System.Diagnostics;

namespace BAL.Service
{
	public class PunchImageService
	{
		IRepository<PunchListImageDO> punchImageRepository;
		SQLiteConnection conn;
		public PunchImageService(SQLiteConnection conn)
		{
			punchImageRepository = RepositoryFactory<PunchListImageDO>.GetRepository(conn);
			this.conn = conn;
		}

		public int SavePunchItemImages(List<byte[]> punchImages, int inspectionTransID, int punchID)
		{
			int result = 0;

			try{
				if (punchImages != null && punchImages.Count > 0)
				{
					foreach (var img in punchImages) 
					{
						PunchListImage punchListImage = new PunchListImage ();
						punchListImage.Image = img;
						PunchListImageDO punchListImageDO = Converter.GetPunchListImageDO (punchListImage);
						punchListImageDO.InspectionTransID = inspectionTransID;
						punchListImageDO.PunchID = punchID;
						result = punchImageRepository.SaveEntity (punchListImageDO);
					}
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Exception Occured in SavePunchItemImages method due to " + ex.Message);
			}
			return result;
		}

		public int DeletePunchItemImages(int inspectionTransID)
		{
			int result = 0;
			try
            {
				result = PunchListImageDO.DeletePunchListImageList(conn,inspectionTransID);
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Exception Occured in DeletePunchItemImages method due to " + ex.Message);
			}
			return result;
		}


		public List<PunchListImageDO> GetPunchItemImages(int inspectionTransID)
		{
			List<PunchListImageDO> punchList = new List<PunchListImageDO> ();
			try
            {
				punchList = PunchListImageDO.getAllPunchImagesForInspection(conn, inspectionTransID);
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Exception Occured in GetPunchItems method due to " + ex.Message);
			}
			return punchList;
		}
	}
}