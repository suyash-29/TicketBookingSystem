using System;
using System.Collections.Generic;
using TicketBookingSystem.bean;
using TicketBookingSystem.exception;
using TicketBookingSystem.service;
using TicketBookingSystem.service.impl;

namespace TicketBookingSystem.app
{
    public class TicketBookingSystem
    {

        static void Main(string[] args)
        {
            IBookingSystemRepository repository = new BookingSystemRepositoryImpl();
            IEventServiceProvider eventService = new EventServiceProviderImpl(repository);
            IBookingSystemServiceProvider bookingService = new BookingSystemServiceProviderImpl(repository);


            while (true)
            {
                Console.WriteLine("===== Ticket Booking System =====");
                Console.WriteLine("1. Create Event");
                Console.WriteLine("2. Display Event Details");
                Console.WriteLine("3. Book Tickets");
                Console.WriteLine("4. Cancel Booking");
                Console.WriteLine("5. Display Booking Details");
                Console.WriteLine("6. Get All Event Details");
                Console.WriteLine("7. Exit");
                Console.Write("Enter your choice: ");
                string choice = Console.ReadLine();
                Console.WriteLine();

                try
                {
                    switch (choice)
                    {
                        case "1":
                            CreateEvent(eventService);
                            break;

                        case "2":
                            DisplayEventDetails(eventService);
                            break;

                        case "3":
                            BookTickets(bookingService);
                            break;

                        case "4":
                            CancelBooking(bookingService);
                            break;

                        case "5":
                            DisplayBookingDetails(bookingService);
                            break;

                        

                        case "6":
                            GetAllEventDetails(eventService);
                            break;

                        case "7":
                            Console.WriteLine("Exiting Ticket Booking System. Goodbye!");
                            return;

                        default:
                            Console.WriteLine("Invalid choice. Please select a valid option.\n");
                            break;
                    }
                }
                catch (EventNotFoundException ex)
                {
                    Console.WriteLine($"Error: {ex.Message}\n");
                }
                catch (InvalidBookingIDException ex)
                {
                    Console.WriteLine($"Error: {ex.Message}\n");
                }
                catch (TicketBookingException ex)
                {
                    Console.WriteLine($"Error: {ex.Message}\n");
                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid input format. Please try again.\n");
                }
                catch (NullReferenceException)
                {
                    Console.WriteLine("A required value was missing. Please try again.\n");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An unexpected error occurred: {ex.Message}\n");
                }
            }
        }

        private static void CreateEvent(IEventServiceProvider eventService)
        {
            Console.Write("Enter Event Name: ");
            string eventName = Console.ReadLine();

            Console.Write("Enter Event Date (yyyy-mm-dd): ");
            string eventDateInput = Console.ReadLine();
            DateTime eventDate;
            while (!DateTime.TryParse(eventDateInput, out eventDate))
            {
                Console.Write("Invalid date format. Please enter Event Date (yyyy-mm-dd): ");
                eventDateInput = Console.ReadLine();
            }

            Console.Write("Enter Event Time (HH:mm): ");
            string eventTimeInput = Console.ReadLine();
            TimeSpan eventTime;
            while (!TimeSpan.TryParse(eventTimeInput, out eventTime))
            {
                Console.Write("Invalid time format. Please enter Event Time (HH:mm): ");
                eventTimeInput = Console.ReadLine();
            }

            Console.Write("Enter Venue Name: ");
            string venueName = Console.ReadLine();

            Console.Write("Enter Venue Address: ");
            string venueAddress = Console.ReadLine();

            Venue venue = new Venue
            {
                VenueName = venueName,
                Address = venueAddress
            };

            Console.Write("Enter Total Seats: ");
            string totalSeatsInput = Console.ReadLine();
            int totalSeats;
            while (!int.TryParse(totalSeatsInput, out totalSeats) || totalSeats <= 0)
            {
                Console.Write("Invalid input. Please enter a positive integer for Total Seats: ");
                totalSeatsInput = Console.ReadLine();
            }

            Console.Write("Enter Ticket Price: ");
            string ticketPriceInput = Console.ReadLine();
            decimal ticketPrice;
            while (!decimal.TryParse(ticketPriceInput, out ticketPrice) || ticketPrice < 0)
            {
                Console.Write("Invalid input. Please enter a non-negative decimal for Ticket Price: ");
                ticketPriceInput = Console.ReadLine();
            }

            Console.Write("Enter Event Type (Movie/Sports/Concert): ");
            string eventType = Console.ReadLine();
            while (!IsValidEventType(eventType))
            {
                Console.Write("Invalid Event Type. Please enter (Movie/Sports/Concert): ");
                eventType = Console.ReadLine();
            }

            // initializing an event object using switch for diffrent event type
            Event newEvent = null;
            switch (eventType)
            {
                case "Movie":
                    Console.Write("Enter Genre: ");
                    string genre = Console.ReadLine();

                    Console.Write("Enter Lead Actor Name: ");
                    string actorName = Console.ReadLine();

                    Console.Write("Enter Lead Actress Name: ");
                    string actressName = Console.ReadLine();

                    newEvent = new Movie
                    {
                        EventName = eventName,
                        EventDate = eventDate,
                        EventTime = eventTime,
                        Venue = venue,
                        TotalSeats = totalSeats,
                        AvailableSeats = totalSeats,
                        TicketPrice = ticketPrice,
                        Genre = genre,
                        ActorName = actorName,
                        ActressName = actressName,
                        EventType = eventType
                    };
                    break;

                case "Concert":
                    Console.Write("Enter Artist Name: ");
                    string artist = Console.ReadLine();

                    Console.Write("Enter Concert Type (Theatrical/Classical/Rock/Recital): ");
                    string concertType = Console.ReadLine();
                    while (!IsValidConcertType(concertType))
                    {
                        Console.Write("Invalid Concert Type. Please enter (Theatrical/Classical/Rock/Recital): ");
                        concertType = Console.ReadLine();
                    }

                    newEvent = new Concert
                    {
                        EventName = eventName,
                        EventDate = eventDate,
                        EventTime = eventTime,
                        Venue = venue,
                        TotalSeats = totalSeats,
                        AvailableSeats = totalSeats,
                        TicketPrice = ticketPrice,
                        Artist = artist,
                        Type = concertType,
                        EventType = eventType

                    };
                    break;

                case "Sports":
                    Console.Write("Enter Sport Name: ");
                    string sportName = Console.ReadLine();

                    Console.Write("Enter Teams Name (e.g., Team A vs Team B): ");
                    string teamsName = Console.ReadLine();

                    newEvent = new Sports
                    {
                        EventName = eventName,
                        EventDate = eventDate,
                        EventTime = eventTime,
                        Venue = venue,
                        TotalSeats = totalSeats,
                        AvailableSeats = totalSeats,
                        TicketPrice = ticketPrice,
                        SportName = sportName,
                        TeamsName = teamsName,
                        EventType = eventType
                    };
                    break;
            }

            // calling  repository to create event
            try
            {
                eventService.CreateEvent(newEvent);
                Console.WriteLine($"Event '{eventName}' created successfully!\n");
            }
            catch (TicketBookingException ex)
            {
                Console.WriteLine($"Error creating event: {ex.Message}\n");
            }
        }

