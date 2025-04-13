﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMBANK
{
        internal class DBUtility
        {
            const string connectionString = @"Server =JESY\SQLEXPRESS ; Database = HMBank ; Integrated Security =True ; MultipleActiveResultSets=true;";
            //private SqlConnection ConnectionObject { get; set; }
            public static SqlConnection GetConnection()
            {
                SqlConnection connectionObject = new SqlConnection(connectionString);
                try
                {
                    connectionObject.Open();
                    return connectionObject;
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error opening the Connection:{e.Message}");
                    return null;
                }
            }
            public static void CloseDbConnection(SqlConnection connectionObject)
            {
                if (connectionObject != null)
                {
                    try
                    {
                        if (connectionObject.State != ConnectionState.Open)
                        {
                            connectionObject.Close();
                            connectionObject.Dispose();
                            Console.WriteLine("connection closed.");
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Error closingconnection{e.Message}");
                    }
                }
                else
                {
                    Console.WriteLine("Connection is already null");
                }
            }
        }
    
}
