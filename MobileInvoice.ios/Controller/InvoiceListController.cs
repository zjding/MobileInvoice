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
using CloudKit;

namespace MobileInvoice.ios
{
	public partial class InvoiceListController : UITableViewController
	{
		public List<Invoice> invoiceList = new List<Invoice>();
		public List<Invoice> filteredInvoiceList = new List<Invoice>();
		public Dictionary<string, List<Invoice>> invoiceDictionary = new Dictionary<string, List<Invoice>>();
		public List<string> keyList = new List<string>();

		public UISearchController searchController;
		public UISearchBar searchBar;

		public int SelectedInvoiceId;
		public string selectedInvoiceName;

		public bool bSearching = false;

		CloudManager cloudManager;

		#region Computed Properties
		public AppDelegate ThisApp
		{
			get { return (AppDelegate)UIApplication.SharedApplication.Delegate; }
		}
		#endregion

		public InvoiceListController(IntPtr handle) : base(handle)
		{
			cloudManager = new CloudManager();
		}


		async public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			TableView.TableFooterView = new UIView();

			SetupMenuView();

			this.ExtendedLayoutIncludesOpaqueBars = true;

			//TableView.Hidden = true;

			LoadingOverlay loadingOverlay = new LoadingOverlay(UIScreen.MainScreen.Bounds);
			this.View.Add(loadingOverlay);

			//await LoadInvoices("a");

			await CK_LoadInvoices("a");

			loadingOverlay.Hide();

			//TableView.Hidden = false;

			TableView.ReloadData();
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);

			this.NavigationController.NavigationBar.BarTintColor = new UIColor(
				(nfloat)(55.0 / 255.0),
				(nfloat)(187 / 255.0),
				(nfloat)(153 / 255.0),
				(nfloat)1.0
			);

			//(this.TabBarController as InvoiceTabBarController).AddPlusButton();

			//var button = this.NavigationController.TabBarController.View.ViewWithTag(1001);
			//button.Hidden = false;

			//(this.TabBarController as InvoiceTabBarController).SelectedIndex = 2;

			//(this.TabBarController as InvoiceTabBarController).SelectedIndex = 1;

			//CGRect frame = button.Frame;
			//frame.X = this.View.Frame.Size.Width / 2 - button.Frame.Size.Width / 2;
			//button.Frame = frame;

			//LoadingOverlay loadingOverlay = new LoadingOverlay(UIScreen.MainScreen.Bounds);
			//this.View.Add(loadingOverlay);

			////await LoadInvoices("a");

			////await CK_LoadInvoices("a");

			//loadingOverlay.Hide();

