using Foundation;
using System;
using UIKit;
using CloudKit;

namespace iCloudPlayground
{
    public partial class iCloudPlaygroundController : UITableViewController
    {
		private const string ReferenceItemRecordName = "ReferenceItems";

		#region Computed Properties
		public AppDelegate ThisApp
		{
			get { return (AppDelegate)UIApplication.SharedApplication.Delegate; }
		}
		#endregion

		public iCloudPlaygroundController (IntPtr handle) : base (handle)
        {
        }

		partial void btnSave_UpInside(UIButton sender)
		{

			// Create a new record
			var newRecord = new CKRecord(ReferenceItemRecordName);
			newRecord["FirstName"] = (NSString)"Jason";
			newRecord["LastName"] = (NSString)"Ding";

			// Save it to the database
			ThisApp.PublicDatabase.SaveRecord(newRecord, (record, err) => {
			    // Was there an error?
			    if (err != null) {
			    
			    }
			});
		}
	}
}