// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
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
        UIKit.UIButton btnCalendar { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnSearch { get; set; }

        [Action ("btnCalendar_UpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void btnCalendar_UpInside (UIKit.UIButton sender);

        [Action ("btnSearch_UpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void btnSearch_UpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (btnCalendar != null) {
                btnCalendar.Dispose ();
                btnCalendar = null;
            }

            if (btnSearch != null) {
                btnSearch.Dispose ();
                btnSearch = null;
            }
        }
    }
}