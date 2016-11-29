using System.Collections.Generic;

namespace Model.ServiceModel
{
   public class Sequence
    {
        public string name { get; set; }
		public List<Level> levels { get; set; }//changed Levels to levels
		public List<Option> options{ get; set; }// changed Options to options
    }
}