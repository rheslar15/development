using System;
using System.IO;
using UIKit;
using MonoTouch;
using Foundation;

using System.Collections.Generic;
using CoreGraphics;
using CoreAnimation;




namespace LiRoInspect.iOS
{
	public class InspectionsItems
	{
		public string inspectionType { get; set; }
		public string inspectionID { get; set; }
		public string inspectionDate { get; set; }
		public string projectName { get; set; }
		public string projectID { get; set; }
		public string pathway { get; set; }
		public string houseownerName { get; set; }
		public string houseownerID { get; set; }
		public string PhoneNo { get; set; }
		public string representativeName { get; set; }
		public string contractorName { get; set; }
		public string inspectionAttemptCount { get; set; }
		public string contractNo { get; set; }
		public string	Activity{get; set;}
	}



	/**************TableSource******************/
	/*class TableSource : UITableViewSource
	{
		public override nint NumberOfSections(UITableView tableView)
		{
			//return the actual number of sections
			return 3;

		}


		public string[] tableItems;
		protected string cellIdentifier = "TableCell";
		public TableSource (string[] items)
		{
			tableItems = items;
		}

		public override nint RowsInSection (UITableView tableview, nint section)
		{
			//return tableItems.Length;
			return 3;
		}

		public override void RowSelected(UITableView tableView, Foundation.NSIndexPath indexPath)
		{

		}

		public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
		{
			// In here you could customize how you want to get the height for row. Then   
			// just return it. 
			if (tableItem.HeaderType == "Overdue" || tableItem.HeaderType == "Today" || tableItem.HeaderType == "Upcoming")
			return 140;
		}
		public void SelectItem(UITableView tableView, NSIndexPath indexPath)
		{
			//  MapController mapController = new MapController();
			//  _lakelevelsController.NavigationController.PushViewController(mapController, true);
		}

		[Export ("tableView:TitleForHeaderInSection:")]
		public virtual string TitleForHeader (UITableView tableView, int section)
		{

			switch (section)
			{
			case 0:
				return "List1";


			case 1:
				return "List1";

			}

			return string.Empty;


			//			}



		}
		public override   UITableViewCell GetCell (UITableView tableView,Foundation.NSIndexPath indexPath)
		{
			// request a recycled cell to save memory
			UITableViewCell cell = tableView.DequeueReusableCell (cellIdentifier);
			// if there are no cells to reuse, create a new one
			if (cell == null)
				cell = new UITableViewCell (UITableViewCellStyle.Default, cellIdentifier);
			cell.Accessory= UITableViewCellAccessory.DisclosureIndicator;
			cell.TextLabel.Text = tableItems[indexPath.Row];
			return cell;
		}

		public override void CommitEditingStyle (UITableView tableView, UITableViewCellEditingStyle editingStyle, Foundation.NSIndexPath indexPath)
		{
			switch (editingStyle) {
			case UITableViewCellEditingStyle.Delete:
				// remove the item from the underlying data source
				//tableItems.RemoveAt(indexPath.Row);
				// delete the row from the table
				tableView.DeleteRows (new NSIndexPath[] { indexPath }, UITableViewRowAnimation.Fade);
				break;
			case UITableViewCellEditingStyle.None:
				Console.WriteLine ("CommitEditingStyle:None called");			
				break;
			}
		}

		public override bool CanEditRow (UITableView tableView, NSIndexPath indexPath)
		{
			return true; // return false if you wish to disable editing for a specific indexPath or for all rows
		}*/
	
	//public override string TitleForDeleteConfirmation (UITableView tableView, NSIndexPath indexPath)
	//	{   // Optional - default text is 'Delete'
	//	return "Trash (" + tableItems[indexPath.Row].SubHeading + ")";

}





	




	


	


	
			
	
	
