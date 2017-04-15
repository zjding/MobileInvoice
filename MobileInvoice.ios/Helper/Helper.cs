using System;
using System.Collections.Generic;
using Foundation;
using MobileInvoice.model;
using UIKit;

namespace MobileInvoice.ios
{
	public static class Helper
	{
		static string WebAPI_URL = "http://webapitry120161228015023.azurewebsites.net";

		public static string GetClientsURL()
		{
			return WebAPI_URL + "/api/Clients/GetClients";
		}

		public static string AddClientURL()
		{
			return WebAPI_URL + "/api/Client/AddClient";
		}

		public static string DeleteClientURL()
		{
			return WebAPI_URL + "/api/Clients/Delete/";
		}

		public static string UpdateClientURL()
		{
			return WebAPI_URL + "/api/Client/PutClient";
		}

		public static string AddItemURL()
		{
			return WebAPI_URL + "/api/Item/AddItem";
		}

		public static string GetItemsURL()
		{
			return WebAPI_URL + "/api/Item/GetItems";
		}

		public static string AddInvoiceItemURL()
		{
			return WebAPI_URL + "/api/InvoiceItem/AddItem";
		}

		public static string GetInvoiceItemsURL()
		{
			return WebAPI_URL + "/api/InvoiceItem/GetItems";
		}

		public static string AddAttachmentURL()
		{
			return WebAPI_URL + "/api/Attachment/AddAttachment";
		}

		public static string UpdateAttachmentURL()
		{
			return WebAPI_URL + "/api/Attachment/PutAttachment";
		}

		public static string DeleteAttachmentURL()
		{
			return WebAPI_URL + "/api/Attachment/Delete/";
		}

		public static string GetInvoicesURL()
		{
			return WebAPI_URL + "/api/Invoice/GetInvoices";
		}

		public static string GetInvoicesByStatusURL()
		{
			return WebAPI_URL + "/api/Invoice/GetInvoicesByStatus";
		}

		public static string GetInvoiceByIdURL()
		{
			return WebAPI_URL + "/api/Invoice/GetInvoice";
		}

		public static string AddInvoiceURL()
		{
			return WebAPI_URL + "/api/Invoice/AddInvoice";
		}

		public static void InsertInOrder(string stringToInsert, ref List<string> list)
		{

			if (list.Count == 0)
			{
				list.Add(stringToInsert);
				return;
			}

			int i = 0;

			while (i < list.Count && list[i].CompareTo(stringToInsert) < 0)
			{
				i++;
			}

			list.Insert(i, stringToInsert);
		}

		public static void InsertInOrder(Client clientToInsert, ref List<Client> list)
		{
			if (list.Count == 0)
			{
				list.Add(clientToInsert);
				return;
			}

			int i = 0;

			while (i < list.Count && list[i].Name.CompareTo(clientToInsert.Name) < 0)
			{
				i++;
			}

			list.Insert(i, clientToInsert);

		}

		public static DateTime NSDateToDateTime(NSDate date)
		{
			DateTime reference = TimeZone.CurrentTimeZone.ToLocalTime(
				new DateTime(2001, 1, 1, 0, 0, 0));
			return reference.AddSeconds(date.SecondsSinceReferenceDate);
		}

		public static NSDate DateTimeToNSDate(DateTime date)
		{
			DateTime reference = TimeZone.CurrentTimeZone.ToLocalTime(
				new DateTime(2001, 1, 1, 0, 0, 0));
			return NSDate.FromTimeIntervalSinceReferenceDate(
				(date - reference).TotalSeconds);
		}
	}
}
