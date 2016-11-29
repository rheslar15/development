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
	public class PunchService
	{
		IRepository<PunchListDO> punchRepository;
		SQLiteConnection conn;


		public PunchService(SQLiteConnection conn)
		{
			punchRepository = RepositoryFactory<PunchListDO>.GetRepository(conn);
			this.conn = conn;
		}
			
		public Punch GetPunch(int PunchID)
		{
			Punch punch = new Punch();
			try
            {
			    PunchListDO punchDo = punchRepository.GetEntity(PunchID);
			    if (punchDo != null)
				    punch = Converter.GetPunchList(punchDo);
			}
            catch (Exception ex)
            {
                Debug.WriteLine("Exception Occured in GetPunch method due to " + ex.Message);
			}
			return punch;
		}

		public List<Punch> GetPunchItems(string inspectionID, string projectID, int inspectionTransID)
		{
			List<Punch> punchList = new List<Punch> ();
			try
            {
			    IEnumerable<PunchListDO> punchListDos = punchRepository.GetEntities().Where(p=>p.InspectionID==inspectionID && p.ProjectID==projectID);
			    foreach (PunchListDO punchDo in punchListDos)
			    {
					punchList.Add(Converter.GetPunchList(punchDo));
			    }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception Occured in GetPunchItems method due to " + ex.Message);
			}
			return punchList;
		}

		public List<Punch> GetPunchItems()
		{
			List<Punch> punchList = new List<Punch> ();
			try
            {
			    IEnumerable<PunchListDO> punchListDos = punchRepository.GetEntities();
			    foreach (PunchListDO punchDo in punchListDos)
			    {
				    punchList.Add(Converter.GetPunchList(punchRepository.GetEntity(punchDo.ID)));
			    }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception Occured in GetPunchItems method due to " + ex.Message);
			}
			return punchList;
		}

		/// <summary>
		/// Saves the punch item.
		/// </summary>
		/// <returns>The punch item.</returns>
		/// <param name="punchItem">Punch item.</param>
		public int SavePunchItem(Punch punchItem)
		{
			int result = 0;
			try
			{
				PunchListDO punchItemDO = Converter.GetPunchListDO(punchItem);
			 	result = punchRepository.SaveEntity(punchItemDO);

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception Occured in SavePunchItem method due to " + ex.Message);
			}
			return result;
		}

		public int DeletePunchItem(Punch punchItem)
		{
			int result = 0;
			try
            {
			    PunchListDO punchItemDO = Converter.GetPunchListDO(punchItem);
			    result = punchRepository.DeleteEntity(punchItemDO.ID);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception Occured in DeletePunchItem method due to " + ex.Message);
			}
			return result;
		}

		public int DeleteAllPunchItem(string inspectionID, string projectID)
		{
			int result = 0;
			try{
				result = PunchListDO.DeletePunchList(conn, inspectionID, projectID);
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Exception Occured in DeletePunchItem method due to " + ex.Message);
			}
			return result;
		}

		public List<Punch> GetPunchList(SQLiteConnection conn, string InspectionID, string ProjectID)
		{
			return PunchListDO.getPunchList (conn, InspectionID,ProjectID);
		}

        public List<Punch> getPunchList(string inspectionID, string projectID)
        {
            return PunchListDO.getPunchList(conn, inspectionID, projectID);
        }
    }
}

