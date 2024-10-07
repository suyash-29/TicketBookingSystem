using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TicketBookingSystem.bean;

namespace TicketBookingSystem.service
{
    public interface IBookingSystemRepository
    {
        Event CreateEvent(Event evnt);
        List<Event> GetEventDetails();
        decimal CalculateBookingCost(int numTickets, decimal ticketPrice);
        Booking BookTickets(string eventName, int numTickets, List<Customer> customers);
        void CancelBooking(int bookingId);
        Booking GetBookingDetails(int bookingId);
        List<Booking> GetAllBookings(); 

        Event GetEventByName(string eventName);
        List<Event> GetAllEvents();

    }
}

