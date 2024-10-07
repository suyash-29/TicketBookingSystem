using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketBookingSystem.bean;

using System;

namespace TicketBookingSystem.bean
{
    public abstract class Event
    {
        public int EventId { get; set; } // Added EventId
        public string EventName { get; set; }
        public DateTime EventDate { get; set; }
        public TimeSpan EventTime { get; set; }
        public Venue Venue { get; set; } // Association with Venue
        public int TotalSeats { get; set; }
        public int AvailableSeats { get; set; }
        public decimal TicketPrice { get; set; }
        public string EventType { get; set; }

        protected Event() { }

        protected Event(int eventId, string eventName, DateTime eventDate, TimeSpan eventTime, Venue venue, int totalSeats, decimal ticketPrice, string eventType)
        {
            EventId = eventId;
            EventName = eventName;
            EventDate = eventDate;
            EventTime = eventTime;
            Venue = venue;
            TotalSeats = totalSeats;
            AvailableSeats = totalSeats; 
            TicketPrice = ticketPrice;
            EventType = eventType;
        }

        public decimal CalculateTotalRevenue()
        {
            return (TotalSeats - AvailableSeats) * TicketPrice;
        }

        public int GetBookedNoOfTickets()
        {
            return TotalSeats - AvailableSeats;
        }

        public void BookTickets(int numTickets)
        {
            if (numTickets <= AvailableSeats)
            {
                AvailableSeats -= numTickets;
                Console.WriteLine($"{numTickets} tickets booked successfully.");
            }
            else
            {
                Console.WriteLine("Not enough tickets available.");
            }
        }

        public void CancelBooking(int numTickets)
        {
            if ((AvailableSeats + numTickets) <= TotalSeats)
            {
                AvailableSeats += numTickets;
                Console.WriteLine($"{numTickets} tickets cancelled successfully.");
            }
            else
            {
                Console.WriteLine("Cannot cancel more tickets than booked.");
            }
        }

        public abstract void DisplayEventDetails();
    }
}

