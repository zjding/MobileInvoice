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
using System.Globalization;
using SharpMobileCode.ModalPicker;

namespace MobileInvoice.ios
{
    public partial class InvoiceListController : UITableViewController
    {
		public List<Invoice> invoiceList = new List<Invoice>();
		public List<Invoice> filteredInvoiceList = new List<Invoice>();

		public UISearchController searchController;
		public UISearchBar searchBar;

		public bool bSearching = false;

        public InvoiceListController (IntPtr handle) : base (handle)
        {
        }

		async public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			TableView.TableFooterView = new UIView();

			SetupMenuView();

			this.ExtendedLayoutIncludesOpaqueBars = true;

			LoadingOverlay loadingOverlay = new LoadingOverlay(UIScreen.MainScreen.Bounds);
			this.View.Add(loadingOverlay);

			await LoadInvoices("a");

			loadingOverlay.Hide();

			TableView.ReloadData();
		}

		public override nint NumberOfSections(UITableView tableView)
		{
			return 1;
		}

		public override nint RowsInSection(UITableView tableView, nint section)
		{
			if (!bSearching)
				return invoiceList.Count;
			else
				return filteredInvoiceList.Count;
		}

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			InvoiceListCell cell = this.TableView.DequeueReusableCell("InvoiceListCell") as InvoiceListCell;

			Invoice _invoice;

			if (!bSearching)
				_invoice = invoiceList[indexPath.Row];
			else
				_invoice = filteredInvoiceList[indexPath.Row];

			cell.lblClientName.Text = _invoice.ClientName;
			cell.lblInvoiceName.Text = _invoice.Name;
			cell.lblTotal.Text = _invoice.Total.ToString("C", CultureInfo.CurrentCulture);
			cell.lblDueDate.Text = _invoice.DueDate.ToShortDateString();
			cell.status = _invoice.Status;

			return cell;
		}

		private void SetupMenuView()
		{
			var items = new string[] { "All", "Draft", "Sent", "Paid", "Overdue" };

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

			menuView.MenuSelected += Menu_Selected;

			this.NavigationItem.TitleView = menuView;
		}

		async void Menu_Selected(object sender, ItemSelectedEventArgs e)
		{
			string status = "";

			if (e.Item.ToString() == "All")
				status = "a";
			else if (e.Item.ToString() == "Draft")
				status = "d";
			else if (e.Item.ToString() == "Sent")
				status = "s";
			else if (e.Item.ToString() == "Paid")
				status = "p";
			else if (e.Item.ToString() == "Overdue")
				status = "o";

			LoadingOverlay loadingOverlay = new LoadingOverlay(UIScreen.MainScreen.Bounds);
			this.View.Add(loadingOverlay);

			await LoadInvoices(status);

			loadingOverlay.Hide();

			TableView.ReloadData();
		}

		async Task<int> LoadInvoices(string status)
		{

			HttpClient httpClient = new HttpClient();

			string result = await httpClient.GetStringAsync(Helper.GetInvoicesByStatusURL()+ "/" + status + "/");

			invoiceList.Clear();

			invoiceList = JsonConvert.DeserializeObject<List<Invoice>>(result);

			return invoiceList.Count;
		}

		partial void btnSearch_UpInside(UIBarButtonItem sender)
		{
			if (searchController == null)
			{
				searchController = new UISearchController((UIViewController)null);

				searchController.DimsBackgroundDuringPresentation = false;
				DefinesPresentationContext = true;

				searchBar = searchController.SearchBar;
				//searchBar = new UISearchBar();
				searchBar.Placeholder = "Enter Search Text";
				searchBar.SizeToFit();
				searchBar.AutocorrectionType = UITextAutocorrectionType.No;
				searchBar.AutocapitalizationType = UITextAutocapitalizationType.None;
				searchBar.BarTintColor = UIColor.White;

				foreach (var view in searchBar.Subviews)
				{
					foreach (var subview in view.Subviews)
					{
						if (subview is UITextField)
						{
							(subview as UITextField).BackgroundColor = UIColor.FromRGB(247, 247, 247);
						}
					}
				}

				searchBar.TextChanged += SearchBar_TextChanged;
				searchBar.CancelButtonClicked += SearchBar_CancelButtonClicked;
				//searchBar.OnEditingStarted += SearchBar_OnEditingStarted;

				TableView.TableHeaderView = searchBar;
			}
			else
			{
				searchController.Active = false;

				searchController.RemoveFromParentViewController();
				searchController = null;
				TableView.TableHeaderView = null;
			}
		}

		void SearchBar_TextChanged(object sender, UISearchBarTextChangedEventArgs e)
		{
			if (searchBar.Text.Length == 0)
			{
				bSearching = false;
				TableView.ReloadData();
			}
			else
			{
				bSearching = true;

				filteredInvoiceList = invoiceList.FindAll(c => 	c.Name.ToUpper().Contains(searchBar.Text.ToUpper()) || 
				                                          c.ClientName.ToUpper().Contains(searchBar.Text.ToUpper()) 
				                                         );
				TableView.ReloadData();
			}
		}

		void SearchBar_CancelButtonClicked(object sender, EventArgs e)
		{
			bSearching = false;
			TableView.ReloadData();
		}

		partial void btnCalendar_UpInside(UIBarButtonItem sender)
		{
			var yearList = new List<string>();
			yearList.Add("2017");
			yearList.Add("2016");

			var modalPicker = new ModalPickerViewController(ModalPickerType.Custom, "Select a Year", this)
			{
				HeaderBackgroundColor = UIColor.FromRGB(26, 188, 156),
				HeaderTextColor = UIColor.White,
				TransitioningDelegate = new ModalPickerTransitionDelegate(),
				ModalPresentationStyle = UIModalPresentationStyle.Custom
			};

			modalPicker.PickerView.Model = new CustomPickerModel(yearList);

			modalPicker.OnModalPickerDismissed += (s, ea) =>
			{
				
			};

			PresentViewController(modalPicker, true, null);
		}

		public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
		{
			//base.RowSelected(tableView, indexPath);
		}
	}
}