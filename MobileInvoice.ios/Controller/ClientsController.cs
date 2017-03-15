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

		public List<string> clientList = new List<string>();
		public List<string> filteredClientList = new List<string>();

		UISearchController searchController;
		UISearchBar searchBar;
		bool bSearching = false;

        public ClientsController (IntPtr handle) : base (handle)
        {
        }

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);

			TableView.ReloadData();
		}

		public override void ViewDidLoad()
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
				return clients.Count;
			else
				return 1;
		}

		public override string TitleForHeader(UITableView tableView, nint section)
		{
			if (!bSearching)
				return keys[(int)section];
			else
				return "";
		}

		public override nint RowsInSection(UITableView tableView, nint section)
		{
			if (!bSearching)
				return clients[keys[(int)section]].Count;
			else
				return filteredClientList.Count;
		}

		public override string[] SectionIndexTitles(UITableView tableView)
		{
			if (!bSearching)
				return keys.ToArray();
			else
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
			if (!bSearching)
				cell.lblName.Text = clients[keys[indexPath.Section]][indexPath.Row];
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