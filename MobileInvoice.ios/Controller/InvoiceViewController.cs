using Foundation;
using System;
using UIKit;
using MobileInvoice.model;
using System.Globalization;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CloudKit;

namespace MobileInvoice.ios
{
	public partial class InvoiceViewController : UITableViewController
	{
		public Invoice invoice;
		public List<UIImage> attachmentImages = new List<UIImage>();
		public int iCurrentSelected;
		public int invoiceId = -1;
		public string invoiceName = "";
		public bool bNewMode = true;

		CloudManager cloudManager;

		#region Computed Properties
		public AppDelegate ThisApp
		{
			get { return (AppDelegate)UIApplication.SharedApplication.Delegate; }
		}
		#endregion

		public InvoiceViewController(IntPtr handle) : base(handle)
		{
			invoice = new Invoice();
			cloudManager = new CloudManager();
		}

		async public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			if (!bNewMode)
			{
				NavigationController.NavigationBar.BarTintColor = UIColor.White;

				UIBarButtonItem backButton = new UIBarButtonItem(UIImage.FromFile("Images/Left-30-green.png"), UIBarButtonItemStyle.Plain, (sender, e) =>
				 {
					 NavigationController.PopViewController(true);
				 });
				NavigationItem.LeftBarButtonItem = backButton;
				NavigationItem.RightBarButtonItem = null;

				LoadingOverlay loadingOverlay = new LoadingOverlay(UIScreen.MainScreen.Bounds);
				this.View.Add(loadingOverlay);

				//invoice = await LoadInvoice(invoiceId);

				await CK_LoadInvoice(invoice.RecordName);

				//attachmentImages.Clear();

				//foreach (Attachment att in invoice.Attachments)
				//{

				//	var bytes = Task.Run(() => ImageManager.GetImage(att.ImageName)).Result;
				//	var data = NSData.FromArray(bytes);
				//	attachmentImages.Add(UIImage.LoadFromData(data));
				//}

				loadingOverlay.Hide();

				TableView.ReloadData();
			}
			else
			{
				//string stRecordName = ThisApp.UserName + "-Invoice-" + DateTime.Now.ToString("s");
				//var invoiceRecordID = new CKRecordID(stRecordName);
				//var invoiceRecord = new CKRecord("Invoice", invoiceRecordID);
				//invoiceRecord["Name"] = (NSString)"Invoice #1";

				//await cloudManager.SaveAsync(invoiceRecord);

				//invoice.RecordName = stRecordName;
			}
		}

		async Task<Invoice> LoadInvoice(int id)
		{
			HttpClient httpClient = new HttpClient();

			string result = await httpClient.GetStringAsync(Helper.GetInvoiceByIdURL() + "/" + id.ToString() + "/");

			Invoice _invoice = JsonConvert.DeserializeObject<Invoice>(result);

			return _invoice;
		}

