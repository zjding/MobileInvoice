using Foundation;
using System;
using UIKit;

namespace MobileInvoice.ios
{
    public partial class InvoicePaymentController : UITableViewController
    {
        public InvoicePaymentController (IntPtr handle) : base (handle)
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

			TableView.TableFooterView = new UIView();
		}


		public override void WillDisplayHeaderView(UITableView tableView, UIView headerView, nint section)
		{
			var header = headerView as UITableViewHeaderFooterView;

			header.TextLabel.TextColor = UIColor.LightGray;
			//header.TextLabel.Font = UIFont.BoldSystemFontOfSize(12)
			header.TextLabel.Font = UIFont.FromName("AvenirNext-Bold", 12);
		}
    }
}