using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;
using System.Collections.Generic;
using System.Linq;
using BAL;
using Model;
using BAL.Service;
using CoreGraphics;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using DAL;
using System.Diagnostics;
using DAL.DO;



namespace LiRoInspect.iOS
{
	//delegate void EnableButtons();

	public partial class InspectionViewController : BaseViewController
	{
		
		public InspectionSequenceSource tableSource;
		bool IsLogOut = false;
		OptionTransactionService optionTransactionService;
		InspectionPunchSource punchSource;
		static int currentSequenceIndex = 0;
		private ITraversible selectedSeq = null;

		public bool IsEdit { get; set; }

		public Model.Inspection ins;
		public List<Model.ITraversible> iseq = new List<Model.ITraversible>();
		public List<Model.Punch> punchItems;

		public Model.Inspection InspectionData { get; set; }

		public Model.ITraversible currSeq;
		private UIImageView currentSelectedImageView;
		private bool iSInspectionInProgress = false;
		public PictureCollectionDelegate pictureCollection;
		public UIImageView image = new UIImageView(UIImage.FromBundle("imagePhoto2.png"));
		//public CGRect clickedTextViewFrame;
		NSTimer autoSaveTimer;
		static double autoSavetimeInSeconds = 0;
		private object SaveLock = new object();
		CameraDataSource Source;
		public List<UIImage> imagesList = new List<UIImage>();
		private int CheckListFilterCount = 0;
		private bool InCheckListSearch = false;
		private List<CheckList> checkListItems = null;
		public ISpace CurrSpace = null;
		private string SeqName = string.Empty;
		public string LevelName = string.Empty;

		public UIImageView cameraImageView { get; set; }
		UICameraController cameraController = null;

		CGPoint previousContentOffset;

		public InspectionViewController(IntPtr handle) : base(handle)
		{
		}

		public void loadStandardCode()
		{
		}


