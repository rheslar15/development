using System;
using Model.ServiceModel;

namespace Model.ServiceModel
{
    public class MasterDataCheckResponse : IResult
	{
		public DBUpdateType updateType{get;set;}
		public string DBVersion{get;set;}
		public Result result { get; set; } 
	}
}