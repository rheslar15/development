using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ServiceModel
{
    public class InspectionDocument
    {
        public string documentID { get; set; }
        public string documentDisplayName { get; set; }
        public DocumentType inspectionDocumentType { get; set; }
    }
}