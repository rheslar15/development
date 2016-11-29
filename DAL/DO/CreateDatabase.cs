using DAL.Helper;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DO
{
   public  class CreateDatabase
    {

        public bool CreateDataBaseAsync()
        {
            try
            {
                SQLiteConnection conn = ConnectionHelper.GetConnection();
				if(conn.TableMappings.Count()==0)
				{
					CreateTables(conn);
					InsertValues(conn);
				}
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


		private void CreateTables(SQLiteConnection conn)
		{
			if (conn != null) {
				conn.CreateTable<InspectionDO> ();
				conn.CreateTable<LevelDO> ();
				conn.CreateTable<OptionsDO> ();
				conn.CreateTable<SequencesDO> ();
				conn.CreateTable<UserDO> ();
				conn.CreateTable<SpaceDO> ();
			}
		}

        public async Task<bool> RunDatabaseQuery(SQLiteConnection conn)
        {
            try
            {
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void InsertValues(SQLiteConnection conn)
        {
			InsertInspection(conn);
			InsertUser (conn);
        }

        private void InsertInspection(SQLiteConnection conn)
        {
            int tableRowCount = conn.Table<InspectionDO>().Count();
            if (tableRowCount == 0)
            {                
                InspectionDO inspection = new InspectionDO()
                {
                    InspectionDesc = "25% seperation & Lift"
                };
                conn.Insert(inspection);
                InspectionDO inspection1 = new InspectionDO()
                {
                    InspectionDesc = "25% Demolition Inspection"
                };
                conn.Insert(inspection1);
                InspectionDO inspection2 = new InspectionDO()
                {
                    InspectionDesc = "50% Interim Progress Inspection"
                };
                conn.Insert(inspection2);
                InspectionDO inspection3 = new InspectionDO()
                {
                    InspectionDesc = "75% Interim Progress Inspection"
                };
                conn.Insert(inspection3);
                InspectionDO inspection4 = new InspectionDO()
                {
                    InspectionDesc = "90% Substantial Completion"
                };
                conn.Insert(inspection4);
                InspectionDO inspection5 = new InspectionDO()
                {
                    InspectionDesc = "Final Inspection"
                };
                conn.Insert(inspection5);
            }
        }

		private void InsertUser(SQLiteConnection conn)
		{
			UserDO user1 = new UserDO()
			{
			};
			conn.Insert(user1);
		}
    }
}
