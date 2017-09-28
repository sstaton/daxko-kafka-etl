﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Odbc;

namespace RedshiftData
{
    namespace redshift.amazon.com.docsamples
    {
        class ConnectToClusterExample
        {
            public static void Main(string[] args)
            {

                DataSet ds = new DataSet();
                DataTable dt = new DataTable();

                // Server, e.g. "examplecluster.xyz.us-west-2.redshift.amazonaws.com"
                string server = "***provide server name part of connection string****";

                // Port, e.g. "5439"
                string port = "***provide port***";

                // MasterUserName, e.g. "masteruser".
                string masterUsername = "***provide master user name***";

                // MasterUserPassword, e.g. "mypassword".
                string masterUserPassword = "***provide master user password***";

                // DBName, e.g. "dev"
                string DBName = "***provide name of database***";

                string query = "select * from information_schema.tables;";

                try
                {
                    // Create the ODBC connection string.
                    //Redshift ODBC Driver - 64 bits
                    /*
                    string connString = "Driver={Amazon Redshift (x64)};" +
                        String.Format("Server={0};Database={1};" +
                        "UID={2};PWD={3};Port={4};SSL=true;Sslmode=Require",
                        server, DBName, masterUsername,
                        masterUserPassword, port);
                    */

                    //Redshift ODBC Driver - 32 bits
                    string connString = "Driver={Amazon Redshift (x86)};" +
                        String.Format("Server={0};Database={1};" +
                        "UID={2};PWD={3};Port={4};SSL=true;Sslmode=Require",
                        server, DBName, masterUsername,
                        masterUserPassword, port);

                    // Make a connection using the psqlODBC provider.
                    OdbcConnection conn = new OdbcConnection(connString);
                    conn.Open();

                    // Try a simple query.
                    string sql = query;
                    OdbcDataAdapter da = new OdbcDataAdapter(sql, conn);
                    da.Fill(ds);
                    dt = ds.Tables[0];
                    foreach (DataRow row in dt.Rows)
                    {
                        Console.WriteLine(row["table_catalog"] + ", " + row["table_name"]);
                    }

                    conn.Close();
                    Console.ReadKey();
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine(ex.Message);
                    Console.ReadKey();
                }

            }
        }
    }

    public class Class1
    {
    }
}
