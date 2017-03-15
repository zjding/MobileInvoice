using Foundation;
using System;
using UIKit;
using System.Collections.Generic;

namespace MobileInvoice.ios
{
    public partial class ClientsController : UITableViewController
    {
		public Dictionary<string, List<string>> clients = new Dictionary<string, List<string>>();
		public List<string> keys = new List<string>();

        public ClientsController (IntPtr handle) : base (handle)
        {
        }

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);

			TableView.ReloadData();
		}

		public override nint NumberOfSections(UITableView tableView)
		{
			return clients.Count;
		}

		public override string TitleForHeader(UITableView tableView, nint section)
		{
			return keys[(int)section];
		}

		public override nint RowsInSection(UITableView tableView, nint section)
		{
			return clients[keys[(int)section]].Count;
		}

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			UITableViewCell cell = this.TableView.DequeueReusableCell("ClientCell");
			cell.TextLabel.Text = clients[keys[indexPath.Section]][indexPath.Row];
			return cell;
		}

		public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
		{
			if (segue.Identifier == "Clients_to_NewClient_Segue")
			{
				NewClientController destCtrl = (segue.DestinationViewController as UINavigationController).ViewControllers[0] as NewClientController;
				destCtrl.callingController = this;
			}

			base.PrepareForSegue(segue, sender);
		}
    }
}