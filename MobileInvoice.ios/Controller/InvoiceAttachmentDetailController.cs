using Foundation;
using System;
using UIKit;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using MobileInvoice.model;

namespace MobileInvoice.ios
{
    public partial class InvoiceAttachmentDetailController : UITableViewController
    {
		UIImagePickerController cameraPicker;
		UIImagePickerController photoPicker;

		public bool bNew;
		public int iAttachment;
		public Attachment attachment = new Attachment();
		public UIImage image;

		public InvoiceViewController callingController;


        public InvoiceAttachmentDetailController (IntPtr handle) : base (handle)
        {
        }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			TableView.TableFooterView = new UIView();

			cameraPicker = new UIImagePickerController();
			cameraPicker.FinishedPickingMedia += camera_FinishedPickingMedia;
			cameraPicker.Canceled += camera_Cancelled;
			cameraPicker.SourceType = UIImagePickerControllerSourceType.Camera;

			photoPicker = new UIImagePickerController();
			photoPicker.FinishedPickingMedia += picker_FinishedPickingMedia;
			photoPicker.Canceled += picker_Cancelled;
			photoPicker.SourceType = UIImagePickerControllerSourceType.PhotoLibrary;

			AddDoneButtonToKeyboard(txtDescription);

			if (!bNew)
			{
				imgImage.Image = image;
				imgImage.ContentMode = UIViewContentMode.ScaleToFill;
				txtDescription.Text = attachment.Description;
			}
		}

		private async void camera_FinishedPickingMedia(object sender, UIImagePickerMediaPickedEventArgs e)
		{
			UIImage pickedImage = e.Info[UIImagePickerController.OriginalImage] as UIImage;
			this.imgImage.Image = pickedImage;
			this.imgImage.ContentMode = UIViewContentMode.ScaleToFill;
			await this.cameraPicker.DismissViewControllerAsync(true);
		}

		async private void camera_Cancelled(object sender, EventArgs e)
		{
			await this.cameraPicker.DismissViewControllerAsync(true);
		}

		private async void picker_FinishedPickingMedia(object sender, UIImagePickerMediaPickedEventArgs e)
		{
			UIImage pickedImage = e.Info[UIImagePickerController.OriginalImage] as UIImage;
			this.imgImage.Image = pickedImage;
			this.imgImage.ContentMode = UIViewContentMode.ScaleToFill;
			await this.photoPicker.DismissViewControllerAsync(true);
		}

		 private void picker_Cancelled(object sender, EventArgs e)
		{
			this.photoPicker.DismissViewController(true, null);
		}

		partial void btnImage_UpInside(UIButton sender)
		{
			// Create a new Alert Controller
			UIAlertController actionSheetAlert = UIAlertController.Create(null, null, UIAlertControllerStyle.ActionSheet);

			// Add Actions
			actionSheetAlert.AddAction(UIAlertAction.Create("From Camera", UIAlertActionStyle.Default, (action) => { LaunchCamera(); Console.WriteLine("Item One pressed."); }));

			actionSheetAlert.AddAction(UIAlertAction.Create("From Photos", UIAlertActionStyle.Default, (action) => { LaunchPhotoLibrary(); Console.WriteLine("Item Two pressed."); }));

			actionSheetAlert.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, (action) => Console.WriteLine("Cancel button pressed.")));

			// Required for iPad - You must specify a source for the Action Sheet since it is
			// displayed as a popover
			//UIPopoverPresentationController presentationPopover = actionSheetAlert.PopoverPresentationController;
			//if (presentationPopover != null)
			//{
			//	presentationPopover.SourceView = this.View;
			//	presentationPopover.PermittedArrowDirections = UIPopoverArrowDirection.Up;
			//}

			actionSheetAlert.View.TintColor = UIColor.FromRGB(26, 188, 156);

			// Display the alert
			this.PresentViewController(actionSheetAlert, true, null);
		}

		public void LaunchCamera()
		{
			this.PresentViewController(cameraPicker, true, null);
		}

		public void LaunchPhotoLibrary()
		{
			this.PresentViewController(photoPicker, true, null);
		}

		void AddDoneButtonToKeyboard(FloatLabeledTextField textField)
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

			UIImage resizedImage = ImageManager.ResizeImage(this.imgImage.Image, 1000, 1000);
			var imgStream = resizedImage.AsJPEG(0.75f).AsStream();

			var name = await ImageManager.UploadImage(imgStream);

			if (bNew)
			{
				attachment.ImageName = name;
				attachment.Description = txtDescription.Text;

				string jsonString = JsonConvert.SerializeObject(attachment);

				HttpClient httpClient = new HttpClient();

				var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

				var result = await httpClient.PostAsync(Helper.AddAttachmentURL(), content);

				var contents = await result.Content.ReadAsStringAsync();

				string returnMessage = contents.ToString();

				var num = Regex.Match(returnMessage, "\\d+").Value;

				attachment.Id = Convert.ToInt32(num);

				callingController.invoice.Attachments.Add(attachment);
				callingController.attachmentImages.Add(resizedImage);
				//callingController.attachmentImages.Add(resizedImage);e

				loadingOverlay.Hide();

				callingController.DismissViewController(true, null);
			}

			else
			{
				await ImageManager.DeleteImage(attachment.ImageName);

				var newName = await ImageManager.UploadImage(imgStream);

				Attachment updatedAttachment = new Attachment();
				updatedAttachment.Id = attachment.Id;
				updatedAttachment.ImageName = newName;
				updatedAttachment.Description = txtDescription.Text;

				//attachment.imageName = name;
				//attachment.image = this.imgAttachment.Image;
				//attachment.description = this.txtDescription.Text;

				string jsonString = JsonConvert.SerializeObject(updatedAttachment);

				HttpClient httpClient = new HttpClient();

				var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

				var result = await httpClient.PutAsync(Helper.UpdateAttachmentURL(), content);

				var contents = await result.Content.ReadAsStringAsync();

				string returnMessage = contents.ToString();


				if (returnMessage == "\"Updated attachment successfully\"")
				{
					//int i = callingController.attachments.FindIndex(a => a.id == att.id);
					//callingController.attachments.RemoveAt(i);
					//callingController.attachments.Insert(i, attachment);
					loadingOverlay.Hide();
					this.NavigationController.PopViewController(true);
				}
			}
			
		}

		partial void btnCancel_UpInside(UIBarButtonItem sender)
		{
			if (bNew)
				callingController.DismissViewController(true, null);
			else
				this.NavigationController.PopViewController(true);
		}

		public override nint NumberOfSections(UITableView tableView)
		{
			if (bNew)
				return 2;
			else
				return 3;
		}
	}
}