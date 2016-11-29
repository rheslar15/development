using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using Model;
using DAL.Utility;
using System.Diagnostics;


namespace DAL.DO
{
	[Table("Document")]
	public class DocumentDO:IDomianObject
	{
		public DocumentDO ()
		{
		}

		int _DocumentID;
		[PrimaryKey, AutoIncrement]
		[Column("DocumentID")]
		public int ID
		{
			get { return _DocumentID; }
			set { _DocumentID = value; }
		}

		int _DocumentTypeID;
		public int DocumentTypeID
		{
			get { return _DocumentTypeID; }
			set { _DocumentTypeID = value; }
		}

		string _DocumentName;
		public string DocumentName
		{
			get { return _DocumentName; }
			set { _DocumentName = value; }
		}

		byte[] _DocumentDesc;
		public byte[] DocumentDesc
		{
			get { return _DocumentDesc; }
			set { _DocumentDesc = value; }
		}

		string _ServiceDocID;
		public string ServiceDocID
		{
			get { return _ServiceDocID; }
			set { _ServiceDocID = value; }
		}

		string _InspectionID;
		public string InspectionID
		{
			get { return _InspectionID; }
			set { _InspectionID = value; }
		}

		string _ProjectID;
		public string ProjectID
		{
			get { return _ProjectID; }
			set { _ProjectID = value; }
		}

		string _DocumentPath;
		public string DocumentPath
		{
			get { return _DocumentPath; }
			set { _DocumentPath = value; }
		}

		public static List<Document> getDocumentsList(SQLiteConnection conn, string InspectionID, string ProjectID)
		{
			List<Document> DocumentList = new List<Document>();
			try
			{
				//inspectionTransactionId, serviceDocID
				string query = "SELECT * FROM Document WHERE InspectionID='" + InspectionID + "' AND ProjectID='" + ProjectID + "'";
				List<DocumentDO> docDosList = conn.Query<DocumentDO>(query);
				foreach (var Abc in docDosList)
				{
					DocumentList.Add(Converter.GetDocument(Abc));
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Exception Occured in getPunchList method due to " + ex.Message);
			}
			return DocumentList;
		}

		public static int DeleteDocument(SQLiteConnection conn, string InspectionID, string ProjectID)
		{
			string query = "DELETE FROM Document WHERE InspectionID='" + InspectionID + "' AND ProjectID='" + ProjectID + "'";
			return conn.Execute(query);
		}
	}
}