using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace DAL.DO
{
    [Table("Sequences")]
    public class SequencesDO:IDomianObject
    {
        int _SequenceID;
        [PrimaryKey]
        [Column("SequenceID")]
        public int ID
        {
            get { return _SequenceID; }
            set { _SequenceID = value; }
        }

        string _SequenceDesc;
        public string SequenceDesc
        {
            get { return _SequenceDesc; }
            set { _SequenceDesc = value; }
        }

        int _priority;
        public int Priority
        {
            get { return _priority; }
            set { _priority = value; }
        }

		public static List<SequencesDO> getPunchListSequenceForInspection(SQLiteConnection conn, int sequenceID)
		{
			string query = "select * from Sequences where  SequenceID=" + sequenceID;
			List<SequencesDO> punchSequence = conn.Query<SequencesDO>(query);
			return punchSequence;
		}
    }
}