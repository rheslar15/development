using DAL.DO;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DAL.Utility
{
	public class Converter
	{
		public Converter()
		{
		}

		public static UserDO GetUserDO(User user)
		{
			UserDO userDO = new UserDO()
			{
				ID = user.ID,
				FirstName = user.FirstName,
				LastName = user.LastName,
				Token = user.Token,
				ExpiryDate=user.ExpiryDate
			};
			return userDO;
		}

		public static User GetUser(UserDO userDO)
		{
			User user = new User()
			{
				ID = userDO.ID,
				FirstName = userDO.FirstName,
				LastName = userDO.LastName,
				Token = userDO.Token,
				ExpiryDate=userDO.ExpiryDate
			};
			return user;
		}

		public static UserSettingDO GetUserSettingDO(UserSetting userSetting)
		{
			UserSettingDO userSettingDO = new UserSettingDO()
			{
				ID = userSetting.ID,
				SettingName = userSetting.SettingName,
				SettingValue = userSetting.SettingValue
			};
			return userSettingDO;
		}

		public static UserSetting GetUserSetting(UserSettingDO userSettingDO)
		{
			UserSetting userSetting = new UserSetting()
			{
				ID = userSettingDO.ID,
				SettingName = userSettingDO.SettingName,
				SettingValue = userSettingDO.SettingValue
			};
			return userSetting;
		}

		public static InspectionDO GetInspectionDO(Inspection inspection)
		{
			InspectionDO inspectionDO = new InspectionDO()
			{
				ID=inspection.ID,
				InspectionId = inspection.inspectionID,
				InspectionDesc=inspection.InspectionType
			};
			return inspectionDO;
		}

		public static Inspection GetInspection(InspectionDO inspectionDO)
		{
			Inspection inspection= new Inspection()
			{
				ID=inspectionDO.ID,
				inspectionID = inspectionDO.InspectionId,
				InspectionType=inspectionDO.InspectionDesc
			};
			return inspection;
		}

		public static SequencesDO GetSequenceDO(Sequence sequence)
		{
			SequencesDO sequenceDO = new SequencesDO()
			{
				ID = sequence.id,
				SequenceDesc = sequence.name
			};
			return sequenceDO;
		}

		public static Sequence GetSequence(SequencesDO sequenceDO)
		{
			Sequence sequence = new Sequence(sequenceDO.ID,sequenceDO.SequenceDesc,"");
			//            {
			//                id = sequenceDO.ID,
			//                name = sequenceDO.SequenceDesc
			//  };
			return sequence;
		}

		public static OptionsDO GetOptionDO(Option option)
		{
			OptionsDO optionsDO = new OptionsDO()
			{
				ID = option.ID,
				OptionsDesc = option.name,
				//IsGuidedPicture=option.isGuidedPicture
			};
			return optionsDO;
		}

		public static Option GetOption(OptionsDO optionsDO)
		{
			Option option = new Option()
			{
				ID = optionsDO.ID,
				name = optionsDO.OptionsDesc,
				//isGuidedPicture=optionsDO.IsGuidedPicture
			};
			return option;
		}

		public static SpaceDO GetSpaceDO(Space space)
		{
			SpaceDO spaceDO = new SpaceDO()
			{
				ID = space.id,
				SpaceDesc = space.name
			};
			return spaceDO;
		}

		public static Space GetSpace(SpaceDO spaceDO)
		{
			Space space = new Space (spaceDO.ID, spaceDO.SpaceDesc);
			//            {
			//                ID = spaceDO.ID,
			//                Name = spaceDO.SpaceDesc
			//            };
			return space;
		}
		public static LevelDO GetLevelDO(Level level)
		{
			LevelDO levelDO = new LevelDO()
			{
				ID = level.ID,
				LevelDesc = level.name
			};
			return levelDO;
		}

		public static Level GetLevel(LevelDO levelDO)
		{
			Level level = new Level()
			{
				ID = levelDO.ID,
				name= levelDO.LevelDesc
			};
			return level;
		}

		//Inspection Transaction DTO to Model Converter
		public static InspectionTransactionDO GetInspectionTransactionDO(Inspection inspection)
		{
			InspectionTransactionDO inspectionTransDO = new InspectionTransactionDO()
			{
				ID = inspection.ID,
				InspectionID = inspection.inspectionID,
				ProjectID = inspection.projectID,
				InspectionDT = inspection.inspectionDateTime,
				InspectionResult = inspection.pass,
				OwnerName = inspection.HouseOwnerName,
				ProjectName = inspection.ProjectName,
				PathwayTypeID = (int)inspection.Pathway,
				AddressLine1 = inspection.InspectionAddress1,
				AddressLine2 = inspection.InspectionAddress2,
				City = inspection.City,
				Pincode = inspection.Pincode,
				PhoneNo = inspection.PhoneNo,
				//Image=inspection.Image,
				IsFinalise = inspection.IsFinalise,
				HouseOwnerID = !string.IsNullOrEmpty(inspection.HouseOwnerID) ? int.Parse(inspection.HouseOwnerID) : 0,
				InspectionAttempt = !string.IsNullOrEmpty(inspection.InspectionAttemptCount) ? int.Parse(inspection.InspectionAttemptCount) : 0,
				ContractorName = inspection.ContractorName,
				IsInspectionSynced = inspection.isInspectionSynced,
				InspectionStarted = inspection.InspectionStarted
			};
			return inspectionTransDO;
		}


		public static Inspection GetInspectionTransaction(InspectionTransactionDO inspectionTransactionDO)
		{
			Inspection inspection = new Inspection()
			{
				ID = inspectionTransactionDO.ID,
				inspectionID = inspectionTransactionDO.InspectionID,
				projectID = inspectionTransactionDO.ProjectID,
				inspectionDateTime = inspectionTransactionDO.InspectionDT,
				pass = inspectionTransactionDO.InspectionResult,
				HouseOwnerName = inspectionTransactionDO.OwnerName,
				ProjectName = inspectionTransactionDO.ProjectName,
				Pathway = (PathwayType)inspectionTransactionDO.PathwayTypeID,
				InspectionAddress1 = inspectionTransactionDO.AddressLine1,
				InspectionAddress2 = inspectionTransactionDO.AddressLine2,
				City = inspectionTransactionDO.City,
				Pincode = inspectionTransactionDO.Pincode,
				PhoneNo = inspectionTransactionDO.PhoneNo,
				//Image=inspectionTransactionDO.Image,
				IsFinalise = inspectionTransactionDO.IsFinalise,
				HouseOwnerID = inspectionTransactionDO.HouseOwnerID != null ? inspectionTransactionDO.HouseOwnerID.ToString() : "",
				InspectionAttemptCount = inspectionTransactionDO.InspectionAttempt != null ? inspectionTransactionDO.InspectionAttempt.ToString() : "",
				ContractorName = inspectionTransactionDO.ContractorName,
				isInspectionSynced = inspectionTransactionDO.IsInspectionSynced,
				InspectionStarted = inspectionTransactionDO.InspectionStarted                                    
			};
			return inspection;
		}


		public static SpaceTransaction GetSpaceTransaction(SpaceTransactionDO spaceTransactionDO)
		{
			SpaceTransaction spaceTransaction = new SpaceTransaction () 
			{
				ID = spaceTransactionDO.ID,
				SpaceID = spaceTransactionDO.SpaceID,
				isSelected = spaceTransactionDO.isSelected,
				SeqID = spaceTransactionDO.SeqID,
				LevelID = spaceTransactionDO.LevelID,
				InspectionTransID = spaceTransactionDO.InspectionTransID
			};
			return spaceTransaction;
		}

		public static SpaceTransactionDO GetSpaceTransactionDO(SpaceTransaction spaceTransaction)
		{
			SpaceTransactionDO spaceTransactionDO = new SpaceTransactionDO () 
			{
				ID = spaceTransaction.ID,
				SpaceID = spaceTransaction.SpaceID,
				isSelected = spaceTransaction.isSelected,
				SeqID = spaceTransaction.SeqID,
				LevelID = spaceTransaction.LevelID,
				InspectionTransID = spaceTransaction.InspectionTransID

			};
			return spaceTransactionDO;
		}

		public static LevelTransaction GetLevelTransaction(LevelTransactionDO levelTransactionDO)
		{
			LevelTransaction levelTransaction = new LevelTransaction () 
			{
				ID = levelTransactionDO.ID,
				LevelID = levelTransactionDO.LevelID, 
				SeqID = levelTransactionDO.SeqID,
				isSelected = levelTransactionDO.isSelected,
				InspectionTransID = levelTransactionDO.InspectionTransID
			};

			return levelTransaction;
		}

		public static LevelTransactionDO GetLevelTransactionDO(LevelTransaction levelTransaction)
		{
			LevelTransactionDO levelTransactionDO = new LevelTransactionDO () 
			{
				ID = levelTransaction.ID,
				LevelID = levelTransaction.LevelID,
				SeqID = levelTransaction.SeqID,
				isSelected = levelTransaction.isSelected,
				InspectionTransID = levelTransaction.InspectionTransID                          
			};
			return levelTransactionDO;
		}

		public static OptionTransactionDO GetOptionTransactionDO(OptionTransaction optionTransaction)
		{
			OptionTransactionDO optionTransactionDO = new OptionTransactionDO()
			{
				ID=optionTransaction.ID,
				OptionId=optionTransaction.OptionId,
				//OptionDesc=optionTransaction.OptionDesc,
				InspectionTransID=optionTransaction.inspectionTransID,
				SpaceID=optionTransaction.SpaceID,
				SequenceID=optionTransaction.SequenceID,
				//ProjectID=optionTransaction.ProjectID,
				LevelID=optionTransaction.LevelID,
				isSelected = optionTransaction.isSelected
				//IsGuidedPicture=optionTransaction.IsGuidedPicture

					
			};
			return optionTransactionDO;
		}

		public static OptionTransaction GetOptionTransaction(OptionTransactionDO optionTransactionDO)
		{
			OptionTransaction optionTransaction = new OptionTransaction()
			{
				ID=optionTransactionDO.ID,
				OptionId= optionTransactionDO.OptionId,
				//OptionDesc = optionTransactionDO.OptionDesc,
				//Result = optionTransactionDO.Result,				
				//Comment = optionTransactionDO.comment,
				inspectionTransID=optionTransactionDO.InspectionTransID,
				//InspectionID = optionTransactionDO.InspectionID,
				SpaceID = optionTransactionDO.SpaceID,
				SequenceID = optionTransactionDO.SequenceID,
				//ProjectID = optionTransactionDO.ProjectID,
				LevelID=optionTransactionDO.LevelID,
				isSelected = optionTransactionDO.isSelected
				//IsGuidedPicture=optionTransactionDO.IsGuidedPicture
			};
			return optionTransaction;
		}
        //converter for reportDO
        public static ReportDO GetReportDO(Report report)
        {
            ReportDO reportDO = new ReportDO()
            {
              ID=report.ReportID,
			  InspectionTransID=report.InspectionTransID,
              ReportType=report.ReportType ,
              ReportDesc = report.ReportDesc
            };
            return reportDO;
        }

        public static Report GetReport(ReportDO reportDO)
        {
            Report report = new Report()
            {
                ReportID = reportDO.ID,
				InspectionTransID=reportDO.InspectionTransID,
                ReportType = reportDO.ReportType,
                ReportDesc=reportDO.ReportDesc
            };
            return report;
        }

        //PunchList converter
        public static PunchListDO GetPunchListDO(Punch punch)
        {
            PunchListDO punchListDO = new PunchListDO()
            {
              ID = punch.PunchID,
              InspectionID=punch.InspectionID,
              ProjectID=punch.ProjectID,
              PunchDesc=punch.punchDescription
            };
            return punchListDO;
        }

        public static Punch GetPunchList(PunchListDO punchListDO)
        {
            Punch punch = new Punch()
            {
               PunchID=punchListDO.ID,
               InspectionID=punchListDO.InspectionID,
               ProjectID=punchListDO.ProjectID,
               punchDescription=punchListDO.PunchDesc
            };
            return punch;
        }

		public static PunchListImageDO GetPunchListImageDO(PunchListImage punchListImage)
		{
			PunchListImageDO punchListImageDO = new PunchListImageDO()
			{
				InspectionTransID = punchListImage.inspectionTransID,
				PunchListImage = punchListImage.Image,
				PunchID = punchListImage.PunchID
			};
			return punchListImageDO;
		}

		public static PunchListImage GetPunchList(PunchListImageDO punchListImageDO)
		{
			PunchListImage punch = new PunchListImage()
			{
				inspectionTransID = punchListImageDO.ID,
				Image = punchListImageDO.PunchListImage,
				PunchID = punchListImageDO.InspectionTransID

			};
			return punch;
		}

        //ConfigurationConverter
        public static ConfigurationDO GetConfigurationDO(Configuration configuration)
        {
            ConfigurationDO configurationDO = new ConfigurationDO()
            {
               ID=configuration.ID,
               ConfigurationDesc=configuration.ConfigDesc,
               ConfigurationUrl=configuration.ConfigUrl
            };
            return configurationDO;
        }

        public static Configuration GetConfiguration(ConfigurationDO configurationDO)
        {
            Configuration configuration = new Configuration()
            {
               ID=configurationDO.ID,
               ConfigDesc=configurationDO.ConfigurationDesc,
               ConfigUrl=configurationDO.ConfigurationUrl 
            };
            return configuration;
        }

		public static CheckList GetCheckList(CheckListDO checkListDO)
		{
			CheckList checkList = new CheckList()
			{
				ID=checkListDO.ID,
				description=checkListDO.CheckListDesc
			};
			return checkList;
		}
		public static CheckListDO GetCheckListDO(CheckList checkList)
		{
			CheckListDO checkListDO = new CheckListDO()
			{
				ID=checkList.ID,
				CheckListDesc=checkList.description
			};
			return checkListDO;
		}

		public static CheckListTransactionDO GetCheckListTransactionDO(CheckListTransaction checkListTransaction)
		{
			CheckListTransactionDO checkListTransactionDO = new CheckListTransactionDO()
			{
				ID=checkListTransaction.ID,
				OptionTransID=checkListTransaction.OptionTransactionID,
				CheckListID=checkListTransaction.CheckListID,
				ResultTypeID=checkListTransaction.result,
				Comment=checkListTransaction.comments,
				PunchID=checkListTransaction.PunchID,
				ItemTypeID=(int)checkListTransaction.itemType
			};
			return checkListTransactionDO;
		}

		public static CheckListTransaction GetCheckListTransaction(CheckListTransactionDO checkListTransactionDO)
		{
			CheckListTransaction checkListTransaction = new CheckListTransaction()
			{
				
				ID=checkListTransactionDO.ID,
				OptionTransactionID=checkListTransactionDO.OptionTransID,
				CheckListID=checkListTransactionDO.CheckListID,
				result=checkListTransactionDO.ResultTypeID,
				comments=checkListTransactionDO.Comment,
				PunchID=checkListTransactionDO.PunchID,
				itemType=(ItemType)checkListTransactionDO.ItemTypeID
			};
			return checkListTransaction;
		}
		//OptionImage Converter

		public static OptionTransactionImageDO GetOptionTransactionImageDO(OptionImage optionImage)
		{
			OptionTransactionImageDO optionTransactionImageDO = new OptionTransactionImageDO()
			{
				ID=optionImage.ID,
				ItemImage=optionImage.Image,
				OptionTransactionID=optionImage.OptionTransID
			};
			return optionTransactionImageDO;
		}



		public static OptionImage GetOptionImage(OptionTransactionImageDO optionTransactionImageDO)
		{
			OptionImage optionImage = new OptionImage()
			{
				ID=optionTransactionImageDO.ID,
				Image=optionTransactionImageDO.ItemImage,
				OptionTransID=optionTransactionImageDO.OptionTransactionID
			};
			return optionImage;
		}

		public static DocumentDO GetDocumentDO(Document document)
		{
			DocumentDO documentDO = new DocumentDO()
			{
				ID = document.ID,
				InspectionID = document.inspectionID,
				DocumentTypeID = (int)document.inspectionDocumentType,
				DocumentName = document.documentDisplayName,
				DocumentDesc = document.documentArray,
				ServiceDocID = document.documentID,
				ProjectID = document.projectID,
				DocumentPath=document.DocumentPath
			};
			return documentDO;
		}

		public static Document GetDocument(DocumentDO documentDO)
		{
			Document document = new Document()
			{
				ID = documentDO.ID,
				inspectionID = documentDO.InspectionID,
				inspectionDocumentType = (DocumentType)documentDO.DocumentTypeID,
				documentDisplayName = documentDO.DocumentName,
				documentArray = documentDO.DocumentDesc,
				documentID = documentDO.ServiceDocID,
				projectID = documentDO.ProjectID,
				DocumentPath=documentDO.DocumentPath
			};
			return document;
		}  

		public static NotificationDO GetNotificationsDO(Notifications notification)
		{
			NotificationDO notificationDO = new NotificationDO () {
				ID = notification.notificationID,
				Notificationmessage = notification.notificationmessage,
				NotificationType = notification.notificationType,
				Count = notification.count,
				NotificationDate = notification.notificationDate,
				NotificationTypeID=notification.notificationTypeID,
			};
			return notificationDO;
		}

		public static Notifications GetNotifications(NotificationDO notificationDO)
		{
			Notifications notifications = new Notifications()
			{
				notificationID=notificationDO.ID,
				notificationmessage=notificationDO.Notificationmessage,
				notificationType=notificationDO.NotificationType,
				count=notificationDO.Count,
				notificationDate= notificationDO.NotificationDate,
				notificationTypeID= notificationDO.NotificationTypeID,
			};
			return notifications;
		}
		public static List<Notifications> GetNotificationList(List<NotificationDO> notificationDOs)
		{
			List<Notifications> notifications = new List<Notifications> ();
			foreach (var notificationDO in notificationDOs)
			{	
				Notifications notification = new Notifications () {
					notificationID = notificationDO.ID,
					notificationmessage = notificationDO.Notificationmessage,
					notificationType = notificationDO.NotificationType,
					count = notificationDO.Count,
					notificationDate = notificationDO.NotificationDate,
					notificationTypeID = notificationDO.NotificationTypeID,
				};
				notifications.Add (notification);
			}
			return notifications;
		}

        public static IEnumerable<PathwayDO> GetMasterPathwayDO(IEnumerable<Model.ServiceModel.MasterPathway> pathways)
        {
            List<PathwayDO> pathwayDO = new List<PathwayDO>();
            foreach (var pathway in pathways)
            {
                PathwayDO pathDO = new PathwayDO()
                {
                    ID = pathway.PathwayID,
                    PathwayDesc = pathway.PathwayDesc
                };
                pathwayDO.Add(pathDO);
            }
            return pathwayDO;
        }

        public static IEnumerable<InspectionDO> GetMasterInspectionDO(IEnumerable<Model.ServiceModel.MasterInspection> inspections)
        {
            List<InspectionDO> inspectionDO = new List<InspectionDO>();
            foreach (var ins in inspections)
            {
                InspectionDO insDO = new InspectionDO()
                {
                    ID = ins.ID,
                    InspectionId=ins.InspectionTypeId,
                    InspectionDesc = ins.InspectionDesc,
					Priority=ins.Priority
                };
                inspectionDO.Add(insDO);
            }
            return inspectionDO;
        }

        public static IEnumerable<SequencesDO> GetMasterSequenceDO(IEnumerable<Model.ServiceModel.MasterSequence> sequences)
        {
            List<SequencesDO> sequenceDO = new List<SequencesDO>();
            foreach (var seq in sequences)
            {
                SequencesDO seqDO = new SequencesDO()
                {
                    ID = seq.SequenceID,
                    SequenceDesc = seq.SequenceDesc,
					Priority=seq.Priority
                };
                sequenceDO.Add(seqDO);
            }
            return sequenceDO;
        }

        public static IEnumerable<LevelDO> GetMasterLevelDO(IEnumerable<Model.ServiceModel.MasterLevel> levels)
        {
            List<LevelDO> levelsDO = new List<LevelDO>();
            foreach (var level in levels)
            {
                LevelDO levelDO = new LevelDO()
                {
                    ID = level.LevelID,
                    LevelDesc = level.LevelDesc,
					Priority=level.Priority
                };
                levelsDO.Add(levelDO);
            }
            return levelsDO;
        }

        public static IEnumerable<SpaceDO> GetMasterSpaceDO(IEnumerable<Model.ServiceModel.MasterSpace> spaces)
        {
            List<SpaceDO> spaceDO = new List<SpaceDO>();
            foreach (var spc in spaces)
            {
                SpaceDO spcDO = new SpaceDO()
                {
                    ID = spc.SpaceID,
                    SpaceDesc = spc.SpaceDesc,
					Priority=spc.Priority
                };
                spaceDO.Add(spcDO);
            }
            return spaceDO;
        }

        public static IEnumerable<OptionsDO> GetMasterOptionsDO(IEnumerable<Model.ServiceModel.MasterOption> options)
        {
            List<OptionsDO> optionsDO = new List<OptionsDO>();
            foreach (var opt in options)
            {
                OptionsDO optDO = new OptionsDO()
                {
                    ID = opt.OptionsID,
                    OptionsDesc = opt.OptionsDesc,
					Priority=opt.Priority
                };
                optionsDO.Add(optDO);
            }
            return optionsDO;
        }

        public static IEnumerable<CheckListDO> GetMasterCheckListDO(IEnumerable<Model.ServiceModel.MasterCheckList> checkListItems)
        {
            List<CheckListDO> checkListDO = new List<CheckListDO>();
            foreach (var checkList in checkListItems)
            {
                CheckListDO chkDO = new CheckListDO()
                {
                    ID = checkList.CheckListID,
                    CheckListDesc = checkList.CheckListDesc
                };
                checkListDO.Add(chkDO);
            }
            return checkListDO;
        }

        public static IEnumerable<InspectionMappingDO> GetMasterInspectionMappingDO(IEnumerable<Model.ServiceModel.MasterInspectionMapping> inspectionMappingData)
        {
            List<InspectionMappingDO> inspectionMappingDO = new List<InspectionMappingDO>();
            foreach (var insMapping in inspectionMappingData)
            {
                InspectionMappingDO insMappingDO = new InspectionMappingDO()
                {
                    ID = insMapping.InspectionMappingID,
                    CheckListID = insMapping.CheckListID,
                    InspectionID=insMapping.InspectionID,
                    LevelID=insMapping.LevelID,
                    OptionID=insMapping.OptionID,
                    PathwayID=insMapping.PathwayID,
                    SequenceID=insMapping.SequenceID,
                    SpaceID=insMapping.SpaceID
                };
                inspectionMappingDO.Add(insMappingDO);
            }
            return inspectionMappingDO;
        }
	}
}