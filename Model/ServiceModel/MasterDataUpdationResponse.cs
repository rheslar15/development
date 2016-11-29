using System;
using System.Collections.Generic;
using Model.ServiceModel;

namespace Model.ServiceModel
{
	public class MasterDataUpdationResponse : IResult
	{
        public string DBVersion { get; set; }
		public IEnumerable<MasterPathway> pathway{get;set;}
		public IEnumerable<MasterInspection> inspection{get;set;}
		public IEnumerable<MasterSequence> sequence{get;set;}
		public IEnumerable<MasterLevel> level{get;set;}
		public IEnumerable<MasterSpace> space{get;set;}
		public IEnumerable<MasterOption> option{get;set;}
		public IEnumerable<MasterCheckList> checkList{get;set;}
		public IEnumerable<MasterInspectionMapping> inspectionMapping{get;set;}
		public Result result { get; set; }
	}
}