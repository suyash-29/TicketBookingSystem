using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;

namespace TicketBookingSystem.bean
{
    public class Movie : Event
    {
        public string Genre { get; set; }
        public string ActorName { get; set; }
        public string ActressName { get; set; }

        public Movie() : base() { }

        public Movie(int eventId, string eventName, DateTime eventDate, TimeSpan eventTime, Venue venue, int totalSeats, decimal ticketPrice, string genre, string actorName, string actressName)
            : base(eventId, eventName, eventDate, eventTime, venue, totalSeats, ticketPrice, "Movie")
        {
            Genre = genre;
            ActorName = actorName;
            ActressName = actressName;
            
        }

        public override void DisplayEventDetails()
        {
            Console.WriteLine("----- Movie Details -----");
            Console.WriteLine($"ID: {EventId}");
            Console.WriteLine($"Name: {EventName}");
            Console.WriteLine($"Date: {EventDate.ToShortDateString()}");
            Console.WriteLine($"Time: {EventTime}");
            Console.WriteLine($"Venue: {Venue.VenueName}, Address: {Venue.Address}");
            Console.WriteLine($"Total Seats: {TotalSeats}");
            Console.WriteLine($"Available Seats: {AvailableSeats}");
            Console.WriteLine($"Ticket Price: {TicketPrice:C}");
            Console.WriteLine($"Genre: {Genre}");
            Console.WriteLine($"Lead Actor: {ActorName}");
            Console.WriteLine($"Lead Actress: {ActressName}");
            Console.WriteLine($"Total Revenue: {CalculateTotalRevenue():C}");
            Console.WriteLine("-------------------------\n");
        }
    }
}

