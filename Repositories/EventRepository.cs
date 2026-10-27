
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using EventManagement.Models;

namespace EventManagement.Repositories
{
    public class EventRepository
    {
        DbConnection db = new DbConnection();

        // INSERT
        public void AddEvent(Event e)
        {
            using SqlConnection con = db.GetConnection();

            string query = @"INSERT INTO Events
                (EventID, EventName, EventDate, Venue, Capacity, RegistrationFee)
                VALUES (@id, @name, @date, @venue, @capacity, @fee)";

            using SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@id", e.eventID);
            cmd.Parameters.AddWithValue("@name", e.eventName);
            cmd.Parameters.AddWithValue("@date", e.eventDate);
            cmd.Parameters.AddWithValue("@venue", e.venue);
            cmd.Parameters.AddWithValue("@capacity", e.capacity);
            cmd.Parameters.AddWithValue("@fee", e.registrationFee);

            con.Open();
            cmd.ExecuteNonQuery();
        }

        // SELECT ALL
        public List<Event> GetAllEvents()
        {
            List<Event> events = new List<Event>();

            using SqlConnection con = db.GetConnection();

            string query = "SELECT * FROM Events";
            using SqlCommand cmd = new SqlCommand(query, con);

            con.Open();
            using SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Event e = new Event(
                    reader["EventID"].ToString()!,
                    reader["EventName"].ToString()!,
                    reader["Venue"].ToString()!,
                    Convert.ToDateTime(reader["EventDate"]),
                    Convert.ToInt32(reader["Capacity"]),
                    Convert.ToDecimal(reader["RegistrationFee"])
                );

                events.Add(e);
            }

            return events;
        }

        // UPDATE
        public int UpdateEvent(Event e)
        {
            using SqlConnection con = db.GetConnection();

            string query = @"UPDATE Events
                SET Venue = @venue,
                    EventDate = @date,
                    Capacity = @capacity,
                    RegistrationFee = @fee
                WHERE EventID = @id";

            using SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@id", e.eventID);
            cmd.Parameters.AddWithValue("@venue", e.venue);
            cmd.Parameters.AddWithValue("@date", e.eventDate);
            cmd.Parameters.AddWithValue("@capacity", e.capacity);
            cmd.Parameters.AddWithValue("@fee", e.registrationFee);

            con.Open();
            return cmd.ExecuteNonQuery();
        }

        // DELETE
        public int DeleteEvent(string id)
        {
            using SqlConnection con = db.GetConnection();

            string query = "DELETE FROM Events WHERE EventID = @id";

            using SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@id", id);

            con.Open();
            return cmd.ExecuteNonQuery();
        }
        // SEARCH EVENT
        public List<Event> SearchEvent(string keyword)
        {
            List<Event> events = new List<Event>();

            using SqlConnection con = db.GetConnection();

            string query = @"SELECT * FROM Events
                     WHERE EventName LIKE @keyword
                     OR Venue LIKE @keyword";

            using SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@keyword", "%" + keyword + "%");

            con.Open();

            using SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Event e = new Event(
                    reader["EventID"].ToString()!,
                    reader["EventName"].ToString()!,
                    reader["Venue"].ToString()!,
                    Convert.ToDateTime(reader["EventDate"]),
                    Convert.ToInt32(reader["Capacity"]),
                    Convert.ToDecimal(reader["RegistrationFee"])
                );

                events.Add(e);
            }

            return events;
        }
    }
}
