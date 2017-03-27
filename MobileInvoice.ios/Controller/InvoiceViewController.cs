using Foundation;
using System;
using UIKit;
using MobileInvoice.model;
using System.Globalization;

namespace MobileInvoice.ios
{
    public partial class InvoiceViewController : UITableViewController
    {
		public Invoice invoice = new Invoice();

        public InvoiceViewController (IntPtr handle) : base (handle)
        {
        }

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);

			TableView.ReloadData();
		}

		public override void WillDisplayHeaderView(UITableView tableView, UIView headerView, nint section)
		{

			var header = headerView as UITableViewHeaderFooterView;

			header.TextLabel.TextColor = UIColor.DarkGray;
			//header.TextLabel.Font = UIFont.BoldSystemFontOfSize(12);
			header.TextLabel.Font = UIFont.FromName("AvenirNext-Bold", 12);
		}

		public override string TitleForHeader(UITableView tableView, nint section)
		{
			if (section == 0)
				return " ";
			if (section == 1)
				return " ";
			if (section == 2)
				return "ITEMS";

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
			return 4;
		}

		public override nint RowsInSection(UITableView tableView, nint section)
		{
			if (section == 0)       // date
				return 1;
			else if (section == 1)  // client name
				return 1;
			else if (section == 2)  // items
				return invoice.Items.Count + 1;
			else if (section == 3)  // totall
				return 5;

			return 0;
		}

		public override string TitleForFooter(UITableView tableView, nint section)
		{
			if (section == 1)
				return " ";

			if (section == 3)
				return " ";

			if (section == 4)
				return " ";

			if (section == 7)
				return " ";

			return base.TitleForFooter(tableView, section);
		}

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			if (indexPath.Section == 0)
			{
				InvoiceDateCell cell = this.TableView.DequeueReusableCell("InvoiceDateCellIdentifier") as InvoiceDateCell;
				cell.callingController = this;
				//cell.TextLabel.Text = "Client";
				//cell.DetailTextLabel.Text = client.FirstName + " " + client.LastName;

				return cell;
			}
			else if (indexPath.Section == 1)
			{
				InvoiceClientNameCell cell = this.TableView.DequeueReusableCell("InvoiceClientNameCellIdentifier") as InvoiceClientNameCell;
				cell.lblClientName.Text = invoice.Client.Name;
				//cell.TextLabel.Text = "Client";
				//cell.DetailTextLabel.Text = client.FirstName + " " + client.LastName;

				return cell;

			}
			else if (indexPath.Section == 2)
			{
				if (indexPath.Row == 0)
				{
					InvoiceAddItemCell cell = this.TableView.DequeueReusableCell("InvoiceAddItemCellIdentifier") as InvoiceAddItemCell;

					return cell;
				}
				else
				{
					InvoiceItemCell cell = this.TableView.DequeueReusableCell("InvoiceItemCellIdentifier") as InvoiceItemCell;

					cell.lblNum.Text = indexPath.Row.ToString();
					cell.lblItemName.Text = invoice.Items[indexPath.Row - 1].Name;
					cell.lblUnitPrice.Text = invoice.Items[indexPath.Row - 1].UnitPrice.ToString("C", CultureInfo.CurrentCulture) + " x " + invoice.Items[indexPath.Row - 1].Quantity.ToString();
					cell.lblTotalPrice.Text = (invoice.Items[indexPath.Row - 1].UnitPrice * invoice.Items[indexPath.Row - 1].Quantity).ToString("C", CultureInfo.CurrentCulture);

					return cell;
				}

			}
			else  // total
			{
				if (indexPath.Row == 0)
				{
					InvoiceSubtotalCell cell = this.TableView.DequeueReusableCell("InvoiceSubtotalCell") as InvoiceSubtotalCell;
					return cell;
				}
				else if (indexPath.Row == 1)
				{
					InvoiceTaxCell cell = this.TableView.DequeueReusableCell("InvoiceTaxCell") as InvoiceTaxCell;
					return cell;
				}
				else if (indexPath.Row == 2)
				{
					InvoiceDiscountCell cell = this.TableView.DequeueReusableCell("InvoiceDiscountCell") as InvoiceDiscountCell;
					return cell;
				}
				else if (indexPath.Row == 3)
				{
					InvoicePaidCell cell = this.TableView.DequeueReusableCell("InvoicePaidCell") as InvoicePaidCell;
					return cell;
				}
				else
				{
					InvoiceBalanceCell cell = this.TableView.DequeueReusableCell("InvoiceBalanceCell") as InvoiceBalanceCell;
					return cell;
				}
			}
		}

		public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
		{
			if (segue.Identifier == "Invoice_to_ClientList_Segue")
			{
				ClientsController clientsCtl = segue.DestinationViewController as ClientsController;
				clientsCtl.bPickClientMode = true;
				clientsCtl.callingController = this;
			}
			else if (segue.Identifier == "Invoice_To_NewItem_Segue")
			{
				InvoiceItemDetailController destCtrl = (segue.DestinationViewController as UINavigationController).ViewControllers[0] as InvoiceItemDetailController;
				destCtrl.callingController = this;
				destCtrl.bNewMode = true;
			}

			base.PrepareForSegue(segue, sender);
		}

		partial void btnClose_UpInside(UIBarButtonItem sender)
		{
			DismissViewController(true, null);
		}
	}
}