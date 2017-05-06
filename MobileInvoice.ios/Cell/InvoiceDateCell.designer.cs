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
    [Register ("InvoiceDateCell")]
    partial class InvoiceDateCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
      public UIKit.UIButton btnDueTerm { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
      public UIKit.UIButton btnIssueDate { get; set; }

        [Action ("btnDueTerm_UpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void btnDueTerm_UpInside (UIKit.UIButton sender);

        [Action ("btnIssueDate_UpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void btnIssueDate_UpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (btnDueTerm != null) {
                btnDueTerm.Dispose ();
                btnDueTerm = null;
            }

            if (btnIssueDate != null) {
                btnIssueDate.Dispose ();
                btnIssueDate = null;
            }
        }
    }
}