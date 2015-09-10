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
        public static object _locker = new object();

        public static string _APIKey = "fsdfsfs";
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.GetEncoding("Cyrillic");
            var stw = new Stopwatch();
            stw.Start();
            var _connectionString = ConfigurationManager.ConnectionStrings["connectToTTN"].ConnectionString;
            stw.Stop();
            SqlConnection connectionWarehouses = new SqlConnection(_connectionString);
            
                connectionWarehouses.Open();

                Parallel.For(0, 20, (i) =>
                {



                    using (
                        SqlCommand cmd2 =
                            new SqlCommand(
                                "INSERT INTO [Warehouses] (Description, DescriptionRu, Phone, TypeOfWarehouse," +
                                " Ref, Number, CityRef, Longitude, Latitude) VALUES (@Description, @DescriptionRu, @Phone, @TypeOfWarehouse," +
                                " @Ref, @Number, @CityRef, @Longitude, @Latitude)", connectionWarehouses))
                    {

                        int x;
                        lock (_locker)
                        {
                            Thread.Sleep(100);
                            x = new Random().Next(1000000000);
                        }
                        Console.WriteLine(x + " Thread:" + Thread.CurrentThread.ManagedThreadId);

                        cmd2.Parameters.AddWithValue("@Description", 1);
                        cmd2.Parameters.AddWithValue("@DescriptionRu", 2);
                        cmd2.Parameters.AddWithValue("@Phone", 3);
                        cmd2.Parameters.AddWithValue("@TypeOfWarehouse", 4);
                        cmd2.Parameters.AddWithValue("@Ref", x);
                        cmd2.Parameters.AddWithValue("@Number", 6);
                        cmd2.Parameters.AddWithValue("@CityRef", 7);
                        cmd2.Parameters.AddWithValue("@Longitude", 8);
                        cmd2.Parameters.AddWithValue("@Latitude", 9);
                            cmd2.ExecuteNonQuery();

                        cmd2.CommandText =
                            "INSERT INTO [Reception] (Ref, Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday) " +
                            "VALUES (@Ref, @Monday, @Tuesday, @Wednesday, @Thursday, @Friday, @Saturday, @Sunday)";
                        cmd2.Parameters.AddWithValue("@Monday", 1);
                        cmd2.Parameters.AddWithValue("@Tuesday", 2);
                        cmd2.Parameters.AddWithValue("@Wednesday", 3);
                        cmd2.Parameters.AddWithValue("@Thursday", 4);
                        cmd2.Parameters.AddWithValue("@Friday", 5);
                        cmd2.Parameters.AddWithValue("@Saturday", 6);
                        cmd2.Parameters.AddWithValue("@Sunday", 7);
                            cmd2.ExecuteNonQuery();
                        cmd2.CommandText =
                            "INSERT INTO [Delivery] (Ref, Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday) " +
                            "VALUES (@Ref, @Monday, @Tuesday, @Wednesday, @Thursday, @Friday, @Saturday, @Sunday)";
                        cmd2.Parameters["@Monday"].Value = 1;
                        cmd2.Parameters["@Tuesday"].Value = 2;
                        cmd2.Parameters["@Wednesday"].Value = 3;
                        cmd2.Parameters["@Thursday"].Value = 4;
                        cmd2.Parameters["@Friday"].Value = 5;
                        cmd2.Parameters["@Saturday"].Value = 6;
                        cmd2.Parameters["@Sunday"].Value = 7;
                            cmd2.ExecuteNonQuery();
                            
                        cmd2.CommandText =
                            "INSERT INTO [Schedule] (Ref, Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday) " +
                            "VALUES (@Ref, @Monday, @Tuesday, @Wednesday, @Thursday, @Friday, @Saturday, @Sunday)";
                        cmd2.Parameters["@Monday"].Value = 1;
                        cmd2.Parameters["@Tuesday"].Value = 1;
                        cmd2.Parameters["@Wednesday"].Value = 1;
                        cmd2.Parameters["@Thursday"].Value = 1;
                        cmd2.Parameters["@Friday"].Value = 1;
                        cmd2.Parameters["@Saturday"].Value = 1;
                        cmd2.Parameters["@Sunday"].Value = 1;
                            cmd2.ExecuteNonQuery();
                        Console.WriteLine(x + "writed!");

                    }



                }
                    );
                Console.WriteLine("SOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOQQQQAAAAAAAAAAAAAAAAAa");
               
            
            

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
