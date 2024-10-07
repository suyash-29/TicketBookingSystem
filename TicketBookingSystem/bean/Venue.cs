using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace TicketBookingSystem.bean
{
    public class Venue
    {
        public int VenueId { get; set; } // Added VenueId
        public string VenueName { get; set; }
        public string Address { get; set; }

        public Venue() { }

        public Venue(int venueId, string venueName, string address)
        {
            VenueId = venueId;
            VenueName = venueName;
            Address = address;
        }

        public Venue(string venueName, string address)
        {
            VenueName = venueName;
            Address = address;
        }

        public void DisplayVenueDetails()
        {
            Console.WriteLine($"Venue: {VenueName}, Address: {Address}");
        }

        // Override Equals and GetHashCode for proper Set and Map operations
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Venue other = (Venue)obj;
            return VenueName == other.VenueName && Address == other.Address;
        }

        public override int GetHashCode()
        {
            return (VenueName + Address).GetHashCode();
        }
    }
}
