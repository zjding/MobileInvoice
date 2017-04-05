﻿using MobileInvoice.model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MobileInvoice.api.Controllers
{
    public class InvoiceController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage AddInvoice(Invoice invoice)
        {
            string commandString = @"INSERT INTO Invoice (Name, IssueDate, DueTerm, DueDate, ClientId, Note)
                                     VALUES (@name, @issueDate, @dueTerm, @dueDate, @clientId, @note)";

            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = Constant.connectionString;

            SqlCommand command = new SqlCommand();
            command.CommandText = commandString;
            command.Connection = connection;

            command.Parameters.AddWithValue("@name", invoice.Name);
            command.Parameters.AddWithValue("@issueDate", invoice.IssueDate);
            command.Parameters.AddWithValue("@dueTerm", invoice.DueTerm);
            command.Parameters.AddWithValue("@dueDate", invoice.DueDate);
            command.Parameters.AddWithValue("@clientId", invoice.Client.Id);
            command.Parameters.AddWithValue("@note", invoice.Note);

            connection.Open();

            int rowInserted = command.ExecuteNonQuery();

            commandString = @"  SELECT  TOP 1 Id 
                                FROM    Invoice 
                                WHERE   Name = @name
                                ORDER BY Id desc";

            command.CommandText = commandString;
            command.Parameters.Clear();
            command.Parameters.AddWithValue("@name", invoice.Name);

            string invoiceId = command.ExecuteScalar().ToString();

            if (invoice.Items.Count > 0)
            {
                string itemIDs = "";

                foreach(InvoiceItem item in invoice.Items)
                {
                    itemIDs += item.Id.ToString() + ",";
                }

                itemIDs = itemIDs.Substring(0, itemIDs.Length - 1);

                commandString = @"UPDATE InvoiceItem SET InvoiceId = " + invoiceId + " WHERE Id IN (" + itemIDs + ")";

                command.CommandText = commandString;
                command.Parameters.Clear();

                command.ExecuteNonQuery();
            }

            if (invoice.Attachments.Count > 0)
            {
                string attachmentIDs = "";

                foreach (Attachment attachment in invoice.Attachments)
                {
                    attachmentIDs += attachment.Id.ToString() + ",";
                }

                attachmentIDs = attachmentIDs.Substring(0, attachmentIDs.Length - 1);

                commandString = @"UPDATE Attachment SET InvoiceId = " + invoiceId + " WHERE Id IN (" + attachmentIDs + ")";

                command.CommandText = commandString;
                command.Parameters.Clear();

                command.ExecuteNonQuery();
            }

            connection.Close();

            return Request.CreateResponse(HttpStatusCode.Created, invoiceId);

        }


    }
}