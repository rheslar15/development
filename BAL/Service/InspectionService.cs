using DAL.Repository;
using DAL.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Utility;
using SQLite;
using Model;
using System.Diagnostics;

namespace BAL.Service
{
    public class InspectionService
    {
        IRepository<InspectionDO> inspectionRepository;

        public InspectionService(SQLiteConnection conn)
        {
			inspectionRepository = RepositoryFactory<InspectionDO>.GetRepository(conn);            
        }

        public List<Inspection> GetInspections()
        {
            List<Inspection> inspections = new List<Inspection>() ;
			try
			{
	            IEnumerable<InspectionDO> inspectionsDOs = inspectionRepository.GetEntities();

	            foreach (InspectionDO inspDo in inspectionsDOs)
	            {

	                inspections.Add(Converter.GetInspection(inspectionRepository.GetEntity(inspDo.ID)));
	            }
			}
			catch(Exception ex)
            {
                Debug.WriteLine("Exception Occured in GetInspections method due to " + ex.Message);
			}
            return inspections;
        }

        public Inspection GetInspection(int inspectionID)
        {
            Inspection inspection = new Inspection();
			try
			{
	            InspectionDO inspectionDO = inspectionRepository.GetEntity(inspectionID);
	            if (inspectionDO != null)
					inspection = Converter.GetInspection(inspectionDO);
			}
            catch (Exception ex)
            {
                Debug.WriteLine("Exception Occured in GetInspection method due to " + ex.Message);
			}
            return inspection;
        }

        public int SaveInspection(Inspection inspection)
        {
			int result = 0;
			try
			{
	            InspectionDO inspectionDO = Converter.GetInspectionDO(inspection);
	              result = inspectionRepository.SaveEntity(inspectionDO);
			}
            catch (Exception ex)
            {
                Debug.WriteLine("Exception Occured in SaveInspection method due to " + ex.Message);
			}
            return result;
        }

        public int DeleteInspection(Inspection inspection)
        {
			int result = 0;
			try
			{
	            InspectionDO inspectionDO = Converter.GetInspectionDO(inspection);
	             result = inspectionRepository.DeleteEntity(inspectionDO.ID);
			}
            catch (Exception ex)
            {
                Debug.WriteLine("Exception Occured in DeleteInspection method due to " + ex.Message);
			}
            return result;
        }
    }
}