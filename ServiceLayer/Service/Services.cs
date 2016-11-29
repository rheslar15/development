using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.ServiceModel;
using ServiceLayer.Service;
using System.Diagnostics;

namespace LiroInspectServiceModel.Services
{
    public class Services : IServices
    {
        public AuthenticationRes authenticate(AuthenticationReq req)
        {
            Uri uri = new Uri(ServiceURL.UrlAuth);
            return request<AuthenticationReq, AuthenticationRes>.executePost(req, uri);
        }
        public InspectionsRes FetchInspections(InspectionsReq req)
        {
            Uri uri = new Uri(ServiceURL.UrlGetInspection + "?id=" + req.token);
			return request<InspectionsReq, InspectionsRes>.executeGet(req, uri);
        }
        public InspectionResultsAck PushInspections(InspectionResults req)
        {
			Uri uri = new Uri(ServiceURL.UrlInspectionResults);
			return request<InspectionResults, InspectionResultsAck>.executePost(req, uri);
        }
		public InspectionReportAck PushReport(InspectionReport req)
        {
			Uri uri = new Uri(ServiceURL.UrlINspectionReport);
			return request<InspectionReport, InspectionReportAck>.executePost(req, uri);
		}

        public PunchListRes FetchPuchList(PunchListReq req)
        {
			Uri uri =  new Uri(ServiceURL.UrlGetPunchList + "?id=" + req.token);
            return request<PunchListReq, PunchListRes>.executePost(req, uri);
        }

        public PunchListAck PushPunchList(PunchListResult req)
        {
            Uri uri = new Uri(ServiceURL.UrlPunchListResults);
            return request<PunchListResult, PunchListAck>.executePost(req, uri);
        }

		public DocumentRes FetchDocument(DocumentReq req)
		{
			Uri uri = new Uri(ServiceURL.UrlGetInspectionDocuments);
			return request<DocumentReq, DocumentRes>.executePost(req, uri);
		}

		int count=0;
		MasterDataUpdationRequest masterDataUpdationRequest;
        public MasterDataUpdationResponse FetchMasterData(MasterDataUpdationRequest req)
        {
			try{
			count++;	
			masterDataUpdationRequest=req;
			Uri uri = new Uri(ServiceURL.UrlMasterDataUpdate + "?id=" + req.token);
            var res= request<MasterDataUpdationRequest, MasterDataUpdationResponse>.executeGet(req, uri);

			if(res==null || (res !=null && res.result != null && (res.result.code<0 )))
			{
				Debug.WriteLine("MasterDataUpdationResponse retry" + res.result.code);
				if (count <= 3) {
					this.FetchMasterData (masterDataUpdationRequest);
				}
				else
				{
					return  (new MasterDataUpdationResponse (){ result = new Result () {
							code = 3,
							type = "2",
							message = "Fetch Master DB Update Failed"
						} });
				}
			}
			return res;
			}
			catch(Exception ex) {
				if (count <= 3) {
					this.FetchMasterData (masterDataUpdationRequest);
				}
				return  (new MasterDataUpdationResponse (){ result = new Result () {
						code = 3,
						type = "2",
						message = "Fetch Master DB Update Failed"
					} });
			}
        }
    }
}