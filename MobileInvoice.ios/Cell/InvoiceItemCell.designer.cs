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
    [Register ("InvoiceItemCell")]
    partial class InvoiceItemCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        public UIKit.UILabel lblItemName { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        public UIKit.UILabel lblNum { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        public UIKit.UILabel lblTotalPrice { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        public UIKit.UILabel lblUnitPrice { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (lblItemName != null) {
                lblItemName.Dispose ();
                lblItemName = null;
            }

            if (lblNum != null) {
                lblNum.Dispose ();
                lblNum = null;
            }

            if (lblTotalPrice != null) {
                lblTotalPrice.Dispose ();
                lblTotalPrice = null;
            }

            if (lblUnitPrice != null) {
                lblUnitPrice.Dispose ();
                lblUnitPrice = null;
            }
        }
    }
}