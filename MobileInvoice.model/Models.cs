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

	public class Item
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

	public class Invoice
	{
		public int Id { get; set; }
		public DateTime IssueDT { get; set; }
		public string DueTerm { get; set; }
		public DateTime DueDT { get; set; }
		public Client Client { get; set; }
		public List<Item> Items { get; set; }

		public Invoice()
		{
			Client = new Client();
			Items = new List<Item>();
		}
	}
}
