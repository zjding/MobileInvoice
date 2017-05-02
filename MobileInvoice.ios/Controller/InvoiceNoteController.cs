using Foundation;
using System;
using UIKit;
using MobileInvoice.model;
using System.Threading.Tasks;
using CloudKit;

namespace MobileInvoice.ios
{
    public partial class InvoiceNoteController : UITableViewController
    {
		public InvoiceViewController callingController;

        public InvoiceNoteController (IntPtr handle) : base (handle)
        {
			
        }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			UIBarButtonItem btnBack = new UIBarButtonItem(UIImage.FromFile("Images/Left-30-green.png"), UIBarButtonItemStyle.Plain, (sender, e) =>
				{
					//await SaveNote();
					callingController.invoice.Note = this.txtNote.Text;
					NavigationController.PopViewController(true);
				}
			);

			NavigationItem.LeftBarButtonItem = btnBack;

			this.txtNote.Text = callingController.invoice.Note;
		}

		public override void WillDisplayHeaderView(UITableView tableView, UIView headerView, nint section)
		{

			var header = headerView as UITableViewHeaderFooterView;

			header.TextLabel.TextColor = UIColor.LightGray;

			header.TextLabel.Font = UIFont.FromName("AvenirNext-Bold", 12);
		}

		//async public Task SaveNote()
		//{
		//	//if (string.IsNullOrEmpty(note.RecordName))
		//	//{
		//	//	string stRecordName = ThisApp.UserName + "-Note-" + DateTime.Now.ToString("s");
		//	//	CKRecordID noteRecordID = new CKRecordID(stRecordName);
		//	//	CKRecord _noteRecord = new CKRecord("Note", noteRecordID);

		//	//	CKReference _invoiceReference = new CKReference(new CKRecordID(stRecordName), CKReferenceAction.DeleteSelf);
		//	//	_noteRecord["Invoice"] = _invoiceReference;

		//	//	_noteRecord["NoteString"] = (NSString)this.txtNote.Text;

		//	//	await cloudManager.SaveAsync(_noteRecord);

		//	//	note.RecordName = stRecordName;
		//	//	note.NoteString = this.txtNote.Text;

		//	//	callingController.invoice.Note = note;
		//	//}
		//	//else
		//	//{
		//	//	CKRecord _noteRecord = await cloudManager.FetchRecordByRecordName(note.RecordName);

		//	//	_noteRecord["NoteString"] = (NSString)this.txtNote.Text;

		//	//	await cloudManager.SaveAsync(_noteRecord);

		//	//	note.NoteString = this.txtNote.Text;

		//	//	callingController.invoice.Note = note;
		//	//}


		//}
    }
}