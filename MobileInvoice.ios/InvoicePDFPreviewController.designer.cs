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
    [Register ("InvoicePDFPreviewController")]
    partial class InvoicePDFPreviewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIWebView wvPDF { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (wvPDF != null) {
                wvPDF.Dispose ();
                wvPDF = null;
            }
        }
    }
}