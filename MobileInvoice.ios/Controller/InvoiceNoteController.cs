using Foundation;
using System;
using UIKit;

namespace MobileInvoice.ios
{
    public partial class InvoiceNoteController : UITableViewController
    {
		public InvoiceViewController callingController;
		public string note;

        public InvoiceNoteController (IntPtr handle) : base (handle)
        {
        }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			UIBarButtonItem btnBack = new UIBarButtonItem(UIImage.FromFile("Images/Left-30-green.png"), UIBarButtonItemStyle.Plain, (sender, e) =>
				{
					NavigationController.PopViewController(true);
				}
			);

			NavigationItem.LeftBarButtonItem = btnBack;
		}

		public override void WillDisplayHeaderView(UITableView tableView, UIView headerView, nint section)
		{

			var header = headerView as UITableViewHeaderFooterView;

			header.TextLabel.TextColor = UIColor.LightGray;

			header.TextLabel.Font = UIFont.FromName("AvenirNext-Bold", 12);
		}
    }
}