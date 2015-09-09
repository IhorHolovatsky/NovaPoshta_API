using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using PostWatcher;

namespace API_NovaPoshta
{
    class Program
    {
        public static string _APIKey = "fsdfsfs";
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.GetEncoding("Cyrillic");
            var stw = new Stopwatch();
            stw.Start();
          var  _connectionString = ConfigurationManager.ConnectionStrings["connectToTTN"].ConnectionString;
            stw.Stop();

            

            //DateTime left = DateTime.Parse("01.01.2015");
            //DateTime right = DateTime.Today;

            //string connectionString = ConfigurationManager.ConnectionStrings["connectToTTN"].ConnectionString;

      
            //using (SqlConnection connection = new SqlConnection(connectionString))
            //using (
            //    SqlCommand cmd = new SqlCommand("SELECT * FROM [TTN] WHERE DateTime BETWEEN @left AND @right ",connection))
            //{
            //    connection.Open();
            //    cmd.Parameters.AddWithValue("@left", left);
            //    cmd.Parameters.AddWithValue("@right", right);
                
            //    using (var reader =  cmd.ExecuteReader())
            //    {
            //        if (reader.HasRows)
            //            while (reader.Read())
            //            {
            //                var DataItem = new DataItem();
            //                DataItem.IntDocNumber = reader.GetString(0);
            //                DataItem.DateTime = reader.GetDateTime(1);
            //                DataItem.CityRecipientDescription = reader.GetString(2);
            //                DataItem.RecipientDescription = reader.GetString(3);
            //                DataItem.RecipientAddress = reader.GetString(4);
            //                DataItem.RecipientContactPhone = reader.GetString(5);
            //                Console.WriteLine(reader.GetDouble(6));
            //                DataItem.Weight = reader.GetDouble(6);
            //                DataItem.Cost = reader.GetDouble(7);
            //                DataItem.CostOnSite = reader.GetDouble(8);
            //                DataItem.StateName = reader.GetString(9);
            //                DataItem.PrintedDescription = reader.GetString(10);
            //            }
            //    }
            //}
       
            Console.ReadLine();
        }


        private static void AddToDateBase(SqlConnection connection, DataItem item)
        {
            using (
                SqlCommand cmd =
                    new SqlCommand(
                        "INSERT INTO [TTN] (TTN, DateTime, CityRecipientDescription, RecipientDescription, " +
                        " RecipientAddressDescription, RecipientContactPhone, Weight, Cost, CostOnSite, StateName," +
                        " PrintedDescription, APIKey) VALUES (@TTN, @DateTime, @CityRecipientDescription, @RecipientDescription, " +
                        "@RecipientAddressDescription, @RecipientContactPhone, @Weight, @Cost, @CostOnSite, @StateName," +
                        "@PrintedDescription, @APIKey)", connection))
            {
                cmd.Parameters.AddWithValue("@TTN", item.IntDocNumber);
                cmd.Parameters.AddWithValue("@DateTime", item.DateTime);
                cmd.Parameters.AddWithValue("@CityRecipientDescription", item.CityRecipientDescription);
                cmd.Parameters.AddWithValue("@RecipientDescription", item.RecipientDescription);
                cmd.Parameters.AddWithValue("@RecipientAddressDescription", item.RecipientAddressDescription);
                cmd.Parameters.AddWithValue("@RecipientContactPhone", item.RecipientContactPhone);
                cmd.Parameters.AddWithValue("@Weight", item.Weight);
                cmd.Parameters.AddWithValue("@Cost", item.Cost);
                cmd.Parameters.AddWithValue("@CostOnSite", item.CostOnSite);
                cmd.Parameters.AddWithValue("@StateName", item.StateName);
                cmd.Parameters.AddWithValue("@PrintedDescription", item.PrintedDescription);
                cmd.Parameters.AddWithValue("@APIKey", _APIKey);
                cmd.ExecuteNonQuery();
            }
        }
        

    }

   
}
