using System;
using UIKit;
using System.Collections.Generic;
using Foundation;
using System.Linq;


namespace LiRoInspect.iOS
{
	public class DB_DetailTableSource:UITableViewSource
	{
		Dictionary<string,string> DescTableItems ;
		nfloat rowHeight=60f;
		public DB_DetailTableSource(IntPtr handle) : base(handle)
		{
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="LiRoInspect.iOS.DB_DetailTableSource"/> class.
		/// </summary>
		/// <param name="inspectioDet">Inspectio det.</param>
		public DB_DetailTableSource (Model.Inspection inspectioDet)
		{
			DescTableItems = inspectioDet.GetProjectDetail ();
		}

		#region implemented abstract members of UITableViewSource
		/// <summary>
		/// Gets the cell.
		/// </summary>
		/// <returns>The cell.</returns>
		/// <param name="tableView">Table view.</param>
		/// <param name="indexPath">Index path.</param>
		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			var cell = (ProjectDetailCell)tableView.DequeueReusableCell ("projectDetailCell");
			var dataDictionary = DescTableItems.ElementAt (indexPath.Row);
			cell.UpdateData (dataDictionary.Key, dataDictionary.Value);
			return cell;
		}
		/// <summary>
		/// Rowses in the section.
		/// </summary>
		/// <returns>The in section.</returns>
		/// <param name="tableview">Tableview.</param>
		/// <param name="section">Section.</param>
		public override nint RowsInSection (UITableView tableview, nint section)
		{
			return DescTableItems.Count;
		}

		/// <summary>
		/// Gets the height for row.
		/// </summary>
		/// <returns>The height for row.</returns>
		/// <param name="tableView">Table view.</param>
		/// <param name="indexPath">Index path.</param>
		public override nfloat GetHeightForRow (UITableView tableView, NSIndexPath indexPath)
		{
			return rowHeight;
		}
		#endregion		
	}
}

