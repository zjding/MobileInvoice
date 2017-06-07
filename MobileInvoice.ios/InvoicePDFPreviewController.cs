using Foundation;
using System;
using UIKit;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;

namespace MobileInvoice.ios
{
    public partial class InvoicePDFPreviewController : UIViewController
    {
        public InvoicePDFPreviewController (IntPtr handle) : base (handle)
        {
        }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			//Document document = new Document(PageSize.A4, 70, 70, 70, 70);

			//MemoryStream PDFData = new MemoryStream();
			//PdfWriter write = PdfWriter.GetInstance(document, PDFData);

			string path = NSBundle.MainBundle.BundlePath;



			path = path + "/invoice.html";

			string content = File.ReadAllText(path);

			//content = content.Replace("#LOGO_IMAGE#", "file://" + NSBundle.MainBundle.BundlePath + "/google.png");
			//content = content.Replace("#LOGO_IMAGE#", "https:///zjdingstorage.blob.core.windows.net//container1//google.png");
			//content = content.Replace("#INVOICE_NUMBER#", "123456789");
			//content = content.Replace("#INVOICE_DATE#", "1/20/2017");


			//this.PDFPreview_wvPDF.LoadHtmlString(content, null);

			//byte[] bPDF = GetPDF(content);

			var document = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

			var fileName = Path.Combine(document, "invoice.pdf");

			//File.WriteAllBytes(fileName, bPDF);

			GeneratePDF(content, fileName);

			this.wvPDF.LoadRequest(new NSUrlRequest(new NSUrl(fileName)));
		}

		public void GeneratePDF(string html, string filepath)
		{
			Document document = new Document(PageSize.A4);
			PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(filepath, FileMode.Create));
			document.Open();

			StringReader sr = new StringReader(html);
			XMLWorkerHelper.GetInstance().ParseXHtml(writer, document, sr);
			document.Close();
		}
    }
}