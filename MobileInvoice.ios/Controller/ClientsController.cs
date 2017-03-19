using Foundation;
using System;
using UIKit;
using System.Collections.Generic;
using System.Net.Http;
using MobileInvoice.model;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace MobileInvoice.ios
{
    public partial class ClientsController : UITableViewController
    {
		public List<Client> clientList = new List<Client>();
		public Dictionary<string, List<Client>> clientDictionary = new Dictionary<string, List<Client>>();
		public List<string> keyList = new List<string>();
		public List<Client> filteredClientList = new List<Client>();

		string[] emptyArray = new string[0];

		UISearchController searchController;
		UISearchBar searchBar;
		public bool bSearching = false;
		public bool bPickClientMode = false;

		public InvoiceViewController callingController;

        public ClientsController (IntPtr handle) : base (handle)
        {
        }

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);

			TableView.ReloadData();
		}

		public async override void ViewDidLoad()
		{
			base.ViewDidLoad();

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
			searchBar.OnEditingStarted += SearchBar_OnEditingStarted;

			TableView.TableHeaderView = searchBar;

			TableView.TableFooterView = new UIView();

			LoadingOverlay loadingOverlay = new LoadingOverlay(UIScreen.MainScreen.Bounds);
			this.View.Add(loadingOverlay);

			await LoadClients();

			loadingOverlay.Hide();

			TableView.ReloadData();
		}

		async Task<int> LoadClients()
		{
			
			HttpClient httpClient = new HttpClient();

			string result = await httpClient.GetStringAsync(Helper.GetClientsURL());

			clientList = JsonConvert.DeserializeObject<List<Client>>(result);

			foreach (Client client in clientList)
			{
				string key = client.Name.Substring(0, 1);

				if (clientDictionary.ContainsKey(key))
				{
					List<Client> _tempList = clientDictionary[key];
					Helper.InsertInOrder(client, ref _tempList);
					clientDictionary[key] = _tempList;
				}
				else
				{
					List<Client> _tempList = new List<Client>();
					_tempList.Add(client);
					clientDictionary.Add(key, _tempList);
					Helper.InsertInOrder(key, ref keyList);
				}
			}

			return 0;
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

				filteredClientList = clientList.FindAll(c => c.Name.ToUpper().Contains(searchBar.Text.ToUpper()));
				TableView.ReloadData();
			}
		}

		void SearchBar_CancelButtonClicked(object sender, EventArgs e)
		{
			bSearching = false;
			TableView.ReloadData();
		}

		void SearchBar_OnEditingStarted(object sender, EventArgs e)
		{
			//filteredClients.Clear();
			//filteredClients.Add(clients[0]);
			//TableView.ReloadData();
		}

		public override nint NumberOfSections(UITableView tableView)
		{
			if (!bSearching)
				return keyList.Count;
			else
				return 1;
		}

		public override string TitleForHeader(UITableView tableView, nint section)
		{
			if (!bSearching)
				return keyList[(int)section];
			else
				return "";
		}

		public override nint RowsInSection(UITableView tableView, nint section)
		{
			if (!bSearching)
				return clientDictionary[keyList[(int)section]].Count;
			else
				return filteredClientList.Count;
		}

		public override string[] SectionIndexTitles(UITableView tableView)
		{
			if (!bSearching)
				return keyList.ToArray();
			else
				return emptyArray;
		}

		public override void WillDisplayHeaderView(UITableView tableView, UIView headerView, nint section)
		{
			var header = headerView as UITableViewHeaderFooterView;

			header.TextLabel.TextColor = UIColor.LightGray;
			header.TextLabel.Font = UIFont.FromName("AvenirNext-Bold", 12);
		}

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			ClientCell cell = this.TableView.DequeueReusableCell("ClientCell") as ClientCell;
			if (!bSearching)
				cell.lblName.Text = clientDictionary[keyList[indexPath.Section]][indexPath.Row].Name;
			else
				cell.lblName.Text = filteredClientList[indexPath.Row].Name;
			return cell;
		}

		public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
		{
			//base.RowSelected(tableView, indexPath);

			if (bPickClientMode)
			{
				if (bSearching)
				{
					callingController.invoice.Client = filteredClientList[indexPath.Row];
				}
				else
				{
					callingController.invoice.Client = clientDictionary[this.TableView.GetHeaderView(indexPath.Section).TextLabel.Text][indexPath.Row];
				}

				this.NavigationController.PopViewController(true);
			}
			else
			{
				NewClientController clientController = (NewClientController)UIStoryboard.FromName("Main", null).InstantiateViewController("NewClientController");
				clientController.client = clientDictionary[this.TableView.GetHeaderView(indexPath.Section).TextLabel.Text][indexPath.Row];
				clientController.bNavigationPush = true;
				clientController.bNewMode = false;
				clientController.callingController = this;

				this.NavigationController.PushViewController(clientController, true);
			}
		}

		public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
		{

			if (segue.Identifier == "Clients_to_NewClient_Segue")
			{
				NewClientController destCtrl = (segue.DestinationViewController as UINavigationController).ViewControllers[0] as NewClientController;
				destCtrl.callingController = this;
				destCtrl.bNewMode = true;
				destCtrl.bNavigationPush = false;
			}

			base.PrepareForSegue(segue, sender);
		}


		partial void btnAdd_UpInside(UIBarButtonItem sender)
		{

			UIStoryboard storyBoard = UIStoryboard.FromName("Main", null);

			//NewClientNavigationController newClientNavigationController = (UINavigationController)storyBoard.InstantiateViewController("NewClientNavigationController") as NewClientNavigationController;
			//NewClientController newClientController = newClientNavigationController.ViewControllers[0] as NewClientController;
			//newClientController.callingController = this;
			//newClientController.bNavigationPush = false;

			//this.PresentViewController(newClientNavigationController, true, null);

			NewClientController newClientController = (UIViewController)storyBoard.InstantiateViewController("NewClientController") as NewClientController;

			newClientController.callingController = this;
			newClientController.bNavigationPush = true;

			this.NavigationController.PushViewController(newClientController, true);
		}
	}
}