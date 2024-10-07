using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketBookingSystem.bean
{
    public class Customer
    {
        public int CustomerId { get; set; } // Added CustomerId
        public string CustomerName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public Customer() { }

        public Customer(int customerId, string customerName, string email, string phoneNumber)
        {
            CustomerId = customerId;
            CustomerName = customerName;
            Email = email;
            PhoneNumber = phoneNumber;
        }

        public Customer(string customerName, string email, string phoneNumber)
        {
            CustomerName = customerName;
            Email = email;
            PhoneNumber = phoneNumber;
        }

        public void DisplayCustomerDetails()
        {
            Console.WriteLine($"Customer ID: {CustomerId}, Name: {CustomerName}, Email: {Email}, Phone: {PhoneNumber}");
        }

        // Override Equals and GetHashCode for proper Set operations
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Customer other = (Customer)obj;
            return Email == other.Email; // Assuming Email is unique
        }

        public override int GetHashCode()
        {
            return Email.GetHashCode();
        }
    }
}
