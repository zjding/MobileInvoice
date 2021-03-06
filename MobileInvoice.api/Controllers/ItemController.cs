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
    public class ItemController : ApiController
    {
        private string m_connectionString = @"Server=tcp:webapitry120161228015023.database.windows.net,1433;Initial Catalog=WebApiTry120161228015023;Persist Security Info=False;User ID=zjding;Password=G4indigo;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        [Route("api/Item/GetItems")]
        public List<Item> Get()
        {
            List<Item> items = new List<Item>();

            string commandString = @"Select * from Item";

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
                Item item = new Item();
                item.Id = Convert.ToInt32(reader["Id"]);
                item.Name = reader["Name"] != DBNull.Value ? Convert.ToString(reader["Name"]) : string.Empty;
                item.UnitPrice = reader["UnitPrice"] != DBNull.Value ? Convert.ToDecimal(reader["UnitPrice"]) : 0;
                
                items.Add(item);
            }

            connection.Close();

            return items;

        }

        [HttpPost]
        public HttpResponseMessage AddItem(Item item)
        {
            string commandString = @"INSERT INTO Item (Name, UnitPrice) 
                                     Values(@Name, @UnitPrice)";

            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = Constant.connectionString;

            SqlCommand command = new SqlCommand();
            command.CommandText = commandString;
            command.Connection = connection;

            command.Parameters.AddWithValue("@Name", item.Name);
            command.Parameters.AddWithValue("@UnitPrice", item.UnitPrice); 

            connection.Open();

            int rowInserted = command.ExecuteNonQuery();

            connection.Close();

            return Request.CreateResponse(HttpStatusCode.Created, "Added item successfully");
        }
    }
}
