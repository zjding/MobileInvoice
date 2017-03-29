using System;
using UIKit;

namespace MobileInvoice.ios
{
	public class Attachment
	{
		public int Id { get; set; }
		public string ImageName { get; set; }
		public UIImage Image { get; set; }
		public string Description { get; set; }
	}
}