		async Task CK_LoadInvoice(string invoiceName)
		{
			CKRecord _invoiceRecord = await cloudManager.FetchRecordByRecordName(invoiceName);

			invoice = new Invoice();

			invoice.RecordName = invoiceName;
			invoice.Name = _invoiceRecord["Name"].ToString();
			invoice.DueDate = Helper.NSDateToDateTime((NSDate)(_invoiceRecord["DueDate"]));
			invoice.DueTerm = _invoiceRecord["DueTerm"].ToString();
 			
			// client
			CKReference _clientReference = (CKReference)_invoiceRecord["Client"];
			string _clientRecordName = _clientReference.RecordId.RecordName;

			CKRecord _clientRecord = await cloudManager.FetchRecordByRecordName(_clientRecordName);

			Client _client = new Client();

			CKRecordID recordId = (CKRecordID)_clientRecord["recordID"];
			string recordName = recordId.RecordName;
			_client.RecordName = recordName;
			_client.Name = _clientRecord["Name"].ToString();
			_client.Email = _clientRecord["Email"].ToString();
			_client.Phone = _clientRecord["Phone"].ToString();
			_client.Street1 = _clientRecord["Street1"].ToString();
			_client.Street2 = _clientRecord["Street2"].ToString();
			_client.City = _clientRecord["City"].ToString();
			_client.State = _clientRecord["State"].ToString();
			_client.Country = _clientRecord["Country"].ToString();
			_client.PostCode = _clientRecord["PostCode"].ToString();

			invoice.Client = _client;
			invoice.ClientName = _client.Name;

			// items
			invoice.Items.Clear();

			CKReference _invoiceReference = new CKReference(new CKRecordID(invoice.RecordName), CKReferenceAction.None);
			NSPredicate _predicate = NSPredicate.FromFormat("Invoice = %@", _invoiceReference);
			List<CKRecord> records = await cloudManager.FetchRecordsByTypeAndPredicate("InvoiceItem", _predicate);

			foreach (CKRecord _invoiceItemRecord in records)
			{
				InvoiceItem _invoiceItem = new InvoiceItem();

				CKRecordID _recordId = (CKRecordID)_invoiceItemRecord["recordID"];
				string _recordName = recordId.RecordName;
				_invoiceItem.RecordName = recordName;
				_invoiceItem.Name = _invoiceItemRecord["Name"].ToString();
				_invoiceItem.UnitPrice = Convert.ToDecimal(((NSNumber)(_invoiceItemRecord["UnitPrice"])).FloatValue);
				_invoiceItem.Quantity = Convert.ToInt16(((NSNumber)(_invoiceItemRecord["Quantity"])).Int16Value);
				_invoiceItem.bTaxable = Convert.ToInt16(((NSNumber)(_invoiceItemRecord["bTaxable"])).Int16Value) == 0 ? false : true;
				_invoiceItem.Note = _invoiceItemRecord["Note"].ToString();

				invoice.Items.Add(_invoiceItem);
			}

			// attachment
			invoice.Attachments.Clear();

			records = await cloudManager.FetchRecordsByTypeAndPredicate("InvoiceAttachment", _predicate);

			foreach (CKRecord _attachmentRecord in records)
			{
				Attachment _attachment = new Attachment();

				CKRecordID _recordId = (CKRecordID)_attachmentRecord["recordID"];
				string _recordName = recordId.RecordName;
				_attachment.RecordName = recordName;

				_attachment.Description = _attachmentRecord["Description"].ToString();

				//invoiceItem.Name = _invoiceItemRecord["Name"].ToString();
				//_invoiceItem.UnitPrice = Convert.ToDecimal(((NSNumber)(_invoiceItemRecord["UnitPrice"])).FloatValue);
				//invoiceItem.Quantity = Convert.ToInt16(_invoiceItemRecord["Quantity"]);
				//				_invoiceItem.bTaxable = Convert.ToInt16(_invoiceItemRecord["bTaxable"]) == 0 ? false : true;
				//				_invoiceItem.Note = _invoiceItemRecord["Note"].ToString();

				invoice.Attachments.Add(_attachment);



				CKAsset asset = _attachmentRecord["Image"] as CKAsset;
				NSData data = NSData.FromUrl(asset.FileUrl);
				UIImage image = UIImage.LoadFromData(data);

				attachmentImages.Add(image);
			}
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);

			NavigationController.NavigationBar.BarTintColor = UIColor.White;

