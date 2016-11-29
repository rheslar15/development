using System;
using DAL.Repository;
using SQLite;
using DAL.DO;
using Model;
using System.Collections.Generic;
using DAL.Utility;
using System.Diagnostics;

namespace BAL
{
	public class LevelService
	{
		IRepository<LevelDO> levelRepository;

		public LevelService(SQLiteConnection conn)
		{
			levelRepository = RepositoryFactory<LevelDO>.GetRepository(conn);

		}

		public List<Level> GetLevels()
		{
			List<Level> levels = new List<Level>() ;
			try
			{
			    IEnumerable<LevelDO> levelDOs = levelRepository.GetEntities();

			    foreach (LevelDO lvlDo in levelDOs)
			    {
				    levels.Add(Converter.GetLevel(levelRepository.GetEntity(lvlDo.ID)));
			    }
			}
            catch (Exception ex)
            {
                Debug.WriteLine("Exception Occured in GetLevels method due to " + ex.Message);
			}
			return levels;
		}

		public Level GetLevel(int levelID)
		{
			Level level = new Level();
			try
			{
			    LevelDO levelDO = levelRepository.GetEntity(levelID);
			    if (levelDO != null)
				    level = Converter.GetLevel(levelDO);
			}
            catch (Exception ex)
            {
                Debug.WriteLine("Exception Occured in GetLevel method due to " + ex.Message);
			}
			return level;
		}

		public int SaveLevel(Level level)
		{
			int result = 0;
			try
            {
			    LevelDO levelDO = Converter.GetLevelDO(level);
		        result = levelRepository.SaveEntity(levelDO);
			}
            catch (Exception ex)
            {
                Debug.WriteLine("Exception Occured in SaveLevel method due to " + ex.Message);
            }
			return result;
		}

		public int DeleteLevel(Level level)
		{
			int result = 0;
			try
            {
			    LevelDO levelDO = Converter.GetLevelDO(level);
			    result = levelRepository.DeleteEntity(levelDO.ID);
			}
            catch (Exception ex)
            {
                Debug.WriteLine("Exception Occured in DeleteLevel method due to " + ex.Message);
			}
			return result;
		}	
	}
}