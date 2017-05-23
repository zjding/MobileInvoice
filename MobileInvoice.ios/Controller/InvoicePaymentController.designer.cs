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
    [Register ("InvoicePaymentController")]
    partial class InvoicePaymentController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        MobileInvoice.ios.FloatLabeledTextField txtBank { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        MobileInvoice.ios.FloatLabeledTextField txtCheck { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextView txtInstruction { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        MobileInvoice.ios.FloatLabeledTextField txtPaypal { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (txtBank != null) {
                txtBank.Dispose ();
                txtBank = null;
            }

            if (txtCheck != null) {
                txtCheck.Dispose ();
                txtCheck = null;
            }

            if (txtInstruction != null) {
                txtInstruction.Dispose ();
                txtInstruction = null;
            }

            if (txtPaypal != null) {
                txtPaypal.Dispose ();
                txtPaypal = null;
            }
        }
    }
}