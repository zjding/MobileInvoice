using Foundation;
using System;
using UIKit;
using System.Collections.Generic;
using MobileInvoice.model;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Globalization;
using CloudKit;

namespace MobileInvoice.ios
{
    public partial class ItemsController : UITableViewController
    {
		public List<Item> itemList = new List<Item>();
		public InvoiceItemDetailController callingController;

		CloudManager cloudManager;

		#region Computed Properties
		public AppDelegate ThisApp
		{
			get { return (AppDelegate)UIApplication.SharedApplication.Delegate; }
		}
		#endregion

        public ItemsController (IntPtr handle) : base (handle)
        {
			cloudManager = new CloudManager();
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

			await CK_LoadItems();

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

		async Task CK_LoadItems()
		{
			NSPredicate predicate = NSPredicate.FromFormat(string.Format("User = '{0}'", ThisApp.UserName));

			List<CKRecord> records = await cloudManager.FetchRecordsByTypeAndPredicate("Items", predicate);

			itemList.Clear();

			foreach (CKRecord record in records)
			{
				Item _item = new Item();

				CKRecordID recordId = (CKRecordID)record["recordID"];
				string recordName = recordId.RecordName;
				_item.RecordName = recordName;

				_item.Name = record["Name"].ToString();
				_item.UnitPrice = Convert.ToDecimal(((NSNumber)(record["UnitPrice"])).FloatValue);

				itemList.Add(_item);
			}
		p
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

		public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
		{
			Item _item = itemList[indexPath.Row];

			callingController.invoiceItem.Name = _item.Name;
			callingController.invoiceItem.UnitPrice = _item.UnitPrice;
			callingController.bBackFromItemsController = true;

			NavigationController.PopViewController(true);
		}
    }
}