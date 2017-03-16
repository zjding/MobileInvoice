using System;
using System.Collections.Generic;
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

	}
}
