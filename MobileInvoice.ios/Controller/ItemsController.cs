using Foundation;
using System;
using UIKit;
using System.Collections.Generic;
using MobileInvoice.model;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Globalization;

namespace MobileInvoice.ios
{
    public partial class ItemsController : UITableViewController
    {
		public List<Item> itemList = new List<Item>();

        public ItemsController (IntPtr handle) : base (handle)
        {
        }

		async public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			UIBarButtonItem btnBack = new UIBarButtonItem(UIImage.FromFile("Images/Left-30-green.png"), UIBarButtonItemStyle.Plain, (sender, e) =>
				{
					NavigationController.PopViewController(true);
				}
			);

			NavigationItem.LeftBarButtonItem = btnBack;

			TableView.TableFooterView = new UIView();

			LoadingOverlay loadingOverlay = new LoadingOverlay(UIScreen.MainScreen.Bounds);
			this.View.Add(loadingOverlay);

			await LoadItems();

			loadingOverlay.Hide();

			TableView.ReloadData();
		}

		async Task<bool> LoadItems()
		{
			HttpClient httpClient = new HttpClient();

			string result = await httpClient.GetStringAsync(Helper.GetItemsURL());

			itemList = JsonConvert.DeserializeObject<List<Item>>(result);

			return true;
		}

		public override nint NumberOfSections(UITableView tableView)
		{
			return 1;
		}

		public override nint RowsInSection(UITableView tableView, nint section)
		{
			return itemList.Count;
		}

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			ItemCell cell = TableView.DequeueReusableCell("ItemCellIdentifier") as ItemCell;

			cell.lblItemName.Text = itemList[indexPath.Row].Name;
			cell.lblUnitPrice.Text = itemList[indexPath.Row].UnitPrice.ToString("C", CultureInfo.CurrentCulture);

			return cell;
		}
    }
}