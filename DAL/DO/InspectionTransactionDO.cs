using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using System.Diagnostics;

namespace DAL.DO
{
	[Table("InspectionTrans")]
	public class InspectionTransactionDO : IDomianObject
	{
		int _ID;
		[PrimaryKey, AutoIncrement]
		public int ID
		{
			get { return _ID; }
			set { _ID = value; }
		}

		string _InspectionID;
		[Column("InspectionID")]
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

		DateTime _InspectionDT;
		public DateTime InspectionDT
		{
			get { return _InspectionDT; }
			set { _InspectionDT = value; }

		}

		string _InspectionResult;
		public string InspectionResult
		{
			get { return _InspectionResult; }
			set { _InspectionResult = value; }
		}

		string _OwnerName;
		public string OwnerName
		{
			get { return _OwnerName; }
			set { _OwnerName = value; }
		}

		string _ProjectName;
		public string ProjectName
		{
			get { return _ProjectName; }
			set { _ProjectName = value; }
		}

		int _PathwayTypeID;
		public int PathwayTypeID
		{
			get { return _PathwayTypeID; }
			set { _PathwayTypeID = value; }
		}

		string _AddressLine1;
		public string AddressLine1
		{
			get { return _AddressLine1; }
			set { _AddressLine1 = value; }
		}

		string _AddressLine2;
		public string AddressLine2
		{
			get { return _AddressLine2; }
			set { _AddressLine2 = value; }
		}

		string _City;
		public string City
		{
			get { return _City; }
			set { _City = value; }
		}

		string _Pincode;
		public string Pincode
		{
			get { return _Pincode; }
			set { _Pincode = value; }
		}

		string _PhoneNo;
		public string PhoneNo
		{
			get { return _PhoneNo; }
			set { _PhoneNo = value; }
		}

		int _isFinalise;
		public int IsFinalise
		{
			get { return _isFinalise; }
			set { _isFinalise = value; }
		}

		int _inspectionAttempt;
		public int InspectionAttempt
		{
			get { return _inspectionAttempt; }
			set { _inspectionAttempt = value; }
		}

		int _houseOwnerID;
		public int HouseOwnerID
		{
			get { return _houseOwnerID; }
			set { _houseOwnerID = value; }
		}

		string _contractorName;
		public string ContractorName
		{
			get { return _contractorName; }
			set { _contractorName = value; }
		}

		bool _IsInspectionSynced;
		public bool IsInspectionSynced
		{
			get { return _IsInspectionSynced; }
			set { _IsInspectionSynced = value; }
		}

		int ? _InspectionStarted;
		public int? InspectionStarted
		{
			get { return _InspectionStarted; }
			set { _InspectionStarted = value; }
		}

		public static List<InspectionTransactionDO> getInspectionProjectID(SQLiteConnection conn, int ID)
		{
			List<InspectionTransactionDO> inspectionTrans = new List<InspectionTransactionDO>();
			string query = "select * from InspectionTrans where ID=" + ID;
			inspectionTrans = conn.Query<InspectionTransactionDO>(query);
			return inspectionTrans;
		}

		public static int getInspectionTransID(SQLiteConnection conn, string projectID, string InspectionID)
		{
			int inspectionTransID = 0;
			try
			{
				string query = "select ID from InspectionTrans where InspectionID='" + InspectionID + "' and ProjectID='" + projectID + "';";
				inspectionTransID = conn.ExecuteScalar<int>(query);
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Exception occured getInspectionTransID method due to" + ex.Message);
			}
			return inspectionTransID;
		}
	}
}