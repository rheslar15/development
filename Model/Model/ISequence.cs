using System;
using System.Collections.Generic;
using Model;

namespace Model
{
   	public interface ISequence : ITraversible,ISequenceOption
	{       
		//bool IsLevel ();
        ///List<Space> getSpaces ();
		//int getLevelID ();
		List<Level> Levels{get; set;}
	}  
}