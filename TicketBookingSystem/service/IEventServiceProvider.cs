using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TicketBookingSystem.bean;
using System.Collections.Generic;

namespace TicketBookingSystem.service
{
    public interface IEventServiceProvider
    {
        Event CreateEvent(Event evnt);
        List<Event> GetEventDetails();
        Event GetEventByName(string eventName);
        List<Event> GetAllEvents();



    }
}

