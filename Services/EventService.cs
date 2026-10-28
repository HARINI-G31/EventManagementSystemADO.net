
using System;
using System.Collections.Generic;
using System.Linq;
using EventManagement.Models;
using EventManagement.Repositories;

namespace EventManagement.Services
{
    public class EventService
    {
        private readonly EventRepository eventRepository =
            new EventRepository();

        private readonly AuditLogService auditService =
            new AuditLogService();

        // CREATE
        public bool CreateEvent(Event newEvent)
        {
            if (newEvent == null)
            {
                Console.WriteLine("Event cannot be null.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(newEvent.eventID) ||
                string.IsNullOrWhiteSpace(newEvent.eventName) ||
                string.IsNullOrWhiteSpace(newEvent.venue))
            {
                Console.WriteLine("Event details cannot be empty.");
                return false;
            }

            if (GetEventById(newEvent.eventID) != null)
            {
                Console.WriteLine("Event ID already exists.");
                return false;
            }

            if (newEvent.capacity <= 0 ||
                newEvent.registrationFee < 0 ||
                newEvent.eventDate < DateTime.Now)
            {
                Console.WriteLine("Invalid event details.");
                return false;
            }

            eventRepository.AddEvent(newEvent);

            // AUDIT LOG - CREATE
            auditService.LogAction(
                "CREATE",
                "Event",
                newEvent.eventID,
                "New event created successfully"
            );

            Console.WriteLine("Event created successfully!");
            return true;
        }

        // DISPLAY
        public void DisplayEvents()
        {
            List<Event> events = eventRepository.GetAllEvents();

            if (events.Count == 0)
            {
                Console.WriteLine("No events available.");
                return;
            }

            foreach (Event e in events)
            {
                Console.WriteLine(
                    $"ID: {e.eventID}, Name: {e.eventName}, " +
                    $"Date: {e.eventDate:yyyy-MM-dd}, " +
                    $"Venue: {e.venue}, Capacity: {e.capacity}, " +
                    $"Fee: {e.registrationFee}");
            }
        }

        // GET BY ID
        public Event? GetEventById(string eventId)
        {
            if (string.IsNullOrWhiteSpace(eventId))
                return null;

            return eventRepository.GetAllEvents()
                .FirstOrDefault(e =>
                    e.eventID.Equals(
                        eventId.Trim(),
                        StringComparison.OrdinalIgnoreCase));
        }

        // SEARCH
        public void SearchEventByNameorVenue(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                Console.WriteLine("Search keyword cannot be empty.");
                return;
            }

            List<Event> events =
                eventRepository.SearchEvent(keyword.Trim());

            if (events.Count == 0)
            {
                Console.WriteLine("No matching events found.");
                return;
            }

            foreach (Event e in events)
            {
                Console.WriteLine(
                    $"{e.eventID} - {e.eventName} - {e.venue}");
            }
        }

        // UPDATE
        public void UpdateEvent(
            string venue,
            DateTime eventDate,
            int capacity,
            decimal registrationFee,
            string eventId)
        {
            Event? existingEvent = GetEventById(eventId);

            if (existingEvent == null)
            {
                Console.WriteLine("Event not found.");
                return;
            }

            if (string.IsNullOrWhiteSpace(venue) ||
                eventDate < DateTime.Now ||
                capacity <= 0 ||
                registrationFee < 0)
            {
                Console.WriteLine("Invalid event details.");
                return;
            }

            existingEvent.venue = venue;
            existingEvent.eventDate = eventDate;
            existingEvent.capacity = capacity;
            existingEvent.registrationFee = registrationFee;

            int rows =
                eventRepository.UpdateEvent(existingEvent);

            if (rows > 0)
            {
                // AUDIT LOG - UPDATE
                auditService.LogAction(
                    "UPDATE",
                    "Event",
                    eventId,
                    "Event details updated successfully"
                );

                Console.WriteLine("Event updated successfully!");
            }
            else
            {
                Console.WriteLine("Event not found!");
            }
        }

        // DELETE
        public void DeleteEvent(string eventId)
        {
            int rows =
                eventRepository.DeleteEvent(eventId);

            if (rows > 0)
            {
                // AUDIT LOG - DELETE
                auditService.LogAction(
                    "DELETE",
                    "Event",
                    eventId,
                    "Event deleted successfully"
                );

                Console.WriteLine("Event deleted successfully!");
            }
            else
            {
                Console.WriteLine("Event not found!");
            }
        }

        // GET ALL
        public List<Event> GetAllEvents()
        {
            return eventRepository.GetAllEvents();
        }

        // RETAINED FOR COMPATIBILITY
        public void LoadEvents(List<Event> loadedEvents)
        {
            // Events are now read directly from SQL Server.
        }
    }
}
