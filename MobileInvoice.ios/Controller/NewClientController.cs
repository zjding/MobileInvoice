using Foundation;
using System;
using UIKit;
using AddressBookUI;

namespace MobileInvoice.ios
{
    public partial class NewClientController : UITableViewController
    {
        public NewClientController (IntPtr handle) : base (handle)
        {
        }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			this.TableView.AllowsSelection = false;

			var tapContactGesture = new UITapGestureRecognizer(TapContact);
			this.imgContact.UserInteractionEnabled = true;
			this.imgContact.AddGestureRecognizer(tapContactGesture);

			AddDoneButtonToKeyboard(txtName);
			AddDoneButtonToKeyboard(txtPhone);
			AddDoneButtonToKeyboard(txtEmail);
		}

		public override void WillDisplayHeaderView(UITableView tableView, UIView headerView, nint section)
		{

			var header = headerView as UITableViewHeaderFooterView;

			header.TextLabel.TextColor = UIColor.LightGray;
			//header.TextLabel.Font = UIFont.BoldSystemFontOfSize(12)
			header.TextLabel.Font = UIFont.FromName("AvenirNext-Bold", 12);
		}

		void TapContact()
		{
			ABPeoplePickerNavigationController contactController = new ABPeoplePickerNavigationController();
			contactController.SelectPerson2 += ContactController_SelectPerson2;

			PresentViewController(contactController, true, null);
		}

		void ContactController_SelectPerson2(object sender, ABPeoplePickerSelectPerson2EventArgs e)
		{
			txtName.Text = e.Person.FirstName + " " + e.Person.LastName;

			var phones = e.Person.GetPhones();
			var emails = e.Person.GetEmails();
			var addresses = e.Person.GetAllAddresses();

			if (phones.Count > 0)
				txtPhone.Text = phones[0].Value;
			else
				txtPhone.Text = "";

			if (emails.Count > 0)
				txtEmail.Text = emails[0].Value;
			else
				txtEmail.Text = "";

			if (addresses.Count > 0)
			{
				txtStreet1.Text = addresses[0].Value.Street;
				txtCity.Text = addresses[0].Value.City;
				txtState.Text = addresses[0].Value.State;
				txtCountry.Text = addresses[0].Value.Country;
				txtPostal.Text = addresses[0].Value.Zip;
			}
			else
			{
				txtStreet1.Text = "";
				txtCity.Text = "";
				txtState.Text = "";
				txtCountry.Text = "";
				txtPostal.Text = "";
			}
		}

		void AddDoneButtonToKeyboard(UITextField textField)
		{
			UIToolbar toolbar = new UIToolbar();

			var doneButton = new UIBarButtonItem(UIBarButtonSystemItem.Done);
			doneButton.Clicked += delegate {
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
    }
}