using System;
using System.Collections.Generic;

namespace Model.ServiceModel
{
    public class InspectionResult:BaseModel
    {
        public string appID { get; set; }//Changed projectID to appID
        public string inspectionTypeID { get; set; }//changed inspectionID to inspectionTypeID
        public DateTime inspectionDateTime { get; set; }
        public string pass { get; set; }
        public List<byte[]> locationPhotos { get; set; }// changed photos to locationPhotos
        public List<byte[]> signaturePhotos { get; set; }//newly added for signature photos
        public List<Sequence> sequences { get; set; }
    }   
    
}