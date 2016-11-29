using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;
using System.Collections.Generic;
using Model;
using DAL.Repository;
using DAL.Utility;
using System.Linq;
using DAL;


namespace LiRoInspect.iOS
{
	//public delegate void updateSyncNotificationn(string notifyStr,string notifyDetails);

	public class notifySource: UITableViewSource
	{
		
		//public List<string> syncNotifications = new List<string> ();
		public List<Notification> syncNotifications = new List<Notification> ();

		#region implemented abstract members of UITableViewSource
		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			UITableViewCell cell = new UITableViewCell(UITableViewCellStyle.Subtitle,"def"); 
			cell.TextLabel.Text = syncNotifications[indexPath.Row].message;
			cell.DetailTextLabel.Text = syncNotifications[indexPath.Row].inspectionDetail;
			if (!syncNotifications [indexPath.Row].seen) {
				cell.BackgroundColor = UIColor.FromRGB (135,206,250);
			}

			return cell;
		}
		public override nint RowsInSection (UITableView tableview, nint section)
		{
			return syncNotifications.Count;
		}
		#endregion
	}
	public class notifySource2: UITableViewSource
	{
		public List<Model.Notifications> syncNotifications = new List<Model.Notifications> ();
		protected const string cellNotificationCell = "notificationCell";
		public notifySource2(List<Notifications> syncNotifications)
		{
			this.syncNotifications = syncNotifications;
		}
		#region implemented abstract members of UITableViewSource
		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			notificationCell cell = tableView.DequeueReusableCell (cellNotificationCell,indexPath) as notificationCell;
			cell.UpdateCell ( syncNotifications [indexPath.Row].notificationmessage, syncNotifications [indexPath.Row].inProgress,syncNotifications [indexPath.Row].count);
//			cell.UpdateCell ("type", "details", "bool");
//			cell.type.Text = syncNotifications [indexPath.Row].notificationType;
//			cell.message.Text = syncNotifications [indexPath.Row].notificationmessage;
			//cell.status.Text = syncNotifications [indexPath.Row].inProgress;

//			cell.ty
//			cell.UpdateCell (this.parentController, tableView, indexPath, this.checkListItems[indexPath.Row]);
//			return cell;

//			UITableViewCell cell = new UITableViewCell(UITableViewCellStyle.Default,"def"); 
//			cell.TextLabel.Text = syncNotifications [indexPath.Row].notificationType + syncNotifications [indexPath.Row].notificationmessage + " " + syncNotifications [indexPath.Row].count;
//	//		cell.DetailTextLabel.Text = syncNotifications[indexPath.Row].notificationmessage +" "+syncNotifications[indexPath.Row].count+" ";
//			if (syncNotifications [indexPath.Row].inProgress)
//			{
//				cell.TextLabel.Text+= "*****in Progress****";
//				cell.BackgroundColor = UIColor.Blue;
//			}
			return cell;
		}
		public override nint RowsInSection (UITableView tableview, nint section)
		{
			return syncNotifications.Count;
		}
		#endregion
	}
	partial class SyncNotification : UIViewController
	{
		public SyncNotification (IntPtr handle) : base (handle)
		{

		}
        public override void ViewDidLoad()
        {
            try
            {
                base.ViewDidLoad();
				AppDelegate.dataSync.notifiy+=  notificationChange;
				reloadNotifications("COMPLETE","");
				//AppDelegate.dataSync.notifiy+= (notificationEventArgs e) => (Console.WriteLine("hey, got event"));
			}
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception Occured in ViewDidLoad method due to " + ex.Message);
            }
        }
		void notificationChange(notificationEventArgs e)
		{
			this.InvokeOnMainThread (delegate 
			{
				Console.WriteLine (" got event");
				if (e.isSyncCompleted)
				{
					reloadNotifications("COMPLETE","");
				} else
				{
					reloadNotifications (e.current.notificationTypeID, e.current.notificationType);
				};
			
			});
		}
		void reloadNotifications(string inProgID,string inProgType)
		{
			NotificationRepository notificationRep = new NotificationRepository (AppDelegate.DatabaseContext);
			List<NotificationDO> notificationDOs = notificationRep.GetEntities ().ToList ();
			if (notificationDOs != null && notificationDOs.Count > 0) {
				notificationDOs = notificationDOs.OrderByDescending(i=>i.ID).ToList();
			}
			DateTime today = DateTime.Today;
			List<NotificationDO> prevNotifications=new List<NotificationDO>();
			prevNotifications = notificationDOs.Where (n => n.NotificationDate < today) != null ? notificationDOs.Where (n => n.NotificationDate < today).ToList():prevNotifications;

			foreach(var prevNotification in prevNotifications)
			{
				notificationRep.DeleteEntity (prevNotification.ID);
				notificationDOs.Remove (prevNotification);
			}
			List<Model.Notifications> notifications=Converter.GetNotificationList(notificationDOs);

			if(inProgID!="COMPLETE" )
			{
				Model.Notifications inProgressNotification = notifications.Find (n => n.notificationType == inProgType && n.notificationTypeID == inProgID);
				if (inProgressNotification != null)
				{
					inProgressNotification.inProgress = true;
					//AppDelegate.dataSync.notifiy-=  notificationChange;
				}
			}

			notifySource2 nsrc = new notifySource2(notifications);
			NotifyTable.Source = nsrc;
			NotifyTable.RowHeight=100.0f;
			NotifyTable.TableFooterView = new UIView(new CoreGraphics.CGRect(0, 0, 0, 0));
			NotifyTable.ReloadData ();
		}

        public override void ViewWillDisappear(bool animated)
        {
            try
            {
				AppDelegate.dataSync.notifiy-=  notificationChange;
                base.ViewWillDisappear(animated);
                BaseViewController.syncNotifications.Clear();
                BaseViewController.syncNotifications.Add(new Notification() { message = "No Notifications", inspectionDetail = "", seen = true });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception Occured in GetSupportedInterfaceOrientations method due to " + ex.Message);
            }
        }
	}
}
