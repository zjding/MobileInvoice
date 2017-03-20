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
    [Register ("ItemDetailController")]
    partial class ItemDetailController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIBarButtonItem btnCancel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnDelete { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIBarButtonItem btnSave { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView imgItems { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UISwitch swTaxable { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField txtName { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextView txtNote { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField txtQuantity { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField txtUnitCost { get; set; }

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

            if (imgItems != null) {
                imgItems.Dispose ();
                imgItems = null;
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
        }
    }
}