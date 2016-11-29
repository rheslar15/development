using Model.ServiceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ServiceModel
{
	public class DocumentReq
	{
		public string token { get; set; }
        //PDF document ID -- Unique ID across projects
        public string documentID { get; set; }
        // added to differentiate inspection document
		public DocumentType documentType { get; set; }//changed DocumentType to documentType
	}
}