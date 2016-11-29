using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace DAL.DO
{
    [Table("Inspection")]
    public class InspectionDO:IDomianObject
    {
        int _Id;
        [PrimaryKey]
        public int ID 
        {
			get { return _Id; }
			set { _Id = value; }
        }

		string  _InspectionId;
        public string InspectionId 
		{
			get { return _InspectionId; }
			set { _InspectionId = value; }
		}

        string _InspectionDesc;
        public string InspectionDesc
        {
            get { return _InspectionDesc; }
            set { _InspectionDesc = value; }
        }

		int _Priority;
		public int Priority 
		{
			get { return _Priority; }
			set { _Priority = value; }
		}


    }
}