        private static bool IsValidEventType(string eventType)
        {
            return eventType.Equals("Movie", StringComparison.OrdinalIgnoreCase) ||
                   eventType.Equals("Sports", StringComparison.OrdinalIgnoreCase) ||
                   eventType.Equals("Concert", StringComparison.OrdinalIgnoreCase);
        }

        private static bool IsValidConcertType(string concertType)
        {
            return concertType.Equals("Theatrical", StringComparison.OrdinalIgnoreCase) ||
                   concertType.Equals("Classical", StringComparison.OrdinalIgnoreCase) ||
                   concertType.Equals("Rock", StringComparison.OrdinalIgnoreCase) ||
                   concertType.Equals("Recital", StringComparison.OrdinalIgnoreCase);
        }

        private static void DisplayEventDetails(IEventServiceProvider eventService)
        {
            List<Event> events = eventService.GetEventDetails();
            if (events.Count == 0)
            {
                Console.WriteLine("No events available to display.\n");
                return;
            }

            Console.WriteLine("Available Events:");
            for (int i = 0; i < events.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {events[i].EventName} ({events[i].EventType})");
            }

            Console.Write("Select Event Number to Display Details: ");
            string choiceInput = Console.ReadLine();
            int choice;
            while (!int.TryParse(choiceInput, out choice) || choice <= 0 || choice > events.Count)
            {
                Console.Write("Invalid input ,  please enter a valid event number: ");
                choiceInput = Console.ReadLine();
            }

            Event selectedEvent = events[choice - 1];
            Console.WriteLine();
            selectedEvent.DisplayEventDetails();
        }

        static void BookTickets(IBookingSystemServiceProvider bookingService)
        {
            List<Event> events = bookingService.GetEventDetails();

            if (events.Count == 0)
            {
                Console.WriteLine("No events available for booking.\n");
                return;
            }

            Console.WriteLine("Available Events for Booking:");
            for (int i = 0; i < events.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {events[i].EventName} ({events[i].EventType}) - Available Seats: {events[i].AvailableSeats}");
            }

            Console.Write("Select Event Number to Book Tickets: ");
            int bookChoice;
            while (!int.TryParse(Console.ReadLine(), out bookChoice) || bookChoice <= 0 || bookChoice > events.Count)
            {
                Console.Write("Invalid input ,  Please enter a valid event number: ");
            }

            Event selectedEvent = events[bookChoice - 1];
            Console.WriteLine($"Selected Event: {selectedEvent.EventName} ({selectedEvent.EventType})");
            Console.WriteLine($"Available Seats: {selectedEvent.AvailableSeats}\n");

            Console.Write("Enter Number of Tickets : ");
            int numTickets;
            while (!int.TryParse(Console.ReadLine(), out numTickets) || numTickets <= 0)
            {
                Console.Write("Invalid number. Please enter a positive integer : ");
            }

            List<Customer> customers = new List<Customer>();
            for (int i = 1; i <= numTickets; i++)
            {
                Console.WriteLine($"\nEnter details for every  Customer {i}:");
                Console.Write("Customer Name: ");
                string customerName = Console.ReadLine();

                Console.Write("Email: ");
                string email = Console.ReadLine();

                Console.Write("Phone Number: ");
                string phoneNumber = Console.ReadLine();

                Customer customer = new Customer(customerName, email, phoneNumber);
                customers.Add(customer);
            }

            bookingService.BookTickets(selectedEvent.EventName, numTickets, customers);
        }


