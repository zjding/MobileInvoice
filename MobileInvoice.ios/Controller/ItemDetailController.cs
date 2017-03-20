using Foundation;
using System;
using UIKit;
using CoreGraphics;

namespace MobileInvoice.ios
{
    public partial class ItemDetailController : UITableViewController
    {
        public ItemDetailController (IntPtr handle) : base (handle)
        {
        }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			swTaxable.Transform = CGAffineTransform.MakeScale(0.7f, 0.7f);

			TableView.AllowsSelection = false;

			AddDoneButtonToKeyboard(txtName);
			AddDoneButtonToKeyboard(txtNote);
			AddDoneButtonToKeyboard(txtQuantity);
			AddDoneButtonToKeyboard(txtUnitCost);
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

		partial void btnSave_UpInside(UIBarButtonItem sender)
		{
			DismissViewController(true, null);
		}

		partial void btnCancel_UpInside(UIBarButtonItem sender)
		{
			DismissViewController(true, null);
		}
	}
}