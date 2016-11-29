using System;
using DAL.Repository;
using DAL.DO;
using SQLite;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using Model;

namespace BAL
{
	public class PathwayService
	{
		IRepository<PathwayDO> pathwayRepository;
		public PathwayService (SQLiteConnection conn)
		{
			pathwayRepository = RepositoryFactory<PathwayDO>.GetRepository(conn); 
		}

		public PathwayDO GetPathway(int PathwayID)
		{
			PathwayDO PathwayDO = new PathwayDO() ;
			try
			{
				PathwayDO = pathwayRepository.GetEntity(PathwayID);
			}
			catch(Exception ex)
			{
				Debug.WriteLine("Exception Occured in GetInspections method due to " + ex.Message);
			}
			return PathwayDO;
		}
	}
}