        private static void CancelBooking(IBookingSystemServiceProvider bookingService)
        {
            List<Booking> bookings = bookingService.GetAllBookings();
            if (bookings.Count == 0)
            {
                Console.WriteLine("No bookings available to cancel.\n");
                return;
            }

            Console.WriteLine("Existing Bookings:");
            foreach (var booking in bookings)
            {
                Console.WriteLine($"Booking ID: {booking.BookingId}, Event: {booking.Event.EventName}, Number of Tickets: {booking.NumTickets}");
            }

            Console.Write("Enter Booking ID to Cancel: ");
            string bookingIdInput = Console.ReadLine();
            int bookingId;
            while (!int.TryParse(bookingIdInput, out bookingId))
            {
                Console.Write("Invalid input. Please enter a valid Booking ID: ");
                bookingIdInput = Console.ReadLine();
            }

            try
            {
                bookingService.CancelBooking(bookingId);
                Console.WriteLine("Booking cancelled successfully.\n");
            }
            catch (InvalidBookingIDException ex)
            {
                Console.WriteLine($"Error: {ex.Message}\n");
            }
            catch (TicketBookingException ex)
            {
                Console.WriteLine($"Error: {ex.Message}\n");
            }
        }

        private static void DisplayBookingDetails(IBookingSystemServiceProvider bookingService)
        {
            List<Booking> bookings = bookingService.GetAllBookings();
            if (bookings.Count == 0)
            {
                Console.WriteLine("No bookings available to display.\n");
                return;
            }

            Console.WriteLine("Existing Bookings:");
            foreach (var booking in bookings)
            {
                Console.WriteLine($"Booking ID: {booking.BookingId}, Event: {booking.Event.EventName}, Number of Tickets: {booking.NumTickets}");
            }

            Console.Write("Enter Booking ID to Display Details: ");
            string bookingIdInput = Console.ReadLine();
            int bookingId;
            while (!int.TryParse(bookingIdInput, out bookingId))
            {
                Console.Write("Invalid input. Please enter a valid Booking ID: ");
                bookingIdInput = Console.ReadLine();
            }

            try
            {
                Booking booking = bookingService.GetBookingDetails(bookingId);
                Console.WriteLine("\n----- Booking Details -----");
                Console.WriteLine($"Booking ID: {booking.BookingId}");
                Console.WriteLine($"Event: {booking.Event.EventName} ({booking.Event.EventType})");
                Console.WriteLine($"Date: {booking.Event.EventDate.ToShortDateString()}");
                Console.WriteLine($"Time: {booking.Event.EventTime}");
                Console.WriteLine($"Venue: {booking.Event.Venue.VenueName}, Address: {booking.Event.Venue.Address}");

                Console.WriteLine($"Number of Tickets: {booking.NumTickets}");
                Console.WriteLine($"Total Cost: {booking.TotalCost:C}");
                Console.WriteLine($"Booking Date: {booking.BookingDate}");
                Console.WriteLine("Customers:");
                foreach (var customer in booking.Customers)
                {
                    Console.WriteLine($" - {customer.CustomerName}, Email: {customer.Email}, Phone: {customer.PhoneNumber}");
                }
                Console.WriteLine("----------------------------\n");
            }
            catch (InvalidBookingIDException ex)
            {
                Console.WriteLine($"Error: {ex.Message}\n");
            }
            catch (TicketBookingException ex)
            {
                Console.WriteLine($"Error: {ex.Message}\n");
            }
        }

       

        private static void GetAllEventDetails(IEventServiceProvider eventService)
        {
            List<Event> events = eventService.GetAllEvents();
            if (events.Count == 0)
            {
                Console.WriteLine("No events available to display.\n");
                return;
            }

            // Sort events by EventName and Venue Address
            events.Sort(new EventComparer());

            Console.WriteLine("----- All Event Details -----");
            foreach (var evt in events)
            {
                evt.DisplayEventDetails();
            }
            Console.WriteLine("------------------------------\n");
        }

        // Custom Comparer to sort events by EventName and Venue Address
        private class EventComparer : IComparer<Event>
        {
            public int Compare(Event x, Event y)
            {
                if (x == null || y == null)
                {
                    throw new ArgumentException("Events to compare cannot be null.");
                }

                int nameComparison = string.Compare(x.EventName, y.EventName, StringComparison.OrdinalIgnoreCase);
                if (nameComparison != 0)
                {
                    return nameComparison;
                }
                else
                {
                    return string.Compare(x.Venue.Address, y.Venue.Address, StringComparison.OrdinalIgnoreCase);
                }
            }
        }
    }
}
