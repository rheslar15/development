using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.ServiceModel;
using Model;
using System.IO;

namespace ServiceLayer.Service
{
    public class MockServices : IServices
    {
        public AuthenticationRes authenticate(AuthenticationReq req)
        {
            return new AuthenticationRes()
            {
                firstName = "Robert",
                lastName = "Heslar",
                token = "uiwe90221921-daksds",
                expiryTime = DateTime.Now.AddHours(10),
				result= new Model.ServiceModel.Result(){code=0,message="OK"},
                DBVersion="1.0"
            };            
        }

		public byte[] getNewByteArray()
		{
			byte[] data = CreateSpecialByteArray (7);
			return data;
		}

		public static byte[] CreateSpecialByteArray(int length)
		{
			var arr = new byte[length];
			for (int i = 0; i < arr.Length; i++)
			{
				arr[i] = 0x20;
			}
			return arr;
		}

		public DocumentRes FetchDocument(DocumentReq req)
		{
			DocumentRes res = new DocumentRes ();
			res.result = new Model.ServiceModel.Result () { code = 0, message = "OK" };
			res.documentID = "22";
			res.document = getNewByteArray();
			return res;
		}

        public InspectionsRes FetchInspections(InspectionsReq req)
        {
			List<Model.ServiceModel.Inspection> ins = new List<Model.ServiceModel.Inspection>();
			ins.Add(new Model.ServiceModel.Inspection()
				{
					inspectionTypeID = "6",
					inspectionDate = DateTime.Today.Date.AddDays(4),
					projectName = "Liro BIB Project - Brooklyn",
					appID = "APP5547",
					pathway = PathwayType.Rehabilitation,
					activityAdress = new Address() { addrline1 = "Lincoln Bolevourd", addrline2 = "", city = "Brooklyn", state = "New York", postalCode = "11791" },
					//ProjectNo = "bib 1234",
					phoneNo = "555-1212",
					info=new Model.InspectionInfo(){contractorName ="Acharya, Tiyasi",homeOwnerID=129,inspectionAttempt=6},
					inspectionDocuments = GetDocuments()
				});

			ins.Add(new Model.ServiceModel.Inspection()
				{
					inspectionTypeID = "7",
					inspectionDate = DateTime.Today.Date.AddDays(-2),
					projectName = "Liro BIB Project - Brooklyn",
					appID = "APP333",
					pathway = PathwayType.Rehabilitation,
					activityAdress = new Address() { addrline1 = "Lincoln Bolevourd", addrline2 = "", city = "Brooklyn", state = "New York", postalCode = "11791" },
					//ProjectNo = "bib 256",
					phoneNo = "12345678909",
					info=new Model.InspectionInfo(){contractorName ="Devi, Aarti",homeOwnerID=125,inspectionAttempt=2},
					inspectionDocuments = GetDocuments()
				});

			ins.Add(new Model.ServiceModel.Inspection()
				{
					inspectionTypeID = "3",
					inspectionDate = DateTime.Today.Date.AddDays(4),
					projectName = "Liro BIB Project - Brooklyn",
					appID = "APP55454",
					pathway = PathwayType.Rehabilitation,
					activityAdress = new Address() { addrline1 = "Lincoln Bolevourd", addrline2 = "", city = "Brooklyn", state = "New York", postalCode = "11791" },
					//ProjectNo = "bib",
					phoneNo = "555-1212",
					info=new Model.InspectionInfo(){contractorName ="Narayanana, Vishnunavaneeth",homeOwnerID=128,inspectionAttempt=8},
					inspectionDocuments = GetDocuments()
				});
			ins.Add(new Model.ServiceModel.Inspection() 
            { 
				inspectionTypeID="2",
                inspectionDate=DateTime.Today,
                projectName ="Liro BIB Project - Brooklyn",
					appID ="APP5678",
					pathway=PathwayType.Elevation,
                activityAdress= new Address(){addrline1="3 aerial way",addrline2="",city="Syosset",state="New York",postalCode="11791"},
					//ProjectNo="bib",
				phoneNo="1112224444",
				info=new Model.InspectionInfo(){contractorName ="Steve Bolevourd",homeOwnerID=123,inspectionAttempt=4},
                inspectionDocuments=GetDocuments()
            });

			ins.Add(new Model.ServiceModel.Inspection() 
				{ 
					inspectionTypeID="2",
					inspectionDate=DateTime.Today,
					projectName ="Liro BIB Project - Brooklyn",
					appID="APP123",
					pathway=PathwayType.Elevation,
					activityAdress= new Address(){addrline1="3 aerial way",addrline2="",city="Syosset",state="New York",postalCode="11791"},
					//ProjectNo="bib",
					phoneNo="1112224444",
					info=new Model.InspectionInfo(){contractorName ="Steve Bolevourd",homeOwnerID=123,inspectionAttempt=4},
					inspectionDocuments=GetDocuments()
				});
          
		
			
			ins.Add(new Model.ServiceModel.Inspection()
				{
					inspectionTypeID = "4",
					inspectionDate = DateTime.Today.Date.AddDays(4),
					projectName = "Liro BIB Project - Brooklyn",
					appID = "APP55478",
					pathway = PathwayType.Elevation,
					activityAdress = new Address() { addrline1 = "Lincoln Bolevourd", addrline2 = "", city = "Brooklyn", state = "New York", postalCode = "11791" },
					//ProjectNo = "bib",
					phoneNo = "555-1212",
					info=new Model.InspectionInfo(){contractorName ="Acharya, Tiyasi",homeOwnerID=129,inspectionAttempt=6},
					inspectionDocuments = GetDocuments()
				});

			ins.Add(new Model.ServiceModel.Inspection()
            {
				inspectionTypeID = "5",
                inspectionDate = DateTime.Today.AddDays(1),
                projectName = "Kevin's Home - Brooklyn",
					appID = "APP626",
				pathway = PathwayType.Elevation,
                activityAdress = new Address() { addrline1 = "Steve Bolevourd", addrline2 = "", city = "Brooklyn", state = "New York", postalCode = "11791" },
					//ProjectNo = "bib",
				phoneNo = "7779998790",
					info=new Model.InspectionInfo(){contractorName ="Shrivastava , Ajeet",homeOwnerID=124,inspectionAttempt=1},
                inspectionDocuments = GetDocuments()
            });			

			ins.Add(new Model.ServiceModel.Inspection()
				{
					inspectionTypeID = "6",
					inspectionDate = DateTime.Today.Date.AddDays(5),
					projectName = "Liro BIB Project - Brooklyn",
					appID = "APP5541",
					pathway = PathwayType.Elevation,
					activityAdress = new Address() { addrline1 = "Lincoln Bolevourd", addrline2 = "", city = "Brooklyn", state = "New York", postalCode = "11791" },
					//ProjectNo = "bib",
					phoneNo = "555-1212",
					info=new Model.InspectionInfo(){contractorName ="Tiwari, Pranav",homeOwnerID=126,inspectionAttempt=3},
                    inspectionDocuments = GetDocuments()
				});

			ins.Add(new Model.ServiceModel.Inspection()
				{
					inspectionTypeID = "7",
					inspectionDate = DateTime.Today.Date.AddDays(-2),
					projectName = "Liro BIB Project - Brooklyn",
					appID = "APP3331",
					pathway = PathwayType.Elevation,
					activityAdress = new Address() { addrline1 = "Lincoln Bolevourd", addrline2 = "", city = "Brooklyn", state = "New York", postalCode = "11791" },
					//ProjectNo = "bib",
					phoneNo = "12345678909",
					info=new Model.InspectionInfo(){contractorName ="Devi, Aarti",homeOwnerID=125,inspectionAttempt=2},
					inspectionDocuments = GetDocuments()
				});

			ins.Add(new Model.ServiceModel.Inspection()
				{
					inspectionTypeID = "1",
					inspectionDate = DateTime.Today.Date.AddDays(4),
					projectName = "Liro BIB Project - Brooklyn",
					appID = "APP3332",
					pathway = PathwayType.Reconstruction,
					activityAdress = new Address() { addrline1 = "Lincoln Bolevourd", addrline2 = "", city = "Brooklyn", state = "New York", postalCode = "11791" },
					//ProjectNo = "bib",
					phoneNo = "555-1212",
					info=new Model.InspectionInfo(){contractorName ="Narayanana, Vishnunavaneeth",homeOwnerID=128,inspectionAttempt=8},
					inspectionDocuments = GetDocuments()
				});
			ins.Add(new Model.ServiceModel.Inspection()
				{
					inspectionTypeID = "4",
					inspectionDate = DateTime.Today.Date.AddDays(4),
					projectName = "Liro BIB Project - Brooklyn",
					appID = "APP3333",
					pathway = PathwayType.Reconstruction,
					activityAdress = new Address() { addrline1 = "Lincoln Bolevourd", addrline2 = "", city = "Brooklyn", state = "New York", postalCode = "11791" },
					//ProjectNo = "bib",
					phoneNo = "555-1212",
					info=new Model.InspectionInfo(){contractorName ="Acharya, Tiyasi",homeOwnerID=129,inspectionAttempt=6},
					inspectionDocuments = GetDocuments()
				});
			ins.Add(new Model.ServiceModel.Inspection()
				{
					inspectionTypeID = "5",
					inspectionDate = DateTime.Today.Date.AddDays(4),
					projectName = "Liro BIB Project - Brooklyn",
					appID = "APP5542",
					pathway = PathwayType.Reconstruction,
					activityAdress = new Address() { addrline1 = "Lincoln Bolevourd", addrline2 = "", city = "Brooklyn", state = "New York", postalCode = "11791" },
					//ProjectNo = "bib",
					phoneNo = "555-1212",
					info=new Model.InspectionInfo(){contractorName ="Mishra, Mansi",homeOwnerID=127,inspectionAttempt=6},
					inspectionDocuments = GetDocuments()
				});
			ins.Add(new Model.ServiceModel.Inspection()
				{
					inspectionTypeID = "6",
					inspectionDate = DateTime.Today.Date.AddDays(4),
					projectName = "Liro BIB Project - Brooklyn",
					appID = "APP5542",
					pathway = PathwayType.Reconstruction,
					activityAdress = new Address() { addrline1 = "Lincoln Bolevourd", addrline2 = "", city = "Brooklyn", state = "New York", postalCode = "11791" },
					//ProjectNo = "bib",
					phoneNo = "555-1212",
					info=new Model.InspectionInfo(){contractorName ="Mishra, Mansi",homeOwnerID=127,inspectionAttempt=6},
					inspectionDocuments = GetDocuments()
				});
			ins.Add(new Model.ServiceModel.Inspection()
				{
					inspectionTypeID = "7",
					inspectionDate = DateTime.Today.Date.AddDays(4),
					projectName = "Liro BIB Project - Brooklyn",
					appID = "APP5542",
					pathway = PathwayType.Reconstruction,
					activityAdress = new Address() { addrline1 = "Lincoln Bolevourd", addrline2 = "", city = "Brooklyn", state = "New York", postalCode = "11791" },
					//ProjectNo = "bib",
					phoneNo = "555-1212",
					info=new Model.InspectionInfo(){contractorName ="Mishra, Mansi",homeOwnerID=127,inspectionAttempt=6},
					inspectionDocuments = GetDocuments()
				});
			
            return new InspectionsRes()
            {
                inspections = ins,
				result=new Model.ServiceModel.Result(){code=0,message="OK"}
            };
        }

