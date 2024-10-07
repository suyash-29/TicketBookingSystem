using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TicketBookingSystem.bean
{
    public class Booking
    {
        public int BookingId { get; set; } 
        public List<Customer> Customers { get; set; } 
        public Event Event { get; set; }
        public int NumTickets { get; set; }
        public decimal TotalCost { get; set; }
        public DateTime BookingDate { get; set; }

        public Booking()
        {
            Customers = new List<Customer>();
            BookingDate = DateTime.Now;
        }

        public Booking(int bookingId, Event eventObj, int numTickets, List<Customer> customers, decimal totalCost, DateTime bookingDate)
        {
            BookingId = bookingId;
            Event = eventObj;
            NumTickets = numTickets;
            Customers = customers;
            TotalCost = totalCost;
            BookingDate = bookingDate;
        }

        public void DisplayBookingDetails()
        {
            Console.WriteLine("----- Booking Details -----");
            Console.WriteLine($"Booking ID: {BookingId}");
            Console.WriteLine($"Event: {Event.EventName} ({Event.EventType})");
            Console.WriteLine($"Date: {Event.EventDate.ToShortDateString()}");
            Console.WriteLine($"Time: {Event.EventTime}");
            Console.WriteLine($"Venue: {Event.Venue.VenueName}, Address: {Event.Venue.Address}");
            Console.WriteLine($"Number of Tickets: {NumTickets}");
            Console.WriteLine($"Total Cost: {TotalCost:C}");
            Console.WriteLine($"Booking Date: {BookingDate}");
            Console.WriteLine("Customers:");
            foreach (var customer in Customers)
            {
                customer.DisplayCustomerDetails();
            }
            Console.WriteLine("----------------------------\n");
        }
    }
}
