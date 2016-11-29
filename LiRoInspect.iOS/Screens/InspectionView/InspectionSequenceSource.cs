using System;
using UIKit;
using System.Collections.Generic;
using CoreGraphics;
using CoreAnimation;
using Foundation;
using System.Linq;
using Model;
using System.Threading.Tasks;


namespace LiRoInspect.iOS
{
	public class InspectionSequenceSource: UITableViewSource
	{
		//List<Model.ITraversible> SequenceItems = new List<Model.ITraversible> ();
		protected string cellIdentifier = "SequenceCell";
		public WeakReference ParentController;
		public WeakReference ParentController1;
		ITraversible CurrSeq;
		bool isInOptionSelect = false;

		public InspectionViewController parentController {
			get {
				if (ParentController == null || !ParentController.IsAlive)
					return null;
				return ParentController.Target as InspectionViewController;
			}
		}


		public WeakReference sequenceItems;

		public List<Model.ITraversible> SequenceItems {
			get {
				if (sequenceItems == null || !sequenceItems.IsAlive)
					return null;
				return sequenceItems.Target as List<Model.ITraversible>;
			}
		}

		public bool IsInOptionSelect {
			get {
				return isInOptionSelect;
			}
			set {
				isInOptionSelect = value;
			}
		}




		public InspectionSequenceSource (List<Model.ITraversible> SequenceItems, InspectionViewController parentController)
		{
			this.sequenceItems = new WeakReference (SequenceItems);
			this.ParentController = new WeakReference (parentController);

		}



		public override nint NumberOfSections (UITableView tableView)
		{ 
			return 1; 
		}

		public override nint RowsInSection (UITableView tableview, nint section)
		{
			return SequenceItems.Count;
		}

		void getRows ()
		{
			//sequences + selected options 
		}

		public override UITableViewCellEditingStyle EditingStyleForRow (UITableView tableView, NSIndexPath indexPath)
		{
			return UITableViewCellEditingStyle.None;
		}

		public override UITableViewCell GetCell (UITableView tableView, Foundation.NSIndexPath indexPath)
		{
			bool iSInspectionInProgress = false;

			CurrSeq = parentController.currSeq;



			UITableViewCell cell = tableView.DequeueReusableCell (cellIdentifier);
			if (cell == null) {
				cell = new UITableViewCell (UITableViewCellStyle.Default, cellIdentifier);
			}


			ITraversible seq = SequenceItems [indexPath.Row];
//			cell.TextLabel.Text = seq.getName();
//			cell.TextLabel.Font = UIFont.FromName("Helvetica", 18f);//move to constants
			cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;
			if (cell.TextLabel != null) {
				if (!seq.enableRow) {
					cell.SelectionStyle = UITableViewCellSelectionStyle.None;
					cell.TextLabel.Enabled = false;
					cell.UserInteractionEnabled = false;
				} else {
					cell.TextLabel.TextColor = UIColor.FromRGB (0, 105, 170);
					cell.SelectionStyle = UITableViewCellSelectionStyle.None;
					cell.TextLabel.Enabled = true;
					cell.UserInteractionEnabled = true;

					//cell.AccessoryView = new UIImageView (new UIImage ("blueForwardSmall"));
				}
				if (seq is ILevel) {
					cell.TextLabel.Font = UIFont.FromName ("Helvetica", 20f);
					cell.TextLabel.Text = "    " + seq.getName ();
					cell.TextLabel.LineBreakMode = UILineBreakMode.WordWrap;
					cell.TextLabel.Lines = 0;
					cell.TextLabel.TextAlignment = UITextAlignment.Left;
					cell.TextLabel.TextColor = UIColor.FromRGB (0, 105, 170);

				} else if (seq is ISpace) {
					cell.TextLabel.Font = UIFont.FromName ("Helvetica", 20f);
					cell.TextLabel.Text = "          " + seq.getName ();
					cell.TextLabel.LineBreakMode = UILineBreakMode.WordWrap;
					cell.TextLabel.Lines = 0;
					cell.TextLabel.TextAlignment = UITextAlignment.Left;
					cell.TextLabel.TextColor = UIColor.FromRGB (0, 105, 170);

				} else if (seq is IOption) {
					string blankSpace = "    "; 
					cell.TextLabel.Font = UIFont.FromName ("Helvetica", 20f);
					if ((seq as Option).LevelID != null && (seq as Option).LevelID > 0) {
						blankSpace =	"               ";
					}
					cell.TextLabel.Text = blankSpace + seq.getName ();
					cell.TextLabel.LineBreakMode = UILineBreakMode.WordWrap;
					cell.TextLabel.Lines = 0;
					cell.TextLabel.TextAlignment = UITextAlignment.Left;

					//cell.TextLabel.TextColor = UIColor.FromRGB (0, 102, 0);
					cell.TextLabel.TextColor = UIColor.FromRGB (0, 105, 170);
				} else {
					cell.TextLabel.Text = seq.getName ();
					cell.TextLabel.LineBreakMode = UILineBreakMode.WordWrap;
					cell.TextLabel.Lines = 0;
					cell.TextLabel.TextAlignment = UITextAlignment.Left;
					cell.TextLabel.Font = UIFont.FromName ("Helvetica", 20f);
					cell.TextLabel.TextColor = UIColor.FromRGB (0, 105, 170);
					
				}
			}


			cell.BackgroundColor = UIColor.Clear;

			if (seq == CurrSeq) {
				if (parentController != null) {
					//if (!isInOptionSelect) {
					iSInspectionInProgress = parentController.GetiSInspectionInProgress ();
					//}
				}
					
				if (iSInspectionInProgress) {
					cell.BackgroundColor = UIColor.LightGray;
					// orange
					cell.TextLabel.TextColor = UIColor.FromRGB (240, 84, 35);
				}
			}


			return cell;
		}

