using Model.ServiceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Service
{
    public interface IServices
    {
        AuthenticationRes authenticate(AuthenticationReq req);
        InspectionsRes FetchInspections(InspectionsReq req);
        InspectionResultsAck PushInspections(InspectionResults req);
        InspectionReportAck PushReport(InspectionReport req);
        PunchListRes FetchPuchList(PunchListReq req);
        PunchListAck PushPunchList(PunchListResult req);
        DocumentRes FetchDocument(DocumentReq req);
        MasterDataUpdationResponse FetchMasterData(MasterDataUpdationRequest req);
    }
}