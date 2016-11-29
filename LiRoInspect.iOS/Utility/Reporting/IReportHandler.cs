using System;
using System.IO;
using Model;

namespace LiRoInspect.iOS.Reporting
{
	public interface IReportHandler
	{
		string GenerateReport (string fileName,Inspection inspectionResult);
		//string LoadReport(string fileName);
		//void DeleteReport(string fileName);
	}
}