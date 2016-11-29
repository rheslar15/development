using System;
using DAL.Repository;
using DAL.DO;
using SQLite;
using Model;
using System.Collections.Generic;
using DAL.Utility;
using System.Linq;
using System.Diagnostics;

namespace BAL.Service
{
	public class ReportService:BaseService
    {

        IRepository<ReportDO> ReportRepository;
        public ReportService(SQLiteConnection conn)
		{
            ReportRepository = RepositoryFactory<ReportDO>.GetRepository(conn);
		}

        public List<Report> GetReports()
		{
            List<Report> report = new List<Report>();
			try
			{
	            IEnumerable<ReportDO> reportDOs = ReportRepository.GetEntities();
				if(reportDOs!=null)
				{
		            foreach (ReportDO rprtDo in reportDOs)
					{
		                report.Add(Converter.GetReport(ReportRepository.GetEntity(rprtDo.ID)));
					}
				}
			}
            catch (Exception ex)
            {
                Debug.WriteLine("Exception Occured in GetReports method due to " + ex.Message);
			}
            return report;
		}

        public Report GetReport(int id)
		{
            Report report = new Report();
			try
			{
	            ReportDO reportDO = ReportRepository.GetEntity(id);
	            if (reportDO != null)
	                report = Converter.GetReport(reportDO);
			}
            catch (Exception ex)
            {
                Debug.WriteLine("Exception Occured in GetReport method due to " + ex.Message);
			}
            return report;
		}

		public ReportDO GetReportOnInspectionTransactionID(SQLiteConnection conn,int id,string reportType)
		{
			ReportDO report = new ReportDO();
			try
			{
				report=ReportDO.getReports(conn,id,reportType).FirstOrDefault();
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Exception Occured in GetReport method due to " + ex.Message);
			}
			return report;
		}

        public int SaveReport(Report report)
		{
			int result=0;
			try
			{				
				var reports = GetReports ().Where (r => r.InspectionTransID==report.InspectionTransID && r.ReportType==report.ReportType);
				if (reports.Count () > 0) 
				{
					foreach (var rep in reports) 
					{
						report.ReportID = rep.ReportID;
						ReportDO reportDO = Converter.GetReportDO (report);
						result = ReportRepository.UpdateEntity (reportDO);
					}				
				} 
				else 
				{
					ReportDO reportDO = Converter.GetReportDO (report);
					result = ReportRepository.SaveEntity (reportDO);
				}
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception Occured in SaveReport method due to " + ex.Message);
			}
			return result;
		}

        public int DeleteReport(Report report)
		{
			int result = 0;
			try
			{
				ReportDO reportDO = Converter.GetReportDO(report);
	            result = ReportRepository.DeleteEntity(reportDO.ID);
			}
            catch (Exception ex)
            {
                Debug.WriteLine("Exception Occured in DeleteReport method due to " + ex.Message);
			}
			return result;
		}
    }
}