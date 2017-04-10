// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace MobileInvoice.ios
{
    [Register ("InvoiceListCell")]
    partial class InvoiceListCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblStatus { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblTotal { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (lblStatus != null) {
                lblStatus.Dispose ();
                lblStatus = null;
            }

            if (lblTotal != null) {
                lblTotal.Dispose ();
                lblTotal = null;
            }
        }
    }
}