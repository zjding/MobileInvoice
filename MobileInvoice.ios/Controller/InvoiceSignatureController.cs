using Foundation;
using System;
using UIKit;
using System.IO;
using CloudKit;

namespace MobileInvoice.ios
{
    public partial class InvoiceSignatureController : UIViewController
    {
		public InvoiceViewController callingController;
		public UIImage signatureImage;

		CloudManager cloudManager;

		#region Computed Properties
		public AppDelegate ThisApp
		{
			get { return (AppDelegate)UIApplication.SharedApplication.Delegate; }
		}
		#endregion

        public InvoiceSignatureController (IntPtr handle) : base (handle)
        {
			cloudManager = new CloudManager();
        }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			if (signatureImage != null)
				signaturePad.signatureImage = signatureImage;

			signaturePad.Initialize();

			signaturePad.Layer.ShadowOpacity = 0f;

			UIDevice.CurrentDevice.SetValueForKey(new NSNumber((int)UIInterfaceOrientation.LandscapeLeft), new NSString("orientation"));

			AttemptRotationToDeviceOrientation();

		}

		async partial void btnBack_UpInside(UIButton sender)
		{
			UIImage signatureImage = signaturePad.GetImage();

			var tmp = Path.GetTempPath();
			string path = Path.Combine(tmp, "signature.tmp");
			NSData imageData = signatureImage.AsJPEG(0.75f);
			imageData.Save(path, true);

			var url = new NSUrl(path, false);

			string stRecordName = ThisApp.UserName + "-Signature-" + DateTime.Now.ToString("s");
			CKRecordID signatureRecordID = new CKRecordID(stRecordName);
			CKRecord signatureRecord = new CKRecord("Signature", signatureRecordID);

			signatureRecord["Image"] = new CKAsset(url);
			signatureRecord["User"] = (NSString)(ThisApp.UserName);

			await cloudManager.SaveAsync(signatureRecord);

			callingController.invoice.SignatureName = stRecordName;
			callingController.signatureImage = signatureImage;

			callingController.DismissViewController(true, null);
		}
	}
}