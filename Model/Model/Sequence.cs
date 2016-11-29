using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
	public class Sequence : ServiceModel.Sequence,ISequence
	{
		public int id { get; set; }
        public int LevelID { get; set; }
        //public string name { get; private set; }
        public string spaceInfo { get; private set; }//ex: select the Interior Basement items to be displayed
        private List<Space> spaces = new List<Space>();

        public new List<Space> Spaces//hiding base spaces
        {
            get
            {
                return spaces;
            }
            set
            {
                spaces = value;
            }
        }
		//int rowIndex=-1;
		public Sequence(int id, string name, string spaceInfo)
		{
			this.id = id;
			this.name = name;
			this.spaceInfo = spaceInfo;
		}
		public Sequence()
		{
		}
		public List<Space> getSelectedSpaces()
		{
			return Spaces.Where(s => s.isSelected).ToList();
		}
		public void selectSpaceByID(int ID)
		{

			Space Space=  Spaces.Where(s => s.id == ID && !s.isDefault).Single();
			if (Space!=null)
			{
				Space.isSelected = true;
			}
		}
		public void unselectSpaceByID(int ID)
		{
			if (Spaces.Count > 1)
			{
				Space Space=  Spaces.Where(s => s.id == ID && !s.isDefault).Single();
				if (Space!=null)
				{
					Space.isSelected = false;
				}
			}
		}
		public List<Option> getAllOptions()
		{
			List<Option> optionList = new List<Option> ();
			List<Option> resultOptionList = Spaces.SelectMany (s => s.options).ToList (); //should only return text,id??
			if (resultOptionList != null) { 
				optionList = resultOptionList;
			}
			return optionList;
				
		}
		public List<Option> getOptionsForSpace(int ID)
		{
			return Spaces.Where(s => s.id == ID).Single().options;
		}


		#region ISequence implementation
		public string getName ()
		{
			return this.name;
		}

		public bool IsLevel()
		{
			if(Spaces.Count>1 )
			{
				return true;
			}
			return false;
		}
//		public List<Option> getOPtions ()
//		{
//
//			List<Option> optionList = new List<Option> ();
//			List<Option> resultOptionList = Spaces.SelectMany (s => s.options).ToList (); //should only return text,id??
//			if (resultOptionList != null) { 
//				optionList = resultOptionList;
//			}
//			return optionList;
//		}
		public List<Space> getSpaces ()
		{
			return this.Spaces;
		}
		public bool enableRow { get;  set; }

		public bool prevSeqNextClicked{ get;  set; }

		public bool isDynamicSeq { get;  set; }

		public int getSequenceID ()
		{
			return id;
		}

		public int getLevelID ()
		{
			return LevelID;
		}

		public List<Level> Levels {
			get;
			set;
		}

		public List<Option> Options {
			get ;
			set;
		}
	}
	#endregion
}