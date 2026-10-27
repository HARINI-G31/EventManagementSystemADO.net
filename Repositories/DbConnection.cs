using Microsoft.Data.SqlClient;

namespace EventManagement.Repositories
{
    public class DbConnection
    {
        private readonly string connectionString =
            @"Server=.\SQLEXPRESS;Database=EventManagementDB;Trusted_Connection=True;TrustServerCertificate=True;";

        public SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
        public void TestConnection()
        {
            try
            {
                using SqlConnection con = GetConnection();

                Console.WriteLine("Connecting to SQL Server...");
                con.Open();

                Console.WriteLine("SQL Server connected successfully!");
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Connection failed:");
                Console.WriteLine(ex.Message);
            }
        }
    }
}