using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;

namespace TicketBookingSystem.bean
{
    public class Sports : Event
    {
        public string SportName { get; set; }
        public string TeamsName { get; set; } // e.g., "India vs Pakistan"

        public Sports() : base() { }

        public Sports(int eventId, string eventName, DateTime eventDate, TimeSpan eventTime, Venue venue, int totalSeats, decimal ticketPrice, string sportName, string teamsName)
            : base(eventId, eventName, eventDate, eventTime, venue, totalSeats, ticketPrice, "Sports")
        {
            SportName = sportName;
            TeamsName = teamsName;
            
        }

        public override void DisplayEventDetails()
        {
            Console.WriteLine("----- Sports Event Details -----");
            Console.WriteLine($"ID: {EventId}");
            Console.WriteLine($"Name: {EventName}");
            Console.WriteLine($"Date: {EventDate.ToShortDateString()}");
            Console.WriteLine($"Time: {EventTime}");
            Console.WriteLine($"Venue: {Venue.VenueName}, Address: {Venue.Address}");
            Console.WriteLine($"Total Seats: {TotalSeats}");
            Console.WriteLine($"Available Seats: {AvailableSeats}");
            Console.WriteLine($"Ticket Price: {TicketPrice:C}");
            Console.WriteLine($"Sport Name: {SportName}");
            Console.WriteLine($"Teams: {TeamsName}");
            Console.WriteLine($"Total Revenue: {CalculateTotalRevenue():C}");
            Console.WriteLine("--------------------------------\n");
        }
    }
}

