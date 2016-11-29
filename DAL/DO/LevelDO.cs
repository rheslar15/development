using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DO
{
    [Table("Level")]
    public class LevelDO : IDomianObject
    {
        int _LevelId;
        [PrimaryKey]
        [Column("LevelID")]
        public int ID
        {
            get { return _LevelId; }
            set { _LevelId = value; }
        }

        string _LevelDesc;
        public string LevelDesc
        {
            get { return _LevelDesc; }
            set { _LevelDesc = value; }
        }

		int _Priority;
		public int Priority 
		{
			get { return _Priority; }
			set { _Priority = value; }
		}
    }
}