			TableView.ReloadData();
		}

		public override void WillDisplayHeaderView(UITableView tableView, UIView headerView, nint section)
		{

			var header = headerView as UITableViewHeaderFooterView;

			header.TextLabel.TextColor = UIColor.LightGray;
			//header.TextLabel.Font = UIFont.BoldSystemFontOfSize(12);
			header.TextLabel.Font = UIFont.FromName("AvenirNext-Bold", 12);
		}

		public override string TitleForHeader(UITableView tableView, nint section)
		{
			if (section == 0)
				return " ";
			if (section == 1)
				return " ";
			if (section == 2)
				return "ITEMS";

			if (section == 4)
				return "ATTACHMENTS";

			if (section == 6)
				return " ";

			if (section == 5)
				return "NOTE";


			return base.TitleForHeader(tableView, section);
		}

		public override nint NumberOfSections(UITableView tableView)
		{
			return 7;
		}

		public override nint RowsInSection(UITableView tableView, nint section)
		{
			if (section == 0)       // date
				return 1;
			else if (section == 1)  // client name
				return 1;
			else if (section == 2)  // items
				return invoice.Items.Count + 1;
			else if (section == 3)  // totall
				return 5;
			else if (section == 4)  // attachmentl
				return invoice.Attachments.Count + 1;
			else if (section == 5)  // note
				return 1;
			else if (section == 6) // signature
				return 1;

			return 0;
		}

		public override string TitleForFooter(UITableView tableView, nint section)
		{
			if (section == 1)
				return " ";

			if (section == 3)
				return " ";

			if (section == 4)
				return " ";


			return base.TitleForFooter(tableView, section);
		}

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			if (indexPath.Section == 0)
			{
				InvoiceDateCell cell = this.TableView.DequeueReusableCell("InvoiceDateCellIdentifier") as InvoiceDateCell;
				cell.callingController = this;
				//cell.TextLabel.Text = "Client";
				//cell.DetailTextLabel.Text = client.FirstName + " " + client.LastName;

				return cell;
			}
			else if (indexPath.Section == 1)
			{
				InvoiceClientNameCell cell = this.TableView.DequeueReusableCell("InvoiceClientNameCellIdentifier") as InvoiceClientNameCell;
				cell.lblClientName.Text = invoice.Client.Name;
				//cell.TextLabel.Text = "Client";
				//cell.DetailTextLabel.Text = client.FirstName + " " + client.LastName;

				return cell;

			}
			else if (indexPath.Section == 2)
			{
				if (indexPath.Row == 0)
				{
					InvoiceAddItemCell cell = this.TableView.DequeueReusableCell("InvoiceAddItemCellIdentifier") as InvoiceAddItemCell;

					return cell;
				}
				else
				{
					InvoiceItemCell cell = this.TableView.DequeueReusableCell("InvoiceItemCellIdentifier") as InvoiceItemCell;

					cell.lblNum.Text = indexPath.Row.ToString();
					cell.lblItemName.Text = invoice.Items[indexPath.Row - 1].Name;
					cell.lblUnitPrice.Text = invoice.Items[indexPath.Row - 1].UnitPrice.ToString("C", CultureInfo.CurrentCulture) + " x " + invoice.Items[indexPath.Row - 1].Quantity.ToString();
					cell.lblTotalPrice.Text = (invoice.Items[indexPath.Row - 1].UnitPrice * invoice.Items[indexPath.Row - 1].Quantity).ToString("C", CultureInfo.CurrentCulture);

					return cell;
				}

			}
			else if (indexPath.Section == 3) // total
			{
				if (indexPath.Row == 0)
				{
					InvoiceSubtotalCell cell = this.TableView.DequeueReusableCell("InvoiceSubtotalCell") as InvoiceSubtotalCell;
					return cell;
				}
				else if (indexPath.Row == 1)
				{
					InvoiceTaxCell cell = this.TableView.DequeueReusableCell("InvoiceTaxCell") as InvoiceTaxCell;
					return cell;
				}
				else if (indexPath.Row == 2)
				{
					InvoiceDiscountCell cell = this.TableView.DequeueReusableCell("InvoiceDiscountCell") as InvoiceDiscountCell;
					return cell;
				}
				else if (indexPath.Row == 3)
				{
					InvoicePaidCell cell = this.TableView.DequeueReusableCell("InvoicePaidCell") as InvoicePaidCell;
					return cell;
				}
				else
				{
					InvoiceBalanceCell cell = this.TableView.DequeueReusableCell("InvoiceBalanceCell") as InvoiceBalanceCell;
					return cell;
				}
			}
			else if (indexPath.Section == 4) // attachment
			{
				if (indexPath.Row == 0)
				{
					InvoiceAddAttachmentCell cell = this.TableView.DequeueReusableCell("InvoiceAddAttachmentCell") as InvoiceAddAttachmentCell;
					return cell;
				}
				else
				{
					InvoiceAttachmentCell cell = this.TableView.DequeueReusableCell("InvoiceAttachmentCell") as InvoiceAttachmentCell;
					cell.imgAttachment.Image = attachmentImages[indexPath.Row - 1];
					cell.lblDescription.Text = invoice.Attachments[indexPath.Row - 1].Description;
					return cell;
				}
			}
			else if (indexPath.Section == 5) // note
			{
				if (indexPath.Row == 0)
				{
					InvoiceNoteCell cell = this.TableView.DequeueReusableCell("InvoiceNoteCell") as InvoiceNoteCell;
					//cell.imgAttachment.Image = attachmentImages[indexPath.Row - 1];
					//cell.lblDescription.Text = invoice.Attachments[indexPath.Row - 1].Description;
					return cell;
				}
			}
			else if (indexPath.Section == 6)
			{
				InvoiceSignatureCell cell = this.TableView.DequeueReusableCell("InvoiceSignatureCell") as InvoiceSignatureCell;

				return cell;
			}

			return null;
		}

		public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
		{
			if (segue.Identifier == "Invoice_to_ClientList_Segue")
			{
				ClientsController clientsCtl = segue.DestinationViewController as ClientsController;
				clientsCtl.bPickClientMode = true;
				clientsCtl.callingController = this;
			}
			else if (segue.Identifier == "Invoice_To_NewItem_Segue")
			{
				InvoiceItemDetailController destCtrl = (segue.DestinationViewController as UINavigationController).ViewControllers[0] as InvoiceItemDetailController;
				destCtrl.callingController = this;
				destCtrl.bNewMode = true;
			}
			else if (segue.Identifier == "Invoice_to_AddAttachment_Segue")
			{
				InvoiceAttachmentDetailController destCtrl = (segue.DestinationViewController as UINavigationController).ViewControllers[0] as InvoiceAttachmentDetailController;
				destCtrl.callingController = this;
				destCtrl.bNew = true;
			}
			else if (segue.Identifier == "Invoice_To_AttachmentDetail_Segue")
			{
				InvoiceAttachmentDetailController destCtrl = segue.DestinationViewController as InvoiceAttachmentDetailController;
				destCtrl.callingController = this;
				destCtrl.bNew = false;
				destCtrl.iAttachment = 0;
				destCtrl.attachment = invoice.Attachments[iCurrentSelected - 1];
				destCtrl.image = attachmentImages[iCurrentSelected - 1];
			}
			else if (segue.Identifier == "Invoice_To_Note_Segue")
			{
				InvoiceNoteController destCtrl = segue.DestinationViewController as InvoiceNoteController;
				destCtrl.callingController = this;
				destCtrl.note = invoice.Note;
			}

			base.PrepareForSegue(segue, sender);
		}

		public override NSIndexPath WillSelectRow(UITableView tableView, NSIndexPath indexPath)
		{
			if (indexPath.Section == 4 && indexPath.Row <= invoice.Attachments.Count) // attachment
			{
				iCurrentSelected = indexPath.Row;
			}

			return indexPath;
		}

		partial void btnClose_UpInside(UIBarButtonItem sender)
		{
			DismissViewController(true, null);
		}

		partial void btnMore_UpInside(UIBarButtonItem sender)
		{
			UIAlertController actionSheetAlert = UIAlertController.Create(null, null, UIAlertControllerStyle.ActionSheet);

			// Add Actions
			actionSheetAlert.AddAction(UIAlertAction.Create("Save", UIAlertActionStyle.Default, async (action) =>
			{
				LoadingOverlay loadingOverlay = new LoadingOverlay(UIScreen.MainScreen.Bounds);
				this.View.Add(loadingOverlay);

				BuildInvoice();
				//int id = await SaveInvoice();
				//invoice.Id = id;

				await CK_SaveInvoice();
				loadingOverlay.Hide();

				DismissViewController(true, null);
			}));

			//			actionSheetAlert.AddAction(UIAlertAction.Create("New Estimate", UIAlertActionStyle.Default, (action) =>
			//			{
			//				UIStoryboard storyBoard = UIStoryboard.FromName("Main", null);
			//				//InvoiceViewController invoiceVC = (InvoiceViewController)storyBoard.InstantiateViewController("estimationVC");r
			//				//invoiceVC.ModalTransitionStyle = UIModalTransitionStyle.CoverVertical;
			//				//this.PresentViewController(invoiceVC, true, null);
			//
			//				UINavigationController invoiceViewNavigationController = (UINavigationController)storyBoard.InstantiateViewController("InvoiceViewNavigationController");
			//				this.PresentViewController(invoiceViewNavigationController, true, null);

			//				//InvoiceViewController estimationVC = new InvoiceViewController();			
			//				//this.PresentViewController(estimationVC, true, null);
			//			}));

			actionSheetAlert.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, (action) => Console.WriteLine("Cancel button pressed.")));

			actionSheetAlert.View.TintColor = UIColor.FromRGB(26, 188, 156);

			this.PresentViewController(actionSheetAlert, true, null);
		}

		private void BuildInvoice()
		{
			string stRecordName = ThisApp.UserName + "-Invoice-" + DateTime.Now.ToString("s");

			invoice.RecordName = stRecordName;

			invoice.Name = txtInvoiceName.Text;

			// date
			NSIndexPath indexPath = NSIndexPath.FromRowSection(0, 0);

			InvoiceDateCell dateCell = TableView.CellAt(indexPath) as InvoiceDateCell;
			if (!string.IsNullOrEmpty(dateCell.btnIssueDate.Title(UIControlState.Normal)))
				invoice.IssueDate = Convert.ToDateTime(dateCell.btnIssueDate.Title(UIControlState.Normal));
			if (!string.IsNullOrEmpty(dateCell.btnDueTerm.Title(UIControlState.Normal)))
				invoice.DueTerm = dateCell.btnDueTerm.Title(UIControlState.Normal);

			if (invoice.DueTerm.Contains("receipt"))
				invoice.DueDate = invoice.IssueDate;
			else
				invoice.DueDate = invoice.IssueDate.AddDays(int.Parse(Regex.Match(invoice.DueTerm, "\\d+").Value));


			// note 
			//indexPath = NSIndexPath.FromRowSection(0, 5);
			//InvoiceNoteCell noteCell = TableView.CellAt(indexPath) as InvoiceNoteCell;
			//invoice.Note = noteCell.lblNote.Text;
		}

		async private Task CK_SaveInvoice()
		{
			var invoiceRecordID = new CKRecordID(invoice.RecordName);

			var invoiceRecord = new CKRecord("Invoice", invoiceRecordID);

			if (!bNewMode)
			{
				invoiceRecord = await cloudManager.FetchRecordByRecordName(invoice.RecordName);
			}

			invoiceRecord["Name"] = (NSString)invoice.Name;

			invoiceRecord["User"] = (NSString)ThisApp.UserName;
			invoiceRecord["Name"] = (NSString)invoice.Name;
			invoiceRecord["IssuedDate"] = Helper.DateTimeToNSDate(invoice.IssueDate);
			invoiceRecord["DueTerm"] = (NSString)invoice.DueTerm;
			invoiceRecord["DueDate"] = Helper.DateTimeToNSDate(invoice.DueDate);

			invoiceRecord["Total"] = (NSNumber)(double)20.23;

			CKReference clientReference = new CKReference(new CKRecordID(invoice.Client.RecordName), CKReferenceAction.None);
			invoiceRecord["Client"] = clientReference;

			invoiceRecord["Note"] = (NSString)"Thank you";

			invoiceRecord["Status"] = (NSString)"d";

			await cloudManager.SaveAsync(invoiceRecord);

			foreach (InvoiceItem item in invoice.Items)
			{
				CKRecord _itemRecord = await cloudManager.FetchRecordByRecordName(item.RecordName);

				CKReference _invoiceReference = new CKReference(new CKRecordID(invoice.RecordName), CKReferenceAction.DeleteSelf);
				_itemRecord["Invoice"] = _invoiceReference;

				await cloudManager.SaveAsync(_itemRecord);
			}

			foreach (Attachment attachment in invoice.Attachments)
			{
				CKRecord _attachmentRecord = await cloudManager.FetchRecordByRecordName(attachment.RecordName);

				CKReference _invoiceReference = new CKReference(new CKRecordID(invoice.RecordName), CKReferenceAction.DeleteSelf);
				_attachmentRecord["Invoice"] = _invoiceReference;

				await cloudManager.SaveAsync(_attachmentRecord);
			}

		}

		async private Task<int> SaveInvoice()
		{
			string jsonString = JsonConvert.SerializeObject(invoice);

			HttpClient httpClient = new HttpClient();

			var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

			var result = await httpClient.PostAsync(Helper.AddInvoiceURL(), content);

			var contents = await result.Content.ReadAsStringAsync();

			string returnMessage = contents.ToString();

			var num = Regex.Match(returnMessage, "\\d+").Value;

			return Convert.ToInt32(num);
		}
	}
}