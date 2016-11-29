using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
	public class Space:ServiceModel.Space, ISpace
	{
		public int id { get; set; }
		//public string name { get;  set; }
		public new List<Option> options { get; set; }//hiding base class property
		public bool isSelected { get; set; }
		public bool isDefault { get; set; }
		public int seqID{ get; set; }
		public int levelID{ get; set; }
		public int SpaceID { get; set; }
		//int rowIndex=-1;
		public Space(int id, string name)
		{
			this.id = id;
			this.name = name;

		}

		public Space()
		{

		}
		#region ISequence implementation
		public string getName ()
		{
			return name;
		}
        //public int getCount ()
        //{
        //    return 1;
        //}
        //public void setRowIndex (int rowIndex)
        //{
        //    this.rowIndex = rowIndex;
        //}
        //public ISequence getSelectedRow (int rowIndex)
        //{
        //    throw new NotImplementedException ();
        //}
		public List<Option> getOPtions ()
		{
			return this.options;
		}
		public List<Space> getSpaces ()
		{
			List<Space> spaces = new List<Space>();
			spaces.Add(this);
			return spaces ;
		}

		public bool enableRow { get;  set; }
		public bool prevSeqNextClicked{ get;  set; }

        //public bool isSpace {
        //    get {
        //        return true;
        //    }
        //}
		public bool isDynamicSeq { get;  set; }

		public int getSequenceID ()
		{
			return seqID;
		}
		public int getLevelID ()
		{
			return levelID;
		}

		public bool IsEnabled { get; set; }

		public List<Option> Options {
			get;
			set;
		}
		#endregion
	}
}

