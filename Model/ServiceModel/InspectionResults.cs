using System.Collections.Generic;

namespace Model.ServiceModel
{
    public class InspectionResults
    {
        public List<InspectionResult> inspections { get; set; }
        public string token { get; set; }
    }
}