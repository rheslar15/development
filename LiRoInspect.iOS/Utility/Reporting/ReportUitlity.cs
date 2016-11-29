using System;
using iTextSharp;
using iTextSharp.text.pdf;
using System.IO;
using Model;

namespace LiRoInspect.iOS.Reporting
{
	public class ReportUtility
	{
		IReportHandler reportHandler;


		public string GenerateReport (string fileName, Inspection inspectionResult)
		{
			switch (inspectionResult.pass.ToLower()) {
			case "pass":
				reportHandler = ReportFactory.GetReportHandler (ReportType.Pass);
				break;
			case "fail":
				reportHandler = ReportFactory.GetReportHandler (ReportType.Fail);
				break;
			}

			var path=reportHandler.GenerateReport(fileName,inspectionResult);
			return path;
		}
	}
}

