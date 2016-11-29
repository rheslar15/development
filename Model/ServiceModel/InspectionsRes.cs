using System.Collections.Generic;

namespace Model.ServiceModel
{
    public class InspectionsRes : IResult
    {
        public Result result { get; set; }
        public List<Inspection> inspections { get; set; }
    }
}