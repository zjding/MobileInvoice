using Foundation;
using System;
using UIKit;

namespace MobileInvoice.ios
{
    public partial class InvoiceSignatureController : UIViewController
    {
        public InvoiceSignatureController (IntPtr handle) : base (handle)
        {
        }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			signaturePad.Layer.ShadowOpacity = 0f;

			UIDevice.CurrentDevice.SetValueForKey(new NSNumber((int)UIInterfaceOrientation.LandscapeLeft), new NSString("orientation"));

			AttemptRotationToDeviceOrientation();

		}
    }
}