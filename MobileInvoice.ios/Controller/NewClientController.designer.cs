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
    [Register ("NewClientController")]
    partial class NewClientController
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
        UIKit.UIImageView imgContact { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField txtCity { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField txtCountry { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField txtEmail { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField txtName { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField txtPhone { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField txtPostal { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField txtState { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField txtStreet1 { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField txtStreet2 { get; set; }

        [Action ("btnCancel_UpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void btnCancel_UpInside (UIKit.UIBarButtonItem sender);

        [Action ("btnDelete_UpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void btnDelete_UpInside (UIKit.UIButton sender);

        [Action ("btnSave_UpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void btnSave_UpInside (UIKit.UIBarButtonItem sender);

        [Action ("txtName_UpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void txtName_UpInside (UIKit.UITextField sender);

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

            if (imgContact != null) {
                imgContact.Dispose ();
                imgContact = null;
            }

            if (txtCity != null) {
                txtCity.Dispose ();
                txtCity = null;
            }

            if (txtCountry != null) {
                txtCountry.Dispose ();
                txtCountry = null;
            }

            if (txtEmail != null) {
                txtEmail.Dispose ();
                txtEmail = null;
            }

            if (txtName != null) {
                txtName.Dispose ();
                txtName = null;
            }

            if (txtPhone != null) {
                txtPhone.Dispose ();
                txtPhone = null;
            }

            if (txtPostal != null) {
                txtPostal.Dispose ();
                txtPostal = null;
            }

            if (txtState != null) {
                txtState.Dispose ();
                txtState = null;
            }

            if (txtStreet1 != null) {
                txtStreet1.Dispose ();
                txtStreet1 = null;
            }

            if (txtStreet2 != null) {
                txtStreet2.Dispose ();
                txtStreet2 = null;
            }
        }
    }
}