using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using TicketBookingSystem.bean;
using TicketBookingSystem.exception;
using TicketBookingSystem.service;

namespace TicketBookingSystem.service.impl
{
    public class EventServiceProviderImpl : IEventServiceProvider
    {
        private IBookingSystemRepository _repository;

        public EventServiceProviderImpl(IBookingSystemRepository repository)
        {
            _repository = repository;
        }

        public Event CreateEvent(Event evnt)
        {
            return _repository.CreateEvent(evnt);
        }

        public List<Event> GetEventDetails()
        {
            return _repository.GetEventDetails();
        }

        public List<Event> GetAllEvents()
        {
            return _repository.GetEventDetails();
        }


        public Event GetEventByName(string eventName)
        {
            return _repository.GetEventByName(eventName);
        }
    }
}

