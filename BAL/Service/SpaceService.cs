using DAL.Repository;
using DAL.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Utility;
using Model;
using SQLite;
using System.Diagnostics;

namespace BAL.Service
{
    class SpaceService
  {
        IRepository<SpaceDO> spaceRepository;
		public SpaceService(SQLiteConnection conn)
        {
			spaceRepository = RepositoryFactory<SpaceDO>.GetRepository(conn);            
        }

        public List<Space> GetSpaces()
        {
            List<Space> space = new List<Space>();
			try
            {
                IEnumerable<SpaceDO> spaceDos = spaceRepository.GetEntities();
                foreach (SpaceDO spacDo in spaceDos)
                {
				    space.Add(Converter.GetSpace(spaceRepository.GetEntity(spacDo.ID)));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception Occured in GetSpaces method due to " + ex.Message);
			}
            return space;
        }

        public Space GetSpace(int SpaceID)
        {
            Space space = new Space(-1,"");//
			try
            {
                SpaceDO spaceDO = spaceRepository.GetEntity(SpaceID);
                if (spaceDO != null)
				    space = Converter.GetSpace(spaceDO);
			}
            catch (Exception ex)
            {
                Debug.WriteLine("Exception Occured in GetSpace method due to " + ex.Message);
			}
            return space;
        }

        public int SaveSpaces(Space space)
        {
			int result = 0;
			try
            {
                SpaceDO spaceDO = Converter.GetSpaceDO(space);
                result = spaceRepository.SaveEntity(spaceDO);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception Occured in SaveSpaces method due to " + ex.Message);
			}
            return result;
        }

        public int DeleteSpaces(Space space)
        {
			int result = 0;
			try
            {
			    SpaceDO spaceDO = Converter.GetSpaceDO(space);
                 result = spaceRepository.DeleteEntity(spaceDO.ID);
			}
            catch (Exception ex)
            {
                Debug.WriteLine("Exception Occured in DeleteSpaces method due to " + ex.Message);
			}
            return result;
        }
    }
}