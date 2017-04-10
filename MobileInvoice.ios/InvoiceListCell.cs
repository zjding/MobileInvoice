using Foundation;
using System;
using UIKit;

namespace MobileInvoice.ios
{
    public partial class InvoiceListCell : UITableViewCell
    {
        public InvoiceListCell (IntPtr handle) : base (handle)
        {
        }

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();

			lblStatus.Layer.CornerRadius = 4.0f;
			lblStatus.ClipsToBounds = true;
		}
    }
}