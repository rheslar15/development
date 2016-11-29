using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace DAL.DO
{
   [Table("Space")]
   public  class SpaceDO : IDomianObject
    {
        int _SpaceId;
        [PrimaryKey]
        [Column("SpaceID")]
        public int ID
        {
            get { return _SpaceId; }
            set { _SpaceId = value; }
        }

        string _SpaceDesc;
        public string SpaceDesc
        {
            get { return _SpaceDesc; }
            set { _SpaceDesc = value; }
        }

		int _Priority;
		public int Priority 
		{
			get { return _Priority; }
			set { _Priority = value; }
		}
    }
}