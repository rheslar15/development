using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
	public  class Level:ServiceModel.Level,ILevel
    {
        public int ID { get;  set; }
		public int seqID{ get; set; }
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

		public bool enableRow { get;  set; }

		private List<Option> options = new List<Option>();
		public new List<Option> Options//hiding base options
		{
			get
			{
				return options;
			}
			set
			{
				options = value;
			}
		}


		#region ILevel implementation

		public List<Space> getSpaces ()
		{
			return this.Spaces;
		}

		#endregion

		#region ITraversible implementation

		public string getName ()
		{
			return this.name;
		}

		public int getLevelID ()
		{
			return ID;
		}

		public int getSequenceID ()
		{
			return seqID;
		}

		public bool prevSeqNextClicked {
			get ;
			set ;
		}
		public bool isSelected { get; set; }

		#endregion
    }
}