        public List<InspectionDocument> GetDocuments()
        {
            List<InspectionDocument> insDocuments = new List<InspectionDocument>();
            insDocuments.Add(new InspectionDocument() { documentDisplayName = "LiRo_InspectionDocuments", documentID = "4", inspectionDocumentType = DocumentType.GCRepo});
            insDocuments.Add(new InspectionDocument() { documentDisplayName = "LiRo_InspectionDocuments", documentID = "4", inspectionDocumentType = DocumentType.General });
            return insDocuments;
        }

        public InspectionResultsAck PushInspections(InspectionResults req)
        {
			return new InspectionResultsAck(){result= new Model.ServiceModel.Result(){code=0,message="OK"}};
        }

		public InspectionReportAck PushReport(InspectionReport req)
        {
			return new InspectionReportAck(){result= new Model.ServiceModel.Result(){code=0,message="OK"}};
        }

        public PunchListRes FetchPuchList(PunchListReq req)
		{
			List<string> commentLst=new List<string> (){ "Ceiling is not proper", "Foundation is not strong" };
            List<ProjectPunchList> punchItemList = new List<ProjectPunchList>();
            List<PunchListInspection> punchListinspection = new List<PunchListInspection>();
			punchListinspection.Add(new PunchListInspection() {comments=commentLst,commentType=CommentType.NonConformanceItem,inspectionTypeId=3 });
			//punchItemList.Add(new ProjectPunchList() { CDCProjectId = "APP333",punchListItems=punchListinspection });
			//punchItemList.Add(new ProjectPunchList() { CDCProjectId = "APP55478",punchListItems=punchListinspection });
			//punchItemList.Add(new ProjectPunchList() { CDCProjectId = "APP3333",punchListItems=punchListinspection });
            return new PunchListRes()
            {
                result = new Model.ServiceModel.Result() { code = 0, message = "OK" },
				projectPunchLists = punchItemList					
            };
        }

