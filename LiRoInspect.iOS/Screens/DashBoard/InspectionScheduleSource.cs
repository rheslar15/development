using System;
using UIKit;
using System.Collections.Generic;
using Foundation;
using CoreAnimation;
using CoreGraphics;
using System.Linq;

namespace LiRoInspect.iOS
{
	public class InspectionScheduleSource :UITableViewSource
	{
		// dataSource 
		List<Model.Inspection> inspectionDetailInfo;

		IEnumerable<Model.Inspection> todayInspection;
		IEnumerable<Model.Inspection> upComingInspection;
		string HeaderTitle="Total Inspections: ";

		//---- declare vars
		List<DashBoardLeftTableItem> tableItems;
		string SectionCellIdentifier = "DashboardLHSSectionCell";
		string ContentCellIdentifier = "DashboardLHSContentCell";
		DashBoardEventArgs dashBoardEvenArgs = new DashBoardEventArgs ();
		public event EventHandler DashBoardRowSelected;

		public InspectionScheduleSource (List<DashBoardLeftTableItem> tableItems,List<Model.Inspection> inspectionDetailInfo)
		{
			this.tableItems = tableItems;
			this.inspectionDetailInfo = inspectionDetailInfo;
			this.todayInspection = inspectionDetailInfo.Where<Model.Inspection> (a => a.inspectionDateTime.Date <= DateTime.Today.Date).OrderByDescending (b=>b.inspectionDateTime);
			this.upComingInspection = inspectionDetailInfo.Where<Model.Inspection> (a => a.inspectionDateTime.Date>DateTime.Today.Date).OrderByDescending (b=>b.inspectionDateTime);
		}


		#region -= data binding/display methods =-

		/// <summary>
		/// Called by the TableView to determine how many sections(groups) there are.
		/// </summary>
		public override nint NumberOfSections (UITableView tableView)
		{
			return 1;

		}


		/// <summary>
		/// Called by the TableView to get the actual UITableViewCell to render for the particular section and row
		/// Left Table cell
		/// </summary>
		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			DashBoardLeftTableItem tableItem = tableItems [indexPath.Row];
			DashBoardLeftTableItem tableItemNext=null;
			int inspectionCount = 0;
			if(tableItems.Count>indexPath.Row+1)
			{
				tableItemNext = tableItems [indexPath.Row+1];
			}

			if (tableItem.HeaderType ==HeaderType.Today || tableItem.HeaderType == HeaderType.Upcoming) {
				string textToDisplay = string.Empty;
				string arrowImage = "";
				string CalDate=DateTime.Today.Date.ToString ("MMM dd");;
				if(indexPath.Row == 0)
				{
					if(todayInspection.Count()>0)
					CalDate= todayInspection.FirstOrDefault ().inspectionDateTime.ToString ("MMM dd");
					textToDisplay = "Today";
					inspectionCount = todayInspection.Count();
					if (tableItemNext != null && (tableItemNext.Heading == null || tableItemNext.Heading.Trim () == "")) {
						arrowImage = "greendownward";
					} else {
						arrowImage = "greenForward";
					}
				}
				else
				{
					textToDisplay = "Upcoming";
					if (upComingInspection.Count () > 0) {
						CalDate = upComingInspection.FirstOrDefault ().inspectionDateTime.ToString ("MMM dd");
					} else {
						CalDate = "";
					}
					inspectionCount = upComingInspection.Count();
					if (tableItemNext != null && (tableItemNext.Heading == null || tableItemNext.Heading.Trim () == "")) {
						arrowImage = "blueDownward";
					} else {
						arrowImage = "blueForward";
					}
				}
				var cell = tableView.DequeueReusableCell (SectionCellIdentifier) as DashBoardLHSSectionCell;
				if (cell == null)
					cell = new DashBoardLHSSectionCell ((NSString)SectionCellIdentifier);
				cell.UpdateCell(tableItem.Heading,tableItem.SubHeading,inspectionCount.ToString(), UIImage.FromBundle(arrowImage),CalDate);
				cell.BackgroundColor=UIColor.Clear;
				return cell;

			}else
			{
				var cell = (DashBoardLHSContentCell)tableView.DequeueReusableCell (ContentCellIdentifier);
				if (cell == null)
					cell = new DashBoardLHSContentCell ((NSString)ContentCellIdentifier);
				cell.UpdateCell (tableItems[indexPath.Row].InspectionContent,DashBoardRowSelected,indexPath.Row);
				cell.AccessoryView = new UIView ();
				cell.BackgroundColor=UIColor.Clear;
				return cell;

			} 
		}

		/// <summary>
		/// Called by the TableView to determine how many cells to create for that particular section.
		/// </summary>
		public override nint RowsInSection (UITableView tableview, nint section)
		{
			return tableItems.Count;
		}


		public override void WillDisplayHeaderView(UITableView tableView, UIView headerView, nint section)
		{
			UITableViewHeaderFooterView view = headerView as UITableViewHeaderFooterView;
			view.TextLabel.Font = UIFont.BoldSystemFontOfSize(30);
			view.TextLabel.Text = HeaderTitle + (todayInspection.Count()+upComingInspection.Count());
			view.TextLabel.TextColor = UIColor.FromRGB (63, 63, 63);
		}

