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
    [Register ("InvoiceItemDetailController")]
    partial class InvoiceItemDetailController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
      public UIKit.UIBarButtonItem btnCancel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
      public UIKit.UIButton btnDelete { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
      public UIKit.UIBarButtonItem btnSave { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
      public UIKit.UISwitch swSave { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
      public UIKit.UISwitch swTaxable { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        MobileInvoice.ios.FloatLabeledTextField txtName { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
      public UIKit.UITextView txtNote { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        MobileInvoice.ios.FloatLabeledTextField txtQuantity { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        MobileInvoice.ios.FloatLabeledTextField txtUnitCost { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
      public UIKit.UIView vwUnder1 { get; set; }

        [Action ("btnCancel_UpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void btnCancel_UpInside (UIKit.UIBarButtonItem sender);

        [Action ("btnSave_UpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void btnSave_UpInside (UIKit.UIBarButtonItem sender);

        void ReleaseDesignerOutlets ()
        {
            if (btnCancel != null) {
                btnCancel.Dispose ();
                btnCancel = null;
            }

            if (btnDelete != null) {
                btnDelete.Dispose ();
                btnDelete = null;
            }

            if (btnSave != null) {
                btnSave.Dispose ();
                btnSave = null;
            }

            if (swSave != null) {
                swSave.Dispose ();
                swSave = null;
            }

            if (swTaxable != null) {
                swTaxable.Dispose ();
                swTaxable = null;
            }

            if (txtName != null) {
                txtName.Dispose ();
                txtName = null;
            }

            if (txtNote != null) {
                txtNote.Dispose ();
                txtNote = null;
            }

            if (txtQuantity != null) {
                txtQuantity.Dispose ();
                txtQuantity = null;
            }

            if (txtUnitCost != null) {
                txtUnitCost.Dispose ();
                txtUnitCost = null;
            }

            if (vwUnder1 != null) {
                vwUnder1.Dispose ();
                vwUnder1 = null;
            }
        }
    }
}