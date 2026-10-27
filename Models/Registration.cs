using System;

namespace EventManagement.Models
{
    public class Registration
    {
        public string RegistrationID { get; set; }
        public string EventID { get; set; }
        public string ParticipantID { get; set; }
        public DateTime RegistrationDate { get; set; }
        public decimal AmountPaid { get; set; }
        public RegistrationStatus Status { get; set; }

        public Registration(
            string registrationId,
            string eventId,
            string participantId,
            DateTime registrationDate,
            decimal amountPaid)
        {
            RegistrationID = registrationId;
            EventID = eventId;
            ParticipantID = participantId;
            RegistrationDate = registrationDate;
            AmountPaid = amountPaid;
            Status = RegistrationStatus.Registered;
        }
    }
}