        public PunchListAck PushPunchList(PunchListResult req)
        {
            return new PunchListAck() { result = new Model.ServiceModel.Result() { code = 0, message = "OK" } }; 
        }

        public MasterDataUpdationResponse FetchMasterData(MasterDataUpdationRequest req)
        {
            MasterDataUpdationResponse masterData = new MasterDataUpdationResponse();
            masterData.checkList = GetCheckLists();
            masterData.inspection = GetInspections();
            masterData.inspectionMapping = GetInspectionMapping();
            masterData.level = GetLevels();
            masterData.option = GetOptions();
            masterData.pathway = GetPathways();
            masterData.sequence = GetSequences();
            masterData.space = GetSpaces();
            masterData.result = new Model.ServiceModel.Result() { code = 0, message = "OK" };
			masterData.DBVersion="2.0";
            return masterData;
        }

        public IEnumerable<MasterCheckList> GetCheckLists()
        {
            List<MasterCheckList> checkListItems = new List<MasterCheckList>();
            checkListItems.Add(new MasterCheckList() { CheckListID = 1, CheckListDesc = "Are the plans approved by NYCDoB and required permit in place?" });
            checkListItems.Add(new MasterCheckList() { CheckListID = 2, CheckListDesc = "Does the work being done outside the street line conform to the applicable government authority specifications?" });
            checkListItems.Add(new MasterCheckList() { CheckListID = 3, CheckListDesc = "Are the existing roadway, sidewalk or curb areas free from any damages caused by demolition?" });
            checkListItems.Add(new MasterCheckList() { CheckListID = 4, CheckListDesc = "Is all material and debris being placed or stored on site and not in the street or on the sidewalk?" });
            checkListItems.Add(new MasterCheckList() { CheckListID = 5, CheckListDesc = "If asbestos containing material was present in the dwelling, have the proper DEP forms been completed and provided by the contractor?" });
            checkListItems.Add(new MasterCheckList() { CheckListID = 6, CheckListDesc = "Has all existing work that interferes with new construction been removed and/or relocated?" });
            checkListItems.Add(new MasterCheckList() { CheckListID = 7, CheckListDesc = "For all existing work that was removed because it interfered with new work, has it been returned or replaced?" });
            checkListItems.Add(new MasterCheckList() { CheckListID = 8, CheckListDesc = "Are all materials, assemblies and methods of construction in accordance with the drawings, specifications and good practices?" });
            checkListItems.Add(new MasterCheckList() { CheckListID = 9, CheckListDesc = "Are there no apparent deviations from the drawings and specifications?" });
            checkListItems.Add(new MasterCheckList() { CheckListID = 10, CheckListDesc = "Have erosion control and storm water protection measures been put in place where necessary?" });
            checkListItems.Add(new MasterCheckList() { CheckListID = 11, CheckListDesc = "Has all excavated excess soil been disposed of off-site?" });
            return checkListItems;
        }

