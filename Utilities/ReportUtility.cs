using System;
using System.Collections.Generic;
using System.Linq;
using EventManagement.Models;
using EventManagement.Services;

namespace EventManagement.Utilities
{
    public class ReportUtility
    {
        private EventService eventService;
        private ParticipantService participantService;
        private RegistrationService registrationService;

        public ReportUtility(
            EventService eventService,
            ParticipantService participantService,
            RegistrationService registrationService)
        {
            this.eventService = eventService;
            this.participantService = participantService;
            this.registrationService = registrationService;
        }
        public void DisplayEventsByParticipant(string participantId)
        {
            Participant? participant =
                participantService.GetParticipantById(participantId);

            if (participant == null)
            {
                Console.WriteLine("Participant not found.");
                return;
            }

            List<Registration> participantRegistrations =
                registrationService.GetAllRegistrations()
                .Where(r =>
                    r.ParticipantID.Equals(
                        participantId, StringComparison.OrdinalIgnoreCase) &&
                    r.Status != RegistrationStatus.Cancelled)
                .ToList();

            Console.WriteLine("\n========== EVENTS BY PARTICIPANT ==========");
            Console.WriteLine("Participant ID: " + participant.participantID);
            Console.WriteLine("Participant Name: " + participant.fullName);

            if (participantRegistrations.Count == 0)
            {
                Console.WriteLine("This participant has no active event registrations.");
                return;
            }

            foreach (Registration registration in participantRegistrations)
            {
                Event? eventObj =
                    eventService.GetEventById(registration.EventID);

                if (eventObj != null)
                {
                    Console.WriteLine("-------------------------------------------");
                    Console.WriteLine("Event ID: " + eventObj.eventID);
                    Console.WriteLine("Event Name: " + eventObj.eventName);
                    Console.WriteLine("Venue: " + eventObj.venue);
                    Console.WriteLine(
                        "Event Date: " + eventObj.eventDate.ToString("dd-MM-yyyy"));
                    Console.WriteLine(
                        "Registration Status: " + registration.Status);
                    Console.WriteLine(
                        "Amount Paid: " + registration.AmountPaid.ToString("F2"));
                }
            }

            Console.WriteLine("===========================================");
        }
        public void DisplayRemainingCapacity()
        {
            List<Event> allEvents = eventService.GetAllEvents();

            if (allEvents.Count == 0)
            {
                Console.WriteLine("No events available.");
                return;
            }

            Console.WriteLine("\n========== REMAINING EVENT CAPACITY ==========");

            foreach (Event eventObj in allEvents)
            {
                int activeRegistrations = registrationService
                    .GetAllRegistrations()
                    .Count(r =>
                        r.EventID.Equals(
                            eventObj.eventID,
                            StringComparison.OrdinalIgnoreCase) &&
                        r.Status != RegistrationStatus.Cancelled);

                int remainingCapacity = eventObj.capacity - activeRegistrations;

                Console.WriteLine("----------------------------------------------");
                Console.WriteLine("Event ID: " + eventObj.eventID);
                Console.WriteLine("Event Name: " + eventObj.eventName);
                Console.WriteLine("Total Capacity: " + eventObj.capacity);
                Console.WriteLine("Active Registrations: " + activeRegistrations);
                Console.WriteLine("Remaining Capacity: " + remainingCapacity);
            }

            Console.WriteLine("==============================================");
        }
        public void DisplayFullEvents()
        {
            List<Event> allEvents = eventService.GetAllEvents();

            if (allEvents.Count == 0)
            {
                Console.WriteLine("No events available.");
                return;
            }

            bool fullEventFound = false;

            Console.WriteLine("\n========== FULL EVENTS ==========");

            foreach (Event eventObj in allEvents)
            {
                int activeRegistrations = registrationService
                    .GetAllRegistrations()
                    .Count(r =>
                        r.EventID.Equals(
                            eventObj.eventID,
                            StringComparison.OrdinalIgnoreCase) &&
                        r.Status != RegistrationStatus.Cancelled);

                if (activeRegistrations >= eventObj.capacity)
                {
                    fullEventFound = true;

                    Console.WriteLine("---------------------------------");
                    Console.WriteLine("Event ID: " + eventObj.eventID);
                    Console.WriteLine("Event Name: " + eventObj.eventName);
                    Console.WriteLine("Venue: " + eventObj.venue);
                    Console.WriteLine("Total Capacity: " + eventObj.capacity);
                    Console.WriteLine("Active Registrations: " + activeRegistrations);
                    Console.WriteLine("Status: FULL");
                }
            }

            if (!fullEventFound)
            {
                Console.WriteLine("No full events found.");
            }

            Console.WriteLine("=================================");
        }
        public void DisplayFreeEvents()
        {
            // Generic list
            List<Event> allEvents = eventService.GetAllEvents();

            // Delegate method
            Predicate<Event> isFreeEvent = e => e.registrationFee == 0;

            // Generic method
            List<Event> freeEvents = new List<Event>();

            
            foreach (Event eventObj in allEvents)
            {
                if (isFreeEvent(eventObj))
                {
                    freeEvents.Add(eventObj);
                }
            }

            
            Console.WriteLine("\n========== FREE EVENTS ==========");

            if (freeEvents.Count == 0)
            {
                Console.WriteLine("No free events found.");
                return;
            }

            foreach (Event eventObj in freeEvents)
            {
                Console.WriteLine("Event ID: " + eventObj.eventID);
                Console.WriteLine("Event Name: " + eventObj.eventName);
                Console.WriteLine("Registration Fee: " + eventObj.registrationFee);
                Console.WriteLine("---------------------------------");
            }
        }
    }
}
