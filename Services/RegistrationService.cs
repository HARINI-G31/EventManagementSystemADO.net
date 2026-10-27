
using System;
using System.Collections.Generic;
using System.Linq;
using EventManagement.Models;
using EventManagement.Repositories;

namespace EventManagement.Services
{
    public class RegistrationService
    {
        private readonly RegistrationRepository registrationRepository =
            new RegistrationRepository();

        private readonly EventService eventService;
        private readonly ParticipantService participantService;

        public RegistrationService(
            EventService eventService,
            ParticipantService participantService)
        {
            this.eventService = eventService;
            this.participantService = participantService;
        }

        // REGISTER PARTICIPANT FOR EVENT
        public void RegisterParticipantForEvent(
            string registrationId,
            string eventId,
            string participantId)
        {
            Event? eventObj = eventService.GetEventById(eventId);
            Participant? participantObj =
                participantService.GetParticipantById(participantId);

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

            List<Registration> registrations =
                registrationRepository.GetAllRegistrations();

            if (registrations.Any(r =>
                r.RegistrationID.Equals(
                    registrationId, StringComparison.OrdinalIgnoreCase)))
            {
                Console.WriteLine("Registration ID already exists.");
                return;
            }

            if (registrations.Any(r =>
                r.EventID == eventId &&
                r.ParticipantID == participantId &&
                r.Status != RegistrationStatus.Cancelled))
            {
                Console.WriteLine(
                    "Participant is already registered for this event.");
                return;
            }

            int occupiedSeats = registrations.Count(r =>
                r.EventID == eventId &&
                r.Status != RegistrationStatus.Cancelled);

            if (occupiedSeats >= eventObj.capacity)
            {
                Console.WriteLine("Event is full.");
                return;
            }

            Registration registration = new Registration(
                registrationId,
                eventObj.eventID,
                participantObj.participantID,
                DateTime.Now,
                eventObj.registrationFee
            );

            registrationRepository.AddRegistration(registration);

            Console.WriteLine("Participant registered successfully!");
        }

        // CANCEL REGISTRATION
        public void CancelRegistration(string registrationId)
        {
            Registration? registration =
                GetRegistrationById(registrationId);

            if (registration == null)
            {
                Console.WriteLine("Registration not found.");
                return;
            }

            if (registration.Status == RegistrationStatus.Cancelled)
            {
                Console.WriteLine("Registration already cancelled.");
                return;
            }

            if (registration.Status == RegistrationStatus.Attended)
            {
                Console.WriteLine(
                    "Cannot cancel after attendance.");
                return;
            }

            int rows = registrationRepository.UpdateStatus(
                registrationId, RegistrationStatus.Cancelled);

            if (rows > 0)
                Console.WriteLine("Registration cancelled successfully!");
        }

        // MARK ATTENDANCE
        public void MarkAttendance(string registrationId)
        {
            Registration? registration =
                GetRegistrationById(registrationId);

            if (registration == null)
            {
                Console.WriteLine("Registration not found.");
                return;
            }

            if (registration.Status == RegistrationStatus.Cancelled)
            {
                Console.WriteLine(
                    "Cannot mark attendance for cancelled registration.");
                return;
            }

            if (registration.Status == RegistrationStatus.Attended)
            {
                Console.WriteLine("Attendance already marked.");
                return;
            }

            int rows = registrationRepository.UpdateStatus(
                registrationId, RegistrationStatus.Attended);

            if (rows > 0)
                Console.WriteLine("Attendance marked successfully!");
        }

        // DISPLAY ALL
        public void DisplayAllRegistrations()
        {
            List<Registration> registrations =
                registrationRepository.GetAllRegistrations();

            if (registrations.Count == 0)
            {
                Console.WriteLine("No registrations available.");
                return;
            }

            foreach (Registration r in registrations)
            {
                Console.WriteLine(
                    $"{r.RegistrationID} | {r.EventID} | " +
                    $"{r.ParticipantID} | {r.Status} | {r.AmountPaid}");
            }
        }

        // GET BY ID
        public Registration? GetRegistrationById(string registrationId)
        {
            return registrationRepository.GetRegistrationById(registrationId);
        }

        // DISPLAY REGISTRATION DETAILS
        public void DisplayRegistrationById(string registrationId)
        {
            Registration? r = GetRegistrationById(registrationId);

            if (r == null)
            {
                Console.WriteLine("Registration not found.");
                return;
            }

            Event? e = eventService.GetEventById(r.EventID);
            Participant? p =
                participantService.GetParticipantById(r.ParticipantID);

            Console.WriteLine("\nREGISTRATION DETAILS");
            Console.WriteLine($"Registration ID: {r.RegistrationID}");
            Console.WriteLine($"Event: {e?.eventName}");
            Console.WriteLine($"Participant: {p?.fullName}");
            Console.WriteLine($"Date: {r.RegistrationDate}");
            Console.WriteLine($"Amount: {r.AmountPaid}");
            Console.WriteLine($"Status: {r.Status}");
        }

        // DELETE
        public void DeleteRegistration(string registrationId)
        {
            int rows = registrationRepository.DeleteRegistration(
                registrationId);

            Console.WriteLine(rows > 0
                ? "Registration deleted successfully!"
                : "Registration not found!");
        }

        // GET ALL FOR REPORTS
        public List<Registration> GetAllRegistrations()
        {
            return registrationRepository.GetAllRegistrations();
        }

        // OLD JSON METHOD RETAINED FOR COMPATIBILITY
        public void LoadRegistrations(List<Registration> loadedRegistrations)
        {
            // Registration data now comes from SQL Server.
        }
    }
}
