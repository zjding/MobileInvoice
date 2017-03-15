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

		public override string[] SectionIndexTitles(UITableView tableView)
		{
			return keys.ToArray();
		}

		public override void WillDisplayHeaderView(UITableView tableView, UIView headerView, nint section)
		{

			var header = headerView as UITableViewHeaderFooterView;

			header.TextLabel.TextColor = UIColor.LightGray;
			//header.TextLabel.Font = UIFont.BoldSystemFontOfSize(12
			header.TextLabel.Font = UIFont.FromName("AvenirNext-Bold", 12);
		}

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			ClientCell cell = this.TableView.DequeueReusableCell("ClientCell") as ClientCell;
			cell.lblName.Text = clients[keys[indexPath.Section]][indexPath.Row];
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