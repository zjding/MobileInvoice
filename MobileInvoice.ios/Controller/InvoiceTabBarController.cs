using Foundation;
using System;
using UIKit;

namespace MobileInvoice.ios
{
    public partial class InvoiceTabBarController : RaisedTabBarController
    {
        public InvoiceTabBarController (IntPtr handle) : base (handle)
        {
        }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			this.InsertEmptyTabItem("", 2);

			var img = UIImage.FromFile("Images/Add-50.png");
			this.AddRaisedButton(img, null);
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);
		}

		public override void onRaisedButton_TouchUpInside(object sender, EventArgs e)
		{
			// Create a new Alert Controller
			UIAlertController actionSheetAlert = UIAlertController.Create("Create a new item", "Select an type from below", UIAlertControllerStyle.ActionSheet);

			// Add Actions
			actionSheetAlert.AddAction(UIAlertAction.Create("New Invoice", UIAlertActionStyle.Default, (action) => {
				UIStoryboard storyBoard = UIStoryboard.FromName("Main", null);
				UINavigationController invoiceViewNavigationController = (UINavigationController)storyBoard.InstantiateViewController("InvoiceViewNavigationController");
				this.PresentViewController(invoiceViewNavigationController, true, null);
				//InvoiceViewController invoiceVC = (InvoiceViewController)storyBoard.InstantiateViewController("estimationVC");r
				//invoiceVC.ModalTransitionStyle = UIModalTransitionStyle.CoverVertical;
				//this.PresentViewController(invoiceVC, true, null);

				//NewClientNavigationController invoiceViewNavigationController = (NewClientNavigationController)storyBoard.InstantiateViewController("NewClientNavigationController");
				//this.PresentViewController(invoiceViewNavigationController, true, null);

				//InvoiceViewController estimationVC = new InvoiceViewController();			
				//this.PresentViewController(estimationVC, true, null);
			}));



			actionSheetAlert.AddAction(UIAlertAction.Create("New Estimate", UIAlertActionStyle.Default, (action) =>
			{
				UIStoryboard storyBoard = UIStoryboard.FromName("Main", null);
				//InvoiceViewController invoiceVC = (InvoiceViewController)storyBoard.InstantiateViewController("estimationVC");r
				//invoiceVC.ModalTransitionStyle = UIModalTransitionStyle.CoverVertical;
				//this.PresentViewController(invoiceVC, true, null);

				UINavigationController invoiceViewNavigationController = (UINavigationController)storyBoard.InstantiateViewController("InvoiceViewNavigationController");
				this.PresentViewController(invoiceViewNavigationController, true, null);

				//InvoiceViewController estimationVC = new InvoiceViewController();			
				//this.PresentViewController(estimationVC, true, null);
			}));

			actionSheetAlert.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, (action) => Console.WriteLine("Cancel button pressed.")));

			// Required for iPad - You must specify a source for the Action Sheet since it is
			// displayed as a popover
			//UIPopoverPresentationController presentationPopover = actionSheetAlert.PopoverPresentationController;
			//if (presentationPopover != null)
			//{
			//	presentationPopover.SourceView = this.View;
			//	presentationPopover.PermittedArrowDirections = UIPopoverArrowDirection.Up;
			//}

			// Display the alert
			this.PresentViewController(actionSheetAlert, true, null);
		}
    }
}