		/// <summary>
		/// Called by the TableView to retrieve the header text for the particular section(group)
		/// </summary>
		public override string TitleForHeader (UITableView tableView, nint section)
		{
			return (HeaderTitle + (todayInspection.Count()+upComingInspection.Count()));
		}

		#endregion

		#region -= user interaction methods =-
		public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
		{

			DashBoardLeftTableItem tableItem = tableItems [indexPath.Row];
			int rowHeight = 0;
			 //In here you could customize how you want to get the height for row. Then   
			// just return it. 
			if (tableItem.HeaderType == HeaderType.Today || tableItem.HeaderType == HeaderType.Upcoming) {
				rowHeight =  120;
			} 
			else {
				rowHeight =  250;
			}
			return rowHeight;
		}

		//Dictionary<int, bool> indexTrack = new Dictionary<int, bool> ();
		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			DashBoardLeftTableItem tableItem = tableItems [indexPath.Row];

			if (tableItem.HeaderType == HeaderType.Today || tableItem.HeaderType == HeaderType.Upcoming) {
				DashBoardLeftTableItem nextItem = null;
				if (tableItems.Count > indexPath.Row + 1) {
					nextItem = tableItems [indexPath.Row + 1];
				}
				bool isInsert = ((nextItem == null) || ((nextItem != null) && (nextItem.HeaderType ==HeaderType.Today|| nextItem.HeaderType == HeaderType.Upcoming)));

				bool isDelete = (nextItem != null) && (nextItem.HeaderType != HeaderType.Today && nextItem.HeaderType != HeaderType.Upcoming);

				UITableViewCell cell = tableView.CellAt(indexPath);

				tableView.Editing = true;
				if (isInsert) {
					cell=(DashBoardLHSSectionCell)tableView.CellAt(indexPath);
					cell.Hidden = true;
					if (tableItem.HeaderType == HeaderType.Today) {
						cell.AccessoryView = new UIImageView (UIImage.FromBundle ("greendownward.png")); 
						tableView.SetEditing (false, true);
						//if we've half-swiped a row
						WillBeginTableEditing (tableView, indexPath,todayInspection);

					} else {
						cell.AccessoryView = new UIImageView (UIImage.FromBundle ("blueDownward.png"));
						tableView.SetEditing (false, true);
						//if we've half-swiped a row
						WillBeginTableEditing (tableView, indexPath,upComingInspection);
					}

				} else if (isDelete) {
					//indexTrack.Add (indexPath.Row, true);
					if (tableItem.HeaderType == HeaderType.Today) {
						cell.AccessoryView = new UIImageView (UIImage.FromBundle ("greenForward.png"));
						tableView.SetEditing (false, true);
						DidFinishTableEditing (tableView, indexPath,todayInspection);
					} else if (tableItem.HeaderType == HeaderType.Upcoming) {
						cell.AccessoryView = new UIImageView (UIImage.FromBundle ("blueForward.png")); 
						tableView.SetEditing (false, true);
						DidFinishTableEditing (tableView, indexPath,upComingInspection);
					}
				} 
				if (null != DashBoardRowSelected && dashBoardEvenArgs!=null) {
					dashBoardEvenArgs.RowType=RowType.Header;
					DashBoardRowSelected.Invoke (null, dashBoardEvenArgs);
				}
			} else {
				if (null != DashBoardRowSelected && dashBoardEvenArgs!=null) {
					dashBoardEvenArgs.RowType=RowType.Content;
					dashBoardEvenArgs.InspectionDetail = tableItem.InspectionContent;
					DashBoardRowSelected.Invoke (null, dashBoardEvenArgs);
				}
			}		
		}

		#endregion

		#region -= editing methods =-



		/// <summary>
		/// Called manually when the table goes into edit mode
		/// </summary>
		public void WillBeginTableEditing (UITableView tableView,NSIndexPath indexPath,IEnumerable<Model.Inspection> inspection)
		{
			//---- start animations
			tableView.BeginUpdates ();
			//---- insert a new row in the table  indexPath.Section
			int i=1;
			foreach (var insp in inspection) {
				tableView.InsertRows(new NSIndexPath[] { NSIndexPath.FromRowSection (indexPath.Row+i, indexPath.Section) }, UITableViewRowAnimation.None);
				//---- create a new item and add it to our underlying data
				tableItems.Insert (indexPath.Row + 1, new DashBoardLeftTableItem ()
					{
						HeaderType=HeaderType.TodayContent,
						InspectionContent=insp
					});
				i++;
			}	
			tableView.EndUpdates();
			tableView.ReloadData ();
		}



		/// <summary>
		/// Called manually when the table leaves edit mode
		/// </summary>
		public void DidFinishTableEditing (UITableView tableView,NSIndexPath indexPath,IEnumerable<Model.Inspection> inspection)
		{
			//---- start animations
			tableView.BeginUpdates ();
			{				
				int i=0;
				foreach (var insp in inspection) {
					//---- remove the row from the table
					tableView.DeleteRows (new NSIndexPath[] { NSIndexPath.FromRowSection (indexPath.Row + i, indexPath.Section) }, UITableViewRowAnimation.None);
					//---- remove our row from the underlying data
					tableItems.RemoveAt (indexPath.Row + 1);
					i++;
				}
				//---- finish animations

			}
			tableView.EndUpdates ();
			tableView.ReloadData ();
		}

		public override bool CanEditRow (UITableView tableView, NSIndexPath indexPath)
		{
			return false;
		}
		#endregion
	}
}

