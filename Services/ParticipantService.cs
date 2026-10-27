
using System;
using System.Collections.Generic;
using System.Linq;
using EventManagement.Models;
using EventManagement.Repositories;

namespace EventManagement.Services
{
    public class ParticipantService
    {
        private readonly ParticipantRepository participantRepository =
            new ParticipantRepository();

        // CREATE
        public bool RegisterParticipant(Participant participant)
        {
            if (participant == null ||
                string.IsNullOrWhiteSpace(participant.participantID) ||
                string.IsNullOrWhiteSpace(participant.fullName) ||
                string.IsNullOrWhiteSpace(participant.email) ||
                string.IsNullOrWhiteSpace(participant.phoneNumber))
            {
                Console.WriteLine("Invalid participant details.");
                return false;
            }

            List<Participant> participants =
                participantRepository.GetAllParticipants();

            if (participants.Any(p =>
                p.participantID.Equals(participant.participantID,
                    StringComparison.OrdinalIgnoreCase)))
            {
                Console.WriteLine("Participant ID already exists.");
                return false;
            }

            if (participants.Any(p =>
                p.email.Equals(participant.email,
                    StringComparison.OrdinalIgnoreCase)))
            {
                Console.WriteLine("Email already exists.");
                return false;
            }

            if (participants.Any(p =>
                p.phoneNumber == participant.phoneNumber))
            {
                Console.WriteLine("Phone number already exists.");
                return false;
            }

            participantRepository.AddParticipant(participant);

            Console.WriteLine("Participant registered successfully!");
            return true;
        }

        // DISPLAY
        public void DisplayParticipants()
        {
            List<Participant> participants =
                participantRepository.GetAllParticipants();

            if (participants.Count == 0)
            {
                Console.WriteLine("No participants available.");
                return;
            }

            Console.WriteLine("Available Participants:");

            foreach (Participant p in participants)
            {
                Console.WriteLine(
                    $"ID: {p.participantID}, " +
                    $"Name: {p.fullName}, " +
                    $"Email: {p.email}, " +
                    $"Phone: {p.phoneNumber}");
            }
        }

        // GET BY ID
        public Participant? GetParticipantById(string participantId)
        {
            if (string.IsNullOrWhiteSpace(participantId))
                return null;

            return participantRepository.GetAllParticipants()
                .FirstOrDefault(p =>
                    p.participantID.Equals(
                        participantId.Trim(),
                        StringComparison.OrdinalIgnoreCase));
        }

        // SEARCH
        public void SearchParticipantByNameorEmail(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                Console.WriteLine("Search text cannot be empty.");
                return;
            }

            List<Participant> participants =
                participantRepository.SearchParticipant(keyword.Trim());

            if (participants.Count == 0)
            {
                Console.WriteLine("No matching participants found.");
                return;
            }

            Console.WriteLine("\n========== SEARCH RESULTS ==========");

            foreach (Participant p in participants)
            {
                Console.WriteLine(
                    $"Name: {p.fullName}, " +
                    $"Email: {p.email}, " +
                    $"Phone: {p.phoneNumber}");
            }
        }

        // UPDATE
        public void UpdateParticipant(
            string fullName,
            string email,
            string phoneNumber,
            string participantId)
        {
            Participant? existingParticipant =
                GetParticipantById(participantId);

            if (existingParticipant == null)
            {
                Console.WriteLine("Participant not found.");
                return;
            }

            if (!string.IsNullOrWhiteSpace(fullName))
                existingParticipant.fullName = fullName;

            if (!string.IsNullOrWhiteSpace(email))
                existingParticipant.email = email;

            if (!string.IsNullOrWhiteSpace(phoneNumber))
                existingParticipant.phoneNumber = phoneNumber;

            int rows = participantRepository.UpdateParticipant(
                existingParticipant);

            Console.WriteLine(rows > 0
                ? "Participant updated successfully!"
                : "Participant not found!");
        }

        // DELETE

        public void DeleteParticipant(string participantId)
        {
            if (string.IsNullOrWhiteSpace(participantId))
            {
                Console.WriteLine("Participant ID cannot be empty.");
                return;
            }

            int result = participantRepository.DeleteParticipant(
                participantId.Trim());

            if (result == -1)
            {
                Console.WriteLine(
                    "Cannot delete participant. " +
                    "Registered or attended registrations exist.");
            }
            else if (result == 0)
            {
                Console.WriteLine("Participant not found.");
            }
            else
            {
                Console.WriteLine(
                    "Participant and cancelled registrations deleted successfully!");
            }
        }


        // GET ALL
        public List<Participant> GetAllParticipants()
        {
            return participantRepository.GetAllParticipants();
        }

        // EXISTING METHOD RETAINED FOR COMPATIBILITY
        public void LoadParticipants(List<Participant> loadedParticipants)
        {
            // Participants are now loaded from SQL Server.
        }
    }
}
