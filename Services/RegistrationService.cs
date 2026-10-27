using EventManagement.Models;
using System;
using System.Collections.Generic;
using System.Text;


namespace EventManagement.Services
{
    public  class RegistrationService
    {
        private List<Registration> registrations = new List<Registration>();
        private EventService eventService;
        private ParticipantService participantService;
        public RegistrationService(
        EventService eventService,
        ParticipantService participantService)
        {
            this.eventService = eventService;
            this.participantService = participantService;

        }
        public void RegisterParticipantForEvent(string registrationId, string eventId, string participantId)
        {
            var eventObj = eventService.GetEventById(eventId);
            var participantObj = participantService.GetParticipantById(participantId);
            if (eventObj == null)
            {
                Console.WriteLine("Event not found.");
                return;
            }
            if (participantObj == null)
            {
                Console.WriteLine("Participant not found.");
                return;
            }
            if (registrations.Exists(r => r.EventID == eventId && r.ParticipantID == participantId))
            {
                Console.WriteLine("Participant is already registered for this event.");
                return;
            }
            if (registrations.Count(r => r.EventID == eventId) >= eventObj.capacity)
            {
                Console.WriteLine("Event is full. Cannot register more participants.");
                return;
            }
            Registration registration = new Registration(
                registrationId,
                eventObj.eventID,
                participantObj.participantID,
                DateTime.Now,
                eventObj.registrationFee
                );

            registrations.Add(registration);

            Console.WriteLine("Participant registered successfully.");
        }
        public void CancelRegistration(string registrationId)
        {
            Registration? registration = registrations.FirstOrDefault(
                r => r.RegistrationID.Equals(
                    registrationId,
                    StringComparison.OrdinalIgnoreCase));

            if (registration == null)
            {
                Console.WriteLine("Registration not found.");
                return;
            }

            if (registration.Status == RegistrationStatus.Cancelled)
            {
                Console.WriteLine("Registration is already cancelled.");
                return;
            }

            if (registration.Status == RegistrationStatus.Attended)
            {
                Console.WriteLine("Cannot cancel a registration after attendance.");
                return;
            }

            registration.Status = RegistrationStatus.Cancelled;

            Console.WriteLine("Registration cancelled successfully.");
        }
        public void MarkAttendance(string registrationId)
        {
            Registration? registration = registrations.FirstOrDefault(
                r => r.RegistrationID.Equals(
                    registrationId,
                    StringComparison.OrdinalIgnoreCase));

            if (registration == null)
            {
                Console.WriteLine("Registration not found.");
                return;
            }

            if (registration.Status == RegistrationStatus.Cancelled)
            {
                Console.WriteLine("Cannot mark attendance for a cancelled registration.");
                return;
            }

            if (registration.Status == RegistrationStatus.Attended)
            {
                Console.WriteLine("Attendance is already marked.");
                return;
            }

            registration.Status = RegistrationStatus.Attended;

            Console.WriteLine("Attendance marked successfully.");
        }
        public void DisplayAllRegistrations()
        {
            if (registrations.Count == 0)
            {
                Console.WriteLine("No registrations available.");
                return;
            }

            Console.WriteLine("\n========== ALL REGISTRATIONS ==========");

            foreach (Registration registration in registrations)
            {
                Console.WriteLine("Registration ID: " + registration.RegistrationID);
                Console.WriteLine("Event ID: " + registration.EventID);
                Console.WriteLine("Participant ID: " + registration.ParticipantID);
                Console.WriteLine("Registration Date: " + registration.RegistrationDate);
                Console.WriteLine("Status: " + registration.Status);
                Console.WriteLine("Amount Paid: " + registration.AmountPaid.ToString("F2"));

                Console.WriteLine("---------------------------------------");
            }
        }
        public Registration? GetRegistrationById(string registrationId)
        {
            return registrations.FirstOrDefault(
                r => r.RegistrationID.Equals(
                    registrationId,
                    StringComparison.OrdinalIgnoreCase));
        }
        public void DisplayRegistrationById(string registrationId)
        {
            Registration? registration = GetRegistrationById(registrationId);

            if (registration == null)
            {
                Console.WriteLine("Registration not found.");
                return;
            }

            Event? eventObj = eventService.GetEventById(registration.EventID);

            Participant? participantObj =
                participantService.GetParticipantById(registration.ParticipantID);

            Console.WriteLine("\n========== REGISTRATION DETAILS ==========");

            Console.WriteLine($"Registration ID: {registration.RegistrationID}");
            Console.WriteLine($"Registration Date: {registration.RegistrationDate:dd-MM-yyyy HH:mm}");
            Console.WriteLine($"Status: {registration.Status}");
            Console.WriteLine($"Amount Paid: {registration.AmountPaid:F2}");

            if (eventObj != null)
            {
                Console.WriteLine("\n--- Event Details ---");
                Console.WriteLine($"Event ID: {eventObj.eventID}");
                Console.WriteLine($"Event Name: {eventObj.eventName}");
                Console.WriteLine($"Venue: {eventObj.venue}");
                Console.WriteLine($"Event Date: {eventObj.eventDate:dd-MM-yyyy HH:mm}");
            }

            if (participantObj != null)
            {
                Console.WriteLine("\n--- Participant Details ---");
                Console.WriteLine($"Participant ID: {participantObj.participantID}");
                Console.WriteLine($"Name: {participantObj.fullName}");
                Console.WriteLine($"Email: {participantObj.email}");
                Console.WriteLine($"Phone: {participantObj.phoneNumber}");
            }
        }
        public List<Registration> GetAllRegistrations()
        {
            return registrations;
        }
        public void LoadRegistrations(List<Registration> loadedRegistrations)
        {
            registrations.Clear();
            registrations.AddRange(loadedRegistrations);
        }


    }
}
