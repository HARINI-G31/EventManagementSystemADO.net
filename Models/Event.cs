using System;
using System.Collections.Generic;
using System.Text;

namespace EventManagement.Models
{
    public class Event
    {
        public string eventName { get; set; } = string.Empty;
        public string eventID { get; set; } = string.Empty;
        public DateTime eventDate { get; set; } 
        public string venue { get; set; } = string.Empty;
        public int capacity { get; set; } 
        public decimal registrationFee { get; set; }

        public Event(
            string EventId,
            string EventName,
            string Venue,
            DateTime EventDate,
            int Capacity,
            decimal RegistrationFee)
        {
            eventID = EventId;
            eventName = EventName;
            venue = Venue;
            eventDate = EventDate;
            capacity = Capacity;
            registrationFee = RegistrationFee;
        }
    }
}
