using EventManagement.Services;
using EventManagement.Models;
using System.Security.Cryptography.X509Certificates;
using EventManagement.Utilities;
using System.IO;
using EventManagement.Repositories;


namespace EventManagement  
{
    public class Program
    {
        public static void Main(string[] args)
        {
            DbConnection db = new DbConnection();
            db.TestConnection();

            string dataFolder = Path.Combine(
            AppContext.BaseDirectory, "Data");

            string eventsFile = Path.Combine(dataFolder, "events.json");
            string participantsFile = Path.Combine(dataFolder, "participants.json");
            string registrationsFile = Path.Combine(dataFolder, "registrations.json");

            JsonFileUtility jsonUtility = new JsonFileUtility();

            EventService eventService = new EventService();

            ParticipantService participantService = new ParticipantService();

            RegistrationService registrationService =new RegistrationService(eventService, participantService);

            ReportUtility reportUtility = new ReportUtility(
                            eventService,
                            participantService,
                            registrationService);

            Console.WriteLine("Welcome to the Event Management System!\n");
            bool running = true;
            while(running)
            {
                
                
                Console.WriteLine("Choose an option:");
                Console.WriteLine("1. Create Event");
                Console.WriteLine("2. Display Events");
                Console.WriteLine("3. Search Event");
                Console.WriteLine("4. Get Event by ID");
                Console.WriteLine("5. Update Event");
                Console.WriteLine("6. Delete Event");
                Console.WriteLine("7. Register Participant");
                Console.WriteLine("8. Display Participants");
                Console.WriteLine("9. Search Participant");
                Console.WriteLine("10. Update Participant");
                Console.WriteLine("11. Register Participant For Event");
                Console.WriteLine("12. Delete Participant");
                Console.WriteLine("13. Cancel Registration");
                Console.WriteLine("14. Mark Attendance");
                Console.WriteLine("15. Display All Registrations");
                Console.WriteLine("16. Display Registration by ID");
                Console.WriteLine("17. Display Events by Participant");
                Console.WriteLine("18. Display Remaining Capacity");
                Console.WriteLine("19. Display Full Events");
                Console.WriteLine("20. Display Free Events");
                Console.WriteLine("21. Exit");

                int choice = Convert.ToInt32(Console.ReadLine());
                switch (choice)
                {
                    case 1:
                        Console.WriteLine("Enter Event ID:");
                        string eventId = Console.ReadLine();

                        Console.WriteLine("Enter Event Name:");
                        string eventName = Console.ReadLine();

                        Console.WriteLine("Enter Venue:");
                        string venue = Console.ReadLine();

                        Console.WriteLine("Enter Event Date (yyyy-MM-dd):");
                        DateTime eventDate = DateTime.Parse(Console.ReadLine());

                        Console.WriteLine("Enter Capacity:");
                        int capacity = Convert.ToInt32(Console.ReadLine());

                        Console.WriteLine("Enter Registration Fee:");
                        decimal registrationFee = Convert.ToDecimal(Console.ReadLine());

                        var newEvent = new Event(eventId, eventName, venue, eventDate, capacity, registrationFee);
                        eventService.CreateEvent(newEvent);

                        break;
                    case 2:
                        eventService.DisplayEvents();
                        break;
                    case 3:
                        Console.WriteLine("Enter keyword to search by name or venue:");
                        string keyword = Console.ReadLine();
                        eventService.SearchEventByNameorVenue(keyword);
                        break;

                    case 4:
                        Console.WriteLine("Enter Event ID to search:");
                        string searchId = Console.ReadLine();
                        Event? foundevent = eventService.GetEventById(searchId);
                        if (foundevent != null)
                        {
                            Console.WriteLine($"Event found: {foundevent.eventName} on {foundevent.eventDate} at {foundevent.venue}");
                        }
                        else
                        {
                            Console.WriteLine("Event not found.");
                        }
                        break;
                    case 5:
                        Console.WriteLine("Enter Event ID to update:");
                        string updateId = Console.ReadLine();

                        Console.WriteLine("Enter new Venue:");
                        string newVenue = Console.ReadLine();

                        Console.WriteLine("Enter new Event Date (yyyy-MM-dd):");
                        DateTime newEventDate = DateTime.Parse(Console.ReadLine());

                        Console.WriteLine("Enter new Capacity:");
                        int newCapacity = Convert.ToInt32(Console.ReadLine());

                        Console.WriteLine("Enter new Registration Fee:");
                        decimal newRegistrationFee = Convert.ToDecimal(Console.ReadLine());

                        eventService.UpdateEvent(newVenue, newEventDate, newCapacity, newRegistrationFee, updateId);
                        break;

                    case 6:
                        Console.WriteLine("Enter Event ID to delete:");
                        eventService.DeleteEvent(Console.ReadLine());
                        break;
                    case 7:
                        Console.WriteLine("Enter Participant ID:");
                        string participantId = Console.ReadLine();

                        Console.WriteLine("Enter Full Name:");
                        string fullName = Console.ReadLine();

                        Console.WriteLine("Enter Email:");
                        string email = Console.ReadLine();

                        Console.WriteLine("Enter PhoneNumber:");
                        string phoneNumber = Console.ReadLine();

                        var newParticipant = new Participant(participantId, fullName, email, phoneNumber);
                        participantService.RegisterParticipant(newParticipant);

                        break;
                    case 8:
                        participantService.DisplayParticipants();
                        break;
                    case 9:
                        Console.WriteLine("Enter name or email to search participant:");
                        string participantKeyword = Console.ReadLine();
                        participantService.SearchParticipantByNameorEmail(participantKeyword);
                        break;
                    case 10:
                        Console.WriteLine("Enter Participant ID to update:");
                        string updateParticipantId = Console.ReadLine();

                        Console.WriteLine("Enter new Full Name:");
                        string newFullName = Console.ReadLine();

                        Console.WriteLine("Enter new Email:");
                        string newEmail = Console.ReadLine();

                        Console.WriteLine("Enter new PhoneNumber:");
                        string newPhoneNumber = Console.ReadLine();

                        participantService.UpdateParticipant(newFullName, newEmail, newPhoneNumber, updateParticipantId);
                        break;
                    case 11:
                        Console.WriteLine("Enter Participant ID to register for an event:");
                        string regParticipantId = Console.ReadLine();
                        Console.WriteLine("Enter Event ID to register for:");
                        string regEventId = Console.ReadLine();
                        Console.WriteLine("Enter Registration ID:");
                        string regId = Console.ReadLine();
                        registrationService.RegisterParticipantForEvent(regId, regEventId, regParticipantId);
                        break;
                    case 12:
                        Console.WriteLine("Enter Participant ID to delete:");
                        string deleteParticipantId = Console.ReadLine();
                        participantService.DeleteParticipant(deleteParticipantId);
                        break;  

                    case 13:
                        Console.WriteLine("Enter Registration ID to cancel:");
                        string cancelRegId = Console.ReadLine();
                        registrationService.CancelRegistration(cancelRegId);
                        break;
                    case 14:
                        Console.WriteLine("Enter Registration ID to mark attendance:");
                        string markAttId = Console.ReadLine();
                        registrationService.MarkAttendance(markAttId);
                        break;
                    case 15:
                        registrationService.DisplayAllRegistrations();
                        break;
                    case 16:
                        Console.WriteLine("Enter Registration ID to display:");
                        string displayRegId = Console.ReadLine();
                        registrationService.DisplayRegistrationById(displayRegId);
                        break;
                    case 17:
                        Console.WriteLine("Enter Participant ID to display events:");
                        string reportParticipantId = Console.ReadLine();
                        reportUtility.DisplayEventsByParticipant(reportParticipantId);
                        break;
                    case 18:
                        reportUtility.DisplayRemainingCapacity();
                        break;  
                    case 19:
                        reportUtility.DisplayFullEvents();
                        break;  
                    case 20:
                        reportUtility.DisplayFreeEvents();
                        break;
                    case 21:
 
                        

                        running = false;

                        Console.WriteLine("All data saved successfully.");
                        Console.WriteLine("Exiting the system. Goodbye!");
                        break;

                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }

            }
        }
    }
}