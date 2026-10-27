using System;
using Microsoft.Data.SqlClient;

namespace EventManagement.Repositories
{
    public class DbConnection
    {
        private readonly string connectionString =
            @"Data Source=.\SQLEXPRESS;
              Initial Catalog=EventManagementDB;
              Integrated Security=True;
              Persist Security Info=False;
              Pooling=False;
              MultipleActiveResultSets=False;
              Encrypt=True;
              TrustServerCertificate=True;
              Connect Timeout=30;";

        public SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }

        public bool TestConnection()
        {
            try
            {
                using SqlConnection con = GetConnection();

                Console.WriteLine("Connecting to SQL Server...");
                con.Open();

                Console.WriteLine("SQL Server connected successfully!");
                return true;
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Connection failed:");
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}