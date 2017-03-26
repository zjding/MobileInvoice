using Foundation;
using System;
using UIKit;
using MobileInvoice.model;
using CoreGraphics;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace MobileInvoice.ios
{
    public partial class InvoiceItemDetailController : UITableViewController
    {
		public InvoiceItem invoiceItem = new InvoiceItem();
		public InvoiceViewController callingController;

		public bool bNewMode;

        public InvoiceItemDetailController (IntPtr handle) : base (handle)
        {
        }

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);

			txtName.Text = invoiceItem.Name;
			txtUnitCost.Text = invoiceItem.UnitPrice.ToString();
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			swTaxable.Transform = CGAffineTransform.MakeScale(0.7f, 0.7f);
			swSave.Transform = CGAffineTransform.MakeScale(0.7f, 0.7f);

			TableView.AllowsSelection = false;

			txtUnitCost.Frame = new CGRect(txtUnitCost.Frame.X, txtUnitCost.Frame.Y, (TableView.Frame.Width - 36) / 2.0f, txtUnitCost.Frame.Height);
			txtQuantity.Frame = new CGRect(txtUnitCost.Frame.X + txtUnitCost.Frame.Width, txtQuantity.Frame.Y, (TableView.Frame.Width - 36) / 2.0f, txtQuantity.Frame.Height);

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

		public override nint NumberOfSections(UITableView tableView)
		{
			if (bNewMode)
				return 4;
			else
				return 5;
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

			BuildInvoiceItem();

			await AddInvoiceItem(invoiceItem);

			callingController.invoice.Items.Add(invoiceItem);

			loadingOverlay.Hide();

			DismissViewController(true, null);
		}

		partial void btnCancel_UpInside(UIBarButtonItem sender)
		{
			DismissViewController(true, null);
		}

		async Task<bool> AddInvoiceItem(InvoiceItem _item)
		{
			string jsonInvoiceItem = JsonConvert.SerializeObject(_item);

			var strContentInvoiceItem = new StringContent(jsonInvoiceItem, Encoding.UTF8, "application/json");

			HttpClient httpClient = new HttpClient();

			var result = await httpClient.PostAsync(Helper.AddInvoiceItemURL(), strContentInvoiceItem);

			var contents = await result.Content.ReadAsStringAsync();

			string returnMessage = contents.ToString();

			if (this.swSave.On)
			{
				Item item = new Item();

				BuildItem(ref item);

				string jsonItem = JsonConvert.SerializeObject(item);

				var strContentItem = new StringContent(jsonItem, Encoding.UTF8, "application/json");

				result = await httpClient.PostAsync(Helper.AddItemURL(), strContentItem);

				contents = await result.Content.ReadAsStringAsync();
			}

			if (returnMessage == "\"Added item successfully\"")
				return true;
			else
				return false;
		}

		void BuildItem(ref Item item)
		{
			item.Name = txtName.Text;
			item.UnitPrice = Convert.ToDecimal(txtUnitCost.Text);
		}

		void BuildInvoiceItem()
		{
			invoiceItem.Name = txtName.Text;
			invoiceItem.UnitPrice = Convert.ToDecimal(txtUnitCost.Text);
			invoiceItem.Quantity = Convert.ToInt16(txtQuantity.Text);
			invoiceItem.bTaxable = swTaxable.On;
			invoiceItem.Note = txtNote.Text;
		}

		public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
		{
			if (segue.Identifier == "InvoiceItem_to_ItemList_Segue")
			{
				ItemsController destCtl = segue.DestinationViewController as ItemsController;
				destCtl.callingController = this;
			}

			base.PrepareForSegue(segue, sender);
		}
    }
}