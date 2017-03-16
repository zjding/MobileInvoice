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
		public Dictionary<string, List<string>> clients = new Dictionary<string, List<string>>();
		public List<string> keys = new List<string>();

		public List<string> clientList = new List<string>();
		public List<string> filteredClientList = new List<string>();

		public List<Client> clientList1 = new List<Client>();
		public Dictionary<string, List<Client>> clientDictionary1 = new Dictionary<string, List<Client>>();
		public List<string> keyList1 = new List<string>();

		string[] emptyArray = new string[0];

		UISearchController searchController;
		UISearchBar searchBar;
		public bool bSearching = false;
		public bool bPickClientMode = false;

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

			clientList1 = JsonConvert.DeserializeObject<List<Client>>(result);

			foreach (Client client in clientList1)
			{
				string key = client.Name.Substring(0, 1);

				if (clientDictionary1.ContainsKey(key))
				{
					List<Client> _tempList = clientDictionary1[key];
					Helper.InsertInOrder(client, ref _tempList);
					clientDictionary1[key] = _tempList;
				}
				else
				{
					List<Client> _tempList = new List<Client>();
					_tempList.Add(client);
					clientDictionary1.Add(key, _tempList);
					Helper.InsertInOrder(key, ref keyList1);
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

				filteredClientList = clientList.FindAll(s => (s.ToUpper().Contains(searchBar.Text.ToUpper())));;
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
				//return clients.Count;
				return keyList1.Count;
			else
				return 1;
		}

		public override string TitleForHeader(UITableView tableView, nint section)
		{
			if (!bSearching)
				//return keys[(int)section];
				return keyList1[(int)section];
			else
				return "";
		}

		public override nint RowsInSection(UITableView tableView, nint section)
		{
			if (!bSearching)
				//return clients[keys[(int)section]].Count;
				return clientDictionary1[keyList1[(int)section]].Count;
			else
				return filteredClientList.Count;
		}

		public override string[] SectionIndexTitles(UITableView tableView)
		{
			if (!bSearching)
				//return keys.ToArray();
				return keyList1.ToArray();
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
				cell.lblName.Text = clientDictionary1[keyList1[indexPath.Section]][indexPath.Row].Name;
			else
				cell.lblName.Text = filteredClientList[indexPath.Row];
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