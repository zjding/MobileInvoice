using Foundation;
using System;
using UIKit;

namespace MobileInvoice.ios
{
    public partial class InvoiceListCell : UITableViewCell
    {
		public string status;

        public InvoiceListCell (IntPtr handle) : base (handle)
        {
        }

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();

			lblStatus.Layer.CornerRadius = 4.0f;
			lblStatus.ClipsToBounds = true;

			if (status == "d")
			{
				lblStatus.BackgroundColor = UIColor.LightGray;
				lblTotal.TextColor = UIColor.DarkGray;
				lblStatus.Text = "DRAFT";
			}
			else if (status == "s")
			{
				lblStatus.BackgroundColor = new UIColor(
													(nfloat)(26.0 / 255.0),
													(nfloat)(188 / 255.0),
													(nfloat)(156 / 255.0),
													(nfloat)1.0
												);
				lblTotal.TextColor = new UIColor(
													(nfloat)(22.0 / 255.0),
													(nfloat)(160 / 255.0),
													(nfloat)(133 / 255.0),
													(nfloat)1.0
												);
				lblStatus.Text = "SENT";
			}
			else if (status == "p")
			{
				lblStatus.BackgroundColor = new UIColor(
													(nfloat)(52.0 / 255.0),
													(nfloat)(152 / 255.0),
													(nfloat)(219 / 255.0),
													(nfloat)1.0
												);
				lblTotal.TextColor = new UIColor(
													(nfloat)(41.0 / 255.0),
													(nfloat)(128 / 255.0),
													(nfloat)(185 / 255.0),
													(nfloat)1.0
												);
				lblStatus.Text = "PAID";
			}
			else if (status == "o")
			{
				lblStatus.BackgroundColor = new UIColor(
													(nfloat)(231.0 / 255.0),
													(nfloat)(76 / 255.0),
													(nfloat)(60 / 255.0),
													(nfloat)1.0
												);
				lblTotal.TextColor = new UIColor(
													(nfloat)(192.0 / 255.0),
													(nfloat)(57 / 255.0),
													(nfloat)(43 / 255.0),
													(nfloat)1.0
												);
				lblStatus.Text = "OVERDUE";
			}
			
		}
    }
}