        public IEnumerable<MasterOption> GetOptions()
        {
            List<MasterOption> options = new List<MasterOption>();
			options.Add(new MasterOption() { OptionsID = 1, OptionsDesc = "Demolition",Priority=1 });
			options.Add(new MasterOption() { OptionsID = 2, OptionsDesc = "Foundation",Priority=2 });
			options.Add(new MasterOption() { OptionsID = 3, OptionsDesc = "Roof",Priority=3 });
			options.Add(new MasterOption() { OptionsID = 4, OptionsDesc = "Windows/Doors",Priority=4 });
			options.Add(new MasterOption() { OptionsID = 5, OptionsDesc = "Miscellaneous",Priority=5 });
			options.Add(new MasterOption() { OptionsID = 6, OptionsDesc = "Site Safety",Priority=6 });
			options.Add(new MasterOption() { OptionsID = 7, OptionsDesc = "Environmental",Priority=7 });
			options.Add(new MasterOption() { OptionsID = 8, OptionsDesc = "Housekeeping",Priority=8 });
			options.Add(new MasterOption() { OptionsID = 9, OptionsDesc = "Drainage",Priority=9 });
			options.Add(new MasterOption() { OptionsID = 10, OptionsDesc = "Carpentry",Priority=10 });
			options.Add(new MasterOption() { OptionsID = 11, OptionsDesc = "Insulation",Priority=11 });
            return options;
        }

