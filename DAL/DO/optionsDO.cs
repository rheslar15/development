using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using System.Diagnostics;

namespace DAL.DO
{
    [Table("options")]
    public class OptionsDO : IDomianObject
    {
        int _OptionsId;
        [PrimaryKey]
        [Column("OptionsID")]
        public int ID
        {
            get { return _OptionsId; }
            set { _OptionsId = value; }
        }

        string _OptionsDesc;
        public string OptionsDesc
        {
            get { return _OptionsDesc; }
            set { _OptionsDesc = value; }
        }

        int _priority;
        public int Priority
        {
            get { return _priority; }
            set { _priority = value; }
        }

		public static List<OptionsDO> GetPunchOptions(SQLiteConnection conn, int optPunchID)
		{
			List<OptionsDO> optPunch = new List<OptionsDO> ();
			try
			{
				string query = "select * from options where OptionsID=" + optPunchID;
				optPunch= conn.Query<OptionsDO>(query);
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Exception Occured in GetOptionPunch method due to " + ex.Message);
			}
			return optPunch;
		}
    }
}