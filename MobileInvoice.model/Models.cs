using System;
using System.Collections.Generic;


namespace MobileInvoice.model
{
	public class Client
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Phone { get; set; }
		public string Email { get; set; }
		public string Street1 { get; set; }
		public string Street2 { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public string Country { get; set; }
		public string PostCode { get; set; }
	}

	public class InvoiceItem
    {
		public int Id { get; set; }
		public string Name { get; set; }
		public decimal UnitPrice { get; set; }
		public int Quantity { get; set; }
		public string DiscountType { get; set; }
		public decimal DiscountAmount { get; set; }
		public bool bTaxable { get; set; }
		public string Note { get; set; }
	}

    public class Attachment
    {
        public int Id { get; set; }
		public string CloudId { get; set; }
        public string ImageName { get; set; }
        public string Description { get; set; }
    }

	public class Invoice
	{
		public int Id { get; set; }
		public string CloudId { get; set; }
        public string Name { get; set; }
		public DateTime IssueDate { get; set; }
		public string DueTerm { get; set; }
		public DateTime DueDate { get; set; }
		public Client Client { get; set; }
        public string ClientName { get; set; }
		public List<InvoiceItem> Items { get; set; }
        public string Note { get; set; }
        public List<Attachment> Attachments { get; set; }
        public string Signature { get; set; }
        public decimal Total { get; set; }
        public string Status { get; set; }

		public Invoice()
		{
			Client = new Client();
			Items = new List<InvoiceItem>();
            Attachments = new List<Attachment>();
		}
	}

    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