			//TableView.ReloadData();

		}

		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);

			//(this.TabBarController as InvoiceTabBarController).AddPlusButton();

			//(this.TabBarController as InvoiceTabBarController).BringButtonToFront();

			//(this.TabBarController as InvoiceTabBarController).SelectedIndex = 2;

			//(this.TabBarController as InvoiceTabBarController).SelectedIndex = 1;

		}

		public override void ViewWillDisappear(bool animated)
		{
			base.ViewWillDisappear(animated);

			//var button = this.NavigationController.TabBarController.View.ViewWithTag(1001);
			//button.Hidden = true;
		}

		public override nint NumberOfSections(UITableView tableView)
		{
			return invoiceDictionary.Count;
		}

		public override nint RowsInSection(UITableView tableView, nint section)
		{
			//if (!bSearching)
			//	return invoiceList.Count;
			//else
			//	return filteredInvoiceList.Count;

			return invoiceDictionary[keyList[(int)section]].Count;
		}

		public override string[] SectionIndexTitles(UITableView tableView)
		{
			return keyList.ToArray();
		}

		public override void WillDisplayHeaderView(UITableView tableView, UIView headerView, nint section)
		{
			var header = headerView as UITableViewHeaderFooterView;

			header.TextLabel.TextColor = UIColor.LightGray;
			header.TextLabel.Font = UIFont.FromName("AvenirNext-Bold", 12);
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
			cell.lblDueDate.Text = _invoice.IssueDate.ToShortDateString();
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

			//await LoadInvoices(status);

			await CK_LoadInvoices(status);

			loadingOverlay.Hide();

			TableView.ReloadData();
		}

		async Task<int> LoadInvoices(string status)
		{

			HttpClient httpClient = new HttpClient();

			string result = await httpClient.GetStringAsync(Helper.GetInvoicesByStatusURL() + "/" + status + "/");

			invoiceList.Clear();

			invoiceList = JsonConvert.DeserializeObject<List<Invoice>>(result);

			return invoiceList.Count;
		}

		async Task<int> CK_LoadInvoices(string status)
		{
			NSPredicate predicate = NSPredicate.FromFormat(string.Format("User = '{0}'", ThisApp.UserName));

			List<CKRecord> _invoiceRecords = await cloudManager.FetchRecordsByTypeAndPredicate("Invoice", predicate);

			invoiceList.Clear();

			foreach (CKRecord _invoiceRecord in _invoiceRecords)
			{
				Invoice _invoice = new Invoice();

				// invoice record name
				CKRecordID _invoiceRecordId = (CKRecordID)_invoiceRecord["recordID"];
				string _invoiceRecordName = _invoiceRecordId.RecordName;

				_invoice.Name = _invoiceRecord["Name"].ToString();
				_invoice.RecordName = _invoiceRecordName;
				_invoice.IssueDate = Helper.NSDateToDateTime((NSDate)(_invoiceRecord["IssuedDate"]));
				_invoice.DueDate = Helper.NSDateToDateTime((NSDate)(_invoiceRecord["DueDate"]));
				_invoice.Total = Convert.ToDecimal(((NSNumber)(_invoiceRecord["Total"])).FloatValue);

				// client
				CKReference _clientReference = (CKReference)_invoiceRecord["Client"];
				string _clientRecordName = _clientReference.RecordId.RecordName;

				CKRecord _cliendRecord = await cloudManager.FetchRecordByRecordName(_clientRecordName);

				Client _client = new Client();

				_client.Name = _cliendRecord["Name"].ToString();

				_invoice.ClientName = _client.Name;

				// invoice item
				//NSPredicate _invoiceItemSearchPredicate = NSPredicate.FromFormat(string.Format("InvoiceName = '{0}'", _invoiceRecordName));
				//List<CKRecord> _invoiceItemRecords = await cloudManager.FetchRecordsByTypeAndPredicate("InvoiceItem", predicate);

				//List<InvoiceItem> _invoiceItems = new List<InvoiceItem>();

				//foreach (CKRecord _invoiceItemRecord in _invoiceItemRecords)
				//{
				//	InvoiceItem _invoiceItem = new InvoiceItem();

				//	_invoiceItem.RecordName = ((CKRecordID)_invoiceItemRecord["recordID"]).RecordName;
				//	_invoiceItem.Name = _invoiceItemRecord["Name"].ToString();
				//	_invoiceItem.UnitPrice = Convert.ToDecimal(_invoiceItemRecord["UnitPrice"]);
				//	_invoiceItem.Quantity = Convert.ToInt16(_invoiceItemRecord["Quantity"]);
				//	_invoiceItem.bTaxable = Convert.ToInt16(_invoiceItemRecord["bTaxable"]) == 0 ? false : true;
				//	_invoiceItem.Note = _invoiceItemRecord["Note"].ToString();

				//	_invoiceItems.Add(_invoiceItem);
				//}

				//_invoice.Items = _invoiceItems;

				// invoice attachment
				//NSPredicate _attachmentSearchPredicate = NSPredicate.FromFormat(string.Format("InvoiceName = '{0}'", _invoiceRecordName));
				//List<CKRecord> _attachmentRecords = await cloudManager.FetchRecordsByTypeAndPredicate("Attachment", predicate);

				//List<Attachment> _attachments = new List<Attachment>();

				//foreach (CKRecord _attachmentRecord in _attachmentRecords)
				//{
				//	Attachment _attachment = new Attachment();

				//	_attachment.RecordName = ((CKRecordID)_attachmentRecord["recordID"]).RecordName;

				//}

				invoiceList.Add(_invoice);

				string m = _invoice.IssueDate.Month.ToString();

				if (invoiceDictionary.ContainsKey(m))
				{
					invoiceDictionary[m].Add(_invoice);
				}
				else
				{
					List<Invoice> _tmpLst = new List<Invoice>();
					_tmpLst.Add(_invoice);
					invoiceDictionary.Add(m, _tmpLst);
					Helper.InsertInOrder(m, ref keyList);
				}

			}

			return invoiceList.Count;
		}

		void btnSearch_UpInside(UIButton sender)
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

		//partial void btnSearch_UpInside(UIBarButtonItem sender)
		//{
		//	if (searchController == null)
		//	{
		//		searchController = new UISearchController((UIViewController)null);

		//		searchController.DimsBackgroundDuringPresentation = false;
		//		DefinesPresentationContext = true;

		//		searchBar = searchController.SearchBar;
		//		//searchBar = new UISearchBar();
		//		searchBar.Placeholder = "Enter Search Text";
		//		searchBar.SizeToFit();
		//		searchBar.AutocorrectionType = UITextAutocorrectionType.No;
		//		searchBar.AutocapitalizationType = UITextAutocapitalizationType.None;
		//		searchBar.BarTintColor = UIColor.White;

		//		foreach (var view in searchBar.Subviews)
		//		{
		//			foreach (var subview in view.Subviews)
		//			{
		//				if (subview is UITextField)
		//				{
		//					(subview as UITextField).BackgroundColor = UIColor.FromRGB(247, 247, 247);
		//				}
		//			}
		//		}

		//		searchBar.TextChanged += SearchBar_TextChanged;
		//		searchBar.CancelButtonClicked += SearchBar_CancelButtonClicked;
		//		//searchBar.OnEditingStarted += SearchBar_OnEditingStarted;

		//		TableView.TableHeaderView = searchBar;
		//	}
		//	else
		//	{
		//		searchController.Active = false;

		//		searchController.RemoveFromParentViewController();
		//		searchController = null;
		//		TableView.TableHeaderView = null;
		//	}
		//}

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

				filteredInvoiceList = invoiceList.FindAll(c => c.Name.ToUpper().Contains(searchBar.Text.ToUpper()) ||
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

		partial void btnCalendar_UpInside(UIButton sender)
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

		public override NSIndexPath WillSelectRow(UITableView tableView, NSIndexPath indexPath)
		{
			SelectedInvoiceId = invoiceList[indexPath.Row].Id;

			selectedInvoiceName = invoiceList[indexPath.Row].RecordName;

			return indexPath;
		}

		public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
		{
			base.PrepareForSegue(segue, sender);

			if (segue.Identifier == "Segue_InvoiceList_To_Detail")
			{
				//var button = this.NavigationController.TabBarController.View.ViewWithTag(1001);
				//button.Hidden = true;			
				//CGRect frame = button.Frame;
				//frame.X = -100;
				//button.Frame = frame;

				(segue.DestinationViewController as InvoiceViewController).invoice.RecordName = selectedInvoiceName;
				(segue.DestinationViewController as InvoiceViewController).bNewMode = false;
			}
		}

		partial void btnInvoiceAdd_UpInside(UIBarButtonItem sender)
		{
			UIStoryboard storyBoard = UIStoryboard.FromName("Main", null);
			UINavigationController invoiceViewNavigationController = (UINavigationController)storyBoard.InstantiateViewController("InvoiceViewNavigationController");
			InvoiceViewController invoiceViewController = invoiceViewNavigationController.ViewControllers[0] as InvoiceViewController;
			invoiceViewController.bNewMode = true;
			this.PresentViewController(invoiceViewNavigationController, true, null);
		}
	}
}