using System;

namespace Model.ServiceModel
{
	public class MasterInspectionMapping
	{		
		public int InspectionMappingID {get;set;}
		public int PathwayID {get; set;	}
        public int InspectionID { get; set; }
        public int SequenceID { get; set; }
        public int LevelID { get; set; }
        public int SpaceID { get; set; }
        public int OptionID { get; set; }
        public int CheckListID { get; set; }
	}
}