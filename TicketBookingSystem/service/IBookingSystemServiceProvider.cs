using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketBookingSystem.bean;

using System.Collections.Generic;

namespace TicketBookingSystem.service
{
    public interface IBookingSystemServiceProvider
    {
        decimal CalculateBookingCost(int numTickets, decimal ticketPrice);
        void BookTickets(string eventName, int numTickets, List<Customer> customers);
        void CancelBooking(int bookingId);
        Booking GetBookingDetails(int bookingId);
        List<Booking> GetAllBookings(); // Added to support displaying all bookings

        List<Event> GetEventDetails();
        

    }
}

