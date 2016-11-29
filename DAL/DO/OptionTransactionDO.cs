using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using System.Diagnostics;
namespace DAL.DO
{
    [Table("OptionsTrans")]
   public class OptionTransactionDO:IDomianObject
    {
        int _Id;
		[PrimaryKey,AutoIncrement]
        public int ID
        {
            get { return _Id; }
            set { _Id = value; }
        }

		int _OptionsId;
		[NotNull]
        public int OptionId
		{
			get { return _OptionsId; }
			set { _OptionsId = value; }
		}
      
		int _InspectionTransID;
        public int InspectionTransID
        {
			get { return _InspectionTransID; }
			set { _InspectionTransID = value; }

        }

        int _SequenceID;
        public int SequenceID
        {
            get { return _SequenceID; }
            set { _SequenceID = value; }
        }

        int? _SpaceID;
        public int? SpaceID
        {
            get { return _SpaceID; }
            set { _SpaceID = value; }
        }      

		int? _LevelID;
        public int? LevelID
		{
			get { return _LevelID; }
			set { _LevelID = value; }
		}

		public int ? _isSelected = 0;

		public int? isSelected 
		{
			get { return _isSelected; }
			set { _isSelected = value; }
		}

        public static List<OptionTransactionDO> getInsoectionOptionsforSeq(SQLiteConnection conn,string inspectionID,int seqID,int projectID)
		{
			List<OptionTransactionDO> optionTransactions = new List<OptionTransactionDO> ();
			try
            {
			    string query = "select * from OptionsTrans  where SequenceID  = " + seqID+ " and InspectionID="+inspectionID+" and ProjectID="+projectID;
			    optionTransactions = conn.Query<OptionTransactionDO>(query);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception Occured in getInsoectionOptionsforSeq method due to " + ex.Message);
			}
			return optionTransactions;
		}

		public static List<OptionTransactionDO> getInsoectionOptionsforsync(SQLiteConnection conn,int inspectionTransID)
		{
			List<OptionTransactionDO> optionTransactions = new List<OptionTransactionDO> ();
			try
			{
				string query = "select * from OptionsTrans  where InspectionTransID  = " + inspectionTransID;
				optionTransactions = conn.Query<OptionTransactionDO>(query);
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Exception Occured in getInsoectionOptionsforsync method due to " + ex.Message);
			}
			return optionTransactions;
		}

		public static int DeleteInspectionOptions(SQLiteConnection conn,int inspTransID)
		{			
			string query = "delete from OptionsTrans  where  InspectionTransID="+inspTransID;
			return conn.Execute(query);
		}
    }
}