using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;

namespace TicketBookingSystem.exception
{
    public class TicketBookingException : Exception
    {
        public TicketBookingException(string message) : base(message)
        {
        }
    }
}

