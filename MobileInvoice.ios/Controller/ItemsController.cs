using Foundation;
using System;
using UIKit;

namespace MobileInvoice.ios
{
    public partial class ItemsController : UITableViewController
    {


        public ItemsController (IntPtr handle) : base (handle)
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
    }
}