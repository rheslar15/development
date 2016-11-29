using Model.ServiceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ServiceModel
{
	public class DocumentRes:IResult
	{
		public Result result { get; set; }
        //PDF document ID
        public string documentID { get; set; }        
        //PDF document
        public byte[] document { get; set; }
    }
}