using Foundation;
using System;
using UIKit;

namespace MobileInvoice.ios
{
    public partial class InvoiceViewController : UITableViewController
    {
        public InvoiceViewController (IntPtr handle) : base (handle)
        {
        }

		public override void WillDisplayHeaderView(UITableView tableView, UIView headerView, nint section)
		{

			var header = headerView as UITableViewHeaderFooterView;

			header.TextLabel.TextColor = UIColor.DarkGray;
			header.TextLabel.Font = UIFont.BoldSystemFontOfSize(12);

		}

		public override string TitleForHeader(UITableView tableView, nint section)
		{
			if (section == 0)
				return " ";
			//if (section == 1)
			//	return "Client";
			if (section == 2)
				return "Items";

			if (section == 4)
				return "Attachments";

			if (section == 6)
				return " ";

			if (section == 5)
				return "Note";

			if (section == 7)
				return " ";

			return base.TitleForHeader(tableView, section);
		}

		public override nint NumberOfSections(UITableView tableView)
		{
			return 1;
		}

		public override nint RowsInSection(UITableView tableView, nint section)
		{
			if (section == 0)
				return 1;

			return 0;
		}

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			//if (indexPath.Section == 0)
			//{
				InvoiceDateCell cell = this.TableView.DequeueReusableCell("InvoiceDateCellIdentifier") as InvoiceDateCell;

				//cell.TextLabel.Text = "Client";
				//cell.DetailTextLabel.Text = client.FirstName + " " + client.LastName;

				return cell;
			//}
		}
    }
}