		private void ClearVisibleRows (UITableView tableView, ITraversible selectedSeq)
		{
			if (tableView != null) {
				var VisibleRows = tableView.IndexPathsForVisibleRows;

				if (VisibleRows != null) {
					foreach (var VisibleRow in VisibleRows) {
						var cell = tableView.CellAt (VisibleRow);

						if (cell != null) {
							if (selectedSeq.enableRow) {
								
								cell.TextLabel.TextColor = UIColor.FromRGB (0, 105, 170);
							}
							cell.BackgroundColor = UIColor.Clear;
						}
					}
				}
			}
				
		}

		public async override void RowSelected (UITableView tableView, Foundation.NSIndexPath indexPath)
		{
			bool iSInspectionInProgress = false;
			if (parentController != null && parentController.TableViewVisibility ()) {
				

				ITraversible selectedSeq = SequenceItems [indexPath.Row];//get object from hiearchy by row
				// clear all visible rows
				ClearVisibleRows (tableView, selectedSeq);
				var cell = tableView.CellAt (indexPath);
				if (cell != null) {
					if (selectedSeq.enableRow) {
						iSInspectionInProgress = parentController.GetiSInspectionInProgress ();
						if (iSInspectionInProgress) {
							cell.BackgroundColor = UIColor.LightGray;
							// orange
							cell.TextLabel.TextColor = UIColor.FromRGB (240, 84, 35);
						}
						// set lblSequence to match current level, space, option
						parentController.SetSeqLabel (cell.TextLabel.Text.Trim ());
					}

				}
				await Task.Run (() => parentController.shouldSavePunchList ());
				// reset layout
				parentController.SetHeaderLayout (selectedSeq);


				if (selectedSeq is IOption) {
					bool HasPunchListItems = false;

					var CheckListItems = (selectedSeq as Option).checkListItems;

					if (CheckListItems != null)
					{
						HasPunchListItems = CheckListItems.Where(x => x.itemType == ItemType.PunchList).Any();


					}

					if (HasPunchListItems)
					{
						parentController.UpdateRightTableView(selectedSeq);
						parentController.FillInspectionItem(selectedSeq);
						parentController.SetlblinspectionNameRightHidden(false);



						parentController.SetControlVisibility(false, selectedSeq);

						parentController.SetButtonVisibility(selectedSeq);

						parentController.SetSeqLabel("Punch List Items");
					}
					else {
						parentController.UpdateRightTableView(selectedSeq);

						parentController.FillInspectionItem(selectedSeq);

						parentController.SetControlVisibility(false, selectedSeq);

						parentController.SetButtonVisibility(selectedSeq);


						parentController.SetSeqLabel((selectedSeq as IOption).getName());

						parentController.SetlblinspectionNameRightHidden(false);
					}


			
				} else if (selectedSeq is ISequence) {
					parentController.UpdateRightTableView (selectedSeq);


					var Levels = (selectedSeq as ISequence).Levels;
					if (Levels != null && Levels.Count > 0)
					{
						parentController.FillLevelsTable(selectedSeq);

						parentController.AddPhotosToCurrentSequence(selectedSeq);
						parentController.SetSeqLabel("Please make a selection");
						parentController.SetlblinspectionNameRightHidden(true);


						parentController.SetControlVisibility(false, selectedSeq);
					}
					else {
						bool HasPunchList = false;
						var Options = (selectedSeq as ISequence).Options;

						if (Options != null && Options.Count > 0)
						{



							foreach (var _Option in Options)
							{
								
								_Option.prevSeqNextClicked = false;

								var CheckLists = _Option.checkListItems;

								if (CheckLists != null)
								{
									HasPunchList = CheckLists.Where(x => x.itemType == ItemType.PunchList).Any();

									if (HasPunchList)
										break;
								}
							}
							if (HasPunchList)
							{
								parentController.RebuildSequenceTable();
								parentController.selectNextRow(false);
								//parentController.FillInspectionItem(selectedSeq);

								parentController.SetlblinspectionNameRightHidden(true);


								//parentController.SetSeqLabel("Punch List Items");

								parentController.SetControlVisibility(false, selectedSeq);
								parentController.SetButtonVisibility(selectedSeq);
							}
							else {
								bool HasCheckList = false;
								foreach (var _Option in Options)
								{
									var CheckLists = _Option.checkListItems;

									if (CheckLists != null)
									{
										HasCheckList = true;
										break;
									}

								}
								if (HasCheckList)
								{
									parentController.UpdateRightTableView(selectedSeq);
									parentController.FillOptionsTable1(selectedSeq, Options);
									parentController.AddPhotosToCurrentSequence(selectedSeq);
									parentController.SetSeqLabel("Please make a selection");
									parentController.SetlblinspectionNameRightHidden(true);


									parentController.SetControlVisibility(false, selectedSeq);
								}
								else {
								}
							}

							//parentController.UpdateRightTableView(selectedSeq);
						}



						//parentController.SetControlVisibility(false, selectedSeq);
					}




				} else if (selectedSeq is ILevel) {
					
					parentController.UpdateRightTableView (selectedSeq);

					var Spaces = (selectedSeq as ILevel).getSpaces();

					if (Spaces != null && Spaces.Count > 0)
					{
						parentController.FillSpacesTable(selectedSeq);
					}
					else {
						var Options = (selectedSeq as ILevel).Options;

						if (Options != null && Options.Count > 0)
						{
							parentController.FillOptionsTable1(selectedSeq, Options);
						} 
					}

					parentController.LevelName = (selectedSeq as ILevel).getName ();

					parentController.SetSeqLabel ("Please make a selection");

					parentController.SetlblinspectionNameRightHidden (true);

					parentController.SetControlVisibility (false, selectedSeq);


				} else if (selectedSeq is ISpace) {
					
					var Options = (selectedSeq as ISpace).Options;

					if (Options != null && Options.Count > 0) {
						parentController.UpdateRightTableView (selectedSeq);

						parentController.FillOptionsTable1 (selectedSeq, Options);



						parentController.SetSeqLabel ("Please make a selection");


						parentController.SetlblinspectionNameRightHidden (true);


						parentController.SetControlVisibility (false, selectedSeq);



					}

				}
				parentController.buttonVisibility ();

				parentController.SetButtonVisibility (selectedSeq);
				parentController.buttonStyleRefresh (null);
			}
		}

		public override nfloat GetHeightForRow (UITableView tableView, NSIndexPath indexPath)
		{
			return 55f;
		}

		public override void CellDisplayingEnded (UITableView tableView, UITableViewCell cell, NSIndexPath indexPath)
		{
			foreach (var subCell in cell.Subviews) {
				subCell.Dispose ();
				subCell.RemoveFromSuperview ();
			}
		}

	}
}