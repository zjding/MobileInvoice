using MobileInvoice.model;
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
        [Route("api/Invoice/GetInvoices")]
        public List<Invoice> Get()
        {
            List<Invoice> invoices = new List<Invoice>();

            string commandString = @"SELECT * FROM Invoice";

            SqlDataReader reader = null;
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = Constant.connectionString;

            SqlCommand command = new SqlCommand();
            command.CommandText = commandString;
            command.Connection = connection;

            connection.Open();
            reader = command.ExecuteReader();

            while (reader.Read())
            {
                Invoice invoice = new Invoice();
                invoice.Id = Convert.ToInt16(reader["Id"]);
                invoice.Name = Convert.ToString(reader["Name"]);
                //invoice.DueDate = Convert.ToDateTime(reader["DueDate"]);
                //invoice.Total = reader["Total"] != DBNull.Value ? Convert.ToDecimal(reader["Total"]) : 0;
                //invoice.Status = reader["Status"] != DBNull.Value ? Convert.ToString(reader["Status"]) : "";

                invoices.Add(invoice);
            }

            connection.Close();

            return invoices;
        }

        [Route("api/Invoice/GetInvoicesByStatus/{status}")]
        public List<Invoice> Get(string status)
        {
            List<Invoice> invoices = new List<Invoice>();

            string commandString;

            if (status != "a")
                commandString = @"  select  i.Id, i.Name as InvoiceName, i.IssueDate, i.DueDate, i.Total, 
                                            i.Status, c.Name as ClientName
                                    from Invoice i, Client c
                                    where i.ClientId = c.Id
                                    and i.Status ='" + status + "'";
            else
                commandString = @"select  i.Id, i.Name as InvoiceName, i.IssueDate, i.DueDate, i.Total, 
                                            i.Status, c.Name as ClientName
                                    from Invoice i, Client c
                                    where i.ClientId = c.Id";

            SqlDataReader reader = null;
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = Constant.connectionString;

            SqlCommand command = new SqlCommand();
            command.CommandText = commandString;
            command.Connection = connection;

            connection.Open();
            reader = command.ExecuteReader();

            while (reader.Read())
            {
                Invoice invoice = new Invoice();
                invoice.Id = Convert.ToInt16(reader["Id"]);
                invoice.Name = Convert.ToString(reader["InvoiceName"]);
                invoice.Status = Convert.ToString(reader["Status"]);
                invoice.ClientName = Convert.ToString(reader["ClientName"]);
                invoice.DueDate = Convert.ToDateTime(reader["DueDate"]);
                invoice.Total = reader["Total"] != DBNull.Value ? Convert.ToDecimal(reader["Total"]) : 0;
                //invoice.Status = reader["Status"] != DBNull.Value ? Convert.ToString(reader["Status"]) : "";

                invoices.Add(invoice);
            }

            connection.Close();

            return invoices;
        }

        [Route("api/Invoice/GetInvoice/{id}")]
        public Invoice GetInvoice(string id)
        {
            Invoice invoice = new Invoice();

            string commandString = @"SELECT * FROM Invoice WHERE Id = " + id;

            SqlDataReader reader = null;
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = Constant.connectionString;

            SqlCommand command = new SqlCommand();
            command.CommandText = commandString;
            command.Connection = connection;

            connection.Open();
            reader = command.ExecuteReader();

            int clientId = 0;

            if (reader.Read())
            {
                invoice.Id = Convert.ToInt16(reader["Id"]);
                invoice.Name = Convert.ToString(reader["Name"]);
                invoice.IssueDate = Convert.ToDateTime(reader["IssueDate"]);
                invoice.DueDate = Convert.ToDateTime(reader["DueDate"]);
                invoice.DueTerm = Convert.ToString(reader["DueTerm"]);
                invoice.Note = Convert.ToString(reader["Note"]);
                invoice.Total = reader["Total"] != DBNull.Value ? Convert.ToDecimal(reader["Total"]) : 0;
                invoice.Status = Convert.ToString(reader["Status"]);

                clientId = Convert.ToInt16(reader["ClientId"]);
            }

            reader.Close();

            commandString = @"SELECT * FROM Client WHERE Id = " + clientId.ToString();
            command.CommandText = commandString;

            reader = command.ExecuteReader();

            if (reader.Read())
            {
                Client client = new Client();
                client.Id = Convert.ToInt16(reader["Id"]);
                client.Name = Convert.ToString(reader["Name"]);
                client.Phone = Convert.ToString(reader["Phone"]);
                client.Email = Convert.ToString(reader["Email"]);
                client.Street1 = Convert.ToString(reader["Street1"]);
                client.Street2 = Convert.ToString(reader["Street2"]);
                client.City = Convert.ToString(reader["City"]);
                client.State = Convert.ToString(reader["State"]);
                client.Country = Convert.ToString(reader["Country"]);
                client.PostCode = Convert.ToString(reader["PostCode"]);

                invoice.Client = client;
                invoice.ClientName = client.Name;
            }

            reader.Close();

            commandString = @"SELECT * FROM InvoiceItem WHERE InvoiceId = " + invoice.Id.ToString();
            command.CommandText = commandString;

            reader = command.ExecuteReader();

            invoice.Items = new List<InvoiceItem>();

            while (reader.Read())
            {
                InvoiceItem item = new InvoiceItem();

                item.Id = Convert.ToInt16(reader["Id"]);
                item.Name = Convert.ToString(reader["Name"]);
                item.UnitPrice = reader["UnitPrice"] != DBNull.Value ? Convert.ToDecimal(reader["UnitPrice"]) : 0;
                item.Quantity = Convert.ToInt16(reader["Quantity"]);
                item.bTaxable = Convert.ToBoolean(reader["Taxable"]);
                item.Note = Convert.ToString(reader["Note"]);

                invoice.Items.Add(item);
            }

            reader.Close();

            commandString = @"SELECT * FROM Attachment WHERE InvoiceId = " + invoice.Id.ToString();
            command.CommandText = commandString;

            reader = command.ExecuteReader();

            invoice.Attachments = new List<Attachment>();

            while (reader.Read())
            {
                Attachment attachment = new Attachment();

                attachment.Id = Convert.ToInt16(reader["Id"]);
                attachment.ImageName = Convert.ToString(reader["ImageName"]);
                attachment.Description = Convert.ToString(reader["Description"]);

                invoice.Attachments.Add(attachment);
            }

            reader.Close();

            return invoice;
        }

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
