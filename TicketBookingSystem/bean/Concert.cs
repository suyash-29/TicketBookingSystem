using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;

namespace TicketBookingSystem.bean
{
    public class Concert : Event
    {
        public string Artist { get; set; }
        public string Type { get; set; } // Theatrical, Classical, Rock, Recital

        public Concert() : base() { }

        public Concert(int eventId, string eventName, DateTime eventDate, TimeSpan eventTime, Venue venue, int totalSeats, decimal ticketPrice, string artist, string type)
            : base(eventId, eventName, eventDate, eventTime, venue, totalSeats, ticketPrice, "Concert")
        {
            Artist = artist;
            Type = type;
           
        }

        public override void DisplayEventDetails()
        {
            Console.WriteLine("----- Concert Details -----");
            Console.WriteLine($"ID: {EventId}");
            Console.WriteLine($"Name: {EventName}");
            Console.WriteLine($"Date: {EventDate.ToShortDateString()}");
            Console.WriteLine($"Time: {EventTime}");
            Console.WriteLine($"Venue: {Venue.VenueName}, Address: {Venue.Address}");
            Console.WriteLine($"Total Seats: {TotalSeats}");
            Console.WriteLine($"Available Seats: {AvailableSeats}");
            Console.WriteLine($"Ticket Price: {TicketPrice:C}");
            Console.WriteLine($"Artist: {Artist}");
            Console.WriteLine($"Type: {Type}");
            Console.WriteLine($"Total Revenue: {CalculateTotalRevenue():C}");
            Console.WriteLine("----------------------------\n");
        }
    }
}

