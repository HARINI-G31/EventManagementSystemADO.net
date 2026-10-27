using System;
using System.Collections.Generic;
using System.Text;

namespace EventManagement.Models
{
    public class Participant
    {
        public string participantID { get; set; }
        public string fullName { get; set; }
        public string email { get; set; }
        public string phoneNumber { get; set; }

        public Participant(string ParticipantID, string FullName, string Email, string PhoneNumber)
        {
            participantID = ParticipantID;
            fullName = FullName;
            email = Email;
            phoneNumber = PhoneNumber;
        }
    }
}
