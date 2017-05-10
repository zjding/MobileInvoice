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
    [Register ("InvoiceListController")]
    partial class InvoiceListController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIBarButtonItem btnInvoiceAdd { get; set; }

        [Action ("btnCalendar_UpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void btnCalendar_UpInside (UIKit.UIButton sender);

        [Action ("btnInvoiceAdd_UpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void btnInvoiceAdd_UpInside (UIKit.UIBarButtonItem sender);

        void ReleaseDesignerOutlets ()
        {
            if (btnInvoiceAdd != null) {
                btnInvoiceAdd.Dispose ();
                btnInvoiceAdd = null;
            }
        }
    }
}