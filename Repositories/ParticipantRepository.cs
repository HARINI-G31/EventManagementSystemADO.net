
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using EventManagement.Models;

namespace EventManagement.Repositories
{
    public class ParticipantRepository
    {
        DbConnection db = new DbConnection();

        // INSERT
        public void AddParticipant(Participant p)
        {
            using SqlConnection con = db.GetConnection();

            string query = @"INSERT INTO Participants
                (ParticipantID, FullName, Email, PhoneNumber)
                VALUES (@id, @name, @email, @phone)";

            using SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@id", p.participantID);
            cmd.Parameters.AddWithValue("@name", p.fullName);
            cmd.Parameters.AddWithValue("@email", p.email);
            cmd.Parameters.AddWithValue("@phone", p.phoneNumber);

            con.Open();
            cmd.ExecuteNonQuery();
        }

        // SELECT ALL
        public List<Participant> GetAllParticipants()
        {
            List<Participant> participants = new List<Participant>();

            using SqlConnection con = db.GetConnection();

            string query = "SELECT * FROM Participants";
            using SqlCommand cmd = new SqlCommand(query, con);

            con.Open();
            using SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Participant p = new Participant(
                    reader["ParticipantID"].ToString()!,
                    reader["FullName"].ToString()!,
                    reader["Email"].ToString()!,
                    reader["PhoneNumber"].ToString()!
                );

                participants.Add(p);
            }

            return participants;
        }

        // SEARCH BY NAME OR EMAIL
        // SEARCH PARTICIPANT
        public List<Participant> SearchParticipant(string keyword)
        {
            List<Participant> participants = new List<Participant>();

            using SqlConnection con = db.GetConnection();

            string query = @"SELECT ParticipantID, FullName, Email, PhoneNumber
                     FROM Participants
                     WHERE FullName LIKE @keyword
                        OR Email LIKE @keyword";

            using SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@keyword", "%" + keyword + "%");

            con.Open();

            using SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Participant p = new Participant(
                    reader["ParticipantID"].ToString()!,
                    reader["FullName"].ToString()!,
                    reader["Email"].ToString()!,
                    reader["PhoneNumber"].ToString()!
                );

                participants.Add(p);
            }

            return participants;
        }

        // UPDATE
        public int UpdateParticipant(Participant p)
        {
            using SqlConnection con = db.GetConnection();

            string query = @"UPDATE Participants
                SET FullName = @name,
                    Email = @email,
                    PhoneNumber = @phone
                WHERE ParticipantID = @id";

            using SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@id", p.participantID);
            cmd.Parameters.AddWithValue("@name", p.fullName);
            cmd.Parameters.AddWithValue("@email", p.email);
            cmd.Parameters.AddWithValue("@phone", p.phoneNumber);

            con.Open();
            return cmd.ExecuteNonQuery();
        }

        // DELETE
        public int DeleteParticipant(string id)
        {
            using SqlConnection con = db.GetConnection();

            string query = "DELETE FROM Participants WHERE ParticipantID = @id";

            using SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@id", id);

            con.Open();
            return cmd.ExecuteNonQuery();
        }
    }
}
