using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DO
{
	[Table("InspectionMapping")]
    public class InspectionMappingDO : IDomianObject
    {
        int _InspectionMappingID;
        [PrimaryKey]
        [Column("InspectionMappingID")]
        public int ID
        {
            get { return _InspectionMappingID; }
            set { _InspectionMappingID = value; }
        }

        int _PathwayID;
        public int PathwayID
        {
            get { return _PathwayID; }
            set { _PathwayID = value; }
        }

        int _InspectionID;
        public int InspectionID
        {
            get { return _InspectionID; }
            set { _InspectionID = value; }
        }

        int _SequenceID;
        public int SequenceID
        {
            get { return _SequenceID; }
            set { _SequenceID = value; }
        }

        int _LevelID;
        public int LevelID
        {
            get { return _LevelID; }
            set { _LevelID = value; }
        }

        int _SpaceID;
        public int SpaceID
        {
            get { return _SpaceID; }
            set { _SpaceID = value; }
        }

        int _OptionID;
        public int OptionID
        {
            get { return _OptionID; }
            set { _OptionID = value; }
        }

        int _CheckListID;
        public int CheckListID
        {
            get { return _CheckListID; }
            set { _CheckListID = value; }
        }
    }
}