        public IEnumerable<MasterSpace> GetSpaces()
        {
            List<MasterSpace> spaces = new List<MasterSpace>();
			spaces.Add(new MasterSpace() { SpaceID = 1, SpaceDesc = "Site",Priority=1 });
			spaces.Add(new MasterSpace() { SpaceID = 2, SpaceDesc = "Garage",Priority=2 });
			spaces.Add(new MasterSpace() { SpaceID = 3, SpaceDesc = "Patio/Deck",Priority=3 });
			spaces.Add(new MasterSpace() { SpaceID = 4, SpaceDesc = "House" ,Priority=4});
			spaces.Add(new MasterSpace() { SpaceID = 5, SpaceDesc = "Foyer",Priority=5 });
			spaces.Add(new MasterSpace() { SpaceID = 6, SpaceDesc = "Living Room",Priority=6 });
			spaces.Add(new MasterSpace() { SpaceID = 7, SpaceDesc = "Dining Room",Priority=7 });
			spaces.Add(new MasterSpace() { SpaceID = 8, SpaceDesc = "Hallway",Priority=8 });
			spaces.Add(new MasterSpace() { SpaceID = 9, SpaceDesc = "Hall Closet",Priority=9 });
			spaces.Add(new MasterSpace() { SpaceID = 10, SpaceDesc = "Kitchen",Priority=10 });
			spaces.Add(new MasterSpace() { SpaceID = 11, SpaceDesc = "Pantry",Priority=11 });
            return spaces;
        }

        public IEnumerable<MasterLevel> GetLevels()
        {
            List<MasterLevel> levels = new List<MasterLevel>();
			levels.Add(new MasterLevel() { LevelID = 1, LevelDesc = "Exterior" ,Priority=1});
			levels.Add(new MasterLevel() { LevelID = 2, LevelDesc = "Basement",Priority=2 });
			levels.Add(new MasterLevel() { LevelID = 3, LevelDesc = "Attic",Priority=3 });
			levels.Add(new MasterLevel() { LevelID = 4, LevelDesc = "1st Floor",Priority=4 });
			levels.Add(new MasterLevel() { LevelID = 5, LevelDesc = "2nd Floor" ,Priority=5});
			levels.Add(new MasterLevel() { LevelID = 6, LevelDesc = "3rd Floor" ,Priority=6});
			levels.Add(new MasterLevel() { LevelID = 7, LevelDesc = "Exterior Elevation" ,Priority=7});
			levels.Add(new MasterLevel() { LevelID = 8, LevelDesc = "Laundry Room" ,Priority=8});
			levels.Add(new MasterLevel() { LevelID = 9, LevelDesc = "Mechanical Room",Priority=9 });
			levels.Add(new MasterLevel() { LevelID = 10, LevelDesc = "Interior",Priority=10 });
			levels.Add(new MasterLevel() { LevelID = 11, LevelDesc = "Garage" ,Priority=11});
            return levels;
        }

