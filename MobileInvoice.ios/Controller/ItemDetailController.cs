using Foundation;
using System;
using UIKit;
using CoreGraphics;
using MobileInvoice.model;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;

namespace MobileInvoice.ios
{
    public partial class ItemDetailController : UITableViewController
    {
		public Item item = new Item();

        public ItemDetailController (IntPtr handle) : base (handle)
        {
        }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			swTaxable.Transform = CGAffineTransform.MakeScale(0.7f, 0.7f);
			swSave.Transform = CGAffineTransform.MakeScale(0.7f, 0.7f);

			TableView.AllowsSelection = false;

			AddDoneButtonToKeyboard(txtName);
			AddDoneButtonToKeyboard(txtNote);
			AddDoneButtonToKeyboard(txtQuantity);
			AddDoneButtonToKeyboard(txtUnitCost);

			TableView.TableFooterView = new UIView();
		}

		public override void WillDisplayHeaderView(UITableView tableView, UIView headerView, nint section)
		{

			var header = headerView as UITableViewHeaderFooterView;

			header.TextLabel.TextColor = UIColor.LightGray;
			//header.TextLabel.Font = UIFont.BoldSystemFontOfSize(12)
			header.TextLabel.Font = UIFont.FromName("AvenirNext-Bold", 12);
		}

		void AddDoneButtonToKeyboard(UITextField textField)
		{
			UIToolbar toolbar = new UIToolbar();

			var doneButton = new UIBarButtonItem(UIBarButtonSystemItem.Done);
			doneButton.Clicked += delegate
			{
				textField.ResignFirstResponder();
			};

			var bbs = new UIBarButtonItem[] {
				new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),
				doneButton
			};

			toolbar.SetItems(bbs, false);
			toolbar.SizeToFit();

			textField.InputAccessoryView = toolbar;
		}

		void AddDoneButtonToKeyboard(UITextView textField)
		{
			UIToolbar toolbar = new UIToolbar();

			var doneButton = new UIBarButtonItem(UIBarButtonSystemItem.Done);
			doneButton.Clicked += delegate
			{
				textField.ResignFirstResponder();
			};

			var bbs = new UIBarButtonItem[] {
				new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),
				doneButton
			};

			toolbar.SetItems(bbs, false);
			toolbar.SizeToFit();

			textField.InputAccessoryView = toolbar;
		}

		async partial void btnSave_UpInside(UIBarButtonItem sender)
		{
			LoadingOverlay loadingOverlay = new LoadingOverlay(UIScreen.MainScreen.Bounds);
			this.View.Add(loadingOverlay);

			BuildItem();

			await AddInvoiceItem(item);

			loadingOverlay.Hide();

			DismissViewController(true, null);
		}

		partial void btnCancel_UpInside(UIBarButtonItem sender)
		{
			DismissViewController(true, null);
		}

		async Task<bool> AddInvoiceItem(Item _item)
		{
			string jsonItem = JsonConvert.SerializeObject(_item);

			var strContentItem = new StringContent(jsonItem, Encoding.UTF8, "application/json");

			HttpClient httpClient = new HttpClient();

			var result = await httpClient.PostAsync(Helper.AddItemURL(), strContentItem);

			var contents = await result.Content.ReadAsStringAsync();

			string returnMessage = contents.ToString();

			if (returnMessage == "\"Added item successfully\"")
				return true;
			else
				return false;
		}

		void BuildItem()
		{
			item.Name = txtName.Text;
			item.UnitPrice = Convert.ToDecimal(txtUnitCost.Text);
			item.Quantity = Convert.ToInt16(txtQuantity.Text);
			item.bTaxable = swTaxable.On;
			item.Note = txtNote.Text;
		}
	}
}