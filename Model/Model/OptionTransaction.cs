using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class OptionTransaction
    {
        public int ID { get; set; }
		public int OptionId { get; set; }
        public string OptionDesc { get; set; }
		public int inspectionTransID{ get; set;}
        public int SequenceID { get; set; }
        public int? SpaceID { get; set; }
        public int? LevelID { get; set;}
		public List<OptionImage> photos { get; set;}
		public List<CheckListTransaction> checkListTransaction = new List<CheckListTransaction>();
		public int PunchID{ get; set;}
		public int ? isSelected { get; set;}
    }
}