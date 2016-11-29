using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Service
{
    public static class  ServiceURL
    {
		public static string UrlAuth = "http://liro1.cdp-inc.com/api/Login/AuthenticationReq";
		public static string UrlGetInspection = "http://liro1.cdp-inc.com/api/Inspection";
		public static string UrlInspectionResults = "http://liro1.cdp-inc.com/api/Inspection/InspectionResultSubmit";
		public static string UrlINspectionReport = "http://liro1.cdp-inc.com/api/Inspection/InspectionReportSubmit";
		public static string UrlGetPunchList = "http://liro1.cdp-inc.com/api/PunchList";
		public static string UrlPunchListResults = "http://liro1.cdp-inc.com/api/PunchList/PunchListSubmit";
		public static string UrlGetInspectionDocuments ="http://liro1.cdp-inc.com/api/Inspection/InspectionDocumentReq";
		public static string UrlMasterDataUpdate = "http://liro1.cdp-inc.com/api/Inspection/MasterDataUpdationReq";

    }
}