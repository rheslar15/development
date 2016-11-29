using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.ServiceModel;
using System.Diagnostics;


namespace Model
{
	public class Inspection : ServiceModel.InspectionResult
	{
		public int ID { get; set; }

		public string inspectionID { get; set; }
		public string InspectionType { get; set; }
		public string InspectionAddress1 { get; set; }
		public string InspectionAddress2 { get; set; }
		//public string pass { get; set; }
		public string Pincode { get; set; }
		public string City { get; set; }
		public byte[] Image { get; set; }
		public List<byte[]> locationIDImages = new List<byte[]>();
		//public DateTime inspectionDateTime { get; set; }
		public string ProjectName { get; set; }
		public string projectID { get; set; }
		public PathwayType Pathway { get; set; }
		//Address activityAdress { get; set; }
		public string HouseOwnerName { get; set; }
		public string HouseOwnerID { get; set; }
		public string PhoneNo { get; set; }
		public string RepresentativeName { get; set; }
		public string ContractorName { get; set; }
		public string InspectionAttemptCount { get; set; }
		public string ContractNo { get; set; }
		private List<Sequence> _sequences = new List<Sequence>();
		public int IsFinalise { get; set; }
		public List<Document> inspectionDocuments { get; set; }
		public bool isInspectionSynced { get; set; }
		public int ? InspectionStarted { get; set; }
		public new List<Sequence> sequences
		{
			get
			{
				return _sequences;
			}
			set
			{
				_sequences = value;
			}
		}

		public Dictionary<string, string> GetProjectDetail()
		{
			Dictionary<string, string> projectDetail = new Dictionary<string, string>();
			//projectDetail.Add ("Home Owner ID:", HouseOwnerID);
			projectDetail.Add("App ID:", projectID);
			projectDetail.Add("Pathway Type:", Pathway.ToString());
			//projectDetail.Add ("Applicant Number:", ContractNo);
			projectDetail.Add("Homeowner Name:", HouseOwnerName);
			projectDetail.Add("Type Of Inspection:", InspectionType);
			projectDetail.Add("Inspection Date:", inspectionDateTime.ToString());
			projectDetail.Add("Inspection Attempt:", InspectionAttemptCount);
			projectDetail.Add("Activity Address:", (InspectionAddress1 + "," + InspectionAddress2 + " " + "" + City + " " + Pincode).Trim());
			projectDetail.Add("Contractor Name:", ContractorName);

			return projectDetail;
		}
		public ServiceModel.InspectionResult getServiceModel()
		{
			List<byte[]> images = new List<byte[]>();
			images.Add(this.Image);
			ServiceModel.InspectionResult serviceInspection = new ServiceModel.InspectionResult()
			{
				appID = this.projectID,
				inspectionTypeID = this.inspectionID,
				inspectionDateTime = this.inspectionDateTime,
				pass = this.pass,
				locationPhotos = locationIDImages,
			};

			try
			{
				serviceInspection.sequences = new List<ServiceModel.Sequence>();
				foreach (var seq in this.sequences)
				{
					var serviceSequence = new ServiceModel.Sequence()
					{
						name = seq.name,
					};

					if (seq.Levels != null && seq.Levels.Count > 0)
					{
						serviceSequence.levels = new List<global::Model.ServiceModel.Level>();

						foreach (var level in seq.Levels)
						{
							var serviceLevel = new ServiceModel.Level()
							{
								name = level.name
							};

							if (level.Spaces != null && level.Spaces.Count > 0)
							{
								serviceLevel.spaces = new List<global::Model.ServiceModel.Space>();
								foreach (var space in level.Spaces)
								{
									var serviceSpace = new ServiceModel.Space()
									{
										name = space.name
									};

									if (space.Options != null)
									{
										serviceSpace.options = new List<ServiceModel.Option>();

										foreach (var option in space.Options)
										{
											serviceSpace.options.Add(CreateOption(option));
										}
									}
									serviceLevel.spaces.Add(serviceSpace);
								}
							}

							if (level.Options != null && level.Options.Count > 0)
							{
								serviceLevel.options = new List<global::Model.ServiceModel.Option>();

								foreach (var option in level.Options)
								{
									serviceLevel.options.Add(CreateOption(option));
								}
							}
							serviceSequence.levels.Add(serviceLevel);
						}
					}

					if (seq.Options != null && seq.Options.Count > 0)
					{
						serviceSequence.options = new List<global::Model.ServiceModel.Option>();
						foreach (var option in seq.Options)
						{
							serviceSequence.options.Add(CreateOption(option));
						}
					}
					serviceInspection.sequences.Add(serviceSequence);
				}

			}
			catch (Exception ex)
			{
				Debug.WriteLine("Exception occured at method getServiceModel: " + ex.Message);
			}
			return serviceInspection;
		}

		private ServiceModel.Option CreateOption(Option option)
		{
			bool? OptionResult = null;
			bool IsGuidedPicture = false;
			if (option.isGuidedPicture)
			{
				IsGuidedPicture = true;
			}

			var checkList = new List<ServiceModel.CheckList>();
			List<byte[]> optionImages = new List<byte[]>();
			if (option != null && option.checkListItems != null && option.checkListItems.Count > 0)
			{
				foreach (var checkListItem in option.checkListItems)
				{
					checkList.Add(new ServiceModel.CheckList()
					{
						comments = checkListItem.comments,
						description = checkListItem.description,
						result = checkListItem.Result
					});
				}
			}

			if (option.photos != null && option.photos.Count > 0)
			{
				foreach (var image in option.photos)
				{
					optionImages.Add(image.Image);
				}
			}
			Debug.WriteLine(string.Format("# of option photos = {0}", optionImages.Count));
			ServiceModel.Option serviceOption = new ServiceModel.Option()
			{
				name = option.name,
				isGuidedPicture = IsGuidedPicture,
				checkListItems = checkList,
				photos = optionImages
			};
			return serviceOption;
		}
	}
}