        public IEnumerable<MasterSequence> GetSequences()
        {
            List<MasterSequence> sequences = new List<MasterSequence>();
			sequences.Add(new MasterSequence() { SequenceID = 1, SequenceDesc = "Exterior" ,Priority=1});
			sequences.Add(new MasterSequence() { SequenceID = 2, SequenceDesc = "Safety",Priority=2 });
			sequences.Add(new MasterSequence() { SequenceID = 3, SequenceDesc = "Mechanical, Electrical, Plumbing",Priority=3 });
			sequences.Add(new MasterSequence() { SequenceID = 4, SequenceDesc = "Interior",Priority=4 });
			sequences.Add(new MasterSequence() { SequenceID = 5, SequenceDesc = "Safety",Priority=5 });
			sequences.Add(new MasterSequence() { SequenceID = 6, SequenceDesc = "Mechanical, Electrical, Plumbing",Priority=6 });
			sequences.Add(new MasterSequence() { SequenceID = 7, SequenceDesc = "Interior",Priority=7 });
			sequences.Add(new MasterSequence() { SequenceID = 8, SequenceDesc = "Punch List Review",Priority=8 });
			sequences.Add(new MasterSequence() { SequenceID = 9, SequenceDesc = "General",Priority=9 });
			sequences.Add(new MasterSequence() { SequenceID = 10, SequenceDesc = "Mechanical, Electrical, Plumbing" ,Priority=10});
			sequences.Add(new MasterSequence() { SequenceID = 11, SequenceDesc = "Safety" ,Priority=11});
            return sequences;
        }

        public IEnumerable<MasterInspection> GetInspections()
        {
            List<MasterInspection> inspections = new List<MasterInspection>();
			inspections.Add(new MasterInspection() { ID = 11, InspectionDesc = "25% Demolition Inspection", InspectionTypeId = "1",Priority=1 });
			inspections.Add(new MasterInspection() { ID = 12, InspectionDesc = "25% Separation and Lift ", InspectionTypeId = "2",Priority=2 });
			inspections.Add(new MasterInspection() { ID = 13, InspectionDesc = "50% Interim Progress", InspectionTypeId = "3",Priority=3 });
			inspections.Add(new MasterInspection() { ID = 14, InspectionDesc = "50% Foundation", InspectionTypeId = "4",Priority=4 });
			inspections.Add(new MasterInspection() { ID = 15, InspectionDesc = "75% Interim Progress", InspectionTypeId = "5",Priority=5 });
			inspections.Add(new MasterInspection() { ID = 16, InspectionDesc = "90% Substantial Completion", InspectionTypeId = "6" ,Priority=6});
			inspections.Add(new MasterInspection() { ID = 17, InspectionDesc = "100% Final", InspectionTypeId = "7",Priority=7 });
            return inspections;
        }

        public IEnumerable<MasterPathway> GetPathways()
        {
            List<MasterPathway> pathways = new List<MasterPathway>();
            pathways.Add(new MasterPathway() { PathwayID = 1, PathwayDesc = "Elevation" });
            pathways.Add(new MasterPathway() { PathwayID = 2, PathwayDesc = "Rehabilitation (Rehab)" });
            pathways.Add(new MasterPathway() { PathwayID = 3, PathwayDesc = "Reconstruction (Recon)" });
            return pathways;
        }

        public IEnumerable<MasterInspectionMapping> GetInspectionMapping()
        {
            List<MasterInspectionMapping> insMapping = new List<MasterInspectionMapping>();
            insMapping.Add(new MasterInspectionMapping() {CheckListID=1,InspectionID=1,InspectionMappingID=1,LevelID=1,OptionID=1,PathwayID = 1,SequenceID=1,SpaceID=1});
            insMapping.Add(new MasterInspectionMapping() { CheckListID = 2, InspectionID = 1, InspectionMappingID = 2, LevelID = 1, OptionID = 1, PathwayID = 1, SequenceID = 1, SpaceID = 1 });
            insMapping.Add(new MasterInspectionMapping() { CheckListID = 3, InspectionID = 1, InspectionMappingID = 3, LevelID = 1, OptionID = 1, PathwayID = 1, SequenceID = 1, SpaceID = 1 });
            insMapping.Add(new MasterInspectionMapping() { CheckListID = 4, InspectionID = 1, InspectionMappingID = 4, LevelID = 1, OptionID = 1, PathwayID = 1, SequenceID = 1, SpaceID = 1 });
            return insMapping;
        }
    }
}