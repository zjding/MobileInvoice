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
    [Register ("InvoiceViewController")]
    partial class InvoiceViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
      public UIKit.UIBarButtonItem btnClose { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
      public UIKit.UIBarButtonItem btnMore { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
      public UIKit.UITextField txtInvoiceName { get; set; }

        [Action ("btnClose_UpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void btnClose_UpInside (UIKit.UIBarButtonItem sender);

        [Action ("btnMore_UpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void btnMore_UpInside (UIKit.UIBarButtonItem sender);

        void ReleaseDesignerOutlets ()
        {
            if (btnClose != null) {
                btnClose.Dispose ();
                btnClose = null;
            }

            if (btnMore != null) {
                btnMore.Dispose ();
                btnMore = null;
            }

            if (txtInvoiceName != null) {
                txtInvoiceName.Dispose ();
                txtInvoiceName = null;
            }
        }
    }
}