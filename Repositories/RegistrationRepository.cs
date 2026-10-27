
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using EventManagement.Models;

namespace EventManagement.Repositories
{
    public class RegistrationRepository
    {
        DbConnection db = new DbConnection();

        // INSERT
        public void AddRegistration(Registration r)
        {
            using SqlConnection con = db.GetConnection();

            string query = @"INSERT INTO Registrations
                (RegistrationID, EventID, ParticipantID,
                 RegistrationDate, AmountPaid, Status)
                VALUES (@id, @eventId, @participantId,
                        @date, @amount, @status)";

            using SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@id", r.RegistrationID);
            cmd.Parameters.AddWithValue("@eventId", r.EventID);
            cmd.Parameters.AddWithValue("@participantId", r.ParticipantID);
            cmd.Parameters.AddWithValue("@date", r.RegistrationDate);
            cmd.Parameters.AddWithValue("@amount", r.AmountPaid);
            cmd.Parameters.AddWithValue("@status", r.Status.ToString());

            con.Open();
            cmd.ExecuteNonQuery();
        }

        // SELECT ALL
        public List<Registration> GetAllRegistrations()
        {
            List<Registration> registrations = new List<Registration>();

            using SqlConnection con = db.GetConnection();

            string query = "SELECT * FROM Registrations";
            using SqlCommand cmd = new SqlCommand(query, con);

            con.Open();
            using SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Registration r = new Registration(
                    reader["RegistrationID"].ToString()!,
                    reader["EventID"].ToString()!,
                    reader["ParticipantID"].ToString()!,
                    Convert.ToDateTime(reader["RegistrationDate"]),
                    Convert.ToDecimal(reader["AmountPaid"])
                );

                r.Status = Enum.Parse<RegistrationStatus>(
                    reader["Status"].ToString()!, true);

                registrations.Add(r);
            }

            return registrations;
        }

        // SELECT BY ID
        public Registration? GetRegistrationById(string id)
        {
            foreach (Registration r in GetAllRegistrations())
            {
                if (r.RegistrationID.Equals(
                    id, StringComparison.OrdinalIgnoreCase))
                {
                    return r;
                }
            }

            return null;
        }

        // UPDATE STATUS
        public int UpdateStatus(string id, RegistrationStatus status)
        {
            using SqlConnection con = db.GetConnection();

            string query = @"UPDATE Registrations
                             SET Status = @status
                             WHERE RegistrationID = @id";

            using SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@status", status.ToString());

            con.Open();
            return cmd.ExecuteNonQuery();
        }

        // DELETE
        public int DeleteRegistration(string id)
        {
            using SqlConnection con = db.GetConnection();

            string query = @"DELETE FROM Registrations
                             WHERE RegistrationID = @id";

            using SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@id", id);

            con.Open();
            return cmd.ExecuteNonQuery();
        }
    }
}
