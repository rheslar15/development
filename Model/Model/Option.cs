using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Option : ServiceModel.Option,IOption
    {
        public int ID { get; set; }
		public string InspectionID{ get; set;}
		public int OptionTransactionID{ get; set;}
		public int SequenceID{ get; set;}
		public int? SpaceID{ get; set;}
		public int? LevelID{ get; set;}
        //public string name { get; set; }
		public ResultType Result { get;  set; }
        public string FailID { get;  set; }
        public string FailCategory { get; set; }
        public string InspectionStandard { get; set; }
		public List<byte[]> passImages = new List<byte[]> ();
		public bool isGuidedPicture{ get; set; }
        //public string comment { get;  set; }
        //public List<Photo> Photo { get;  set; }
		public int PunchID { get; set; }
		public new  List<OptionImage> photos { get; set; }
		public List<CheckListTransaction> checkList = new List<CheckListTransaction> ();
        public new List<CheckList> checkListItems { get; set; }
		public bool isSelected { get; set;}
		public bool isEnabled { get; set; }



        public List<Option> Options
        {
            get;
            set;
        }
//
//        public ServiceModel.Option getServiceModel()
//        {
//            ServiceModel.Option serviceOpt = this as ServiceModel.Option;
//            if(Result=="pass")
//            {
//                serviceOpt.result=true;
//            }
//            else if(Result=="fail")
//            {
//                serviceOpt.result=false;
//            }
//            else
//            {
//                serviceOpt.result=null;
//            }
//            int standardCode;
//            int.TryParse(FailID, out standardCode);
//            serviceOpt.standardCode=standardCode;
//            return serviceOpt;
//        }
        
        public string getName()
        {
            return this.name;
        }



        public bool prevSeqNextClicked { get; set;}

        public bool enableRow { get; set; }

        public int getSequenceID()
		{
			return SequenceID;
		}

        public List<CheckList> getCheckListItems()
        {
			return checkListItems;
        }
    }
   
}