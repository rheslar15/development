using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.DO;
using DAL.Helper;

namespace DAL.Repository
{
    public class RepositoryFactory<T>
    {
        public static IRepository<T> GetRepository(SQLiteConnection conn)
        {
            //SQLiteConnection conn = ConnectionHelper.GetConnection();
            IRepository<T> repository = null;
			Type repositoryType = typeof(T);

			if (repositoryType == typeof(InspectionTransactionDO))
			{
				repository = (IRepository<T>)new InspectionTransactionRepository(conn,string.Empty);
				return repository;
			}
            if (repositoryType == typeof(InspectionDO))
            {
                repository = (IRepository<T>)new InspectionDetailRepository(conn,string.Empty);
				return repository;
            }
            if (repositoryType == typeof(SequencesDO))
            {
                repository = (IRepository<T>)new SequenceRepository(conn, string.Empty);
				return repository;
            }
            if (repositoryType == typeof(SpaceDO))
            {
                repository = (IRepository<T>)new SpaceRepository(conn, string.Empty);
				return repository;
            }
            if (repositoryType == typeof(OptionsDO))
            {
                repository = (IRepository<T>)new OptionsRepository(conn, string.Empty);
				return repository;
            }
            if (repositoryType == typeof(LevelDO))
            {
                repository = (IRepository<T>)new LevelRepository(conn, string.Empty);
				return repository;
            }

			if (repositoryType == typeof(SpaceTransactionDO)) {
				repository = (IRepository<T>)new SpaceTransactionRepository(conn, string.Empty);
				return repository;
			}

			if (repositoryType == typeof(LevelTransactionDO)) {
				repository = (IRepository<T>)new LevelTransactionRepository(conn, string.Empty);
				return repository;
			}
          
			if (repositoryType == typeof(UserDO))
			{
				repository = (IRepository<T>)new UserRepository(conn, string.Empty);
				return repository;
			}

			if (repositoryType == typeof(OptionTransactionDO)) {
				repository = (IRepository<T>)new OptionTransactionRepository(conn, string.Empty);
				return repository;
			}

			if (repositoryType == typeof(ReportDO)) {
				repository = (IRepository<T>)new ReportRepository(conn, string.Empty);
				return repository;
			}				

			if (repositoryType == typeof(PunchListDO)){
				repository = (IRepository<T>)new PunchRepository(conn, string.Empty);
				return repository;
			}

			if (repositoryType == typeof(PunchListImageDO)){
				repository = (IRepository<T>)new PunchListImageRepository(conn, string.Empty);
				return repository;
			}

			if (repositoryType == typeof(CheckListDO)){
				repository = (IRepository<T>)new CheckListRepository(conn, string.Empty);
				return repository;
			}

			if (repositoryType == typeof(OptionTransactionImageDO)){
				repository = (IRepository<T>)new OptionTransactionImageRepository(conn, string.Empty);
				return repository;
			}
			if (repositoryType == typeof(DocumentDO)){
				repository = (IRepository<T>)new DocRepository(conn, string.Empty);
				return repository;
			}

			if (repositoryType == typeof(UserSettingDO)){
				repository = (IRepository<T>)new UserSettingRepository(conn, string.Empty);
				return repository;
			}

			if (repositoryType == typeof(NotificationDO)){
				repository = (IRepository<T>)new NotificationRepository(conn, string.Empty);
				return repository;
			}

			if (repositoryType == typeof(PathwayDO)){
				repository = (IRepository<T>)new PathwayRepository(conn, string.Empty);
				return repository;
			}
            return repository;
        }
    }
}