		List<Option> GetTransOptions(ITraversible selectedSeq)
		{
			Option option = null;
			List<Option> Options = null;
			try
			{
				lock (SaveLock)
				{
					using (optionTransactionService = new OptionTransactionService(AppDelegate.DatabaseContext))
					{
						var data = optionTransactionService.GetOptionTransactions();
						if (selectedSeq is Level)
						{
							var lvl = selectedSeq as Level;
							if (lvl.Spaces != null && lvl.Spaces.Count > 0)
							{
								foreach (var spc in lvl.Spaces)
								{
									Options = spc.Options;
									if (Options != null && Options.Count > 0)
									{
										if (ins != null)
										{

											var ExistingOpts = data.Where(o => o.inspectionTransID == ins.ID && o.SequenceID == lvl.seqID && o.LevelID == lvl.ID && o.SpaceID == spc.id).ToList();
											if (ExistingOpts != null && ExistingOpts.Count > 0)
											{
												foreach (var opt in ExistingOpts)
												{
													option = Options.Where(i => i.ID == opt.OptionId).FirstOrDefault() as Option;


													if (option != null)
													{
														option.isSelected = (opt.isSelected == 1) ? true : false;
													}
												}
											}
										}
									}
								}
							}
						}
						else if (selectedSeq is ISpace)
						{

							Options = (selectedSeq as ISpace).Options;


							if (Options != null && Options.Count > 0)
							{

								foreach (var Opt in Options)
								{
									var ExistingOpt = data.Where(o => o.inspectionTransID == ins.ID && o.OptionId == Opt.ID).FirstOrDefault();
									if (ExistingOpt != null)
									{
										if (Opt != null)
										{
											Opt.isSelected = (ExistingOpt.isSelected == 1) ? true : false;

										}
									}
								}

							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Exception Occured in GetTransOptions method due to " + ex.Message);
			}
			return null;
		}

		/// <summary>
		/// Updates the location identifier image view.
		/// </summary>
		/// <param name="img">Image.</param>

		public void UpdateLocationIDImageView(UIImage img)
		{
			if (imageShowView != null)
			{
				imageShowView.Image = img;
			}
			if (cameraImageView != null)
			{
				cameraImageView.Image = img;
			}
		}

		public async Task<double> getAutoSaveInterval()
		{
			double autoSaveTime;
			List<UserSetting> userSetting = new UserSettingService(AppDelegate.DatabaseContext).GetUserSettings();
			string strAutoIntervalTime = userSetting.Where(u => u.SettingName.Trim() == "Auto Save Interval(in Secs)").SingleOrDefault().SettingValue;
			Double.TryParse(strAutoIntervalTime, out autoSaveTime);
			return autoSaveTime;
		}

		public async override void ViewDidLoad()
		{
			try
			{
				base.ViewDidLoad();
				ResetUIView();

				rhsTableHeaderView.Hidden = false;
				rhsHeaderHeightConstraint.Constant = 85f;
				punchAddItemBtn.Hidden = true;

				punchFinishButton.Hidden = true;
				OptionsTableView.Hidden = true;
				imageShowView.Hidden = true;
				addPictureView.Hidden = false;
				//locationIdHeaderLabel.Hidden = false;



				LoadOverLayPopup();
				registerForKeyboardNotifications();
				syncInit();
				NotifyCount();

				lblSyncNumber.Layer.CornerRadius = lblSyncNumber.Frame.Height / 2;
				lblSyncNumber.ClipsToBounds = true;

				LblNotifyNbr.Layer.CornerRadius = LblNotifyNbr.Frame.Height / 2;
				LblNotifyNbr.ClipsToBounds = true;
				btnSave.TouchUpInside -= SaveSpace_TouchUpInside;
				btnSave.TouchUpInside += SaveSpace_TouchUpInside;
				btnNext.TouchUpInside -= BtnNext_TouchUpInside;
				btnNext.TouchUpInside += BtnNext_TouchUpInside;
				btnLogout.TouchUpInside -= BtnLogout_TouchUpInside;
				btnLogout.TouchUpInside += BtnLogout_TouchUpInside;
				btnHome.TouchUpInside -= BtnHome_TouchUpInside;
				btnHome.TouchUpInside += BtnHome_TouchUpInside;
				beginInspectionButton.Enabled = false;
				beginInspectionButton.Hidden = false;
				beginInspectionButton.Layer.CornerRadius = 5f;
				beginInspectionButton.Layer.BorderColor = UIColor.FromRGB(18, 74, 143).CGColor;
				beginInspectionButton.Layer.BorderWidth = 0.5f;
				beginInspectionButton.TouchUpInside -= beginInspectionButton_TouchUpInside;
				beginInspectionButton.TouchUpInside += beginInspectionButton_TouchUpInside;
				takePictureButton.TouchUpInside -= takePictureButton_TouchUpInside;
				takePictureButton.TouchUpInside += takePictureButton_TouchUpInside;
				progressInspectionBtn.TouchUpInside -= progressInspectionBtn_TouchUpInside;
				progressInspectionBtn.TouchUpInside += progressInspectionBtn_TouchUpInside;
				btnNotify.TouchUpInside -= BtnNotify_TouchUpInside;
				btnNotify.TouchUpInside += BtnNotify_TouchUpInside;
				docViewBtn.TouchUpInside -= BtnDocView_TouchUpInside;
				docViewBtn.TouchUpInside += BtnDocView_TouchUpInside;
				btnSync.Enabled = false;
				createPunchListBtn.TouchUpInside -= createPunchListBtn_TouchUpInside;
				createPunchListBtn.TouchUpInside += createPunchListBtn_TouchUpInside;


				takePictureBtn.TouchUpInside -= TakeOptionPictures_TouchUpInside;
				takePictureBtn.TouchUpInside += TakeOptionPictures_TouchUpInside;
				takePictureBtn.Hidden = true;
				punchFinishButton.Layer.CornerRadius = 5f;
				punchFinishButton.Layer.BorderColor = UIColor.FromRGB(18, 74, 143).CGColor;
				punchFinishButton.Layer.BorderWidth = 0.5f;
				btnSearchOptions.Layer.CornerRadius = 5f;
				btnSearchOptions.Layer.BorderColor = UIColor.FromRGB(18, 74, 143).CGColor;
				btnSearchOptions.Layer.BorderWidth = 0.5f;
				btnSearchOptions.Hidden = true;
				btnClearSearch.Hidden = true;
				btnSelectAll.TouchUpInside -= BtnSelectAll_TouchUpInside;
				btnSelectAll.TouchUpInside += BtnSelectAll_TouchUpInside;
				btnSelectAll.Layer.CornerRadius = 5f;
				btnSelectAll.Layer.BorderColor = UIColor.FromRGB(18, 74, 143).CGColor;
				btnSelectAll.Layer.BorderWidth = 0.5f;
				btnSelectAll.Hidden = false;
				txtSearchOptions.Hidden = true;
				htLblSequence.Constant = 60;
				//uncommented to test
				spSequenceLabel.Constant = -15;

				btnClearSearch.Layer.CornerRadius = 5f;
				btnClearSearch.Layer.BorderColor = UIColor.FromRGB(18, 74, 143).CGColor;
				btnClearSearch.Layer.BorderWidth = 0.5f;


				//lblSearchOptions.Hidden = true;

				txtSearchOptions.Layer.CornerRadius = 5f;
				txtSearchOptions.Layer.BorderColor = UIColor.FromRGB(18, 74, 143).CGColor;
				txtSearchOptions.Layer.BorderWidth = 0.5f;
				// center text in textview
				txtSearchOptions.TextAlignment = UITextAlignment.Left;


				takePictureButton.Layer.CornerRadius = 5f;
				takePictureButton.Layer.BorderColor = UIColor.FromRGB(18, 74, 143).CGColor;
				takePictureButton.Layer.BorderWidth = 0.5f;
				using (var inspectionDetailService = new InspectionDetailService(AppDelegate.DatabaseContext))
				{
					if (inspectionDetailService != null && InspectionData != null)
					{
						ins = inspectionDetailService.GetInspectionDetail(InspectionData, false);

						ins.InspectionStarted = InspectionData.InspectionStarted;


						fillInspectionDetails();



						if (ins != null)
						{
							GetTransactions();
							iseq = new List<Model.ITraversible>(ins.sequences.Cast<Model.ITraversible>());
							currSeq = (ITraversible)iseq[currentSequenceIndex];
							currSeq.prevSeqNextClicked = true;



							if (ins.InspectionStarted.HasValue && ins.InspectionStarted == 1)
							{

								tableSource = new InspectionSequenceSource(processEnabling(iseq, true), this);
							}
							else
							{
								tableSource = new InspectionSequenceSource(processEnabling(iseq, false), this);
							}


							if ((currSeq is ISequence) && (currSeq as ISequence).Levels != null && (currSeq as ISequence).Levels.Count > 0)
							{
								FillLevelsTable(currSeq);

								lblSequence.Text = "Please make a selection";
								btnSelectAll.Hidden = false;
								if (ins.InspectionStarted == 0)
								{
									ins.InspectionStarted = 1;
									using (InspectionTransactionService inspectionTransactionService = new InspectionTransactionService(AppDelegate.DatabaseContext))
									{
										var inspectionTraansaction = inspectionTransactionService.GetInspectionTransactions().Where(i => i.inspectionID == ins.inspectionID && i.projectID == ins.projectID && i.appID == ins.appID).FirstOrDefault();
										{
											inspectionTraansaction.InspectionStarted = ins.InspectionStarted;
											inspectionTransactionService.SaveInspectionTransaction(ins);
										}
									}
								}
							}
							else
							{
								UpdateRightTableView(currSeq);
							}

							sequenceTable.Source = tableSource;

							sequenceTable.TableFooterView = new UIView(new CoreGraphics.CGRect(0, 0, 0, 0));
							OptionsTableView.TableFooterView = new UIView(new CoreGraphics.CGRect(0, 0, 0, 0));
						}
					}
				}
				sequenceTable.BackgroundColor = UIColor.Clear;
				buttonVisibility();



				OptionsTableView.KeyboardDismissMode = UIScrollViewKeyboardDismissMode.Interactive;
				OptionsTableView.KeyboardDismissMode = UIScrollViewKeyboardDismissMode.OnDrag;

				UITapGestureRecognizer tap = new UITapGestureRecognizer();
				tap.AddTarget(tapOwnerDetailsViewAction);
				this.headerView.AddGestureRecognizer(tap);

				UITapGestureRecognizer tapHeaderView = new UITapGestureRecognizer();
				tapHeaderView.AddTarget(tapOwnerDetailsViewAction);
				this.ownerDetailsView.AddGestureRecognizer(tapHeaderView);

				UITapGestureRecognizer tapTableHeaderView = new UITapGestureRecognizer();
				tapTableHeaderView.AddTarget(tapOwnerDetailsViewAction);
				this.rhsTableHeaderView.AddGestureRecognizer(tapTableHeaderView);
				// click progress inspection button
				//progressInspectionBtn_TouchUpInside (this, new EventArgs ());
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Exception Occured in InspectionView ViewDidLoad method due to " + ex.Message);
			}
		}
		public void GetTransactions()
		{
			try
			{
				if (ins != null)
				{
					if (ins.sequences != null && ins.sequences.Count > 0)
					{
						foreach (var seq in ins.sequences)
						{
							var sequence = seq as Sequence;
							if (sequence.Levels != null && sequence.Levels.Count > 0)
							{
								foreach (var lvl in sequence.Levels)
								{
									using (LevelTransactionService levelTransactionService = new LevelTransactionService(AppDelegate.DatabaseContext))
									{


										var levelTransaction = levelTransactionService.GetLevelTransactions().Where(x => x.InspectionTransID == ins.ID && x.LevelID == lvl.ID && x.SeqID == lvl.seqID).FirstOrDefault();

										if (levelTransaction != null && levelTransaction.LevelID == lvl.ID && levelTransaction.SeqID == sequence.id)
										{
											//var Level = lvl as Model.Level;

											lvl.isSelected = (levelTransaction.isSelected.HasValue) ? ((levelTransaction.isSelected.Value == 1) ? true : false) : false;
										}

									}
									if (lvl.Spaces != null && lvl.Spaces.Count > 0)
									{
										foreach (var spc in lvl.Spaces)
										{

											using (SpaceTransactionService spaceTransactionService = new SpaceTransactionService(AppDelegate.DatabaseContext))
											{

												var spaceTransaction = spaceTransactionService.GetSpaceTransactions().Where(x => x.InspectionTransID == ins.ID && x.SeqID == spc.seqID && x.LevelID == spc.levelID && x.SpaceID == spc.SpaceID).FirstOrDefault();

												if (spaceTransaction != null)
												{
													spc.isSelected = (spaceTransaction.isSelected.HasValue) ? ((spaceTransaction.isSelected.Value == 1) ? true : false) : false;
												}

											}
											if (spc.Options != null && spc.Options.Count > 0)
											{
												using (OptionTransactionService optionTransactionService = new OptionTransactionService(AppDelegate.DatabaseContext))
												{

													foreach (var opt in spc.Options)
													{

														OptionTransaction optionTransaction = optionTransactionService.GetOptionTransactions().Where(x => x.inspectionTransID == ins.ID && x.OptionId == opt.OptionId && x.LevelID == opt.LevelID && x.SpaceID == opt.SpaceID && x.SequenceID == opt.SequenceID).FirstOrDefault();

														if (optionTransaction != null)
														{
															opt.isSelected = (optionTransaction.isSelected.HasValue) ? ((optionTransaction.isSelected.Value == 1) ? true : false) : false;

															if (opt.OptionTransactionID > 0)
															{

																if (opt.photos == null)
																{
																	opt.photos = new List<OptionImage>();


																	using (var imageservice = new OptionImageService(AppDelegate.DatabaseContext))
																	{

																		var transResult = imageservice.GetOptionTransactionImage(opt.OptionTransactionID);
																		if (transResult != null && transResult.Count() > 0)
																		{
																			foreach (var res in transResult)
																			{

																				opt.photos.Add(new OptionImage()
																				{
																					Image = res.Image,
																					OptionTransID = res.OptionTransID
																				});

																			}
																		}

																	}

																}
																else
																{

																	using (var imageservice = new OptionImageService(AppDelegate.DatabaseContext))
																	{

																		var transResult = imageservice.GetOptionTransactionImage(opt.OptionTransactionID);
																		if (transResult != null && transResult.Count() > 0)
																		{
																			if (opt.photos != null)
																			{
																				opt.photos.Clear();
																			}

																			foreach (var res in transResult)
																			{

																				opt.photos.Add(new OptionImage()
																				{
																					Image = res.Image,
																					OptionTransID = res.OptionTransID
																				});

																			}
																		}

																	}


																}
															}
														}
													}
												}
											}
										}
									}
									else if (lvl.Options != null && lvl.Options.Count > 0)
									{
										using (OptionTransactionService optionTransactionService = new OptionTransactionService(AppDelegate.DatabaseContext))
										{
											//ar Options1 = 
											foreach (var opt in lvl.Options)
											{

												OptionTransaction optionTransaction = optionTransactionService.GetOptionTransactions().Where(x => x.inspectionTransID == ins.ID && x.OptionId == opt.OptionId && x.LevelID == opt.LevelID && x.SpaceID == opt.SpaceID && x.SequenceID == opt.SequenceID).FirstOrDefault();

												if (optionTransaction != null)
												{
													opt.isSelected = (optionTransaction.isSelected.HasValue) ? ((optionTransaction.isSelected.Value == 1) ? true : false) : false;

													if (opt.OptionTransactionID > 0)
													{

														if (opt.photos == null)
														{

															opt.photos = new List<OptionImage>();



															using (var imageservice = new OptionImageService(AppDelegate.DatabaseContext))
															{

																var transResult = imageservice.GetOptionTransactionImage(opt.OptionTransactionID);
																if (transResult != null && transResult.Count() > 0)
																{

																	foreach (var res in transResult)
																	{

																		opt.photos.Add(new OptionImage()
																		{
																			Image = res.Image,
																			OptionTransID = res.OptionTransID
																		});

																	}
																}



															}
														}
														else
														{


															using (var imageservice = new OptionImageService(AppDelegate.DatabaseContext))
															{

																var transResult = imageservice.GetOptionTransactionImage(opt.OptionTransactionID);
																if (transResult != null && transResult.Count() > 0)
																{
																	if (opt.photos != null)
																	{
																		opt.photos.Clear();
																	}

																	foreach (var res in transResult)
																	{

																		opt.photos.Add(new OptionImage()
																		{
																			Image = res.Image,
																			OptionTransID = res.OptionTransID
																		});

																	}
																}

															}


														}
													}
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Exception Occured in IGetTransactions method due to " + ex.Message);
			}

		}






		void tapOwnerDetailsViewAction()
		{
			this.OptionsTableView.EndEditing(true);
		}

		public bool TableViewVisibility()
		{
			if (addPictureView != null && addPictureView.Hidden)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		void BtnSelectAll_TouchUpInside(object sender, EventArgs e)
		{
			bool AllSelected = false;
			if (currSeq != null)
			{
				if (currSeq is ISequence)
				{
					var seq = (currSeq as ISequence);
					var Levels = seq.Levels;

					if (Levels != null && Levels.Count > 0)
					{

						var SelectedList = Levels.Where(x => x.isSelected == true).ToList();

						if (SelectedList != null && SelectedList.Count == Levels.Count)
						{
							AllSelected = true;
						}

						if (!AllSelected)
						{
							if (seq.Levels != null && seq.Levels.Count > 0)
							{
								foreach (var level in seq.Levels)
								{
									level.isSelected = true;
								}

								FillLevelsTable(currSeq);

							}
						}
						else
						{
							foreach (var level in seq.Levels)
							{
								level.isSelected = false;
							}
							FillLevelsTable(currSeq);
						}
					}

				}
				else if (currSeq is ILevel)
				{
					var Spaces = (currSeq as ILevel).getSpaces();

					if (Spaces != null && Spaces.Count > 0)
					{

						var SelectedList = Spaces.Where(x => x.isSelected == true).ToList();

						if (SelectedList != null && SelectedList.Count == Spaces.Count)
						{
							AllSelected = true;
						}

						if (!AllSelected)
						{
							if (Spaces != null && Spaces.Count() > 0)
							{
								foreach (var space in Spaces)
								{
									space.isSelected = true;
								}
								FillSpacesTable(currSeq);
							}
						}
						else
						{
							foreach (var space in Spaces)
							{
								space.isSelected = false;
							}
							FillSpacesTable(currSeq);
						}
					}
					else
					{

						var Options = (currSeq as ILevel).Options;

						if (Options != null && Options.Count > 0)
						{
							var SelectedList = Options.Where(x => x.isSelected == true).ToList();

							if (SelectedList != null && SelectedList.Count == Options.Count)
							{
								AllSelected = true;
							}

							if (!AllSelected)
							{
								foreach (var option in Options)
								{
									option.isSelected = true;
								}
								FillOptionsTable1(currSeq, Options);
							}
							else
							{
								foreach (var option in Options)
								{
									option.isSelected = false;
								}
								FillOptionsTable1(currSeq, Options);
							}

						}
					}

				}
				else if (currSeq is ISpace)
				{
					var Options = (currSeq as ISpace).Options;

					if (Options != null && Options.Count > 0)
					{
						var SelectedList = Options.Where(x => x.isSelected == true).ToList();

						if (SelectedList != null && SelectedList.Count == Options.Count)
						{
							AllSelected = true;
						}

						if (!AllSelected)
						{
							foreach (var option in Options)
							{
								option.isSelected = true;
							}
							FillOptionsTable1(currSeq, Options);
						}
						else
						{
							foreach (var option in Options)
							{
								option.isSelected = false;
							}
							FillOptionsTable1(currSeq, Options);
						}


					}

				}
			}
		}

		/// <summary>
		/// Takes the option pictures touch up inside.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">EventArgs.</param>
		void TakeOptionPictures_TouchUpInside(object sender, EventArgs e)
		{
			if (currSeq is IOption)
			{
				try
				{
					UpdateImageViewCollection();
					rhsHeaderHeightConstraint.Constant = 85f;

					//if (cameraController != null)
					//{

					//	cameraController.DismissViewControllerAsync(false);


					//	if (cameraController.image != null && cameraController.image.Image != null)
					//	{
					//		cameraController.image.Image.Dispose();
					//	}

					//	cameraController.pictureTaken = null;
					//	cameraController.Dispose();
					//	cameraController = null;
					//}
					GC.Collect();

					Debug.WriteLine("TakeOptionPictures_TouchUpInside - line# 759");


					cameraController = this.Storyboard.InstantiateViewController("UICameraController") as UICameraController;
					cameraController.ClearCamearaView();
					cameraController.ModalPresentationStyle = UIModalPresentationStyle.OverCurrentContext;
					cameraController.picture = this.UpdateOptionCollectionView;

					if (cameraController.MediaLst != null)
					{
						foreach (var view in cameraController.MediaLst)
						{
							view.RemoveFromSuperview();
							view.Dispose();
						}
					}
					if (cameraController.imagesList != null)
					{
						cameraController.imagesList.Clear();
					}
					cameraController.MediaLst = ImageLst;
					this.PresentViewController(cameraController, false, null);


				}
				catch (Exception ex)
				{
					Debug.WriteLine("Exception Occured in UpdateCollectionImageView method due to " + ex.Message);
				}
			}
		}

		public void ResetTheCameraImageView()
		{
			try
			{

				if (imagesScrollView != null)
				{
					foreach (var view in imagesScrollView.Subviews)
					{
						if (view is UIImageView)
						{
							(view as UIImageView).Image = null;
						}
						view.RemoveFromSuperview();
						view.Dispose();
					}
				}
				rhsHeaderHeightConstraint.Constant = 85f;
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Exception Occured in ResetTheCameraImageView method due to " + ex.Message);
			}
		}

		public void HideTakePicture()
		{
			try
			{
				if (imagesScrollView != null)
				{
					foreach (var view in imagesScrollView.Subviews)
					{
						if (view is UIImageView)
						{
							(view as UIImageView).Image = null;
						}
						view.RemoveFromSuperview();
						view.Dispose();
					}
				}
				takePictureBtn.Hidden = true;
				rhsHeaderHeightConstraint.Constant = 85f;
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Exception Occured in ResetTheCameraImageView method due to " + ex.Message);
			}
		}

		/// <summary>
		/// Updates the option collection view.
		/// </summary>
		/// <param name="images">Images.</param>
		public async void UpdateOptionCollectionView(List<UIImage> images)
		{
			try
			{
				Debug.WriteLine("UpdateOptionCollectionView - line# 845");
				if (currSeq is Option)
				{
					var opt = currSeq as Option;
					if (opt.photos == null)
					{
						opt.photos = new List<OptionImage>();
					}
					else
					{
						opt.photos.Clear();
					}
					List<byte[]> cameraImages = new List<byte[]>();
					foreach (var img in images)
					{
						byte[] task = ImageToByteArray(img);
						var imgArr = task;
						cameraImages.Add(imgArr);
					}
					foreach (var img in cameraImages)
					{
						opt.photos.Add(new OptionImage()
						{
							Image = img
						});
					}
					if (images != null)
					{
						images.Clear();
						images = null;
					}

					if (cameraController != null)
					{


						cameraController.pictureTaken = null;
						cameraController.Dispose();
						cameraController = null;
					}

					GC.Collect();
					Debug.WriteLine("camera controller disposed");



					LoadOptionImages();
					UpdateImageViewCollection();

					if (currSeq is IOption)
					{
						// save transactions and fill option checklists
						Task.Run(() =>
						{

							SaveInpsectionOptionResult();
							GC.Collect();
							UIApplication.SharedApplication.InvokeOnMainThread(delegate
							{
								FillInspectionItem(currSeq);
							});
						});

						GC.Collect();

					}



					buttonStyleRefresh(null);
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Exception Occured in UpdateCollectionImageView method due to " + ex.Message);

			}
		}

		private void UpdateImageViewCollection()
		{
			if (ImageLst != null && ImageLst.Count > 0)
			{
				ImageLst.Clear();
			}
			foreach (var view in imagesScrollView.Subviews)
			{
				if (view is UIImageView)
				{
					UIImageView imageview = view as UIImageView;
					if (imageview.Image != null && imageview.Image.CGImage.Height > 5 && imageview.Image.CGImage.Width > 5)
					{
						ImageLst.Add(imageview);
					}
				}
			}
		}

		void LoadOptionImages()
		{
			if (currSeq is IOption)
			{
				try
				{
					var opt = currSeq as Option;
					ResetTheCameraImageView();
					if (opt.photos != null && opt.photos.Count > 0)
					{
						List<UIImage> currentoptionImages = new List<UIImage>();
						foreach (var img in opt.photos)
						{
							var imag = ByteArrayToImage(img.Image);
							currentoptionImages.Add(imag);
						}
						if (currentoptionImages.Count > 0)
						{
							var scrollViewImgs = BaseRestructureImages(currentoptionImages);
							imagesScrollView.AddSubviews(scrollViewImgs.ToArray());
							if (scrollViewImgs != null && scrollViewImgs.Count > 0)
							{
								imagesScrollView.ContentSize = new CoreGraphics.CGSize(scrollViewImgs.Count * scrollViewImgs.FirstOrDefault().Frame.Width + scrollViewImgs.Count * 10, 100);
							}
							if (null != ImageLst)
							{
								ImageLst.Clear();
							}
							ImageLst.AddRange(scrollViewImgs);
							AddGestureEvents(opt);
							foreach (var currImg in currentoptionImages)
							{
								currImg.Dispose();
							}
						}
						rhsHeaderHeightConstraint.Constant = 215f;
						buttonStyleRefresh(null);
					}
					else
					{
						if (currSeq is IOption)
						{
							rhsHeaderHeightConstraint.Constant = 110f;
						}
						else
						{
							rhsHeaderHeightConstraint.Constant = 85f;
						}
					}

				}
				catch (Exception ex)
				{
					Debug.WriteLine("Exception Occured in LoadPassView method due to " + ex.Message);
				}
			}
		}

		private void AddGestureEvents(Option opt)
		{
			try
			{
				if (ImageLst != null)
				{
					foreach (var view in ImageLst)
					{
						Action action = new Action(
											delegate
											{
												UpdateImageViewCollection();
												var answer = new UIActionSheet("Do you want to Delete the Image?", null, "Cancel", "Ok", new string[1] { "Cancel" });

												answer.Clicked += delegate (object sender, UIButtonEventArgs args)
												{
													var btnarg = args.ButtonIndex;
													if (btnarg == 0)
													{
														view.RemoveFromSuperview();
														int index = -1;
														for (int i = 0; i < ImageLst.Count; i++)
														{
															if (view == ImageLst[i])
															{
																index = i;
																break;
															}
														}
														ImageLst.Remove(view);
														if (index >= 0)
														{
															if (opt != null && opt.photos != null && opt.photos.Count > 0)
															{
																if (index < opt.photos.Count)
																	opt.photos.RemoveAt(index);
															}
														}

														LoadOptionImages();
														buttonStyleRefresh(null);
													}
												};
												answer.ShowInView(this.View);
											});
						UILongPressGestureRecognizer gr = new UILongPressGestureRecognizer();
						gr.AddTarget(action);
						if (view is UIImageView)
						{
							view.AddGestureRecognizer(gr);
							view.UserInteractionEnabled = true;
						}

					}
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Exception Occured in AddGestureEvents method due to " + ex.Message);
			}
		}

		private void fillInspectionDetails()
		{
			try
			{
				lblOwnerName.Text = InspectionData.HouseOwnerName;

				lblInspectionType.Text = InspectionData.InspectionType;
				lblinspectionName.Text = InspectionData.InspectionType;
				lblinspectionNameRight.Text = InspectionData.InspectionType;
				lblAddress1.Text = InspectionData.InspectionAddress1;
				lblPhoneNumber.Text = InspectionData.PhoneNo;

				lblInspectionDate.Text = ins.inspectionDateTime.ToString();

				lblAddress2.Text = InspectionData.City + " " + InspectionData.Pincode;

				//lblCityStateZip.Text = InspectionData.City + ", " + InspectionData.State.Trim () + " " + InspectionData.Pincode;
				//lblCityStateZip.Hidden = true;
				lblCalDate.Text = InspectionData.inspectionDateTime.ToString("MMM dd");
				int isToday = DateTime.Compare(InspectionData.inspectionDateTime.Date, DateTime.Today.Date);
				if (isToday == 0)
				{
					lblCalDay.Text = "Today";
				}
				else if (isToday > 0)
				{
					lblCalDay.Text = "Upcoming";
				}

				if (ins != null)
				{
					ins.City = InspectionData.City;
					ins.projectID = InspectionData.projectID;
					ins.ContractNo = InspectionData.ContractNo;
					ins.ContractorName = InspectionData.ContractorName;
					ins.HouseOwnerName = InspectionData.HouseOwnerName;
					ins.InspectionAddress1 = InspectionData.InspectionAddress1;
					ins.InspectionAddress2 = InspectionData.InspectionAddress2;
					ins.inspectionDateTime = InspectionData.inspectionDateTime;
					ins.Pathway = InspectionData.Pathway;
					ins.Pincode = InspectionData.Pincode;
					ins.PhoneNo = InspectionData.PhoneNo;
					ins.ProjectName = InspectionData.ProjectName;
					ins.RepresentativeName = InspectionData.RepresentativeName;
					ins.locationIDImages = InspectionData.locationIDImages;
				}

				if (ins.locationIDImages != null && ins.locationIDImages.Count > 0)
				{
					MediaLst = new List<UIImageView>();
					ins.locationIDImages.ForEach(i => MediaLst.Add(new UIImageView(ByteArrayToImage(i))));
					RestructureImages();
					//    imageShowView.Image = ByteArrayToImage (ins.Image);
					imageShowView.Hidden = false;
					beginInspectionButton.Layer.CornerRadius = 5f;
					beginInspectionButton.Layer.BorderColor = UIColor.FromRGB(18, 74, 143).CGColor;
					beginInspectionButton.BackgroundColor = UIColor.FromRGB(18, 74, 143);
					beginInspectionButton.SetTitleColor(UIColor.White, UIControlState.Normal);
					beginInspectionButton.Layer.BorderWidth = 0.5f;
					takePictureView.Hidden = true;
					beginInspectionButton.Enabled = true;
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Exception Occured in FillInspectionDetails method due to " + ex.Message);
			}

		}

		public async override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);
			this.NavigationController.NavigationBarHidden = true;
			this.InvokeOnMainThread(delegate
			{
				headerView.BackgroundColor = UIColor.FromPatternImage(UIImage.FromFile("HeaderViewBackground.png"));
			});

			if (!lblInspectionType.Text.Contains(@"100%"))
			{
				createPunchListBtn.Hidden = false;
				if (lblInspectionType.Text.Contains(@"90%"))
				{
					createPunchListBtn.SetImage(UIImage.FromFile(@"punch-list.png"), UIControlState.Normal);
				}
				else
				{
					createPunchListBtn.SetImage(UIImage.FromFile(@"NC-Item.png"), UIControlState.Normal);
				}
			}
			else
				createPunchListBtn.Hidden = true;

			// hide createPunchListBtn button
			//createPunchListBtn.Hidden = true;
			beginInspectionButton.Enabled = false;

			if (imageShowView.Image != null)
			{
				beginInspectionButton.Enabled = true;
				createPunchListBtn.Enabled = false;

				if (imagesList != null || imagesList.Count > 0)
					imagesList.Clear();

				if (MediaLst != null || MediaLst.Count > 0)
					MediaLst.Clear();

				if (ins.locationIDImages != null && ins.locationIDImages.Count == 0)
				{
					var images = LocationImageDo.getImageForLocationIdentification(AppDelegate.DatabaseContext, ins.ID);
					if (images != null && images.Count > 0)
					{
						foreach (var img in images)
						{
							ins.locationIDImages.Add(img.Image);
						}
					}
				}
				for (int index = 0; index < ins.locationIDImages.Count; index++)
				{
					imagesList.Add(ByteArrayToImage(ins.locationIDImages[index]));
					UIImageView imageView = new UIImageView();
					imageView.Image = (ByteArrayToImage(ins.locationIDImages[index]));
					MediaLst.Add(imageView);
				}

				if (Source == null)
				{
					Source = new CameraDataSource(imagesList, this);
				}
				else
				{
					Source.itemsList = imagesList;
				}
				imageCollectionView.DataSource = Source;
				imageCollectionView.ReloadData();
			}
			else
			{
				beginInspectionButton.Enabled = false;
				createPunchListBtn.Enabled = false;
			}

			headerView.Layer.ShadowColor = UIColor.FromRGB(142, 187, 223).CGColor;
			headerView.Layer.ShadowOpacity = 0.8f;
			headerView.Layer.ShadowRadius = 2.0f;
			headerView.Layer.ShadowOffset = new System.Drawing.SizeF(0f, 3f);
			headerView.Layer.MasksToBounds = false;

			autoSavetimeInSeconds = await Task.Run(() => getAutoSaveInterval());
			if (autoSavetimeInSeconds > 0)
			{
				autoSaveTimer = NSTimer.CreateRepeatingScheduledTimer(autoSavetimeInSeconds, delegate
				{

					auoSave();
				});
			}
			AppDelegate.dataSync.syncProgress += syncProgressChange;
		}

		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);

			HideOverLay();
		}

		public override void ViewWillDisappear(bool animated)
		{
			base.ViewWillDisappear(animated);
			AppDelegate.dataSync.syncProgress -= syncProgressChange;
		}

		void BtnLogout_TouchUpInside(object sender, EventArgs e)
		{
			IsLogOut = LogOut();
			if (IsLogOut)
			{
				this.DismissViewControllerAsync(false);
				IsLogOut = false;
			}
		}

		/// <summary>
		/// Takes the user to Home screen.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		void BtnHome_TouchUpInside(object sender, EventArgs e)
		{
			if (punchSource != null)
			{
				if (punchItems != null)
					shouldSavePunchList();
			}
			clearOptionsTable();
			using (InspectionTransactionService inspectionTransactionService = new InspectionTransactionService(AppDelegate.DatabaseContext))
			{
				var inspectionTraansaction = inspectionTransactionService.GetInspectionTransactions().Where(i => i.inspectionID == ins.inspectionID && i.projectID == ins.projectID && i.appID == ins.appID).FirstOrDefault();
				{
					ins.InspectionStarted = 0;
					inspectionTraansaction.InspectionStarted = 0;

					inspectionTransactionService.SaveInspectionTransaction(ins);
				}
			}

			DashBoardViewController dashBoardViewController = this.Storyboard.InstantiateViewController("DashBoardViewController") as DashBoardViewController;
			this.DismissViewControllerAsync(false);
			this.NavigationController.PushViewController(dashBoardViewController, false);
		}

		void BtnDocView_TouchUpInside(object sender, EventArgs e)
		{
			shouldSavePunchList();
			List<Document> documentsList = null;
			List<Document> documentsContainedInDB = null;
			DocViewService documentService = new DocViewService(AppDelegate.DatabaseContext);

			if (documentService != null)
			{
				documentsContainedInDB = documentService.GetDocumentItems(ins.inspectionID, ins.projectID);
				documentsList = new List<Document>();

				if (documentsContainedInDB == null || documentsContainedInDB.Count == 0)
				{

				}
				else
				{
					documentsList = documentsContainedInDB;
				}
			}
			if (documentsContainedInDB != null && documentsContainedInDB.Count > 0)
			{
				DocumentController documentViewController = this.Storyboard.InstantiateViewController("DocumentController") as DocumentController;
				documentViewController.token = AppDelegate.user.UserDetails.Token;
				documentViewController.documentsList = documentsList;
				this.NavigationController.PushViewController(documentViewController, false);
			}
			else
			{
				UIAlertView alert = new UIAlertView(@"Warning", @"No Documents Available.", null, NSBundle.MainBundle.LocalizedString("OK", "OK"));
				alert.Show();
			}
		}


		public bool shouldSavePunchList()
		{
			if (punchSource != null)
			{
				if (punchItems != null)
				{
					PunchService punchService = new PunchService(AppDelegate.DatabaseContext);
					List<Model.Punch> localPunchItemsList = new List<Model.Punch>();

					if (ins != null)
						localPunchItemsList = punchService.GetPunchItems(ins.inspectionID, ins.projectID, ins.ID);

					if (PunchCell.IsPunchValueChanged)
					{
						int buttonClicked = -1;
						this.InvokeOnMainThread(delegate
						{
							UIAlertView alert1 = new UIAlertView(@"Alert", @"Do you want to save this punch list?", null, NSBundle.MainBundle.LocalizedString("Cancel", "Cancel"), NSBundle.MainBundle.LocalizedString("OK", "OK"));

							alert1.Show();
							alert1.Clicked += (sender, buttonArgs) =>
							{
								buttonClicked = (int)buttonArgs.ButtonIndex;

							};
						});
						// Wait for a button press.
						while (buttonClicked == -1)
							NSRunLoop.Current.RunUntil(NSDate.FromTimeIntervalSinceNow(0.5));

						if (buttonClicked == 1)
							savePunchListChanges();


						localPunchItemsList.Clear();
						localPunchItemsList = null;
						PunchCell.IsPunchValueChanged = false;
					}
				}
			}
			return true;
		}

		/// <summary>
		/// Saves the punch list changes.
		/// </summary>
		void savePunchListChanges()
		{





			try
			{
				PunchService punchService = new PunchService(AppDelegate.DatabaseContext);
				PunchImageService punchImageService = new PunchImageService(AppDelegate.DatabaseContext);

				//Deleting all punch items and Punch Images
				punchService.DeleteAllPunchItem(ins.inspectionID, ins.projectID);
				punchImageService.DeletePunchItemImages(ins.ID);

				if (punchItems.Count > 0)
				{
					for (int i = 0; i < punchItems.Count; i++)
					{
						punchItems[i].InspectionID = InspectionData.inspectionID;
						punchItems[i].ProjectID = InspectionData.projectID;
						if (!string.IsNullOrEmpty(punchItems[i].punchDescription))
						{
							punchService.SavePunchItem(punchItems[i]);
							punchItems[i].PunchID = punchService.GetPunchItems().LastOrDefault().PunchID;
							punchImageService.SavePunchItemImages(punchItems[i].punchImages, ins.ID, punchItems[i].PunchID);
						}
					}

					if (punchItems != null)
					{
						foreach (var punch in punchItems)
						{
							if (punch.punchImages != null)
							{
								punch.punchImages.Clear();
								punch.punchImages = null;
							}
						}
						punchItems.Clear();
					}
				}
				this.InvokeOnMainThread(delegate
				{
					punchFinishButton.Hidden = true;
					btnNext.Hidden = false;
					lblinspectionNameRight.Text = InspectionData.InspectionType;
					ITraversible selectedSeq = currSeq;//get object from hiearchy by row
					clearOptionsTable();
					if (selectedSeq is ISequence && (currSeq as ISequence).Levels != null && (currSeq as ISequence).Levels.Count > 0)
					{
						FillSpacesTable(selectedSeq);
						buttonVisibility();
						return;
					}
					if (selectedSeq is IOption)
					{
						txtSearchOptions.Hidden = false;
						btnSearchOptions.Hidden = false;
						btnClearSearch.Hidden = false;
						spSequenceLabel.Constant = -45;

						if (rhsHeaderHeightConstraint.Constant != 110f)
						{
							rhsHeaderHeightConstraint.Constant = 110f;
						}

						if (imagesScrollView.Subviews.Count() > 0)
						{
							if (rhsHeaderHeightConstraint.Constant != 215f)
							{
								rhsHeaderHeightConstraint.Constant = 215f;
							}
						}


						FillInspectionItem(selectedSeq);


					}
					else if (selectedSeq is ILevel)
					{
						FillSpacesTable(selectedSeq);
					}
					else
					{
						FillOptionsTable(selectedSeq);
					}
					buttonVisibility();
					createPunchListBtn.Enabled = true;

				});

			}
			catch (Exception ex)
			{
				Debug.WriteLine("Exception Occured in savePunchListChanges method due to " + ex.Message);
			}
		}

		/// <summary>
		/// Saves the Location Identification images into database.
		/// </summary>
		void saveLocationIdentifierImages()
		{
			ins.locationIDImages = new List<byte[]>();
			imagesList.ForEach(i => ins.locationIDImages.Add(ImageToByteArray(i)));
			using (InspectionTransactionService inspectionTransactionService = new InspectionTransactionService(AppDelegate.DatabaseContext))
			{
				inspectionTransactionService.UpdateInspectionTransaction(ins);
				var items = LocationImageDo.getImageForLocationIdentification(AppDelegate.DatabaseContext, ins.ID);
				if (items != null && items.Count > 0)
				{
					LocationImageDo.DeleteImage(AppDelegate.DatabaseContext, ins.ID);
				}
				if (ins.locationIDImages != null && ins.locationIDImages.Count > 0)
				{
					foreach (var img in ins.locationIDImages)
					{
						LocationImageDo.InsertImageForInspection(AppDelegate.DatabaseContext, ins.ID, img);
					}
				}
			}
			//createPunchListBtn is enabled if location identifier image is available else it is disabled.
			createPunchListBtn.Enabled = true;
		}


		public void FillOptionsTable(ITraversible selectedSeq)
		{
			this.OptionsTableView.EndEditing(true);
			lblinspectionNameRight.Text = InspectionData.InspectionType;


			try
			{
				if (lblInspectionType.Text.Contains(@"90%"))
				{
					createPunchListBtn.Hidden = false;
				}
				ResetTheCameraImageView();
				takePictureBtn.Hidden = true;
				imagesScrollView.Hidden = true;
				punchAddItemBtn.Hidden = true;
				punchFinishButton.Hidden = true;
				lblSequence.Hidden = false;
				OptionsTableView.Hidden = true;

				clearPhotoBuffer();
				currSeq = selectedSeq;
				AddPhotosToCurrentSequence(currSeq);


				lblSequence.Text = currSeq.getName();
				//SeqName = lblSequence.Text;


			}
			catch (Exception ex)
			{
				Debug.WriteLine("Exception Occured in FillOptiontable method due to " + ex.Message);
			}
		}

		public void UpdateRightTableView(ITraversible selectedSeq)
		{
			try
			{
				btnSelectAll.Hidden = true;
				if (lblInspectionType.Text.Contains(@"90%"))
				{
					createPunchListBtn.Hidden = false;
				}
				ResetTheCameraImageView();
				punchAddItemBtn.Hidden = true;
				punchFinishButton.Hidden = true;
				lblSequence.Hidden = false;
				OptionsTableView.Hidden = true;
				takePictureBtn.Hidden = true;
				imagesScrollView.Hidden = true;
				clearPhotoBuffer();
				currSeq = selectedSeq;
				AddPhotosToCurrentSequence(currSeq);

				lblSequence.Text = currSeq.getName();

				//SeqName = lblSequence.Text;
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Exception Occured in FillOptiontable method due to " + ex.Message);
			}
		}



		public void FillInspectionItem(ITraversible selectedSeq)
		{
			try
			{
				if (lblInspectionType.Text.Contains(@"90%"))
				{
					createPunchListBtn.Hidden = false;
				}

				lblinspectionNameRight.Text = InspectionData.InspectionType;
				punchAddItemBtn.Hidden = true;
				punchFinishButton.Hidden = true;
				lblSequence.Hidden = false;
				OptionsTableView.Hidden = false;



				if (selectedSeq is IOption)
				{
					var opt = selectedSeq as Option;
					if (opt.ID == Constants.FINALPUNCH_OPTIONID)
					{
						takePictureBtn.Hidden = true;
					}
					else
					{

						takePictureBtn.Hidden = false;
						takePictureBtn.Enabled = true;
					}
				}
				imagesScrollView.Hidden = false;
				clearPhotoBuffer();
				currSeq = selectedSeq;
				//SaveInpsectionOptionResult();


				AddPhotosToCurrentSequence(currSeq);
				lblSequence.Text = currSeq.getName();
				//SeqName = lblSequence.Text;
				if (currSeq is IOption)
				{
					//currSeq = (Option)selectedSeq;
					LoadOptionImages();
					ClearOptionsTableView();
					InspectionItemSource optionsSource = new InspectionItemSource(currSeq as Option, this, OptionsTableView, false);

					OptionsTableView.Source = optionsSource;
					OptionsTableView.RowHeight = 250;



					OptionsTableView.AllowsSelection = false;
					OptionsTableView.ReloadData();
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Exception Occured in FillInspectionItem method due to " + ex.Message);
			}
		}

		public void clearOptionsTable()
		{
			NSIndexPath[] visibleRows = OptionsTableView.IndexPathsForVisibleRows;
			foreach (var indexPath in visibleRows)
			{
				var cell = OptionsTableView.CellAt(indexPath);
				if (cell is ICell)
				{
					(cell as ICell).cleanCell();

				}
			}
		}

		public void SetCheckListCommentsActive(ItemCell item, bool ItemChecked)
		{
			var Source = OptionsTableView.Source as InspectionItemSource;

			if (Source != null)
			{
				var NSIndex = OptionsTableView.IndexPathForCell(item);

				var Row = NSIndex.Row;

				Source.CheckListCommentsActiveItems[Row] = ItemChecked;
			}
		}

		public bool GetCheckListCommentsActive(int Row)
		{
			var Source = OptionsTableView.Source as InspectionItemSource;

			if (Source != null)
			{


				if (Source.CheckListCommentsActiveItems[Row] == true)
				{

					return true;
				}

			}

			return false;
		}

		public void RefreshOptionsTable()
		{
			var VisibleIndexes = OptionsTableView.IndexPathsForVisibleRows;

			if (VisibleIndexes != null)
			{
				// clean up
				OptionsTableView.ReloadRows(VisibleIndexes, UITableViewRowAnimation.None);
			}
			OptionsTableView.TableFooterView = new UIView(new CoreGraphics.CGRect(0, 0, 0, 0));
		}

		public void clearSequenceTable()
		{
			NSIndexPath[] visibleRows = sequenceTable.IndexPathsForVisibleRows;
			foreach (var indexPath in visibleRows)
			{
				var cell = sequenceTable.CellAt(indexPath);
				foreach (var subCell in cell.Subviews)
				{
					subCell.Dispose();
					subCell.RemoveFromSuperview();
				}
			}
		}

		public string GetSeqName()
		{
			return SeqName;
		}

		public void SetlblinspectionNameRightHidden(bool value)
		{
			lblinspectionNameRight.Hidden = value;
		}



		public void FillLevelsTable(ITraversible selectedSeq)
		{
			lblSequence.Hidden = false;

			punchAddItemBtn.Hidden = true;
			punchFinishButton.Hidden = true;
			lblSequence.Hidden = false;
			OptionsTableView.Hidden = false;
			takePictureBtn.Hidden = true;
			imagesScrollView.Hidden = true;
			clearPhotoBuffer();
			currSeq = selectedSeq;
			AddPhotosToCurrentSequence(currSeq);
			lblSequence.Text = currSeq.getName();
			//SeqName = lblSequence.Text;//
			btnSave.Hidden = false;
			btnFinish.Hidden = true;
			btnNext.Hidden = true;
			ClearOptionsTableView();

			InspectionLevelSource inspectionLevelSource = new InspectionLevelSource(selectedSeq, this, OptionsTableView);
			OptionsTableView.Source = inspectionLevelSource;
			OptionsTableView.RowHeight = 50;


			OptionsTableView.ReloadData();
		}

		public void FillSpacesTable(ITraversible selectedSeq)
		{
			try
			{
				lblinspectionNameRight.Text = InspectionData.InspectionType;
				ResetTheCameraImageView();
				punchAddItemBtn.Hidden = true;
				punchFinishButton.Hidden = true;
				lblSequence.Hidden = false;
				OptionsTableView.Hidden = false;
				takePictureBtn.Hidden = true;
				imagesScrollView.Hidden = true;
				clearPhotoBuffer();
				currSeq = selectedSeq;
				AddPhotosToCurrentSequence(currSeq);
				lblSequence.Text = currSeq.getName();
				ClearOptionsTableView();
				//SeqName = lblSequence.Text;
				InspectionSpaceSource spaceSource = new InspectionSpaceSource(selectedSeq, this, OptionsTableView);
				OptionsTableView.Source = spaceSource;
				OptionsTableView.RowHeight = 50;

				OptionsTableView.ReloadData();
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Exception Occured in FillSpaceTable method due to " + ex.Message);
			}
		}

		public void reloadRows(int from, int to)
		{
			NSIndexPath[] rowsToReload = new NSIndexPath[] {
				NSIndexPath.FromRowSection (1, 0) // points to second row in the first section of the model
			};
			OptionsTableView.ReloadRows(rowsToReload, UITableViewRowAnimation.None);
		}

		async partial void btnNext_TouchUpInside(UIButton sender)
		{
			if (currSeq is ILevel)
			{
				var spc = (currSeq as ILevel).getSpaces();
				if (spc != null)
				{
					var res = spc.Any(i => i.isSelected == true);
					if (res)
					{
						SaveAndNextAction(false);


						return;
					}
				}
			}
			UIApplication.SharedApplication.IdleTimerDisabled = true;
			this.InvokeOnMainThread(delegate
			{
				LoadOverLayPopup();
			});
			Debug.WriteLine("Before Save Operation");
			await Task.Run(() => SaveInpsectionOptionResult());
			Debug.WriteLine("After Save Operation");


			using (RootViewController rootViewController = this.Storyboard.InstantiateViewController("RootViewController") as RootViewController)
			{
				rootViewController.InspectionResult = ins;
				this.InvokeOnMainThread(delegate
				{
					HideOverLay();
				});
				Debug.WriteLine("Before Navigation");
				this.NavigationController.PushViewController(rootViewController, true);
				Debug.WriteLine("After Navigation");
			}
		}

		//		List<Model.ITraversible> GetOptionsSelected(List<Model.ITraversible> SequenceItems)
		//		{
		//			ILevel levelSeq;
		//
		//			if (SequenceItems != null) {
		//				foreach (ITraversible seq in SequenceItems) {
		//					if (seq is Level) {
		//						Level level = (seq as Level);
		//						if ((level.Spaces != null && level.Spaces.Count > 0) || (level.Options != null && level.Options.Count > 0)) {
		//							foreach (var space in level.Spaces) {
		//								if (space.isSelected) {
		//
		//								}
		//							}
		//						}
		//					}
		//
		//				}
		//			}
		//			return SequenceItems;
		//		}

		List<Model.ITraversible> processEnabling(List<Model.ITraversible> SequenceItems, bool bypass)
		{
			if (!IsEdit)
			{
				if (SequenceItems != null)
				{
					if (!bypass)
					{
						foreach (ITraversible seq in SequenceItems)
						{
							seq.enableRow = false;
						}
					}
					else
					{
						foreach (ITraversible seq in SequenceItems)
						{
							seq.prevSeqNextClicked = false;
							seq.enableRow = true;
						}
					}

					foreach (var seq in SequenceItems)
					{//ins.Sequences)//(int i=0;i<SequenceItems.Count;i++)
						if (!bypass)
						{
							if (!addPictureView.Hidden)
							{
								seq.enableRow = false;
								break;
							}
							if (!seq.prevSeqNextClicked)
							{
								seq.enableRow = true;
								break;
							}
						}
						seq.enableRow = true;

					}
				}
			}
			else
			{
				if (SequenceItems != null)
				{
					foreach (ITraversible seq in SequenceItems)
					{
						seq.enableRow = true;
					}
				}
			}
			return SequenceItems;
		}

		ITraversible SetCurrentSequence(ITraversible selectedSeq)
		{
			int nextTraversibleIndex = 0;
			if (selectedSeq is ISequence || selectedSeq is ISpace || (selectedSeq is ILevel && (selectedSeq as Level).Options != null && (selectedSeq as Level).Options.Count > 0))
			{
				nextTraversibleIndex = iseq.FindIndex(traversible => traversible == selectedSeq);
				if (nextTraversibleIndex < (iseq.Count - 1))
				{
					nextTraversibleIndex = nextTraversibleIndex + 1;
				}
				else
				{//last
					selectedSeq = iseq[nextTraversibleIndex];
					return selectedSeq;

				}
				if (iseq[nextTraversibleIndex] != null)
				{
					selectedSeq = iseq[nextTraversibleIndex];

				}
				if (selectedSeq is ISequence || selectedSeq is ISpace || (selectedSeq is ILevel && (selectedSeq as Level).Options != null && (selectedSeq as Level).Options.Count > 0))
				{
					if ((selectedSeq is ISpace) && (selectedSeq as ISpace).Options.Count == 0)
					{
						selectedSeq = SetCurrentSequence(selectedSeq);
					}
					else if ((selectedSeq is ISequence) && (selectedSeq as ISequence).Levels == null)
					{
						selectedSeq = SetCurrentSequence(selectedSeq);
					}
				}
				for (int i = 0; i < nextTraversibleIndex; i++)
				{
					iseq[i].prevSeqNextClicked = true;
				}
				bool bypass = ins.InspectionStarted == 1 ? true : false;
				tableSource = new InspectionSequenceSource(processEnabling(iseq, bypass), this);
				sequenceTable.Source = tableSource;
				sequenceTable.ReloadData();
			}
			return selectedSeq;
		}

		List<Model.ITraversible> travesibleCollection = null;



		void rebuildSequence()
		{
			currSeq.prevSeqNextClicked = true;
			iseq = new List<Model.ITraversible>(ins.sequences.Cast<Model.ITraversible>());
			travesibleCollection = new List<Model.ITraversible>();
			try
			{
				foreach (ITraversible travesible in iseq)
				{
					if (travesible is ISequence)
					{
						Sequence seq = (travesible as Sequence);
						travesibleCollection.Add(seq);
						if (seq.Levels != null && seq.Levels.Count > 0)
						{
							foreach (var level in seq.Levels)
							{
								if (level.isSelected)
								{
									travesibleCollection.Add(level);
									AddSpacesAndOptionsForLevel(level);
								}
								else
								{
									if (travesibleCollection.Contains(level))
									{
										travesibleCollection.Remove(level);
									}
								}
							}
						}
						AddOption(travesible as ISequenceOption);
					}
				}
				iseq = travesibleCollection;
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Error occured at method rebuildSequence due to " + ex.Message);
			}
		}

		void AddSpacesAndOptionsForLevel(ILevel levelSeq)
		{
			Level level = (levelSeq as Level);
			if ((level.Spaces != null && level.Spaces.Count > 0) || (level.Options != null && level.Options.Count > 0))
			{
				foreach (var space in level.Spaces)
				{
					if (level.isSelected)
					{


						if (space.isSelected)
						{
							travesibleCollection.Add(space);
							AddOption(space as ISequenceOption);
						}
						else
						{

							if (travesibleCollection.Contains(space))
							{
								travesibleCollection.Remove(space);
							}



						}
					}
					else
					{
						if (travesibleCollection.Contains(space))
						{
							travesibleCollection.Remove(space);
						}
					}
				}
				AddOption(levelSeq as ISequenceOption);
			}

		}

		void AddOption(ISequenceOption sequenceOption)
		{
			if (sequenceOption.Options != null && sequenceOption.Options.Count > 0)
			{

				foreach (Option option in sequenceOption.Options)
				{
					if (option.InspectionID == "7" && option.prevSeqNextClicked == false)
					{
						if (option.checkListItems != null && option.checkListItems.Count > 0)
						{
							var HasPunchListItems = option.checkListItems.Where(x => x.itemType == Model.ItemType.PunchList).Any();
							if (HasPunchListItems)
							{


								travesibleCollection.Add(option);

							}
							else
							{
								if (option.isSelected)
								{
									travesibleCollection.Add(option);
								}
								else
								{
									if (travesibleCollection.Contains(option))
									{
										travesibleCollection.Remove(option);
									}
								}

							}
						}
					}
					else
					{
						if (option.isSelected)
						{
							travesibleCollection.Add(option);
						}
						else
						{

							if (travesibleCollection.Contains(option))
							{
								travesibleCollection.Remove(option);
							}


						}
					}

				}


			}
		}

		public void SetControlVisibility(bool isVisible, ITraversible selectedSeq)
		{
			OptionsTableView.Hidden = isVisible;

			if (selectedSeq is IOption && (selectedSeq as Option).InspectionID != "7")
			{
				takePictureBtn.Hidden = false;
				btnSelectAll.Hidden = true;
				spSequenceLabel.Constant = -45;
				htLblSequence.Constant = 120;
				rhsHeaderHeightConstraint.Constant = 110f;

				if (imagesScrollView.Subviews.Count() > 0)
				{
					rhsHeaderHeightConstraint.Constant = 215f;
				}
				CGPoint zero = new CGPoint(0, 0);
				OptionsTableView.SetContentOffset(zero, true);
			}
			else
			{
				if ((selectedSeq is ISequence) && (selectedSeq as ISequence).Levels == null && (selectedSeq as ISequence).Options.Count > 0)
				{


					OptionsTableView.Hidden = false;
					takePictureBtn.Hidden = true;
					btnSelectAll.Hidden = true;

					txtSearchOptions.Hidden = true;
					btnSearchOptions.Hidden = true;
					btnClearSearch.Hidden = true;
					htLblSequence.Constant = 60;
					spSequenceLabel.Constant = -15;
					rhsHeaderHeightConstraint.Constant = 85f;

					CGPoint zero = new CGPoint(0, 0);
					OptionsTableView.SetContentOffset(zero, true);


				}
				else
				{

					if (selectedSeq is IOption)
					{
						bool HasPunchListItems = false;

						var CheckListItems = (selectedSeq as Option).checkListItems;

						if (CheckListItems != null)
						{
							HasPunchListItems = CheckListItems.Where(x => x.itemType == ItemType.PunchList).Any();


						}
						if (HasPunchListItems)
						{
							takePictureBtn.Hidden = true;
							btnSelectAll.Hidden = true;

							txtSearchOptions.Hidden = true;
							btnSearchOptions.Hidden = true;
							btnClearSearch.Hidden = true;
							htLblSequence.Constant = 60;
							spSequenceLabel.Constant = -15;
							rhsHeaderHeightConstraint.Constant = 85f;


							CGPoint zero = new CGPoint(0, 0);
							OptionsTableView.SetContentOffset(zero, true);
						}
						else
						{
							takePictureBtn.Hidden = false;
							btnSelectAll.Hidden = true;
							btnClearSearch.Hidden = false;
							spSequenceLabel.Constant = -45;
							htLblSequence.Constant = 120;
							rhsHeaderHeightConstraint.Constant = 110f;
							txtSearchOptions.Hidden = false;
							btnSearchOptions.Hidden = false;


							if (imagesScrollView.Subviews.Count() > 0)
							{
								rhsHeaderHeightConstraint.Constant = 215f;
							}
							CGPoint zero = new CGPoint(0, 0);
							OptionsTableView.SetContentOffset(zero, true);
						}
					}
					else
					{

						takePictureBtn.Hidden = true;
						btnSelectAll.Hidden = false;

						txtSearchOptions.Hidden = true;
						btnSearchOptions.Hidden = true;
						btnClearSearch.Hidden = true;
						htLblSequence.Constant = 60;
						spSequenceLabel.Constant = -15;
						rhsHeaderHeightConstraint.Constant = 85f;
						lblSequence.Text = "Please make a selection";

						CGPoint zero = new CGPoint(0, 0);
						OptionsTableView.SetContentOffset(zero, true);

					}

				}





			}

		}

		public void SetButtonVisibility(ITraversible selectedSeq)
		{
			if ((selectedSeq is ILevel) || (selectedSeq is ISpace) || (selectedSeq is ISequence))
			{
				if ((selectedSeq is ISequence) && (selectedSeq as ISequence).Levels == null && (selectedSeq as ISequence).Options.Count > 0)
				{
					btnNext.Hidden = false;
					btnSave.Hidden = true;
					btnFinish.Hidden = true;
					takePictureBtn.Hidden = true;
				}
				else
				{
					btnFinish.Hidden = true;
					btnNext.Hidden = true;
					btnSave.Hidden = false;
					takePictureBtn.Hidden = true;
				}

			}
			else
			{
				int index = iseq.IndexOf(currSeq);
				if (index == iseq.Count - 1)
				{//last
					btnNext.Hidden = true;
					btnSave.Hidden = true;
					btnFinish.Hidden = false;
					takePictureBtn.Hidden = false;
				}
				else
				{
					if ((selectedSeq is IOption) && (selectedSeq as IOption).getName().Contains("Punch List Items"))
					{
						btnNext.Hidden = false;
						btnSave.Hidden = true;
						btnFinish.Hidden = true;
						takePictureBtn.Hidden = true;
					}
					else
					{
						btnNext.Hidden = false;
						btnSave.Hidden = true;
						btnFinish.Hidden = true;
						takePictureBtn.Hidden = false;
					}
				}
			}
		}



		void SaveSpace_TouchUpInside(object sender1, EventArgs e)
		{
			if (currSeq is ILevel || currSeq is ISpace)
			{

				if (currSeq is ILevel)
				{

					txtSearchOptions.Hidden = true;
					btnSearchOptions.Hidden = true;
					btnClearSearch.Hidden = true;
					htLblSequence.Constant = 60;
					spSequenceLabel.Constant = -15;
					SaveInpsectionOptionResult();

					ResetTheCameraImageView();




					GetTransactions();


					RebuildSequenceTable();




					lblSequence.Text = "Please make a selection";
					btnSelectAll.Hidden = false;
					lblinspectionNameRight.Hidden = true;
					selectNextRow1(false);

					int numberOfRowsVisibleFromTop = 5;
					int index = iseq.IndexOf(currSeq);
					if (index > numberOfRowsVisibleFromTop && index < iseq.Count)
					{
						NSIndexPath indexPath = NSIndexPath.FromRowSection(index, 0);
						sequenceTable.ScrollToRow(indexPath, UITableViewScrollPosition.Bottom, true);
					}

				}
				else if (currSeq is ISpace)
				{

					//var spc = currSeq as Space;

					//var SpaceName = spc.getName ();
					lblSequence.Text = "Please make a selection";
					btnSelectAll.Hidden = false;

					txtSearchOptions.Hidden = true;
					btnSearchOptions.Hidden = true;
					btnClearSearch.Hidden = true;
					htLblSequence.Constant = 60;
					spSequenceLabel.Constant = -15;

					ResetTheCameraImageView();



					SaveInpsectionOptionResult();


					GetTransactions();

					RebuildSequenceTable();


					lblinspectionNameRight.Hidden = true;



					selectNextRow1(false);

					int numberOfRowsVisibleFromTop = 5;
					int index = iseq.IndexOf(currSeq);
					if (index > numberOfRowsVisibleFromTop && index < iseq.Count)
					{
						NSIndexPath indexPath = NSIndexPath.FromRowSection(index, 0);
						sequenceTable.ScrollToRow(indexPath, UITableViewScrollPosition.Bottom, true);
					}


				}
				else
				{
					SaveAndNextAction(true);
				}

			}
			else if (currSeq is ISequence)
			{
				txtSearchOptions.Hidden = true;
				btnSearchOptions.Hidden = true;
				btnClearSearch.Hidden = true;
				htLblSequence.Constant = 60;
				spSequenceLabel.Constant = -15;


				SaveInpsectionOptionResult();

				GetTransactions();



				RebuildSequenceTable();
				selectNextRow1(false);

				int numberOfRowsVisibleFromTop = 5;
				int index = iseq.IndexOf(currSeq);
				if (index > numberOfRowsVisibleFromTop && index < iseq.Count)
				{
					NSIndexPath indexPath = NSIndexPath.FromRowSection(index, 0);
					sequenceTable.ScrollToRow(indexPath, UITableViewScrollPosition.Bottom, true);
				}




			}


		}



		bool CheckValidOptions(List<Option> options)
		{
			bool ValidOptions = false;
			if (options != null && options.Count > 0)
			{
				// validate option check list

				var OptSelections = options.Where(i => i.isSelected == true);

				if (OptSelections != null && OptSelections.Count() > 0)
				{
					ValidOptions = true;


				}

			}
			return ValidOptions;
		}

		void BtnNext_TouchUpInside(object sender, EventArgs e)
		{

			SaveAndNextAction(true);
		}

		public bool selectNextRow1(bool ByPassSelection)
		{
			List<Option> Options = null;

			try
			{
				int index = 0;
				index = iseq.FindIndex(i => i == currSeq);

				if (!ByPassSelection)
				{

					if (index < (iseq.Count - 1))
					{
						if (currSeq is ISequence)
						{
							var Levels = (currSeq as ISequence).Levels;

							if (Levels != null)
							{
								var Levels1 = Levels.Where(x => x.isSelected == true).ToList();

								if (Levels1 != null && Levels1.Count == 0)
								{
									UIAlertView alert1 = new UIAlertView(@"Warning", @"No items are selected, please select at least one item.", null, NSBundle.MainBundle.LocalizedString("OK", "OK"));
									alert1.Show();
									return false;
								}
							}
							else
							{
								Options = (selectedSeq as ISequence).Options;

								if (Options != null)
								{
								}
							}

						}
						else if (currSeq is ISpace)
						{
							Options = (currSeq as ISpace).Options;

							if (Options != null)
							{
								var Options1 = Options.Where(x => x.isSelected == true).ToList();

								if (Options1 != null && Options.Count == 0)
								{
									UIAlertView alert1 = new UIAlertView(@"Warning", @"No items are selected, please select at least one item.", null, NSBundle.MainBundle.LocalizedString("OK", "OK"));
									alert1.Show();
									return false;
								}
							}
						}

						selectedSeq = iseq[index + 1];


					}
					else
					{
						if (selectedSeq != null && selectedSeq != iseq[index])
						{
							selectedSeq = iseq[index];
						}
						else
						{
							selectedSeq = iseq[index];

							if (selectedSeq is ISequence)
							{
								bool selected = false;
								var Levels = (selectedSeq as ISequence).Levels;
								if (Levels != null && Levels.Count > 0)
								{


									foreach (var _Level in Levels)
									{
										if (_Level.isSelected)
										{
											selected = true;
											break;
										}
									}

									if (!selected)
									{

										UIAlertView alert1 = new UIAlertView(@"Warning", @"No items are selected, please select at least one item.", null, NSBundle.MainBundle.LocalizedString("OK", "OK"));
										alert1.Show();
										return false;
									}
								}
								else if (selectedSeq is ISpace)
								{
									Options = (currSeq as ISpace).Options;

									if (Options != null)
									{
										var Options1 = Options.Where(x => x.isSelected == true).ToList();

										if (Options1 != null && Options.Count == 0)
										{
											UIAlertView alert1 = new UIAlertView(@"Warning", @"No items are selected, please select at least one item.", null, NSBundle.MainBundle.LocalizedString("OK", "OK"));
											alert1.Show();
											return false;
										}
									}
								}

							}
							else if (selectedSeq is ILevel)
							{
								bool selected = false;
								var Spaces = (selectedSeq as ILevel).getSpaces();

								if (Spaces != null && Spaces.Count > 0)
								{
									foreach (var _Space in Spaces)
									{
										if (_Space.isSelected)
										{
											selected = true;
											break;
										}
									}
									if (!selected)
									{
										UIAlertView alert1 = new UIAlertView(@"Warning", @"No items are selected, please select at least one item.", null, NSBundle.MainBundle.LocalizedString("OK", "OK"));
										alert1.Show();
										return false;
									}
								}
								else
								{
									Options = (selectedSeq as ILevel).Options;

									if (Options != null && Options.Count > 0)
									{
										foreach (var _Option in Options)
										{
											if (_Option.isSelected)
											{
												selected = true;
												break;
											}
										}
										if (!selected)
										{
											UIAlertView alert1 = new UIAlertView(@"Warning", @"No items are selected, please select at least one item.", null, NSBundle.MainBundle.LocalizedString("OK", "OK"));
											alert1.Show();
											return false;
										}
									}
								}
							}
							else if (selectedSeq is ISpace)
							{
							}
						}


					}
				}

				if (!ByPassSelection)
				{

					selectedSeq = SetCurrentSequence1(selectedSeq);
				}

				if (selectedSeq is ISequence)
				{
					var _Levels = (selectedSeq as ISequence).Levels;
					if (_Levels != null)
					{
						var Levels1 = _Levels.Where(x => x.isSelected == true).ToList();
						if (Levels1 != null && Levels1.Count > 0)
						{
							FillLevelsTable(selectedSeq);

							lblSequence.Text = "Please make a selection";

							btnNext.Hidden = true;
							btnSave.Hidden = false;

							return false;
						}
						else
						{

							btnNext.Hidden = true;
							btnSave.Hidden = false;
							btnSelectAll.Hidden = false;
							lblSequence.Text = "Please make a selection";
							bool bypass = ins.InspectionStarted == 1 ? true : false;
							tableSource = new InspectionSequenceSource(processEnabling(iseq, bypass), this);
							//tableSource = new InspectionSequenceSource (iseq, this); 
							sequenceTable.Source = tableSource;
							sequenceTable.ReloadData();

							UIAlertView alert1 = new UIAlertView(@"Warning", @"No selection has been made. Please make a Selection", null, NSBundle.MainBundle.LocalizedString("OK", "OK"));

							alert1.Show();

						}
					}
					else
					{
						btnNext.Hidden = true;
						btnSave.Hidden = false;
						btnSelectAll.Hidden = false;
						lblSequence.Text = "Please make a selection";
						bool bypass = ins.InspectionStarted == 1 ? true : false;
						tableSource = new InspectionSequenceSource(processEnabling(iseq, bypass), this);
						//tableSource = new InspectionSequenceSource (iseq, this); 
						sequenceTable.Source = tableSource;
						sequenceTable.ReloadData();

						UIAlertView alert1 = new UIAlertView(@"Warning", @"No selection has been made. Please make a Selection", null, NSBundle.MainBundle.LocalizedString("OK", "OK"));

						alert1.Show();
					}


				}

				if (selectedSeq is ILevel)
				{

					var Spaces = (selectedSeq as ILevel).getSpaces().Where(x => x.IsEnabled == true).ToList();

					if (Spaces != null && Spaces.Count() > 0)
					{


						FillSpacesTable(selectedSeq);


						lblSequence.Text = "Please make a selection";
						btnSelectAll.Hidden = false;
						currSeq = selectedSeq;
						SaveInpsectionOptionResult();



						// first space
						btnNext.Hidden = true;
						btnSave.Hidden = false;

						return false;
					}
					else
					{
						Options = (selectedSeq as ILevel).Options;


						if (Options != null && Options.Count > 0)
						{
							FillOptionsTable1(selectedSeq, Options);
							btnNext.Hidden = true;
							btnSave.Hidden = false;
							lblSequence.Text = "Please make a selection";
							currSeq = selectedSeq;

						}
						else
						{


							btnNext.Hidden = true;
							btnSave.Hidden = false;
							btnSelectAll.Hidden = false;

							//tableSource = new InspectionSequenceSource(processEnabling(iseq), this);
							////tableSource = new InspectionSequenceSource (iseq, this); 
							//sequenceTable.Source = tableSource;
							//sequenceTable.ReloadData();


							UIAlertView alert1 = new UIAlertView(@"Warning", @"No selection has been made. Please make a Selection", null, NSBundle.MainBundle.LocalizedString("OK", "OK"));

							alert1.Show();

							index = iseq.FindIndex(i => i == currSeq);

							if (index > 0)
							{
								currSeq = iseq[(index - 1)];

							}
							index = iseq.FindIndex(i => i == currSeq);

							selectedSeq = iseq[index];



							selectNextRow1(true);


							return false;

						}


					}
				}
				if (selectedSeq is ISpace)
				{
					//var spc = selectedSeq as Space;

					//if (spc.isSelected) {
					Options = (selectedSeq as Space).Options.Where(x => x.isEnabled).ToList();

					if (Options != null && Options.Count > 0)
					{
						lblSequence.Text = "Please make a selection";
						btnSelectAll.Hidden = false;

						// fill option table with selections
						FillOptionsTable1(selectedSeq, Options);

						currSeq = selectedSeq;
						SaveInpsectionOptionResult();

						btnNext.Hidden = true;
						btnSave.Hidden = false;
					}
					else
					{

						lblSequence.Text = "Please make a selection";
						btnSelectAll.Hidden = false;

						// fill option table with selections
						FillOptionsTable1(selectedSeq, Options);

						currSeq = selectedSeq;

						btnNext.Hidden = true;
						btnSave.Hidden = false;

						UIAlertView alert1 = new UIAlertView(@"Warning", @"No selection has been made. Please make a Selection", null, NSBundle.MainBundle.LocalizedString("OK", "OK"));

						alert1.Show();
					}



					return false;
				}
				if (selectedSeq is IOption)
				{

					btnSelectAll.Hidden = true;
					txtSearchOptions.Hidden = false;
					btnSearchOptions.Hidden = false;
					btnClearSearch.Hidden = false;
					spSequenceLabel.Constant = -45;
					htLblSequence.Constant = 120;
					btnSearchOptions.TouchUpInside -= BtnSearchOptions_TouchUpInside;
					btnSearchOptions.TouchUpInside += BtnSearchOptions_TouchUpInside;
					txtSearchOptions.EditingDidEndOnExit -= TxtSearchOptions_EditingDidEnd;
					txtSearchOptions.EditingDidEndOnExit += TxtSearchOptions_EditingDidEnd;
					btnClearSearch.TouchUpInside -= BtnClearSearch_TouchUpInside;
					btnClearSearch.TouchUpInside += BtnClearSearch_TouchUpInside;

					lblSequence.Text = (selectedSeq as IOption).getName();

					lblinspectionNameRight.Hidden = false;

					if ((selectedSeq as Option).ID == BALConstant.GUIDEDPICTURE_OPTIONID)
					{
						txtSearchOptions.Hidden = true;
						btnSearchOptions.Hidden = true;
						btnClearSearch.Hidden = true;
						htLblSequence.Constant = 60;
						spSequenceLabel.Constant = -15;
						rhsHeaderHeightConstraint.Constant = 85f;
						btnSearchOptions.TouchUpInside -= BtnSearchOptions_TouchUpInside;
						btnClearSearch.TouchUpInside -= BtnClearSearch_TouchUpInside;
					}
					else if ((selectedSeq as Option).ID == BALConstant.PUNCH_OPTIONID)
					{
						txtSearchOptions.Hidden = true;
						btnSearchOptions.Hidden = true;
						btnClearSearch.Hidden = true;
						htLblSequence.Constant = 60;
						spSequenceLabel.Constant = -15;
						rhsHeaderHeightConstraint.Constant = 85f;
						txtSearchOptions.Hidden = true;
						btnSearchOptions.Hidden = true;
						btnClearSearch.Hidden = true;
						htLblSequence.Constant = 60;
						spSequenceLabel.Constant = -15;
						rhsHeaderHeightConstraint.Constant = 85f;
						btnSearchOptions.TouchUpInside -= BtnSearchOptions_TouchUpInside;
						btnClearSearch.TouchUpInside -= BtnClearSearch_TouchUpInside;
					}

					FillInspectionItem(selectedSeq);



					buttonVisibility();

					takePictureBtn.Hidden = false;

					if (imagesScrollView.Subviews.Count() > 0)
					{
						rhsHeaderHeightConstraint.Constant = 215f;
					}
					CGPoint zero = new CGPoint(0, 0);
					OptionsTableView.SetContentOffset(zero, true);
				}


			}
			catch (Exception ex)
			{
				Debug.WriteLine("Error occured at method selectNextRow1 due to " + ex.Message);
			}
			return false;
		}

		ITraversible SetCurrentSequence1(ITraversible selectedSeq)
		{
			try
			{
				int nextTraversibleIndex = 0;
				if (selectedSeq is ISequence || selectedSeq is ISpace || (selectedSeq is ILevel && (selectedSeq as Level).Options != null && (selectedSeq as Level).Options.Count > 0))
				{
					nextTraversibleIndex = iseq.FindIndex(traversible => traversible == selectedSeq);
					if (nextTraversibleIndex < (iseq.Count - 1))
					{
						nextTraversibleIndex = nextTraversibleIndex + 1;
					}
					if (selectedSeq is IOption)
					{
						if (iseq[nextTraversibleIndex] != null)
						{
							selectedSeq = iseq[nextTraversibleIndex];

						}
						if (selectedSeq is IOption)
						{
							selectedSeq = SetCurrentSequence1(selectedSeq);
						}
						for (int i = 0; i < nextTraversibleIndex; i++)
						{
							iseq[i].prevSeqNextClicked = true;
						}

					}
					else
					{


					}
					bool bypass = ins.InspectionStarted == 1 ? true : false;
					tableSource = new InspectionSequenceSource(processEnabling(iseq, bypass), this);
					//tableSource = new InspectionSequenceSource (iseq, this); 
					sequenceTable.Source = tableSource;
					sequenceTable.ReloadData();
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Exception Occured in SetCurrentSequence method due to " + ex.Message);
			}
			return selectedSeq;
		}

		private void SaveAndNextAction(bool BypassChecklistItemCheck)
		{
			if (currSeq is IOption)
			{
				txtSearchOptions.Hidden = false;
				btnSearchOptions.Hidden = false;
				btnClearSearch.Hidden = false;
				spSequenceLabel.Constant = -45;
				htLblSequence.Constant = 120;
			}
			else
			{
				txtSearchOptions.Hidden = true;
				btnSearchOptions.Hidden = true;
				btnClearSearch.Hidden = true;
				htLblSequence.Constant = 60;
				spSequenceLabel.Constant = -15;
			}
			// set top position of sequence label



			btnSearchOptions.TouchUpInside -= BtnSearchOptions_TouchUpInside;
			btnSearchOptions.TouchUpInside += BtnSearchOptions_TouchUpInside;
			txtSearchOptions.EditingDidEndOnExit -= TxtSearchOptions_EditingDidEnd;
			txtSearchOptions.EditingDidEndOnExit += TxtSearchOptions_EditingDidEnd;
			btnClearSearch.TouchUpInside -= BtnClearSearch_TouchUpInside;
			btnClearSearch.TouchUpInside += BtnClearSearch_TouchUpInside;

			SaveInpsectionOptionResult();
			int index1;

			ResetTheCameraImageView();



			GetTransactions();

			RebuildSequenceTable();

			if (currSeq is IOption)
			{
				//AddPhotosToCurrentSequence(currSeq);
				index1 = iseq.FindIndex(i => i == currSeq);
				if ((currSeq as IOption).prevSeqNextClicked == true && (currSeq as Option).InspectionID == "7")
				{
					var CheckListItems = (currSeq as Option).checkListItems;
					if (CheckListItems != null)
					{
						var HasPunchListItems = CheckListItems.Where(x => x.itemType == ItemType.PunchList).Any();

						if (HasPunchListItems)
						{
							if (index1 < (iseq.Count - 1))
							{
								currSeq = iseq[index1 + 1];
							}
						}
					}
				}

			}



			bool ByPassed = selectNextRow(BypassChecklistItemCheck);


			//Autoscroll sequence table
			int numberOfRowsVisibleFromTop = 1;
			int index = iseq.IndexOf(currSeq);
			if (index > numberOfRowsVisibleFromTop && index < iseq.Count)
			{
				NSIndexPath indexPath = NSIndexPath.FromRowSection(index, 0);
				sequenceTable.ScrollToRow(indexPath, UITableViewScrollPosition.Bottom, true);
			}



			//			NSIndexPath optionsTableIndexpath = NSIndexPath.FromRowSection (0, 0);
			//			OptionsTableView.ScrollToRow (optionsTableIndexpath, UITableViewScrollPosition.Top, true);


			if (!ByPassed)
			{
				SetHeaderLayout(currSeq);


				if (imagesScrollView.Subviews.Count() > 0)
				{
					rhsHeaderHeightConstraint.Constant = 215f;
				}


				CGPoint zero = new CGPoint(0, 0);
				OptionsTableView.SetContentOffset(zero, true);
			}

		}

		void BtnClearSearch_TouchUpInside(object sender, EventArgs e)
		{
			InCheckListSearch = false;
			CheckListFilterCount = 0;
			// reload non- filtered checklist
			LoadOptionImages();
			ClearOptionsTableView();
			InspectionItemSource optionsSource = new InspectionItemSource(currSeq as Option, this, OptionsTableView, false);


			OptionsTableView.Source = optionsSource;


			OptionsTableView.AllowsSelection = false;
			OptionsTableView.ReloadData();

			var VisibleIndexes = OptionsTableView.IndexPathsForVisibleRows;

			if (VisibleIndexes != null)
			{

			}
			OptionsTableView.TableFooterView = new UIView(new CoreGraphics.CGRect(0, 0, 0, 0));
		}

		public void SetCheckListItems(List<CheckList> _checkListItems)
		{
			this.checkListItems = _checkListItems;
		}


		private void FilterCheckListItems(string text)
		{
			Model.ITraversible selSeq;
			List<CheckList> FilteredCheckListItems = null;

			if (!InCheckListSearch)
				return;

			// traverse through sequences until cehcklist item is found
			for (int i = 0; i < iseq.Count; i++)
			{
				try
				{
					selSeq = (ITraversible)iseq[i];

					if (this.checkListItems != null)
					{

						var checkListItems = this.checkListItems;

						if (checkListItems != null)
						{

							if (selSeq != null && selSeq is IOption)
							{

								FilteredCheckListItems = checkListItems.Where(x => !string.IsNullOrEmpty(x.description) && x.description.ToUpper().Contains(text.ToUpper())).ToList();

								if (FilteredCheckListItems != null && FilteredCheckListItems.Count > 0)
								{
									LoadOptionImages();
									ClearOptionsTableView();

									// reload OptionsTableView with new list
									InspectionItemSource optionsSource = new InspectionItemSource(selSeq as Option, this, OptionsTableView, true) { checkListItems = FilteredCheckListItems };

									//optionsSource.CheckListCommentsActiveItems = CheckListCommentsActiveItems;
									OptionsTableView.Source = optionsSource;


									OptionsTableView.AllowsSelection = false;
									OptionsTableView.ReloadData();

									var VisibleIndexes = OptionsTableView.IndexPathsForVisibleRows;

									if (VisibleIndexes != null)
									{
										// clean up
										OptionsTableView.ReloadRows(VisibleIndexes, UITableViewRowAnimation.None);
									}


									OptionsTableView.TableFooterView = new UIView(new CoreGraphics.CGRect(0, 0, 0, 0));
									// jump to first row
									JumpToCheckListItem(text);



									txtSearchOptions.Text = "";
									//return;
									break;
								}

							}
						}
					}

				}
				catch (Exception ex)
				{
					Debug.WriteLine("Error occured at method JumpToCheckListItem due to " + ex.Message);
				}
			}
		}

		private void JumpToCheckListItem(string text)
		{

			OptionsTableView.ScrollRectToVisible(new CGRect(0, 0, 1, 1), false);


		}

		void TxtSearchOptions_EditingDidEnd(object sender, EventArgs e)
		{
			var SearchText = txtSearchOptions.Text;

			if (!string.IsNullOrEmpty(SearchText))
			{
				var Item = SearchForOptionDesc(SearchText);

				if (Item != null)
				{

					// filter checklist item to serach text
					InCheckListSearch = true;

					FilterCheckListItems(SearchText);


				}
				else
				{
					txtSearchOptions.Text = "";
					UIAlertView alert = new UIAlertView(@"Warning", @"Check List Item Not Found.", null, NSBundle.MainBundle.LocalizedString("OK", "OK"));
					alert.Show();

				}
			}
			else
			{


			}
		}

		void BtnSearchOptions_TouchUpInside(object sender, EventArgs e)
		{

			var SearchText = txtSearchOptions.Text;

			if (!string.IsNullOrEmpty(SearchText))
			{
				var Item = SearchForOptionDesc(SearchText);
				if (Item != null)
				{
					//
					InCheckListSearch = true;

					FilterCheckListItems(SearchText);
				}
				else
				{
					UIAlertView alert = new UIAlertView(@"Warning", @"Check List Item Not Found.", null, NSBundle.MainBundle.LocalizedString("OK", "OK"));
					alert.Show();
				}
			}
			else
			{
				UIAlertView alert = new UIAlertView(@"Warning", @"Please Enter Some Text.", null, NSBundle.MainBundle.LocalizedString("OK", "OK"));
				alert.Show();
			}
		}

		private CheckList SearchForOptionDesc(string text)
		{
			var checkListItems = (currSeq as IOption).getCheckListItems();

			if (checkListItems != null)
			{

				try
				{

					var Item = checkListItems.Where(x => !string.IsNullOrEmpty(x.description) && x.description.ToUpper().Contains(text.ToUpper())).FirstOrDefault();

					if (Item != null)
					{
						return Item;
					}
				}
				catch (Exception ex)
				{
					Debug.WriteLine("Exception Occured in SearchForOptionDesc method due to " + ex.Message);
				}
			}

			return null;
		}



		public void RebuildSequenceTable()
		{
			rebuildSequence();
			bool bypass = ins.InspectionStarted == 1 ? true : false;
			tableSource = new InspectionSequenceSource(processEnabling(iseq, bypass), this);
			sequenceTable.Source = tableSource;
			sequenceTable.ReloadData();
		}

		public void buttonVisibility()
		{
			if ((currSeq is ILevel) || (currSeq is ISpace))
			{
				int index = iseq.IndexOf(currSeq);
				btnNext.Hidden = true;
				if (index == iseq.Count - 1)
				{//last
					btnSave.Hidden = true;
					btnFinish.Hidden = false;
				}
				else
				{
					btnSave.Hidden = false;
					btnFinish.Hidden = true;
				}
			}
			else
			{
				int index = iseq.IndexOf(currSeq);
				if (index == iseq.Count - 1)
				{//last
					btnNext.Hidden = true;
					btnSave.Hidden = true;
					btnFinish.Hidden = false;
				}
				else
				{
					btnNext.Hidden = false;
					btnSave.Hidden = true;
					btnFinish.Hidden = true;
				}
			}
		}

		public void SetSeqLabel(string text)
		{
			lblSequence.Text = text;
			SeqName = lblSequence.Text;
		}

		public void SetHeaderLayout(ITraversible selectedSeq)
		{
			if (selectedSeq != null)
			{
				if (selectedSeq is IOption)
				{
					txtSearchOptions.Hidden = false;
					btnSearchOptions.Hidden = false;
					btnClearSearch.Hidden = false;
					spSequenceLabel.Constant = -45;
					htLblSequence.Constant = 120;
				}
				else
				{
					txtSearchOptions.Hidden = true;
					btnSearchOptions.Hidden = true;
					btnClearSearch.Hidden = true;
					htLblSequence.Constant = 60;
					spSequenceLabel.Constant = -15;
				}
			}
		}

		public bool buttonStyleRefresh(ITraversible selectedSeq)
		{
			List<CheckList> checkListItems = null;
			List<OptionImage> image = null;
			bool isValidCondition = false;
			bool AllNAResults = false;
			bool ALLNAResult = false;
			if (currSeq is IOption)
			{
				if (selectedSeq != null)
				{
					Option opt = (Option)selectedSeq;
					checkListItems = (selectedSeq as IOption).getCheckListItems();
					image = opt.photos;
				}
				else
				{
					Option opt = (Option)currSeq;
					checkListItems = (currSeq as IOption).getCheckListItems();
					image = opt.photos;
				}

				//If I select N/A for all my checkList items on any given page, pictures should not be required
				if (checkListItems != null && checkListItems.Count > 0)
				{
					var NAResults = checkListItems.Where(CL => CL.Result != ResultType.NA && CL.itemType != ItemType.PunchList);

					if (NAResults != null && NAResults.Count() == 0)
					{

						AllNAResults = true;




					}
				}
			}

			if (checkListItems != null && checkListItems.Count > 0)
			{
				foreach (var checkList in checkListItems)
				{
					if (checkList.itemType == ItemType.PunchList)
					{
						if (checkList.photos == null)
						{
							isValidCondition = true;
							break;
						}
						else if (checkList.photos.Count <= 0)
						{
							isValidCondition = true;
							break;
						}

						if ((int)checkList.Result == 1)
						{
							isValidCondition = true;
							break;
						}
					}
					else if (checkList.itemType == ItemType.GuidedPicture)
					{
						if (checkList.photos == null)
						{
							isValidCondition = true;
							break;
						}
						else if (checkList.photos.Count <= 0)
						{
							isValidCondition = true;
							break;
						}
					}
					else
					{
						if (!AllNAResults)
						{
							if (image == null || image.Count <= 0)
							{
								if ((int)checkList.Result == 0)
								{
								}
								else
								{
									isValidCondition = true;
									break;
								}

							}


						}
						else
						{
							ALLNAResult = true;




						}
					}
				}
			}

			if (isValidCondition)
			{
				DisableNext();
				return false;
			}

			EnableNextStyle();



			return ALLNAResult;
		}

		private void DisableNext()
		{
			btnNext.Enabled = false;
			btnFinish.Enabled = false;
			btnNext.BackgroundColor = UIColor.White;
			btnSave.BackgroundColor = UIColor.White;
			btnFinish.BackgroundColor = UIColor.White;
			btnNext.SetTitleColor(UIColor.FromRGB(18, 74, 143), UIControlState.Normal);
			btnSave.SetTitleColor(UIColor.FromRGB(18, 74, 143), UIControlState.Normal);
			btnFinish.SetTitleColor(UIColor.FromRGB(18, 74, 143), UIControlState.Normal);
			btnNext.Layer.CornerRadius = 5f;
			btnNext.Layer.BorderColor = UIColor.FromRGB(18, 74, 143).CGColor;
			btnNext.Layer.BorderWidth = 0.5f;
			btnSave.Layer.CornerRadius = 5f;
			btnSave.Layer.BorderColor = UIColor.FromRGB(18, 74, 143).CGColor;
			btnSave.Layer.BorderWidth = 0.5f;
			btnFinish.Layer.CornerRadius = 5f;
			btnFinish.Layer.BorderColor = UIColor.FromRGB(18, 74, 143).CGColor;
			btnFinish.Layer.BorderWidth = 0.5f;
		}

		private void EnableNextStyle()
		{
			btnNext.Enabled = true;
			btnSave.Enabled = true;
			btnFinish.Enabled = true;
			btnNext.BackgroundColor = UIColor.FromRGB(18, 74, 143);
			btnSave.BackgroundColor = UIColor.FromRGB(18, 74, 143);
			btnFinish.BackgroundColor = UIColor.FromRGB(18, 74, 143);
			btnNext.SetTitleColor(UIColor.White, UIControlState.Normal);
			btnSave.SetTitleColor(UIColor.White, UIControlState.Normal);
			btnFinish.SetTitleColor(UIColor.White, UIControlState.Normal);
		}

		public void clearPhotoBuffer()
		{
			using (optionTransactionService = new OptionTransactionService(AppDelegate.DatabaseContext))
			{
				if (punchItems != null)
				{
					punchItems.ForEach(i => i = null);
					punchItems.Clear();
					punchItems = null;
				}
				if (MediaLst != null)
				{
					foreach (var view in MediaLst)
					{

						view.RemoveFromSuperview();
						view.Dispose();
					}
					MediaLst.Clear();
				}
				if (ImageLst != null)
				{
					foreach (var view in ImageLst)
					{
						view.Image = null;
						view.RemoveFromSuperview();
						view.Dispose();
					}
					ImageLst.Clear();
				}
				if (ins.locationIDImages != null)
				{
					ins.locationIDImages.Clear();
				}
				if (Source != null)
				{
					Source.Dispose();
					Source = null;
				}

				if (imageCollectionView != null)
				{
					foreach (var view in imageCollectionView.Subviews)
					{
						if (view is UIImageView)
						{
							(view as UIImageView).Image = null;
						}
						view.RemoveFromSuperview();
						view.Dispose();
					}
					imageCollectionView.Source = null;
					imageCollectionView.Delegate = null;
					imageCollectionView.DataSource = null;
					imageCollectionView.WeakDelegate = null;
					imageCollectionView.WeakDataSource = null;
				}

				if (imagesScrollView != null)
				{
					foreach (var view in imagesScrollView.Subviews)
					{
						if (view is UIImageView)
						{
							(view as UIImageView).Image = null;
						}
						view.RemoveFromSuperview();
						view.Dispose();
					}
				}

				if (currSeq is ITraversible)
				{
					optionTransactionService.clearPhotoBuffer(ins, currSeq.getSequenceID());

				}


			}

		}

		public void AddPhotosToCurrentSequence(ITraversible seq)
		{
			if (currSeq is IOption)
			{
				var opt = currSeq as Option;
				(currSeq as Option).photos = new List<OptionImage>();
				using (var optService = new OptionTransactionService(AppDelegate.DatabaseContext))
				{
					if (opt.ID == BALConstant.GUIDEDPICTURE_OPTIONID)
					{
						foreach (var chk in opt.checkListItems)
						{
							if (chk.CheckListTransID > 0)
							{
								var images = GuildedPhotoDO.getGuidedImageList(AppDelegate.DatabaseContext, chk.CheckListTransID).Select(i => i.Image);
								if (images != null && images.Count() > 0)
								{
									chk.photos = images.ToList();
								}
							}
						}
					}
					else if (opt.ID == BALConstant.PUNCH_OPTIONID)
					{
						foreach (var chk in opt.checkListItems)
						{
							var images = PunchListImageDO.getPunchListImageList(AppDelegate.DatabaseContext, ins.ID, chk.PunchID).Select(i => i.PunchListImage);
							if (images != null && images.Count() > 0)
							{
								chk.photos = images.ToList();
							}
						}
					}
					else
					{
						using (var imageservice = new OptionImageService(AppDelegate.DatabaseContext))
						{
							if (opt.OptionTransactionID > 0)
							{
								var transResult = imageservice.GetOptionTransactionImage(opt.OptionTransactionID);
								if (transResult != null && transResult.Count() > 0)
								{
									foreach (var res in transResult)
									{
										(currSeq as Option).photos.Add(new OptionImage()
										{
											Image = res.Image,
											OptionTransID = res.OptionTransID
										});
									}
								}
							}
						}
					}
				}
				buttonStyleRefresh(null);
			}
		}

		public bool selectNextRow(bool BypassCheckListItemsCheck)
		{
			int index = 0;
			index = iseq.FindIndex(i => i == currSeq);
			ITraversible selectedSeq;
			ITraversible selectedSeq1;
			bool IsAllCheckListItemsNA = false;
			bool IsJumpToByPassed = false;
			List<Option> Options = null;

			selectedSeq1 = iseq[index];

			if (!(selectedSeq1 is ILevel))
			{
				if (BypassCheckListItemsCheck)
				{
					if (buttonStyleRefresh(selectedSeq1) == true)
					{
						IsJumpToByPassed = true;

						IsAllCheckListItemsNA = true;
						int buttonClicked = -1;

						UIAlertView alert1 = new UIAlertView(@"Warning", @"No selection has been made. Proceed anyway?", null, NSBundle.MainBundle.LocalizedString("YES", "YES"), NSBundle.MainBundle.LocalizedString("NO", "NO"));

						alert1.Show();


						alert1.Clicked += (sender, buttonArgs) =>
						{
							buttonClicked = (int)buttonArgs.ButtonIndex;




						};



						//Wait for a button press.
						while (buttonClicked == -1)
						{

							if (selectedSeq1 is IOption)
							{
								if (rhsHeaderHeightConstraint.Constant != 110f)
								{
									rhsHeaderHeightConstraint.Constant = 110f;
								}

								if (imagesScrollView.Subviews.Count() > 0)
								{
									if (rhsHeaderHeightConstraint.Constant != 215f)
									{
										rhsHeaderHeightConstraint.Constant = 215f;
									}
								}
							}
							else
							{
								rhsHeaderHeightConstraint.Constant = 85f;
							}
							NSRunLoop.Current.RunUntil(NSDate.FromTimeIntervalSinceNow(0.5));
						}

						// wait for button press
						if (buttonClicked == 1)
						{

							selectedSeq = iseq[index];
							if (selectedSeq is IOption)
							{
								txtSearchOptions.Hidden = false;
								btnSearchOptions.Hidden = false;
								btnClearSearch.Hidden = false;
								spSequenceLabel.Constant = -45;
								htLblSequence.Constant = 120;
								rhsHeaderHeightConstraint.Constant = 110f;

								if (imagesScrollView.Subviews.Count() > 0)
								{
									rhsHeaderHeightConstraint.Constant = 215f;
								}

							}
							else
							{
								txtSearchOptions.Hidden = true;
								btnSearchOptions.Hidden = true;
								btnClearSearch.Hidden = true;
								htLblSequence.Constant = 60;
								spSequenceLabel.Constant = -15;
								rhsHeaderHeightConstraint.Constant = 85f;
							}
							clearPhotoBuffer();
							GetTransactions();

							AddPhotosToCurrentSequence(selectedSeq);

							LoadOptionImages();

							return true;
						}
						else
						{


							ResetTheCameraImageView();
							selectedSeq = iseq[index];





							RebuildSequenceTable();



							bool result = selectNextRow(false);

							return result;

						}

					}
				}
			}
			if (!IsAllCheckListItemsNA)
			{
				bool bypassnextItemSelect = false;

				bool FinalItemSelected = false;

				if (index < (iseq.Count - 1))
				{

					selectedSeq = iseq[index + 1];

					if (selectedSeq is ISequence)
					{
						var Levels = (selectedSeq as ISequence).Levels;

						if (Levels != null && Levels.Count > 0)
						{
							bypassnextItemSelect = true;
							FinalItemSelected = true;
						}
						else
						{
							var HasPunchListItems = false;
							Options = (selectedSeq as ISequence).Options;

							if (Options != null && Options.Count > 0)
							{
								foreach (var _Option in Options)
								{
									if (_Option.checkListItems != null)
									{
										HasPunchListItems = _Option.checkListItems.Where(x => x.itemType == ItemType.PunchList).Any();

										if (HasPunchListItems)
										{
											break;
										}
									}
								}
							}
							if (!HasPunchListItems)
							{
								//bypassnextItemSelect = true;
							}

						}
					}

					if (selectedSeq is ISpace)
					{
						var Opts = (selectedSeq as ISpace).Options;

						if (Opts != null && Opts.Count() > 0)
						{
							bypassnextItemSelect = true;


						}
					}
					else if (selectedSeq is ILevel)
					{
						var Spaces = (selectedSeq as ILevel).getSpaces();

						if (Spaces != null && Spaces.Count() > 0)
						{
							bypassnextItemSelect = true;

						}
					}
					else if (selectedSeq is IOption)
					{
						if (selectedSeq1 != selectedSeq)
						{
							bypassnextItemSelect = true;
							FinalItemSelected = true;
						}
					}

				}
				else
				{
					selectedSeq = iseq[index];
				}


				if (!bypassnextItemSelect)
				{

					selectedSeq = SetCurrentSequence(selectedSeq);
				}


				clearOptionsTable();
				SetHeaderLayout(selectedSeq);



				if (selectedSeq is ISpace)
				{
					var Opts = (selectedSeq as ISpace).Options;

					if (Opts != null && Opts.Count() > 0)
					{
						FinalItemSelected = true;
						IsJumpToByPassed = true;

					}
				}
				else if (selectedSeq is ILevel)
				{
					var Spaces = (selectedSeq as ILevel).getSpaces();

					if (Spaces != null && Spaces.Count() > 0)
					{
						FinalItemSelected = true;
						IsJumpToByPassed = true;
					}
					else
					{
						Options = (selectedSeq as ILevel).Options;

						if (Options != null && Options.Count > 0)
						{
							FinalItemSelected = true;
							IsJumpToByPassed = true;
						}
					}
				}
				else if (selectedSeq is IOption)
				{

				}

				if (!FinalItemSelected)
				{
					int index1 = iseq.IndexOf(selectedSeq);


					if (index1 == iseq.Count - 1)
					{//last
						txtSearchOptions.Hidden = false;
						btnSearchOptions.Hidden = false;
						btnClearSearch.Hidden = false;
						spSequenceLabel.Constant = -45;
						htLblSequence.Constant = 120;
						btnFinish.Hidden = false;
						btnSave.Hidden = true;
						btnNext.Hidden = true;
						rhsHeaderHeightConstraint.Constant = 110f;

						if (imagesScrollView.Subviews.Count() > 0)
						{
							rhsHeaderHeightConstraint.Constant = 215f;
						}





						return true;
					}
				}

				if (selectedSeq is ISequence)
				{
					var Levels = (selectedSeq as ISequence).Levels;

					if (Levels != null && Levels.Count > 0)
					{
						txtSearchOptions.Hidden = true;
						btnSearchOptions.Hidden = true;
						btnClearSearch.Hidden = true;
						htLblSequence.Constant = 60;
						spSequenceLabel.Constant = -15;
						rhsHeaderHeightConstraint.Constant = 85f;
						FillLevelsTable(selectedSeq);
						currSeq = selectedSeq;

						lblSequence.Text = "Please make a selection";
						btnSelectAll.Hidden = false;

					}
					else
					{

					}
					SetButtonVisibility(selectedSeq);


				}

				if (selectedSeq is ILevel)
				{

					Options = (selectedSeq as ILevel).Options;
					if (Options != null && Options.Count > 0)
					{
						FillOptionsTable1(selectedSeq, Options);
						txtSearchOptions.Hidden = true;
						btnSearchOptions.Hidden = true;
						btnClearSearch.Hidden = true;
						htLblSequence.Constant = 60;
						spSequenceLabel.Constant = -15;
						rhsHeaderHeightConstraint.Constant = 85f;


						currSeq = selectedSeq;
						btnSelectAll.Hidden = false;



						lblSequence.Text = "Please make a selection";
					}
					else
					{
						var Spaces = (selectedSeq as ILevel).getSpaces();

						if (Spaces != null && Spaces.Count > 0)
						{
							txtSearchOptions.Hidden = true;
							btnSearchOptions.Hidden = true;
							btnClearSearch.Hidden = true;
							htLblSequence.Constant = 60;
							spSequenceLabel.Constant = -15;
							rhsHeaderHeightConstraint.Constant = 85f;

							FillSpacesTable(selectedSeq);
							currSeq = selectedSeq;

							btnSelectAll.Hidden = false;

							lblSequence.Text = "Please make a selection";

						}

					}
					lblinspectionNameRight.Hidden = true;

					SetButtonVisibility(selectedSeq);

					buttonStyleRefresh(null);
					return false;
				}


				takePictureBtn.Hidden = true;
				imagesScrollView.Hidden = true;




				if (selectedSeq is ISpace)
				{

					Options = (selectedSeq as ISpace).Options;
					if (Options != null && Options.Count > 0)
					{

						lblinspectionNameRight.Hidden = true;
						txtSearchOptions.Hidden = true;
						btnSearchOptions.Hidden = true;
						btnClearSearch.Hidden = true;
						htLblSequence.Constant = 60;
						spSequenceLabel.Constant = -15;
						rhsHeaderHeightConstraint.Constant = 85f;



						lblSequence.Text = "Please make a selection";

						btnSelectAll.Hidden = false;

						// fill option table with selections
						FillOptionsTable1(selectedSeq, Options);


						SetButtonVisibility(selectedSeq);

					}
					else
					{

					}

					return IsJumpToByPassed;
				}

				if (selectedSeq is IOption)
				{


					//GetTransactions();
					takePictureBtn.Hidden = false;

					lblinspectionNameRight.Hidden = false;
					btnSelectAll.Hidden = true;
					if ((selectedSeq as Option).ID == BALConstant.GUIDEDPICTURE_OPTIONID)
					{
						txtSearchOptions.Hidden = true;
						btnSearchOptions.Hidden = true;
						btnClearSearch.Hidden = true;
						htLblSequence.Constant = 60;
						spSequenceLabel.Constant = -15;
						rhsHeaderHeightConstraint.Constant = 85f;
					}
					else if ((selectedSeq as Option).ID == BALConstant.PUNCH_OPTIONID)
					{
						txtSearchOptions.Hidden = true;
						btnSearchOptions.Hidden = true;
						btnClearSearch.Hidden = true;
						htLblSequence.Constant = 60;
						spSequenceLabel.Constant = -15;
						rhsHeaderHeightConstraint.Constant = 85f;
					}

					FillInspectionItem(selectedSeq);
					buttonVisibility();
				}
				else
				{
					if (!(selectedSeq is ISequence))
					{
						UpdateRightTableView(selectedSeq);
					}
					else
					{
						punchAddItemBtn.Hidden = true;
						punchFinishButton.Hidden = true;
						lblSequence.Hidden = false;
						OptionsTableView.Hidden = false;
						takePictureBtn.Hidden = true;
						imagesScrollView.Hidden = true;
					}
					SetButtonVisibility(selectedSeq);
				}
				//buttonVisibility ();
				if (!IsAllCheckListItemsNA)
				{
					buttonStyleRefresh(null);
				}

			}


			return IsJumpToByPassed;
		}

		private List<CheckListTransaction> GetCheckListItems(List<CheckList> checkList)
		{
			List<CheckListTransaction> checkListTransactions = new List<CheckListTransaction>();
			if (checkList != null && checkList.Count > 0)
			{
				foreach (var checkItem in checkList)
				{
					checkListTransactions.Add(new CheckListTransaction()
					{
						comments = checkItem.comments,
						result = (int)checkItem.Result,
						CheckListID = checkItem.ID,
						ID = checkItem.CheckListTransID,
						PunchID = checkItem.PunchID,
						GuidedPictures = checkItem.photos,
						itemType = checkItem.itemType
					});
				}
			}
			return checkListTransactions;
		}

		public void ClearOptionsTableView()
		{
			try
			{
				var IndexPaths = OptionsTableView.IndexPathsForVisibleRows;

				if (IndexPaths != null && IndexPaths.Count() > 0)
				{
					Debug.WriteLine("ClearOptionsTableView - clearing OptionsTableView ");
					var source = (OptionsTableView.Source as InspectionItemSource);

					if (source != null)
					{
						if ((source as InspectionItemSource).checkListItems != null && (source as InspectionItemSource).checkListItems.Count() > 0)
						{


							source.Dispose();
							source = null;
							GC.Collect();
							OptionsTableView.Source = source;
							OptionsTableView.ReloadData();
						}

					}
					var source1 = (OptionsTableView.Source as InspectionLevelSource);

					if (source1 != null)
					{
						source1.Dispose();
						source1 = null;
						GC.Collect();
						OptionsTableView.Source = source1;
						OptionsTableView.ReloadData();
					}
					var source2 = (OptionsTableView.Source as InspectionSpaceSource);

					if (source2 != null)
					{
						source2.Dispose();
						source2 = null;
						GC.Collect();
						OptionsTableView.Source = source2;
						OptionsTableView.ReloadData();
					}
					var source3 = (OptionsTableView.Source as InspectionOptionSource);

					if (source3 != null)
					{
						source3.Dispose();
						source3 = null;
						GC.Collect();
						OptionsTableView.Source = source3;
						OptionsTableView.ReloadData();
					}

					var source4 = (OptionsTableView.Source as InspectionSequenceSource);

					if (source3 != null)
					{
						source4.Dispose();
						source4 = null;
						GC.Collect();
						OptionsTableView.Source = source4;
						OptionsTableView.ReloadData();
					}

				}


				Debug.WriteLine("ClearOptionsTableView - OptionsTableView cleared ");




			}
			catch (Exception ex)
			{
				Debug.WriteLine("Exception Occured in ClearOptionsTableView method due to " + ex.Message);
			}
		}




		public void FillOptionsTable1(ITraversible selectedSeq, List<Option> options)
		{
			this.OptionsTableView.EndEditing(true);
			lblinspectionNameRight.Text = InspectionData.InspectionType;
			try
			{
				ClearOptionsTableView();

				InspectionOptionSource inspectionOptionSource = new InspectionOptionSource(selectedSeq, this, OptionsTableView, options);
				//InspectionSpaceSource spaceSource = new InspectionSpaceSource (selectedSeq, this, optionsSelectTable);
				OptionsTableView.Source = inspectionOptionSource;
				OptionsTableView.RowHeight = 50;
				OptionsTableView.BackgroundColor = UIColor.Clear;

				OptionsTableView.TableFooterView = new UIView(new CoreGraphics.CGRect(0, 0, 0, 0));
				currSeq = selectedSeq;
				OptionsTableView.ReloadData();

			}
			catch (Exception ex)
			{
				Debug.WriteLine("Exception Occured in FillOptionsTable method due to " + ex.Message);
			}
		}

		void auoSave()
		{
			try
			{
				// run SaveInpsectionOptionResult in background thread

				SaveInpsectionOptionResult();


				GC.Collect();
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Exception Occured in auoSave method due to " + ex.Message);
			}

		}

		private int SaveInpsectionOptionResult()
		{


			int result = 0;
			//List<Option> Options = null;
			lock (SaveLock)
			{
				try
				{
					if (currSeq != null)
					{

						if (currSeq is IOption)
						{
							List<OptionTransaction> optTrans = new List<OptionTransaction>();
							Option option = (currSeq as Option);
							using (optionTransactionService = new OptionTransactionService(AppDelegate.DatabaseContext))
							{

								var ExistingOpt = optionTransactionService.GetOptionTransaction(option.OptionId);
								if (ExistingOpt != null)
								{

									optionTransactionService.DeleteOptionTransactions(ExistingOpt);
									CheckListTransactionDO.DeletecheckList(AppDelegate.DatabaseContext, ExistingOpt.ID);
									OptionTransactionImageDO.DeleteOptionImagesSync(AppDelegate.DatabaseContext, ExistingOpt.ID);


								}


								var data2 = optionTransactionService.GetOptionTransactions();
								optTrans.Add(new OptionTransaction()
								{
									ID = option.ID,
									OptionId = option.OptionId,
									OptionDesc = string.IsNullOrEmpty(option.name) ? DBNull.Value.ToString() : option.name,
									inspectionTransID = ins.ID,
									SequenceID = option.SequenceID,
									//inspectionTransID = ins.ID,
									SpaceID = option.SpaceID,
									LevelID = option.LevelID,
									checkListTransaction = GetCheckListItems(option.checkListItems),
									isSelected = (option.isSelected == true) ? 1 : 0,
									photos = option.photos
								});


								result = optionTransactionService.SaveOptionTransactions(optTrans.LastOrDefault(), data2, option);

								Debug.WriteLine(string.Format("Option {1} store result = {0}", result, option.ID));
							}

						}
						else if (currSeq is ILevel)
						{
							var Level = (currSeq as Level);


							if (Level.Spaces != null && Level.Spaces.Count > 0)
							{
								foreach (var spc in Level.Spaces)
								{
									var Space = (spc as Space);

									using (SpaceTransactionService spaceTransactionService = new SpaceTransactionService(AppDelegate.DatabaseContext))
									{

										var ExistingTrans = spaceTransactionService.GetSpaceTransactions().Where(o => o.InspectionTransID == ins.ID && o.LevelID == Level.ID && o.SeqID == Level.seqID && o.SpaceID == Space.SpaceID);
										if (ExistingTrans != null && ExistingTrans.Count() > 0)
										{
											foreach (var ExistingTran in ExistingTrans)
											{
												spaceTransactionService.DeleteSpaceTransactions(ExistingTran);
											}
										}

									}


								}

								using (SpaceTransactionService spaceTransactionService = new SpaceTransactionService(AppDelegate.DatabaseContext))
								{
									List<SpaceTransaction> spaceTrans = new List<SpaceTransaction>();
									var data1 = spaceTransactionService.GetSpaceTransactions();
									foreach (var spc in Level.Spaces)
									{

										spaceTrans.Add(new SpaceTransaction()
										{
											ID = spc.id,
											SpaceID = spc.SpaceID,
											LevelID = spc.levelID,
											SeqID = spc.seqID,
											isSelected = (spc.isSelected == true) ? 1 : 0,
											InspectionTransID = ins.ID

										});
										result = spaceTransactionService.SaveSpaceTransactions(spaceTrans.LastOrDefault(), data1, null);

										Debug.WriteLine(string.Format("store space {1} result = {0}", result, spaceTrans.LastOrDefault().ID));
										//var spaceTransactions = spaceTransactionService.GetSpaceTransactions();

									}

								}

							}
							else if (Level.Options != null && Level.Options.Count > 0)
							{
								using (optionTransactionService = new OptionTransactionService(AppDelegate.DatabaseContext))
								{


									foreach (var _Option in Level.Options)
									{
										var ExistingOpts = optionTransactionService.GetOptionTransactions().Where(o => o.inspectionTransID == ins.ID && o.SequenceID == _Option.SequenceID && o.LevelID == _Option.LevelID && o.SpaceID == _Option.SpaceID);
										if (ExistingOpts != null && ExistingOpts.Count() > 0)
										{
											Debug.WriteLine(string.Format("ExistingOpts count = {0}", ExistingOpts.Count()));

											foreach (var ExistingOpt in ExistingOpts)
											{


												optionTransactionService.DeleteOptionTransactions(ExistingOpt);


											}
										}
									}
									var optTrans = new List<OptionTransaction>();
									var data7 = optionTransactionService.GetOptionTransactions();
									foreach (var _Option in Level.Options)
									{

										//Debug.WriteLine(string.Format("new Options count = {0}", data7.Count()));
										optTrans.Add(new OptionTransaction()
										{
											ID = _Option.ID,
											OptionId = _Option.OptionId,
											OptionDesc = string.IsNullOrEmpty(_Option.name) ? DBNull.Value.ToString() : _Option.name,
											inspectionTransID = ins.ID,
											SequenceID = _Option.SequenceID,
											//inspectionTransID = ins.ID,
											SpaceID = _Option.SpaceID,
											LevelID = _Option.LevelID,
											//checkListTransaction = GetCheckListItems(_Option.checkListItems),
											isSelected = (_Option.isSelected == true) ? 1 : 0,
											//photos = _Option.photos
										});

										result = optionTransactionService.SaveOptionTransactions(optTrans.LastOrDefault(), data7, null);

										Debug.WriteLine(string.Format("Option {1} store result = {0}", result, _Option.ID));
									}

								}
							}
						}
						else if (currSeq is ISequence)
						{
							var Seq = (currSeq as ISequence);
							using (LevelTransactionService levelTransactionService = new LevelTransactionService(AppDelegate.DatabaseContext))
							{


								if (Seq.Levels != null && Seq.Levels.Count > 0)
								{
									foreach (var lvl in Seq.Levels)
									{

										var ExistingTran = levelTransactionService.GetLevelTransactions().Where(o => o.InspectionTransID == ins.ID && o.SeqID == lvl.getSequenceID() && o.LevelID == lvl.ID).FirstOrDefault();

										if (ExistingTran != null)
										{
											levelTransactionService.DeleteLevelTransaction(ExistingTran);

										}


									}
								}
							}


							using (LevelTransactionService levelTransactionService = new LevelTransactionService(AppDelegate.DatabaseContext))
							{
								if (Seq.Levels != null && Seq.Levels.Count > 0)
								{
									List<LevelTransaction> levelTrans = new List<LevelTransaction>();
									var data1 = levelTransactionService.GetLevelTransactions();
									foreach (var lvl in Seq.Levels)
									{

										levelTrans.Add(new LevelTransaction()
										{
											ID = lvl.ID,
											LevelID = lvl.ID,
											SeqID = Seq.getSequenceID(),
											isSelected = (lvl.isSelected == true) ? 1 : 0,
											InspectionTransID = ins.ID
										});



										result = levelTransactionService.SaveLevelTransactions(levelTrans.LastOrDefault(), data1, null);

										Debug.WriteLine(string.Format("sequence Level {1} store result = {0}", result, levelTrans.LastOrDefault().ID));


									}

								}

							}

						}
						else if (currSeq is ISpace)
						{
							var Space = (currSeq as Space);
							if (Space.Options != null && Space.Options.Count > 0)
							{

								using (OptionTransactionService optionTransactionService = new OptionTransactionService(AppDelegate.DatabaseContext))
								{



									foreach (var opt in Space.Options)
									{

										var ExistingOpts = optionTransactionService.GetOptionTransactions().Where(o => o.inspectionTransID == ins.ID && o.SequenceID == opt.SequenceID && o.LevelID == opt.LevelID && o.SpaceID == opt.SpaceID);
										if (ExistingOpts != null && ExistingOpts.Count() > 0)
										{
											foreach (var ExistingOpt in ExistingOpts)
											{
												if ((opt.photos == null || opt.photos.Count == 0))
												{
													if (opt.OptionTransactionID == 0)
													{
														optionTransactionService.DeleteOptionTransactions(ExistingOpt);
													}

												}
											}
										}


									}
								}

							}

							List<OptionTransaction> optTrans1 = new List<OptionTransaction>();


							if (Space.Options != null && Space.Options.Count > 0)
							{
								using (OptionTransactionService optionTransactionService = new OptionTransactionService(AppDelegate.DatabaseContext))
								{
									var data2 = optionTransactionService.GetOptionTransactions();

									foreach (var option in Space.Options)
									{

										//var Option = (option as Option);
										optTrans1.Add(new OptionTransaction()
										{
											ID = option.ID,
											OptionId = option.OptionId,
											OptionDesc = string.IsNullOrEmpty(option.name) ? DBNull.Value.ToString() : option.name,
											inspectionTransID = ins.ID,
											SequenceID = Space.seqID,
											SpaceID = Space.SpaceID,
											LevelID = Space.levelID,
											//inspectionTransID = ins.ID,
											//checkListTransaction = GetCheckListItems(option.checkListItems),
											isSelected = (option.isSelected == true) ? 1 : 0,
											//photos = option.photos
										});
										//var _Option1 = new Option();
										result = optionTransactionService.SaveOptionTransactions(optTrans1.LastOrDefault(), data2, null);

										Debug.WriteLine(string.Format("Option {0} store result = {1}", option.ID, result));



									}


								}

							}

						}

					}
				}
				catch (Exception ex)
				{
					Debug.WriteLine("Exception Occured in SaveInpsectionOptionResult method due to " + ex.Message);
				}

			}
			return result;
		}


		private int SaveOptionImages(Option opt, List<OptionTransaction> optionTransactions)
		{
			int result = 0;

			try
			{

			}
			catch (Exception ex)
			{
				Debug.WriteLine("Exception Occured in GetOptionImages method due to " + ex.Message);
			}


			return result;
		}

		public void registerForKeyboardNotifications()
		{
			NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillHideNotification, OnKeyboardNotification);
			NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillShowNotification, OnKeyboardNotification);
		}

		private void OnKeyboardNotification(NSNotification notification)
		{
			if (!IsViewLoaded)
				return;

			//Check if the keyboard is becoming visible
			var visible = notification.Name == UIKeyboard.WillShowNotification;

			//Start an animation, using values from the keyboard
			UIView.BeginAnimations("AnimateForKeyboard");
			UIView.SetAnimationBeginsFromCurrentState(true);
			UIView.SetAnimationDuration(UIKeyboard.AnimationDurationFromNotification(notification));
			UIView.SetAnimationCurve((UIViewAnimationCurve)UIKeyboard.AnimationCurveFromNotification(notification));
			var keyboardFrame = visible ? UIKeyboard.FrameEndFromNotification(notification) : UIKeyboard.FrameBeginFromNotification(notification);
			// Move up the textfield when the keyboard comes up
			if (visible)
			{
				previousContentOffset = OptionsTableView.ContentOffset;
			}
			else
			{
				OptionsTableView.ContentOffset = previousContentOffset;
			}

			NSDictionary info = notification.UserInfo;

			//Commit the animation
			UIView.CommitAnimations();
		}

		/// <summary>
		/// Punch list table creation and showcase
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		void createPunchListBtn_TouchUpInside(object sender, EventArgs e)
		{
			try
			{
				if (ins.inspectionID == Constants.FINAL_INSPECTIONID || ins.inspectionID == Constants.NINTY_PERCENT_INSPECTIONID)
				{
					lblinspectionNameRight.Text = "Punch List Item Creation";
				}
				else
				{
					lblinspectionNameRight.Text = "Non Conformance Item Creation";
				}
				lblSequence.Hidden = true;
				addPictureView.Hidden = true;
				btnSave.Hidden = true;
				btnNext.Hidden = true;
				btnClearSearch.Hidden = true;
				punchAddItemBtn.Hidden = false;
				punchFinishButton.Hidden = false;
				OptionsTableView.Hidden = true;
				createPunchListBtn.Enabled = true;
				takePictureBtn.Hidden = true;

				txtSearchOptions.Hidden = true;
				btnSearchOptions.Hidden = true;
				imagesScrollView.Hidden = true;
				rhsHeaderHeightConstraint.Constant = 85f;

				punchAddItemBtn.Layer.CornerRadius = 5f;
				punchAddItemBtn.Layer.BorderColor = UIColor.FromRGB(18, 74, 143).CGColor;
				punchAddItemBtn.Layer.BorderWidth = 0.5f;

				punchAddItemBtn.TouchUpInside -= punchAddItemBtnTouchUpInsideMethod;
				punchAddItemBtn.TouchUpInside += punchAddItemBtnTouchUpInsideMethod;

				punchFinishButton.TouchUpInside -= punchFinishButton_TouchUpInside;
				punchFinishButton.TouchUpInside += punchFinishButton_TouchUpInside;

				punchFinishButton.BackgroundColor = UIColor.FromRGB(18, 74, 143);
				punchFinishButton.SetTitleColor(UIColor.White, UIControlState.Normal);

				PunchService punchService = new PunchService(AppDelegate.DatabaseContext);

				if (ins != null)
				{
					punchItems = punchService.GetPunchItems(ins.inspectionID, ins.projectID, ins.ID);

					if (punchItems.Count > 0)
					{
						List<PunchListImageDO> punchImagesList = new List<PunchListImageDO>();

						PunchImageService punchImageService = new PunchImageService(AppDelegate.DatabaseContext);
						punchImagesList = punchImageService.GetPunchItemImages(ins.ID);

						for (int i = 0; i < punchItems.Count; i++)
						{
							var punchImageList = punchImagesList.FindAll(p => p.PunchID == punchItems[i].PunchID);
							foreach (PunchListImageDO punchImageDO in punchImageList)
							{
								punchItems[i].punchImages.Add(punchImageDO.PunchListImage);
							}
						}

						Model.Punch punchItem = punchItems.FirstOrDefault();

						if (punchItem.punchDescription.Length > 0 || punchItem.punchImages.Count > 0)
						{

							OptionsTableView.Hidden = false;
							ClearOptionsTableView();
							punchSource = new InspectionPunchSource(this, OptionsTableView, punchItems);
							OptionsTableView.Source = punchSource;
							OptionsTableView.RowHeight = 250;
							OptionsTableView.AllowsSelection = false;
							OptionsTableView.SetEditing(true, true);
						}
						else
							punchAddItemBtn.Enabled = true;
					}
					else
					{
						punchAddItemBtn.Enabled = true;
					}
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Exception Occured in createPunchListBtn_TouchUpInside method due to " + ex.Message);
			}
		}

		/// <summary>
		/// Add Item Button Event method
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		void punchAddItemBtnTouchUpInsideMethod(object sender, EventArgs e)
		{
			try
			{
				OptionsTableView.Hidden = false;

				if (punchItems.Count > 0)
				{
					if (punchItems.Count >= 20)
					{
						UIAlertView alert = new UIAlertView(@"Alert", @"You have reached the maximum limit.", null, NSBundle.MainBundle.LocalizedString("OK", "OK"));
						alert.Show();
						return;
					}

					Model.Punch previousPunch = punchItems.LastOrDefault();
					if (previousPunch.punchDescription == null)
					{
						UIAlertView alert = new UIAlertView(@"Alert", @"Please add your comment to current item before moving to next", null, NSBundle.MainBundle.LocalizedString("OK", "OK"));
						alert.Show();
						return;
					}
				}

				Model.Punch punch = new Model.Punch();
				punch.InspectionID = this.InspectionData.inspectionID;
				punch.ProjectID = this.InspectionData.projectID;
				punch.punchHeading = "Item 1";
				punchItems.Add(punch);

				punchSource = new InspectionPunchSource(this, OptionsTableView, punchItems);
				OptionsTableView.Source = punchSource;
				OptionsTableView.RowHeight = 250;
				OptionsTableView.AllowsSelection = false;
				OptionsTableView.SetEditing(true, true);
				OptionsTableView.ReloadData();

				if (punchItems.Count > 2)
				{
					NSIndexPath indexpath = NSIndexPath.FromRowSection(punchItems.Count - 1, 0);
					OptionsTableView.ScrollToRow(indexpath, UITableViewScrollPosition.Bottom, true);
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Exception Occured in punchAddItemBtnTouchUpInsideMethod method due to " + ex.Message);
			}
		}

		private bool CheckforPunchlistPhotoAndComment()
		{
			if (punchItems.Count > 0)
			{
				punchFinishButton.Hidden = true;
				btnNext.Hidden = true;
				NSIndexPath indexpath = NSIndexPath.FromRowSection(punchItems.Count - 1, 0);



				Model.Punch punchItem = punchItems.ElementAt(indexpath.Row);


				if (punchItem != null)
				{
					if (punchItem.punchImages.Count() == 0)
					{
						string PuchListItem = punchItem.punchHeading.Substring(0, 4) + " " + punchItems.Count;
						UIAlertView alert = new UIAlertView(@"Warning", string.Format("Please take at least one photo for {0}", PuchListItem), null, NSBundle.MainBundle.LocalizedString("OK", "OK"));
						alert.Show();
					}
					else
					{
						string Description = punchItem.punchDescription;

						if (string.IsNullOrEmpty(Description))
						{
							string PuchListItem = punchItem.punchHeading.Substring(0, 4) + " " + punchItems.Count;
							UIAlertView alert = new UIAlertView(@"Alert", string.Format("Please add a description for {0}", PuchListItem), null, NSBundle.MainBundle.LocalizedString("OK", "OK"));
							alert.Show();

						}
						else
						{
							punchFinishButton.Hidden = false;
							return true;
						}

					}
				}




			}
			return false;
		}

		/// <summary>
		/// Punch list finish and save table
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		void punchFinishButton_TouchUpInside(object sender, EventArgs e)
		{
			// check if punchlist photos exists and if comments exist
			bool HasPunchListItems = CheckforPunchlistPhotoAndComment();

			if (HasPunchListItems)
			{
				savePunchListChanges();
			}
			else
			{
				punchFinishButton.Hidden = false;
			}
		}

		#region sync & Notify

		public override void updateSyncCount(int count)
		{
			string countStr = "";
			if (count > 0)
			{
				countStr = count.ToString();
			}
			//run on ui thread
			InvokeOnMainThread(delegate
			{
				if (count > 0)
				{
					lblSyncNumber.Text = countStr;
					lblSyncNumber.Hidden = false;
				}
				else
				{
					lblSyncNumber.Hidden = true;
				}
			});
		}

		public override void updateNotifyCount(int count, bool fromSync)
		{
			string countStr = "";
			if (count > 0)
			{
				countStr = count.ToString();
			}
			//run on ui thread
			InvokeOnMainThread(delegate
			{
				if (count > 0)
				{
					LblNotifyNbr.Text = countStr;
					LblNotifyNbr.Hidden = false;
				}
				else
				{
					LblNotifyNbr.Hidden = true;
				}
			});

		}

		void BtnNotify_TouchUpInside(object sender, EventArgs e)
		{
			SyncNotification SyncNotification = this.Storyboard.InstantiateViewController("SyncNotification") as SyncNotification;
			UIPopoverController cl = new UIPopoverController(SyncNotification);
			cl.SetPopoverContentSize(new CGSize(500, 600), true);
			cl.PresentFromRect(btnNotify.Frame, View, UIPopoverArrowDirection.Any, true);
			clearSeen();
		}

		/// <summary>
		/// This method is used to clear the imagesList and add new images in that.
		/// </summary>
		/// <param name="image">Image.</param>
		public async void CamerapictureTaken(UIImageView image)
		{

			MediaLst.Add(image);
			if (MediaLst.Count == 0)
			{
				imageShowView.Hidden = true;
				takePictureView.Hidden = false;
			}
			if (MediaLst != null && MediaLst.Count > 0)
			{
				imageShowView.Image = MediaLst[MediaLst.Count - 1].Image;
				imageShowView.Hidden = false;
				takePictureView.Hidden = true;
				beginInspectionButton.Enabled = true;

				if (imagesList != null && imagesList.Count > 0)
					imagesList.Clear();

				foreach (var img in MediaLst)
				{
					imagesList.Add(img.Image);
				}

				beginInspectionButton.Layer.CornerRadius = 5f;
				beginInspectionButton.Layer.BorderColor = UIColor.FromRGB(18, 74, 143).CGColor;
				beginInspectionButton.Layer.BorderWidth = 0.5f;
				beginInspectionButton.BackgroundColor = UIColor.FromRGB(18, 74, 143);
				beginInspectionButton.SetTitleColor(UIColor.White, UIControlState.Normal);
			}
			if (Source == null)
			{
				Source = new CameraDataSource(imagesList, this);
			}
			else
			{
				Source.itemsList = imagesList;
			}
			imageCollectionView.DataSource = Source;
			imageCollectionView.ReloadData();

			if (cameraImageView != null)
				this.cameraImageView = imageShowView;
		}

		/// <summary>
		/// Begins the inspection after capturing the picture/pictures of the location.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>

		public void beginInspectionButton_TouchUpInside(object sender, EventArgs e)
		{
			if (imageShowView.Image != null)
			{
				ins.Image = ImageToByteArray(imageShowView.Image);
				ins.locationIDImages = new List<byte[]>();
				MediaLst.ForEach(i => ins.locationIDImages.Add(ImageToByteArray(i.Image)));
				saveLocationIdentifierImages();
				if (MediaLst != null)
				{
					MediaLst.Clear();
				}
				if (ImageLst != null)
				{
					ImageLst.Clear();
				}
				if (ins.locationIDImages != null)
				{
					ins.locationIDImages.Clear();
				}
				if (imageCollectionView != null)
				{
					imageCollectionView.DataSource = null;
				}
				addPictureView.Hidden = true;
				// set this later
				OptionsTableView.Hidden = false;
				if (iseq != null)
				{
					currSeq = (ITraversible)iseq[currentSequenceIndex];
					currSeq.prevSeqNextClicked = false;
					bool bypass = ins.InspectionStarted == 1 ? true : false;
					processEnabling(iseq, bypass);
				}

				ClearLocationIdentification();


				if (!iSInspectionInProgress)
				{



					RebuildSequenceTable();
					if (currSeq != null)
					{
						if (currSeq is ISequence)
						{
							if ((currSeq as ISequence).Levels != null && (currSeq as ISequence).Levels.Count > 0)
							{

								FillLevelsTable(currSeq);




								lblinspectionNameRight.Hidden = true;

								lblSequence.Text = "Please make a selection";

							}
							else
							{
								// must have punchlist items (treated like options)

								selectNextRow(false);




								txtSearchOptions.Hidden = true;
								btnSearchOptions.Hidden = true;
								btnClearSearch.Hidden = true;
								htLblSequence.Constant = 60;
								spSequenceLabel.Constant = -15;
								takePictureBtn.Hidden = true;

								OptionsTableView.Hidden = false;


								btnSelectAll.Hidden = true;
								CGPoint zero = new CGPoint(0, 0);
								OptionsTableView.SetContentOffset(zero, true);

							}
						}
					}

				}
				iSInspectionInProgress = true;
				// hide create punchlist button
				//createPunchListBtn.Hidden = true;

				//lblSearchOptions.Hidden = false;
				//if imageShowview image is saved, punch list button will be enabled
				createPunchListBtn.Enabled = true;
			}
			else
			{
				beginInspectionButton.Enabled = false;
				addPictureView.Hidden = true;
				// set this later
				OptionsTableView.Hidden = false;
				if (iseq != null)
				{
					currSeq = (ITraversible)iseq[currentSequenceIndex];
					currSeq.prevSeqNextClicked = false;
					bool bypass = ins.InspectionStarted == 1 ? true : false;
					processEnabling(iseq, bypass);
				}

				if (!iSInspectionInProgress)
				{


					RebuildSequenceTable();
					if (currSeq != null)
					{
						if (currSeq is ISequence)
						{
							if ((currSeq as ISequence).Levels != null && (currSeq as ISequence).Levels.Count > 0)
							{

								FillLevelsTable(currSeq);




								lblinspectionNameRight.Hidden = true;

								lblSequence.Text = "Please make a selection";

							}
							else
							{
							}

						}
					}


				}
				iSInspectionInProgress = true;
				// hide create punchlist button
				//createPunchListBtn.Hidden = false;
				createPunchListBtn.Enabled = true;
			}
		}

		public bool GetiSInspectionInProgress()
		{
			return iSInspectionInProgress;
		}

		/// <summary>
		/// Clears the Location Identification images.
		/// </summary>
		private void ClearLocationIdentification()
		{
			if (MediaLst != null)
			{
				this.MediaLst.Clear();
				_mediaLst.Clear();
			}

			if (ins.locationIDImages != null)
			{
				ins.locationIDImages.Clear();
			}
			if (currentSelectedImageView != null)
			{
				currentSelectedImageView.RemoveFromSuperview();
				currentSelectedImageView = null;
			}
		}

		/// <summary>
		/// Checks whether any picture of the current location  is available in the collection view.
		/// If no picture is there then beginInspection button will be disabled.
		/// </summary>
		/// <param name="count">Count.</param>
		public void isAnyPictureAvailable(int count)
		{
			if (count == 0)
			{
				imageShowView.Hidden = true;
				takePictureView.Hidden = false;
				beginInspectionButton.Enabled = false;
				beginInspectionButton.Layer.CornerRadius = 5f;
				beginInspectionButton.Layer.BorderColor = UIColor.FromRGB(18, 74, 143).CGColor;
				beginInspectionButton.Layer.BorderWidth = 0.5f;
				beginInspectionButton.BackgroundColor = UIColor.FromRGB(255, 255, 255);
				beginInspectionButton.SetTitleColor(UIColor.LightGray, UIControlState.Normal);
			}
			else
				imageShowView.Image = this.MediaLst[MediaLst.Count - 1].Image;
		}

		private void ResetUIView()
		{
			cameraImage.Image = null;
			foreach (var view in imageCollectionView.Subviews)
			{
				view.RemoveFromSuperview();
				view.Dispose();
			}
			imageShowView.Image = null;

			foreach (var view in imagesScrollView.Subviews)
			{
				view.RemoveFromSuperview();
				view.Dispose();
			}
			if (MediaLst != null)
			{
				MediaLst.Clear();
			}
			if (ImageLst != null)
			{
				ImageLst.Clear();
			}

			lblAddress1.Text = "";
			lblAddress2.Text = "";
			lblCalDate.Text = "";
			lblCalDay.Text = "";
			lblInspectionDate.Text = "";
			lblinspectionName.Text = "";
			lblinspectionNameRight.Text = "";
		}

		/// <summary>
		/// Tapping this button will open the camera page so that user can take picture. 
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		void takePictureButton_TouchUpInside(object sender, EventArgs e)
		{
			tapOwnerDetailsViewAction();

			if (imagesList.Count <= ImageLimit)
			{



				CameraController cameraController = new CameraController();
				cameraController.ModalPresentationStyle = UIModalPresentationStyle.OverCurrentContext;
				cameraController.pictureTaken = this.CamerapictureTaken;
				this.PresentViewController(cameraController, false, null);

			}
			else
			{
				UIAlertView alert = new UIAlertView("Maximum", "Maximum Images were added to the list", null, NSBundle.MainBundle.LocalizedString("OK", "OK"));
				alert.Show();
			}
		}

		/// <summary>
		/// Saving Location Identification Images and progressing the inspection.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		void progressInspectionBtn_TouchUpInside(object sender, EventArgs e)
		{
			shouldSavePunchList();
			addPictureView.Hidden = false;
			this.OptionsTableView.EndEditing(true);

			if (ins.locationIDImages != null && ins.locationIDImages.Count == 0)
			{
				var images = LocationImageDo.getImageForLocationIdentification(AppDelegate.DatabaseContext, ins.ID);
				if (images != null && images.Count > 0)
				{
					foreach (var img in images)
					{
						ins.locationIDImages.Add(img.Image);
					}
				}
				if (imagesList != null || imagesList.Count > 0)
					imagesList.Clear();

				if (MediaLst != null || MediaLst.Count > 0)
					MediaLst.Clear();

				for (int index = 0; index < ins.locationIDImages.Count; index++)
				{
					imagesList.Add(ByteArrayToImage(ins.locationIDImages[index]));
					UIImageView imageView = new UIImageView();
					imageView.Image = (ByteArrayToImage(ins.locationIDImages[index]));
					MediaLst.Add(imageView);
				}

				if (Source == null)
				{
					Source = new CameraDataSource(imagesList, this);
				}
				else
				{
					Source.itemsList = imagesList;
				}
				imageCollectionView.DataSource = Source;
				imageCollectionView.ReloadData();


				if (ins.locationIDImages != null && ins.locationIDImages.Count > 0)
				{
					imageShowView.Hidden = false;
					beginInspectionButton.Hidden = true;
					beginInspectionButton.Layer.CornerRadius = 5f;
					beginInspectionButton.Layer.BorderColor = UIColor.FromRGB(18, 74, 143).CGColor;
					beginInspectionButton.BackgroundColor = UIColor.FromRGB(18, 74, 143);
					beginInspectionButton.SetTitleColor(UIColor.White, UIControlState.Normal);
					beginInspectionButton.Layer.BorderWidth = 0.5f;
					takePictureView.Hidden = true;

				}
				beginInspectionButton.Enabled = true;
				beginInspectionButton_TouchUpInside(sender, e);
			}
		}

		void HandleTouchUpInside(object sender, EventArgs ea)
		{
			if (imagesList.Count <= ImageLimit)
			{

				CameraController cameraController = new CameraController();
				cameraController.ModalPresentationStyle = UIModalPresentationStyle.FullScreen;
				cameraController.pictureTaken = this.CamerapictureTaken;
				this.PresentViewController(cameraController, false, null);


			}
			else
			{
				UIAlertView alert = new UIAlertView("Maximum", "Maximum Images were added to the list", null, NSBundle.MainBundle.LocalizedString("OK", "OK"));
				alert.Show();
			}
		}

		private List<UIImageView> _imageLst = new List<UIImageView>();

		public List<UIImageView> ImageLst
		{
			get
			{
				return _imageLst;
			}
			set
			{
				_imageLst = value;
			}
		}

		public async void UpdateCollectionImageView(List<UIImageView> images)
		{
			List<byte[]> cameraImages = new List<byte[]>();
			foreach (var img in images)
			{
				byte[] task = ImageToByteArray(img.Image);
				var imgArr = task;
				cameraImages.Add(imgArr);
			}
			ImageLst = images;
			RestructureImages();
		}

		private async Task UsePhotoAction()
		{
			pictureCollection(MediaLst);
		}

		public UIImageView HighlightSelectedImage(UIImageView imageView)
		{
			if (currentSelectedImageView != null)
			{
				currentSelectedImageView.Layer.CornerRadius = 0.0f;
				currentSelectedImageView.Layer.BorderColor = UIColor.FromRGB(0, 0, 0).CGColor;
				currentSelectedImageView.Layer.BorderWidth = 0.0f;
				currentSelectedImageView.BackgroundColor = UIColor.Clear;
			}

			imageView.Layer.CornerRadius = 5f;
			imageView.Layer.BorderColor = UIColor.FromRGB(18, 74, 143).CGColor;
			imageView.Layer.BorderWidth = 0.5f;
			imageView.BackgroundColor = UIColor.White;

			currentSelectedImageView = imageView;
			imageShowView.Image = imageView.Image;
			return imageView;
		}

		private List<UIImageView> _mediaLst = new List<UIImageView>();

		public List<UIImageView> MediaLst
		{
			get
			{
				return _mediaLst;
			}
			set
			{
				_mediaLst = value;
			}
		}

		public void RestructureImages()
		{
			nfloat h = 100.0f;
			nfloat w = 100.0f;
			nfloat padding = 10.0f;

			for (int i = 0; i < MediaLst.Count; i++)
			{
				UIImageView imageView = MediaLst[i];
				imageView.Frame = new CGRect(padding * (i + 1) + (i * w), 0, w, h);
				Action action = new Action(
									delegate
									{
										var answer = new UIActionSheet("Do you want to Delete the Image?", null, "Cancel", "Ok", new string[1] { "Cancel" });

										answer.Clicked += delegate (object sender, UIButtonEventArgs args)
										{
											var btnarg = args.ButtonIndex;
											if (btnarg == 0)
											{
												imageView.RemoveFromSuperview();
												MediaLst.Remove(imageView);
												RestructureImages();

												if (imageShowView.Image == imageView.Image)
												{
													imageShowView.Image = null;
													takePictureView.Hidden = false;
													if (currentSelectedImageView != null)
													{
														currentSelectedImageView.Layer.CornerRadius = 0.0f;
														currentSelectedImageView.Layer.BorderColor = UIColor.FromRGB(0, 0, 0).CGColor;
														currentSelectedImageView.Layer.BorderWidth = 0.0f;
														currentSelectedImageView.BackgroundColor = UIColor.Clear;
													}
												}

												if (MediaLst.Count == 0)
												{
													imageShowView.Hidden = true;
													takePictureView.Hidden = false;
													beginInspectionButton.Enabled = false;
													//createPunchListBtn is enabled if location identifier image is available else it is disabled.
													createPunchListBtn.Enabled = false;
													beginInspectionButton.Layer.CornerRadius = 5f;
													beginInspectionButton.Layer.BorderColor = UIColor.FromRGB(18, 74, 143).CGColor;
													beginInspectionButton.Layer.BorderWidth = 0.5f;
													beginInspectionButton.BackgroundColor = UIColor.FromRGB(255, 255, 255);
													beginInspectionButton.SetTitleColor(UIColor.LightGray, UIControlState.Normal);
												}
											}
										};
										answer.ShowInView(View);
									});

				Action tapAction = new Action(
									   delegate
									   {
										   imageShowView.Image = imageView.Image;
										   imageView = HighlightSelectedImage(imageView);
									   });

				UITapGestureRecognizer tap = new UITapGestureRecognizer();
				tap.AddTarget(tapAction);
				imageView.AddGestureRecognizer(tap);

				imageView = HighlightSelectedImage(imageView);

				UILongPressGestureRecognizer gr = new UILongPressGestureRecognizer();
				gr.AddTarget(action);
				imageView.AddGestureRecognizer(gr);
				imageView.UserInteractionEnabled = true;
				takePictureView.Hidden = true;
			}
		}

		public override void ViewDidUnload()
		{
			base.ViewDidUnload();

			NSNotificationCenter.DefaultCenter.RemoveObserver(UIKeyboard.WillHideNotification);
			NSNotificationCenter.DefaultCenter.RemoveObserver(UIKeyboard.WillShowNotification);
		}

		public override void ViewDidDisappear(bool animated)
		{
			base.ViewDidDisappear(animated);
			if (autoSaveTimer != null)
			{
				autoSaveTimer.Invalidate();
				autoSaveTimer.Dispose();
			}
		}

		private void ClearMemory()
		{
			try
			{
				btnSave.TouchUpInside -= SaveSpace_TouchUpInside;
				btnNext.TouchUpInside -= BtnNext_TouchUpInside;
				btnLogout.TouchUpInside -= BtnLogout_TouchUpInside;
				btnHome.TouchUpInside -= BtnHome_TouchUpInside;
				beginInspectionButton.TouchUpInside -= beginInspectionButton_TouchUpInside;
				takePictureButton.TouchUpInside -= takePictureButton_TouchUpInside;
				progressInspectionBtn.TouchUpInside -= progressInspectionBtn_TouchUpInside;
				btnNotify.TouchUpInside -= BtnNotify_TouchUpInside;
				docViewBtn.TouchUpInside -= BtnDocView_TouchUpInside;
				createPunchListBtn.TouchUpInside -= createPunchListBtn_TouchUpInside;
				takePictureBtn.TouchUpInside -= TakeOptionPictures_TouchUpInside;
				// clean up
				btnSearchOptions.TouchUpInside -= BtnSearchOptions_TouchUpInside;
				txtSearchOptions.EditingDidEndOnExit -= TxtSearchOptions_EditingDidEnd;
				btnClearSearch.TouchUpInside -= BtnClearSearch_TouchUpInside;

				ClearLocationIdentification();
				ResetTheCameraImageView();
				if (punchItems != null)
				{
					this.punchItems.Clear();
					this.punchItems = null;
				}

				if (this.punchSource != null)
				{
					this.punchSource.Dispose();
				}

				if (this.currSeq != null)
				{
					this.currSeq = null;
				}

				if (this.currentSelectedImageView != null)
				{
					this.currentSelectedImageView.Dispose();
					this.currentSelectedImageView = null;
				}

				if (this.iseq != null)
				{
					iseq.Clear();
					iseq = null;
				}
				if (this.ins != null)
				{
					this.ins.Dispose();
					this.ins = null;
				}

				if (!IsLogOut)
				{
					base.ClearMemory();
				}
				this.Dispose();
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Exception Occured in ClearMemory method due to " + ex.Message);
			}
		}

		public void ClearSequence()
		{
			if (this.iseq != null)
			{
				iseq.Clear();
				iseq = null;
			}
		}

		public override Task DismissViewControllerAsync(bool animated)
		{
			ClearMemory();
			return base.DismissViewControllerAsync(animated);
		}

		void syncProgressChange(bool inProgress)
		{
			this.InvokeOnMainThread(delegate
			{
				btnLogout.Enabled = !inProgress;
			});
			syncInit();
		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			NSUrlCache.SharedCache.RemoveAllCachedResponses();
			GC.SuppressFinalize(this);
			GC.Collect();
		}
	}
}
#endregion