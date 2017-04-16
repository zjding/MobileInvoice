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
    [Register ("InvoiceSignatureController")]
    partial class InvoiceSignatureController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        SignaturePad.SignaturePadView signaturePad { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (signaturePad != null) {
                signaturePad.Dispose ();
                signaturePad = null;
            }
        }
    }
}