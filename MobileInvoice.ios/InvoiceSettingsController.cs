using Foundation;
using System;
using UIKit;
using CloudKit;

namespace MobileInvoice.ios
{
    public partial class InvoiceSettingsController : UITableViewController
    {
		private const string ReferenceItemRecordName = "NameItems";

		#region Computed Properties
		public AppDelegate ThisApp
		{
			get { return (AppDelegate)UIApplication.SharedApplication.Delegate; }
		}
		#endregion

        public InvoiceSettingsController (IntPtr handle) : base (handle)
        {
        }

		partial void btnSave_UpInside(UIButton sender)
		{
			var newRecord = new CKRecord(ReferenceItemRecordName);
			newRecord["FirstName"] = (NSString)"Jason";
			newRecord["LastName"] = (NSString)"Ding";

			// Save it to the database
			ThisApp.PublicDatabase.SaveRecord(newRecord, (record, err) => {
			    // Was there an error?
			    if (err != null) {
					UIAlertView alert = new UIAlertView()
					{
						Title = "",
						Message = "Save succeed"
					};
					alert.AddButton("OK");
					alert.Show ();
			    }
			});
		}
	}
}