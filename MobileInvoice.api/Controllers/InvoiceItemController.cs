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
    public class InvoiceItemController : ApiController
    {
        private string m_connectionString = @"Server=tcp:webapitry120161228015023.database.windows.net,1433;Initial Catalog=WebApiTry120161228015023;Persist Security Info=False;User ID=zjding;Password=G4indigo;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        [Route("api/InvoiceItem/GetItems")]
        public List<InvoiceItem> Get()
        {
            List<InvoiceItem> items = new List<InvoiceItem>();

            string commandString = @"Select * from InvoiceItem";

            SqlDataReader reader = null;
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = m_connectionString;

            SqlCommand command = new SqlCommand();
            command.CommandText = commandString;
            command.Connection = connection;

            connection.Open();
            reader = command.ExecuteReader();

            while (reader.Read())
            {
                InvoiceItem item = new InvoiceItem();
                item.Id = Convert.ToInt32(reader["Id"]);
                item.Name = reader["Name"] != DBNull.Value ? Convert.ToString(reader["Name"]) : string.Empty;
                item.UnitPrice = reader["UnitPrice"] != DBNull.Value ? Convert.ToDecimal(reader["UnitPrice"]) : 0;
                item.Quantity = reader["Quantity"] != DBNull.Value ? Convert.ToInt16(reader["Quantity"]) : 0;
                item.bTaxable = reader["Taxable"] != DBNull.Value ? Convert.ToBoolean(reader["Taxable"]) : false;
                item.Note = reader["Note"] != DBNull.Value ? Convert.ToString(reader["Note"]) : string.Empty;

                items.Add(item);
            }

            connection.Close();

            return items;

        }

        [HttpPost]
        public HttpResponseMessage AddInvoiceItem(InvoiceItem item)
        {
            string commandString = @"INSERT INTO InvoiceItem (Name, UnitPrice, Quantity, Taxable, Note) 
                                     Values(@Name, @UnitPrice, @Quantity, @Taxable, @Note)";

            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = Constant.connectionString;

            SqlCommand command = new SqlCommand();
            command.CommandText = commandString;
            command.Connection = connection;

            command.Parameters.AddWithValue("@Name", item.Name);
            command.Parameters.AddWithValue("@UnitPrice", item.UnitPrice);
            command.Parameters.AddWithValue("@Quantity", item.Quantity);
            //command.Parameters.AddWithValue("@DiscountType", item.DiscountType);
            //command.Parameters.AddWithValue("@DiscountAmount", item.DiscountAmount);
            command.Parameters.AddWithValue("@Taxable", item.bTaxable);
            command.Parameters.AddWithValue("@Note", item.Note);

            connection.Open();

            int rowInserted = command.ExecuteNonQuery();

            commandString = @"  SELECT  TOP 1 Id 
                                FROM    InvoiceItem 
                                WHERE   Name = @name
                                ORDER BY Id desc";

            command.CommandText = commandString;
            command.Parameters.Clear();
            command.Parameters.AddWithValue("@name", item.Name);

            string itemId = command.ExecuteScalar().ToString();

            connection.Close();

            return Request.CreateResponse(HttpStatusCode.Created, itemId);
        }
    }
}
