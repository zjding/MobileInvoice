using Foundation;
using System;
using UIKit;

namespace iCloudPlayground
{
    public partial class iCloudPlaygroundController : UITableViewController
    {
        public iCloudPlaygroundController (IntPtr handle) : base (handle)
        {
        }

		partial void btnSave_UpInside(UIButton sender)
		{
			throw new NotImplementedException();
		}
	}
}