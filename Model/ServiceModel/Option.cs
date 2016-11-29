
using System.Collections.Generic;
namespace Model.ServiceModel
{
    public class Option
    {
        public string name { get; set; }
        public List<CheckList> checkListItems { get; set; }
        public List<byte[]> photos { get; set; }
		public bool isGuidedPicture{ get; set; } ///changed IsGuidedPicture to isGuidedPicture
		public bool isSelected { get; set;}
		public int OptionId { get; set; }
    }
}