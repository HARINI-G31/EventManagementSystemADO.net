using System;
using Microsoft.Data.SqlClient;
using EventManagement.Models;

namespace EventManagement.Repositories
{
    public class AuditLogRepository
    {
        private readonly string connectionString =
            @"Server=.\SQLEXPRESS;Database=EventManagementDB;Integrated Security=True;TrustServerCertificate=True;";

        public void AddAuditLog(AuditLog log)
        {
            using SqlConnection connection =
                new SqlConnection(connectionString);

            connection.Open();

            string query = @"
                INSERT INTO AuditLogs
                (Action, EntityName, RecordId, Description)
                VALUES
                (@Action, @EntityName, @RecordId, @Description)";

            using SqlCommand cmd =
                new SqlCommand(query, connection);

            cmd.Parameters.AddWithValue("@Action", log.Action);
            cmd.Parameters.AddWithValue("@EntityName", log.EntityName);
            cmd.Parameters.AddWithValue("@RecordId", log.RecordId);
            cmd.Parameters.AddWithValue("@Description", log.Description);

            cmd.ExecuteNonQuery();
        }
    }
}