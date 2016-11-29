using System;
using DAL.Repository;
using DAL.DO;
using DAL;
using SQLite;
using Model;
using System.Collections.Generic;
using DAL.Utility;
using System.Linq;
using System.Diagnostics;
using ServiceLayer.Service;
using Model.ServiceModel;
using LiroInspectServiceModel.Services;

namespace BAL.Service
{
	
	public class DocViewService:BaseService
	{
		IRepository<DocumentDO> docRepository;
		SQLiteConnection conn;
		public Model.ServiceModel.DocumentRes ServiceResonse{ get; set;}
		public byte[] pdfToByteData{ get; set;}

		/// <summary>
		/// Initializes a new instance of the <see cref="BAL.Service.DocViewService"/> class.
		/// </summary>
		/// <param name="conn">Conn.</param>
		public DocViewService(SQLiteConnection conn)
		{
			docRepository = RepositoryFactory<DocumentDO>.GetRepository(conn);
			this.conn = conn;
		}

		/// <summary>
		/// Gets the document.
		/// </summary>
		/// <returns>The document.</returns>
		/// <param name="documentID">Document I.</param>
		/// <param name="token">Token.</param>
		public Document GetDocument(int documentID, string token)
		{
			Document document = new Document();
			try
			{
				DocumentDO documentDo = docRepository.GetEntity(documentID);

				if (documentDo != null)
					document = Converter.GetDocument(documentDo);

				if(document.DocumentPath == null)
				{
					ServiceResonse = new Model.ServiceModel.DocumentRes ();
					IServices service = new Services ();
					ServiceResonse = service.FetchDocument (new Model.ServiceModel.DocumentReq (){ 
						documentID = document.documentID, 
						documentType = document.inspectionDocumentType, token = token });

	
					if(ServiceResonse!=null && ServiceResonse.result.code==0)
					{
						document.documentArray=ServiceResonse.document;
						document.documentID=ServiceResonse.documentID;
					}

				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Exception Occured in GetDocument method due to " + ex.Message);
			}
			return document;
		}

		/// <summary>
		/// Gets the list of document items.
		/// </summary>
		/// <returns>The document items.</returns>
		/// <param name="inspectionID">Inspection I.</param>
		/// <param name="projectID">Project I.</param>
		public List<Document> GetDocumentItems(string inspectionID, string projectID)
		{
			List<Document> documentList = new List<Document> ();
			try
			{
				IEnumerable<DocumentDO> DocumentDos = docRepository.GetEntities().Where(p=>p.InspectionID==inspectionID && p.ProjectID==projectID);
				foreach (DocumentDO documentDo in DocumentDos)
				{
					documentList.Add(Converter.GetDocument(documentDo));
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Exception Occured in GetDocumentItems method due to " + ex.Message);
			}
			return documentList;
		}

		public Document GetLastItem()
		{
			Document document = new Document ();
			try
			{
				DocumentDO DocumentDo = docRepository.GetEntities().LastOrDefault();
				if(DocumentDo!=null)
				{
				document=Converter.GetDocument(DocumentDo);
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Exception Occured in GetLastItem method due to " + ex.Message);
			}
			return document;
		}

		public int SaveDocumentItems(Document document)
		{
			int result = 0;
			try
			{
				DocumentDO doocumentDO = Converter.GetDocumentDO(document);
				result = docRepository.SaveEntity(doocumentDO);
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Exception Occured in SaveDocumentItems method due to " + ex.Message);
			}
			return result;
		}

		public int UpdateDocumentItems(Document document)
		{
			int result = 0;
			try
			{
				DocumentDO doocumentDO = Converter.GetDocumentDO(document);
				result = docRepository.UpdateEntity(doocumentDO);
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Exception Occured in SaveDocumentItems method due to " + ex.Message);
			}
			return result;
		}

		public int DeleteDocumentItems(Document document)
		{
			int result = 0;
			try
			{
				DocumentDO documentDO = Converter.GetDocumentDO(document);
				result = docRepository.DeleteEntity(documentDO.ID);
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Exception Occured in DeleteInspection method due to " + ex.Message);
			}
			return result;
		}

		public int DeleteDocumentItemsForSync(string InspectionID, string projectID )
		{
			int result = 0;
			try
			{
				result = DocumentDO.DeleteDocument(conn,InspectionID,projectID);
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Exception Occured in DeleteInspection method due to " + ex.Message);
			}
			return result;
		}

		public List<Document> GetDocumentsForSync(string InspectionID, string projectID )
		{
			List<Document> documents=new List<Document>();
			try
			{
				documents = DocumentDO.getDocumentsList(conn,InspectionID,projectID);
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Exception Occured in DeleteInspection method due to " + ex.Message);
			}
			return documents;
		}
	}
}