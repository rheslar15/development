using System;

namespace LiRoInspect.iOS.Reporting
{
	public class ReportFactory
	{
		static IReportHandler reportHandler;
		public static IReportHandler GetReportHandler (ReportType reportType)
		{
			switch (reportType) {

			case ReportType.Pass:
				reportHandler =  new PassReportType1 ();
				break;
			case ReportType.Fail:
				reportHandler = new FailReport ();
				break;
			case ReportType.PhotoLog:
				reportHandler =  new PhotologReport ();
				break;
			case ReportType.TempPhotolog:
				reportHandler =  new TempPhotologReport ();
				break;
			default:
				break;
			}
			return reportHandler;
		}
	}
}