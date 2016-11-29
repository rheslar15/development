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

namespace BAL.Service
{
   class OptionsService
   {
        IRepository<OptionsDO> optionRepository;
        public OptionsService(SQLiteConnection conn)
        {
			optionRepository = RepositoryFactory<OptionsDO>.GetRepository(conn);           
        }

        public List<Option> GetOptions()
        {
            List<Option> options = new List<Option>();
			try
            {
                IEnumerable<OptionsDO> optionsDOs = optionRepository.GetEntities();
                foreach (OptionsDO optDo in optionsDOs)
                {
                    options.Add(Converter.GetOption(optionRepository.GetEntity(optDo.ID)));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception Occured in GetOptions method due to " + ex.Message);
			}
            return options;
        }

        public Option GetOption(int OptionID)
        {
            Option option = new Option();
			try
            {
                OptionsDO optionsDO = optionRepository.GetEntity(OptionID);
                if (optionsDO != null)
				    option = Converter.GetOption(optionsDO);
			    }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception Occured in GetOption method due to " + ex.Message);
			}
            return option;
        }

        public int SaveOptions(Option option)
        {
			int result = 0;
			try
            {
			    OptionsDO optionsDO = Converter.GetOptionDO(option);
                result = optionRepository.SaveEntity(optionsDO);
			}
            catch (Exception ex)
            {
                Debug.WriteLine("Exception Occured in SaveOptions method due to " + ex.Message);
			}
            return result;
        }

        public int DeleteOptions(Option option)
        {
			int result = 0;
			try
            {
			    OptionsDO optionsDO = Converter.GetOptionDO(option);
                 result = optionRepository.DeleteEntity(optionsDO.ID);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception Occured in DeleteOptions method due to " + ex.Message);
			}
            return result;
        }
    }
}