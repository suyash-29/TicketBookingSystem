using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using System.Collections.Generic;
using TicketBookingSystem.bean;
using TicketBookingSystem.exception;
using TicketBookingSystem.service;

namespace TicketBookingSystem.service.impl
{
    public class BookingSystemServiceProviderImpl : EventServiceProviderImpl, IBookingSystemServiceProvider
    {
        private IBookingSystemRepository _repository;

        public BookingSystemServiceProviderImpl(IBookingSystemRepository repository) : base(repository)
        {
            _repository = repository;
        }

        public decimal CalculateBookingCost(int numTickets, decimal ticketPrice)
        {
            return _repository.CalculateBookingCost(numTickets, ticketPrice);
        }

        public void BookTickets(string eventName, int numTickets, List<Customer> customers)
        {
            _repository.BookTickets(eventName, numTickets, customers);
        }

        public void CancelBooking(int bookingId)
        {
            _repository.CancelBooking(bookingId);
        }

        public Booking GetBookingDetails(int bookingId)
        {
            return _repository.GetBookingDetails(bookingId);
        }

        public List<Booking> GetAllBookings()
        {
            return _repository.GetAllBookings();
        }
    }
}

