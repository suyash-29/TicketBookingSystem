using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;

namespace TicketBookingSystem.exception
{
    public class EventNotFoundException : TicketBookingException
    {
        public EventNotFoundException(string message) : base(message)
        {
        }
    }
}
