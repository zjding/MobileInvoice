using Foundation;
using System;
using UIKit;
using CoreGraphics;
using System.Linq;
using System.Collections.Generic;
using MobileInvoice.model;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace MobileInvoice.ios
{
    public partial class InvoiceListController : UITableViewController
    {
		public List<Invoice> invoiceList = new List<Invoice>();

        public InvoiceListController (IntPtr handle) : base (handle)
        {
        }

		async public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			SetupMenuView();

			await LoadInvoices();

			TableView.ReloadData();
		}

		public override nint NumberOfSections(UITableView tableView)
		{
			return 1;
		}

		public override nint RowsInSection(UITableView tableView, nint section)
		{
			return invoiceList.Count;
		}

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			InvoiceListCell cell = this.TableView.DequeueReusableCell("InvoiceListCell") as InvoiceListCell;

			return cell;
		}

		private void SetupMenuView()
		{
			var items = new string[] { "All", "Unpaid", "Paid", "Overdue" };

			this.NavigationController.NavigationBar.Translucent = false;
			this.NavigationController.NavigationBar.BarTintColor = new UIColor(
				(nfloat)(55.0 / 255.0),
				(nfloat)(187 / 255.0),
				(nfloat)(153 / 255.0),
				(nfloat)1.0
			);

			UINavigationBar.Appearance.TitleTextAttributes = new UIStringAttributes
			{
				ForegroundColor = UIColor.White
			};

			var menuView = new DropdownMenuView(
				new CGRect(0.0, 0.0, 300, 44),
				items.First(),
				items,
				this.View);

			menuView.CellHeight = 50;
			menuView.CellBackgroundColor = this.NavigationController.NavigationBar.TintColor;

			menuView.CellSelectionColor = new UIColor(
				(nfloat)(0.0 / 255.0),
				(nfloat)(160.0 / 255.0),
				(nfloat)(195.0 / 255.0),
				(nfloat)1.0
			);

			menuView.CellTextLabelColor = UIColor.White;
			menuView.CellTextLabelFont = UIFont.FromName(name: "AvenirNext-Medium", size: 16);
			menuView.ArrowPadding = 15;
			menuView.AnimationDuration = 0.3;
			menuView.MaskBackgroundColor = UIColor.Black;
			menuView.MaskBackgroundOpacity = (nfloat)0.3;
			menuView.BounceOffset = 5;


			this.NavigationItem.TitleView = menuView;
		}

		async Task<int> LoadInvoices()
		{

			HttpClient httpClient = new HttpClient();

			string result = await httpClient.GetStringAsync(Helper.GetInvoicesURL());

			invoiceList = JsonConvert.DeserializeObject<List<Invoice>>(result);

			return invoiceList.Count;
		}
    }
}