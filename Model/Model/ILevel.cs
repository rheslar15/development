using System;
using System.Collections.Generic;

namespace Model
{
	public interface ILevel : ITraversible,ISequenceOption
	{
		List<Space> getSpaces ();
		int getLevelID ();
	}
}