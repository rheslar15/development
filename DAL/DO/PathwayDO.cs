using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DO
{
    [Table("Pathway")]
	public class PathwayDO:IDomianObject
    {
        int _PathwayID;
        [PrimaryKey]
        [Column("PathwayID")]
        public int ID
        {
            get { return _PathwayID; }
            set { _PathwayID = value; }
        }

        string _PathwayDesc;
        public string PathwayDesc
        {
            get { return _PathwayDesc; }
            set { _PathwayDesc = value; }
        }


    }
}
