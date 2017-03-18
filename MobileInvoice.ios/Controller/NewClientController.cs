using Foundation;
using System;
using UIKit;
using AddressBookUI;
using MobileInvoice.model;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MobileInvoice.ios
{
    public partial class NewClientController : UITableViewController
    {
		public ClientsController callingController;

		public Client client = new Client();
		public Client oldClient;
		public bool bNewMode = true;
		public bool bNavigationPush = true;

        public NewClientController (IntPtr handle) : base (handle)
        {
        }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			oldClient = client;

			this.TableView.AllowsSelection = false;

			var tapContactGesture = new UITapGestureRecognizer(TapContact);
			this.imgContact.UserInteractionEnabled = true;
			this.imgContact.AddGestureRecognizer(tapContactGesture);

			AddDoneButtonToKeyboard(txtName);
			AddDoneButtonToKeyboard(txtPhone);
			AddDoneButtonToKeyboard(txtEmail);

			LoadClient(client);
		}

		void LoadClient(Client _client)
		{
			txtName.Text = _client.Name;
			txtPhone.Text = _client.Phone;
			txtEmail.Text = _client.Email;

			txtStreet1.Text = _client.Street1;
			txtStreet2.Text = _client.Street2;
			txtCity.Text = _client.City;
			txtState.Text = _client.State;
			txtCountry.Text = _client.Country;
			txtPostal.Text = _client.PostCode;
		}

		public override nint NumberOfSections(UITableView tableView)
		{
			if (bNewMode)
				return 2;
			else
				return 3;
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
			txtName.Text = e.Person.ToString();

			var phones = e.Person.GetPhones();
			var emails = e.Person.GetEmails();
			var addresses = e.Person.GetAllAddresses();

			if (phones.Count > 0)
			{
				txtPhone.Text = phones[0].Value;
			}
			else
			{
				txtPhone.Text = "";
			}

			if (emails.Count > 0)
			{
				txtEmail.Text = emails[0].Value;
			}
			else
			{
				txtEmail.Text = "";
			}

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

		async partial void btnSave_UpInside(UIBarButtonItem sender)
		{
			LoadingOverlay loadingOverlay = new LoadingOverlay(UIScreen.MainScreen.Bounds);
			this.View.Add(loadingOverlay);

			BuildClientFromInput();

			string key = client.Name.Substring(0, 1);

			if (bNewMode)
			{
				if (callingController.clientDictionary.ContainsKey(key))
				{
					List<Client> _tempList = callingController.clientDictionary[key];
					Helper.InsertInOrder(client, ref _tempList);
					callingController.clientDictionary[key] = _tempList;
				}
				else
				{
					List<Client> _tempList = new List<Client>();
					_tempList.Add(client);
					callingController.clientDictionary.Add(key, _tempList);

					Helper.InsertInOrder(key, ref callingController.keyList);
				}

				callingController.clientList.Add(client);

				await AddClient(client);
			}
			else
			{
				List<Client> _tempList = callingController.clientDictionary[key];
				_tempList.Remove(oldClient);
				Helper.InsertInOrder(client, ref _tempList);
				callingController.clientDictionary[key] = _tempList;

				await UpdateClient(client);
			}

			loadingOverlay.Hide();

			if (!bNavigationPush)
				DismissViewController(true, null);
			else
				this.NavigationController.PopViewController(true);
		}

		partial void btnCancel_UpInside(UIBarButtonItem sender)
		{
			if (!bNavigationPush)
				DismissViewController(true, null);
			else
				this.NavigationController.PopViewController(true);
		}

		async Task<bool> AddClient(Client _client)
		{
			string jsonClient = JsonConvert.SerializeObject(_client);

			var strContentClient = new StringContent(jsonClient, Encoding.UTF8, "application/json");

			HttpClient httpClient = new HttpClient();

			var result = await httpClient.PostAsync(Helper.AddClientURL(), strContentClient);

			var contents = await result.Content.ReadAsStringAsync();

			string returnMessage = contents.ToString();

			if (returnMessage == "\"Added client successfully\"")
				return true;
			else
				return false;
				
		}

		async Task<bool> DeleteClient(Client _client)
		{
			HttpClient httpClient = new HttpClient();

			var result = await httpClient.DeleteAsync(Helper.DeleteClientURL() + client.Id.ToString());

			result.EnsureSuccessStatusCode();

			if (result.IsSuccessStatusCode)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		async Task<bool> UpdateClient(Client _client)
		{
			string jsonClient = JsonConvert.SerializeObject(_client);

			var strContentClient = new StringContent(jsonClient, Encoding.UTF8, "application/json");

			HttpClient httpClient = new HttpClient();

			var result = await httpClient.PutAsync(Helper.UpdateClientURL(), strContentClient);

			var contents = await result.Content.ReadAsStringAsync();

			string returnMessage = contents.ToString();

			if (returnMessage == "\"Updated client successfully\"")
			{
				return true;
			}
			else
			{
				return false;
			}
		}



		void BuildClientFromInput()
		{
			client.Name = txtName.Text;
			client.Phone = txtPhone.Text;
			client.Email = txtEmail.Text;
			client.Street1 = txtStreet1.Text;
			client.Street2 = txtStreet2.Text;
			client.City = txtCity.Text;
			client.State = txtState.Text;
			client.Country = txtCountry.Text;
			client.PostCode = txtPostal.Text;;
		}

		async partial void btnDelete_UpInside(UIButton sender)
		{

			LoadingOverlay loadingOverlay = new LoadingOverlay(UIScreen.MainScreen.Bounds);
			this.View.Add(loadingOverlay);

			bool result = await DeleteClient(client);

			loadingOverlay.Hide();

			if (result)
			{
				string key = client.Name.Substring(0, 1);

				callingController.clientDictionary[key].Remove(client);
				callingController.clientList.Remove(client);

				if (callingController.clientDictionary[key].Count == 0)
				{
					callingController.clientDictionary.Remove(key);
					callingController.keyList.Remove(key);
				}

				NavigationController.PopViewController(true);
			}
		}
	}
}