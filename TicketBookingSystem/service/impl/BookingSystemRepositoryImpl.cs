using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using TicketBookingSystem.bean;
using TicketBookingSystem.exception;
using TicketBookingSystem.util;

namespace TicketBookingSystem.service.impl
{
    public class BookingSystemRepositoryImpl : IBookingSystemRepository
    {
        public Event CreateEvent(Event evnt)
        {
            using (SqlConnection conn = DBUtil.GetDBConn())
            {
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    if (string.IsNullOrEmpty(evnt.EventType))
                    {
                        throw new TicketBookingException("Event Type is missing.");
                    }
                    int venueId = InsertVenue(conn, transaction, evnt.Venue);
                   

                    // Inserting data in  event table
                    string insertEventQuery = @"
                        INSERT INTO Events (EventName, EventDate, EventTime, VenueId, TotalSeats, AvailableSeats, TicketPrice, EventType)
                        VALUES (@EventName, @EventDate, @EventTime, @VenueId, @TotalSeats, @AvailableSeats, @TicketPrice, @EventType);
                        SELECT CAST(SCOPE_IDENTITY() as int);";
                    int eventId;
                    using (SqlCommand cmd = new SqlCommand(insertEventQuery, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@EventName", evnt.EventName);
                        cmd.Parameters.AddWithValue("@EventDate", evnt.EventDate);
                        cmd.Parameters.AddWithValue("@EventTime", evnt.EventTime);
                        cmd.Parameters.AddWithValue("@VenueId", venueId);
                        cmd.Parameters.AddWithValue("@TotalSeats", evnt.TotalSeats);
                        cmd.Parameters.AddWithValue("@AvailableSeats", evnt.AvailableSeats);
                        cmd.Parameters.AddWithValue("@TicketPrice", evnt.TicketPrice);
                        cmd.Parameters.AddWithValue("@EventType", evnt.EventType);
                        eventId = (int)cmd.ExecuteScalar();
                    }

                    // inserting into eventType table
                    switch (evnt.EventType)
                    {
                        case "Movie":
                            Movie movie = (Movie)evnt;
                            if (movie == null)
                            {
                                Console.WriteLine("movie details are missing during insertion");
                                throw new TicketBookingException("Movie details are missing.");
                            }
                            string insertMovieQuery = "INSERT INTO Movies (EventId, Genre, ActorName, ActressName) VALUES (@EventId, @Genre, @ActorName, @ActressName)";
                            using (SqlCommand cmd = new SqlCommand(insertMovieQuery, conn, transaction))
                            {
                                cmd.Parameters.AddWithValue("@EventId", eventId);
                                cmd.Parameters.AddWithValue("@Genre", movie.Genre);
                                cmd.Parameters.AddWithValue("@ActorName", movie.ActorName);
                                cmd.Parameters.AddWithValue("@ActressName", movie.ActressName);
                                cmd.ExecuteNonQuery();
                            }
                            break;

                        case "Concert":
                            Concert concert = (Concert)evnt;
                            if (concert == null)
                            {
                                Console.WriteLine("Concert details are missing during insertion");
                                throw new TicketBookingException("Concert details are missing.");
                            }
                            string insertConcertQuery = "INSERT INTO Concerts (EventId, Artist, Type) VALUES (@EventId, @Artist, @Type)";
                            using (SqlCommand cmd = new SqlCommand(insertConcertQuery, conn, transaction))
                            {
                                cmd.Parameters.AddWithValue("@EventId", eventId);
                                cmd.Parameters.AddWithValue("@Artist", concert.Artist);
                                cmd.Parameters.AddWithValue("@Type", concert.Type);
                                cmd.ExecuteNonQuery();
                            }
                            break;

                        case "Sports":
                            Sports sport = (Sports)evnt;
                            if (sport == null)
                            {
                                Console.WriteLine("sports details are missing during insertion");
                                throw new TicketBookingException("Sports details are missing.");
                            }
                            string insertSportsQuery = "INSERT INTO Sports (EventId, SportName, TeamsName) VALUES (@EventId, @SportName, @TeamsName)";
                            using (SqlCommand cmd = new SqlCommand(insertSportsQuery, conn, transaction))
                            {
                                cmd.Parameters.AddWithValue("@EventId", eventId);
                                cmd.Parameters.AddWithValue("@SportName", sport.SportName);
                                cmd.Parameters.AddWithValue("@TeamsName", sport.TeamsName);
                                cmd.ExecuteNonQuery();
                            }
                            break;

                        default:
                            throw new TicketBookingException("Invalid Event Type.");
                    }

                    // commiting transaction
                    transaction.Commit();

                    // getting venue object from database
                    Venue dbVenue = GetVenueById(conn, eventId);

                    // Creating and return the Event object
                    Event newEvent = null;
                    switch (evnt.EventType)
                    {
                        case "Movie":
                            Movie movieDetails = GetMovieByEventId(conn, eventId);
                            newEvent = new Movie
                            {
                                EventType = evnt.EventType,
                                EventId = eventId,
                                EventName = evnt.EventName,
                                EventDate = DateTime.Parse(evnt.EventDate.ToString()),
                                EventTime = TimeSpan.Parse(evnt.EventTime.ToString()),
                                Venue = dbVenue,
                                TotalSeats = evnt.TotalSeats,
                                AvailableSeats = evnt.AvailableSeats,
                                TicketPrice = evnt.TicketPrice,
                                Genre = movieDetails.Genre,
                                ActorName = movieDetails.ActorName,
                                ActressName = movieDetails.ActressName
                            };
                            break;

                        case "Concert":
                            Concert concertDetails = GetConcertByEventId(conn, eventId);
                            newEvent = new Concert
                            {
                                EventType = evnt.EventType,
                                EventId = eventId,
                                EventName = evnt.EventName,
                                EventDate = DateTime.Parse(evnt.EventDate.ToString()),
                                EventTime = TimeSpan.Parse(evnt.EventTime.ToString()),
                                Venue = dbVenue,
                                TotalSeats = evnt.TotalSeats,
                                AvailableSeats = evnt.AvailableSeats,
                                TicketPrice = evnt.TicketPrice,
                                Artist = concertDetails.Artist,
                                Type = concertDetails.Type
                            };
                            break;

                        case "Sports":
                            Sports sportDetails = GetSportsByEventId(conn, eventId);
                            newEvent = new Sports
                            {

                                EventType = evnt.EventType,
                                EventId = eventId,
                                EventName = evnt.EventName,
                                EventDate = DateTime.Parse(evnt.EventDate.ToString()),
                                EventTime = TimeSpan.Parse(evnt.EventTime.ToString()),
                                Venue = dbVenue,
                                TotalSeats = evnt.TotalSeats,
                                AvailableSeats = evnt.AvailableSeats,
                                TicketPrice = evnt.TicketPrice,
                                SportName = sportDetails.SportName,
                                TeamsName = sportDetails.TeamsName
                            };
                            break;
                    }

                    return newEvent;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new TicketBookingException($"Error creating event: {ex.Message}");
                }
            }
        }

        private int InsertVenue(SqlConnection conn, SqlTransaction transaction, Venue venue)
        {
            
            string checkVenueQuery = "SELECT VenueId FROM Venues WHERE VenueName = @VenueName AND Address = @Address";
            using (SqlCommand cmd = new SqlCommand(checkVenueQuery, conn, transaction))
            {
                cmd.Parameters.AddWithValue("@VenueName", venue.VenueName);
                cmd.Parameters.AddWithValue("@Address", venue.Address);
                object result = cmd.ExecuteScalar();
                if (result != null)
                {
                    return (int)result;
                }
            }

            // Insert new venue
            string insertVenueQuery = "INSERT INTO Venues (VenueName, Address) VALUES (@VenueName, @Address); SELECT CAST(SCOPE_IDENTITY() as int);";
            using (SqlCommand cmd = new SqlCommand(insertVenueQuery, conn, transaction))
            {
                cmd.Parameters.AddWithValue("@VenueName", venue.VenueName);
                cmd.Parameters.AddWithValue("@Address", venue.Address);
                return (int)cmd.ExecuteScalar();
            }
        }

        public List<Event> GetEventDetails()
            {
                List<Event> events = new List<Event>();

                using (SqlConnection conn = DBUtil.GetDBConn())
                {
                    string query = @"
                        SELECT e.EventId, e.EventName, e.EventDate, e.EventTime, e.VenueId, v.VenueName, v.Address, e.TotalSeats, e.AvailableSeats, e.TicketPrice, e.EventType
                        FROM Events e
                        JOIN Venues v ON e.VenueId = v.VenueId";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            int eventId = reader.GetInt32(0);
                            string eventName = reader.GetString(1);
                            DateTime eventDate = reader.GetDateTime(2);
                            TimeSpan eventTime = reader.GetTimeSpan(3);
                            int venueId = reader.GetInt32(4);
                            string venueName = reader.GetString(5);
                            string address = reader.GetString(6);
                            int totalSeats = reader.GetInt32(7);
                            int availableSeats = reader.GetInt32(8);
                            decimal ticketPrice = reader.GetDecimal(9);
                            string eventType = reader.GetString(10);

                            Venue venue = new Venue
                            {
                                VenueId = venueId,
                                VenueName = venueName,
                                Address = address
                            };

                            Event evt = null;

                            switch (eventType)
                            {
                                case "Movie":
                                    evt = GetMovieByEventId(conn, eventId);
                                    if (evt != null)
                                    {
                                        evt.EventId = eventId;
                                        evt.EventName = eventName;
                                        evt.EventDate = eventDate;
                                        evt.EventTime = eventTime;
                                        evt.Venue = venue;
                                        evt.TotalSeats = totalSeats;
                                        evt.AvailableSeats = availableSeats;
                                        evt.TicketPrice = ticketPrice;
                                    }
                                    break;

                                case "Concert":
                                    evt = GetConcertByEventId(conn, eventId);
                                    if (evt != null)
                                    {
                                        evt.EventId = eventId;
                                        evt.EventName = eventName;
                                        evt.EventDate = eventDate;
                                        evt.EventTime = eventTime;
                                        evt.Venue = venue;
                                        evt.TotalSeats = totalSeats;
                                        evt.AvailableSeats = availableSeats;
                                        evt.TicketPrice = ticketPrice;
                                    }
                                    break;

                                case "Sports":
                                    evt = GetSportsByEventId(conn, eventId);
                                    if (evt != null)
                                    {
                                        evt.EventId = eventId;
                                        evt.EventName = eventName;
                                        evt.EventDate = eventDate;
                                        evt.EventTime = eventTime;
                                        evt.Venue = venue;
                                        evt.TotalSeats = totalSeats;
                                        evt.AvailableSeats = availableSeats;
                                        evt.TicketPrice = ticketPrice;
                                    }
                                    break;

                                
                                    
                                    
                            }

                            if (evt != null)
                            {
                                events.Add(evt);
                            }
                        }
                    }
                }

                return events;
            }

            

            public decimal CalculateBookingCost(int numTickets, decimal ticketPrice)
            {
                return numTickets * ticketPrice;
            }

            public Booking BookTickets(string eventName, int numTickets, List<Customer> customers)
            {
                using (SqlConnection conn = DBUtil.GetDBConn())
                {
                    SqlTransaction transaction = conn.BeginTransaction();

                    try
                    {
                        // retieving event data
                        string getEventQuery = @"SELECT EventId, TicketPrice, AvailableSeats FROM Events WHERE EventName = @EventName";
                        int eventId;
                        decimal ticketPrice;
                        int availableSeats;

                        using (SqlCommand cmd = new SqlCommand(getEventQuery, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@EventName", eventName);
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    eventId = reader.GetInt32(0);
                                    ticketPrice = reader.GetDecimal(1);
                                    availableSeats = reader.GetInt32(2);
                                }
                                else
                                {
                                    throw new EventNotFoundException($"Event '{eventName}' not found.");
                                }
                            }
                        }

                        // Checking availabilty of seats
                        if (numTickets > availableSeats)
                        {
                            throw new TicketBookingException("Not enough tickets available.");
                        }

                        // Calculating cost 
                        decimal totalCost = CalculateBookingCost(numTickets, ticketPrice);

                        // Insert Booking
                        string insertBookingQuery = @"
                            INSERT INTO Bookings (EventId, NumTickets, TotalCost, BookingDate)
                            VALUES (@EventId, @NumTickets, @TotalCost, @BookingDate);
                            SELECT CAST(SCOPE_IDENTITY() as int);";

                        int bookingId;
                        using (SqlCommand cmd = new SqlCommand(insertBookingQuery, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@EventId", eventId);
                            cmd.Parameters.AddWithValue("@NumTickets", numTickets);
                            cmd.Parameters.AddWithValue("@TotalCost", totalCost);
                            cmd.Parameters.AddWithValue("@BookingDate", DateTime.Now);
                            bookingId = (int)cmd.ExecuteScalar();
                        }

                        // Update AvailableSeats
                        string updateSeatsQuery = @"UPDATE Events SET AvailableSeats = AvailableSeats - @NumTickets WHERE EventId = @EventId";
                        using (SqlCommand cmd = new SqlCommand(updateSeatsQuery, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@NumTickets", numTickets);
                            cmd.Parameters.AddWithValue("@EventId", eventId);
                            cmd.ExecuteNonQuery();
                        }

                        // Inserting data in  Customers and BookingCustomers(transit table) 
                        foreach (var customer in customers)
                        {
                            // Checking if customer already exists to have many to many relarionship
                            string checkCustomerQuery = @"SELECT CustomerId FROM Customers WHERE Email = @Email";
                            int customerId = -1;
                            using (SqlCommand cmd = new SqlCommand(checkCustomerQuery, conn, transaction))
                            {
                                cmd.Parameters.AddWithValue("@Email", customer.Email);
                                object result = cmd.ExecuteScalar();
                                if (result != null)
                                {
                                    customerId = Convert.ToInt32(result);
                                }
                                else
                                {
                                    // Insert Customer
                                    string insertCustomerQuery = @"
                                        INSERT INTO Customers (CustomerName, Email, PhoneNumber)
                                        VALUES (@CustomerName, @Email, @PhoneNumber);
                                        SELECT CAST(SCOPE_IDENTITY() as int);";
                                    using (SqlCommand insertCmd = new SqlCommand(insertCustomerQuery, conn, transaction))
                                    {
                                        insertCmd.Parameters.AddWithValue("@CustomerName", customer.CustomerName);
                                        insertCmd.Parameters.AddWithValue("@Email", customer.Email);
                                        insertCmd.Parameters.AddWithValue("@PhoneNumber", customer.PhoneNumber);
                                        customerId = (int)insertCmd.ExecuteScalar();
                                    }
                                }
                            }

                            // Inserting in BookingCustomers table
                            string insertBookingCustomerQuery = @"INSERT INTO BookingCustomers (BookingId, CustomerId) VALUES (@BookingId, @CustomerId)";
                            using (SqlCommand cmd = new SqlCommand(insertBookingCustomerQuery, conn, transaction))
                            {
                                cmd.Parameters.AddWithValue("@BookingId", bookingId);
                                cmd.Parameters.AddWithValue("@CustomerId", customerId);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        transaction.Commit();

                        // Retrieve Booking Details
                        Booking booking = GetBookingDetails(bookingId);
                        return booking;
                    }
                    catch (EventNotFoundException)
                    {
                        transaction.Rollback();
                        throw;
                    }
                    catch (TicketBookingException)
                    {
                        transaction.Rollback();
                        throw;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new TicketBookingException($"Error booking tickets: {ex.Message}");
                    }
                }
            }

            public void CancelBooking(int bookingId)
            {
                using (SqlConnection conn = DBUtil.GetDBConn())
                {
                    SqlTransaction transaction = conn.BeginTransaction();

                    try
                    {
                        // Get Booking
                        string getBookingQuery = "SELECT EventId, NumTickets FROM Bookings WHERE BookingId = @BookingId";
                        int eventId;
                        int numTickets;

                        using (SqlCommand cmd = new SqlCommand(getBookingQuery, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@BookingId", bookingId);
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    eventId = reader.GetInt32(0);
                                    numTickets = reader.GetInt32(1);
                                }
                                else
                                {
                                    throw new InvalidBookingIDException($"Booking ID '{bookingId}' is invalid.");
                                }
                            }
                        }

                        // Update AvailableSeats
                        string updateSeatsQuery = "UPDATE Events SET AvailableSeats = AvailableSeats + @NumTickets WHERE EventId = @EventId";
                        using (SqlCommand cmd = new SqlCommand(updateSeatsQuery, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@NumTickets", numTickets);
                            cmd.Parameters.AddWithValue("@EventId", eventId);
                            cmd.ExecuteNonQuery();
                        }

                        // Delete from BookingCustomers
                        string deleteBookingCustomersQuery = "DELETE FROM BookingCustomers WHERE BookingId = @BookingId";
                        using (SqlCommand cmd = new SqlCommand(deleteBookingCustomersQuery, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@BookingId", bookingId);
                            cmd.ExecuteNonQuery();
                        }

                        // Delete Booking
                        string deleteBookingQuery = "DELETE FROM Bookings WHERE BookingId = @BookingId";
                        using (SqlCommand cmd = new SqlCommand(deleteBookingQuery, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@BookingId", bookingId);
                            cmd.ExecuteNonQuery();
                        }

                        transaction.Commit();
                    }
                    catch (InvalidBookingIDException)
                    {
                        transaction.Rollback();
                        throw;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new TicketBookingException($"Error cancelling booking: {ex.Message}");
                    }
                }
            }

            public Booking GetBookingDetails(int bookingId)
            {
                Booking booking = null;

                using (SqlConnection conn = DBUtil.GetDBConn())
                {

                    // Get Booking info
                    string getBookingQuery = @"
                        SELECT b.BookingId, b.EventId, b.NumTickets, b.TotalCost, b.BookingDate, 
                               e.EventName, e.EventType, e.EventDate, e.EventTime, v.VenueId, v.VenueName, v.Address
                        FROM Bookings b
                        JOIN Events e ON b.EventId = e.EventId
                        JOIN Venues v ON e.VenueId = v.VenueId
                        WHERE b.BookingId = @BookingId";
                    using (SqlCommand cmd = new SqlCommand(getBookingQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@BookingId", bookingId);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int eventId = reader.GetInt32(1);
                                string eventName = reader.GetString(5);
                                string eventType = reader.GetString(6);
                                DateTime eventDate = reader.GetDateTime(7);
                                TimeSpan eventTime = reader.GetTimeSpan(8);
                                int venueId = reader.GetInt32(9);
                                string venueName = reader.GetString(10);
                                string address = reader.GetString(11);

                                Venue venue = new Venue
                                {
                                    VenueId = venueId,
                                    VenueName = venueName,
                                    Address = address
                                };

                                Event evt = null;
                                switch (eventType)
                                {
                                case "Movie":
                                    Movie movieDetails = GetMovieByEventId(conn, eventId);
                                    evt = new Movie
                                    {
                                        EventType = eventType,
                                        EventId = eventId, // Ensure EventId is assigned
                                        EventName = eventName,
                                        EventDate = eventDate,
                                        EventTime = eventTime,
                                        Venue = venue,
                                        Genre = movieDetails.Genre,
                                        ActorName = movieDetails.ActorName,
                                        ActressName = movieDetails.ActressName
                                    };
                                    break;

                                case "Concert":
                                    Concert concertDetails = GetConcertByEventId(conn, eventId);
                                    evt = new Concert
                                    {
                                        EventType = eventType,
                                        EventId = eventId, // Ensure EventId is assigned
                                        EventName = eventName,
                                        EventDate = eventDate,
                                        EventTime = eventTime,
                                        Venue = venue,
                                        Artist = concertDetails.Artist,
                                        Type = concertDetails.Type
                                    };
                                    break;

                                case "Sports":
                                    Sports sportsDetails = GetSportsByEventId(conn, eventId);
                                    evt = new Sports
                                    {
                                        EventType = eventType,
                                        EventId = eventId, // Ensure EventId is assigned
                                        EventName = eventName,
                                        EventDate = eventDate,
                                        EventTime = eventTime,
                                        Venue = venue,
                                        SportName = sportsDetails.SportName,
                                        TeamsName = sportsDetails.TeamsName
                                    };
                                    break;

                                default:
                                    Console.WriteLine($"Warning: Unknown Event Type '{eventType}' for EventId: {eventId}");
                                    break;
                            }

                                booking = new Booking
                                {
                                    BookingId = reader.GetInt32(0),
                                    Event = evt,
                                    NumTickets = reader.GetInt32(2),
                                    TotalCost = reader.GetDecimal(3),
                                    BookingDate = reader.GetDateTime(4),
                                    Customers = new List<Customer>()
                                };
                            }
                            else
                            {
                                throw new InvalidBookingIDException($"Booking ID '{bookingId}' is invalid.");
                            }
                        }
                    }

                    // Get Customers
                    string getCustomersQuery = @"
                        SELECT c.CustomerId, c.CustomerName, c.Email, c.PhoneNumber
                        FROM Customers c
                        JOIN BookingCustomers bc ON c.CustomerId = bc.CustomerId
                        WHERE bc.BookingId = @BookingId";
                    using (SqlCommand cmd = new SqlCommand(getCustomersQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@BookingId", bookingId);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int customerId = reader.GetInt32(0);
                                string customerName = reader.GetString(1);
                                string email = reader.GetString(2);
                                string phoneNumber = reader.GetString(3);

                                Customer customer = new Customer
                                {
                                    CustomerId = customerId,
                                    CustomerName = customerName,
                                    Email = email,
                                    PhoneNumber = phoneNumber
                                };

                                booking.Customers.Add(customer);
                            }
                        }
                    }
                }

                return booking;
            }

            public List<Booking> GetAllBookings()
            {
                List<Booking> bookings = new List<Booking>();

                using (SqlConnection conn = DBUtil.GetDBConn())
                {
                    

                    string query = @"
                        SELECT b.BookingId, b.EventId, b.NumTickets, b.TotalCost, b.BookingDate, 
                               e.EventName, e.EventType, e.EventDate, e.EventTime, v.VenueId, v.VenueName, v.Address
                        FROM Bookings b
                        JOIN Events e ON b.EventId = e.EventId
                        JOIN Venues v ON e.VenueId = v.VenueId";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int bookingId = reader.GetInt32(0);
                                int eventId = reader.GetInt32(1);
                                int numTickets = reader.GetInt32(2);
                                decimal totalCost = reader.GetDecimal(3);
                                DateTime bookingDate = reader.GetDateTime(4);
                                string eventName = reader.GetString(5);
                                string eventType = reader.GetString(6);
                                DateTime eventDate = reader.GetDateTime(7);
                                TimeSpan eventTime = reader.GetTimeSpan(8);
                                int venueId = reader.GetInt32(9);
                                string venueName = reader.GetString(10);
                                string address = reader.GetString(11);

                                Venue venue = new Venue
                                {
                                    VenueId = venueId,
                                    VenueName = venueName,
                                    Address = address
                                };


                            Event newEvent = null;
                                switch (eventType)
                                {
                                    case "Movie":
                                        Movie movieDetails = GetMovieByEventId(conn, eventId);
                                    newEvent = new Movie
                                    {
                                        EventType = eventType,
                                        EventId = eventId,
                                        EventName = eventName,
                                        EventDate = eventDate,
                                        EventTime = eventTime,
                                        Venue = venue,
                                        
                                        Genre = movieDetails.Genre,
                                        ActorName = movieDetails.ActorName,
                                        ActressName = movieDetails.ActressName,
                                    };
                                        break;
                                    case "Concert":
                                        Concert concertDetails = GetConcertByEventId(conn, eventId);
                                    newEvent = new Concert
                                    {
                                        EventType = eventType,
                                        EventId = eventId,
                                        EventName = eventName,
                                        EventDate = eventDate,
                                        EventTime = eventTime,
                                        Venue = venue,
                                        Artist = concertDetails.Artist,
                                        Type = concertDetails.Type
                                    };
                                        break;
                                    case "Sports":
                                        Sports sportsDetails = GetSportsByEventId(conn, eventId);
                                    newEvent = new Sports
                                    {
                                        EventType = eventType,
                                        EventId = eventId,
                                        EventName = eventName,
                                        EventDate = eventDate,
                                        EventTime = eventTime,
                                        Venue = venue,
                                        SportName = sportsDetails.SportName,
                                        TeamsName = sportsDetails.TeamsName
                                    };
                                        break;
                                
                            }



                            Booking booking = new Booking
                                {
                                    BookingId = bookingId,
                                    Event = newEvent,
                                    NumTickets = numTickets,
                                    TotalCost = totalCost,
                                    BookingDate = bookingDate,
                                    Customers = new List<Customer>()
                                };

                            bookings.Add(booking);
                            
                            }
                        }
                    }

                    // Retrieve customers for each booking
                    foreach (var booking in bookings)
                    {
                        string getCustomersQuery = @"
                            SELECT c.CustomerId, c.CustomerName, c.Email, c.PhoneNumber
                            FROM Customers c
                            JOIN BookingCustomers bc ON c.CustomerId = bc.CustomerId
                            WHERE bc.BookingId = @BookingId";

                        using (SqlCommand cmd = new SqlCommand(getCustomersQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@BookingId", booking.BookingId);
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    int customerId = reader.GetInt32(0);
                                    string customerName = reader.GetString(1);
                                    string email = reader.GetString(2);
                                    string phoneNumber = reader.GetString(3);

                                    Customer customer = new Customer
                                    {
                                        CustomerId = customerId,
                                        CustomerName = customerName,
                                        Email = email,
                                        PhoneNumber = phoneNumber
                                    };

                                    booking.Customers.Add(customer);
                                }
                            }
                        }
                    }
                }

                return bookings;
            }

            public List<Event> GetAllEvents()
            {
                return GetEventDetails();
            }

            public Event GetEventByName(string eventName)
            {
                Event evt = null;

                using (SqlConnection conn = DBUtil.GetDBConn())
                {
                    string query = @"
                        SELECT e.EventId, e.EventName, e.EventDate, e.EventTime, e.VenueId, v.VenueName, v.Address, e.TotalSeats, e.AvailableSeats, e.TicketPrice, e.EventType
                        FROM Events e
                        JOIN Venues v ON e.VenueId = v.VenueId
                        WHERE e.EventName = @EventName";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@EventName", eventName);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int eventId = reader.GetInt32(0);
                                DateTime eventDate = reader.GetDateTime(2);
                                TimeSpan eventTime = reader.GetTimeSpan(3);
                                int venueId = reader.GetInt32(4);
                                string venueName = reader.GetString(5);
                                string address = reader.GetString(6);
                                int totalSeats = reader.GetInt32(7);
                                int availableSeats = reader.GetInt32(8);
                                decimal ticketPrice = reader.GetDecimal(9);
                                string eventType = reader.GetString(10);

                                Venue venue = new Venue
                                {
                                    VenueId = venueId,
                                    VenueName = venueName,
                                    Address = address
                                };

                                switch (eventType)
                                {
                                    case "Movie":
                                        evt = GetMovieByEventId(conn, eventId);
                                        if (evt != null)
                                        {
                                            evt.EventId = eventId;
                                            evt.EventName = eventName;
                                            evt.EventDate = eventDate;
                                            evt.EventTime = eventTime;
                                            evt.Venue = venue;
                                            evt.TotalSeats = totalSeats;
                                            evt.AvailableSeats = availableSeats;
                                            evt.TicketPrice = ticketPrice;
                                        }
                                        break;

                                    case "Concert":
                                        evt = GetConcertByEventId(conn, eventId);
                                        if (evt != null)
                                        {
                                            evt.EventId = eventId;
                                            evt.EventName = eventName;
                                            evt.EventDate = eventDate;
                                            evt.EventTime = eventTime;
                                            evt.Venue = venue;
                                            evt.TotalSeats = totalSeats;
                                            evt.AvailableSeats = availableSeats;
                                            evt.TicketPrice = ticketPrice;
                                        }
                                        break;

                                    case "Sports":
                                        evt = GetSportsByEventId(conn, eventId);
                                        if (evt != null)
                                        {
                                            evt.EventId = eventId;
                                            evt.EventName = eventName;
                                            evt.EventDate = eventDate;
                                            evt.EventTime = eventTime;
                                            evt.Venue = venue;
                                            evt.TotalSeats = totalSeats;
                                            evt.AvailableSeats = availableSeats;
                                            evt.TicketPrice = ticketPrice;
                                        }
                                        break;

                                    default:
                                        // Unknown event type
                                        break;
                                }
                            }
                            else
                            {
                                throw new EventNotFoundException($"Event '{eventName}' not found.");
                            }
                        }
                    }
                }

                return evt;
            }

            // private methods for data retrieval 
            private int GetVenueId(SqlConnection conn, SqlTransaction transaction, Venue venue)
            {
                string query = @"SELECT VenueId FROM Venues WHERE VenueName = @VenueName AND Address = @Address";
                using (SqlCommand cmd = new SqlCommand(query, conn, transaction))
                {
                    cmd.Parameters.AddWithValue("@VenueName", venue.VenueName);
                    cmd.Parameters.AddWithValue("@Address", venue.Address);
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        return Convert.ToInt32(result);
                    }
                    else
                    {
                        return -1; // Venue not found
                    }
                }
            }

            private Venue GetVenueById(SqlConnection conn, int eventId)
            {
                string query = @"
                    SELECT v.VenueId, v.VenueName, v.Address
                    FROM Events e
                    JOIN Venues v ON e.VenueId = v.VenueId
                    WHERE e.EventId = @EventId";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@EventId", eventId);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Venue
                            {
                                VenueId = reader.GetInt32(0),
                                VenueName = reader.GetString(1),
                                Address = reader.GetString(2)
                            };
                        }
                        else
                        {
                            throw new TicketBookingException("Venue not found for the event.");
                        }
                    }
                }
            }

            private Movie GetMovieByEventId(SqlConnection conn, int eventId )
            {

            string query = @"SELECT  Genre, ActorName, ActressName FROM Movies WHERE EventId = @EventId";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@EventId", eventId);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Movie
                            {
                               
                                Genre = reader.GetString(0),
                                ActorName = reader.GetString(1),
                                ActressName = reader.GetString(2),
                                
                                
                            };
                        }
                        else
                        {
                        Console.WriteLine($"Warning: No movie details found for EventId: {eventId}");

                        throw new TicketBookingException("Movie details not found for the event.");
                        }
                    }
                }
            }

            private Concert GetConcertByEventId(SqlConnection conn, int eventId)
            {
            string query = @"SELECT Artist, Type FROM Concerts WHERE EventId = @EventId";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@EventId", eventId);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Concert
                            {
                                EventId = eventId,
                                Artist = reader.GetString(0),
                                Type = reader.GetString(1)
                            };
                        }
                        else
                        {
                            throw new TicketBookingException("Concert details not found for the event.");
                        }
                    }
                }
            }

            private Sports GetSportsByEventId(SqlConnection conn, int eventId)
            {
            string query = @"SELECT SportName, TeamsName FROM Sports WHERE EventId = @EventId";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@EventId", eventId);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Sports
                            {
                                EventId = eventId,
                                SportName = reader.GetString(0),
                                TeamsName = reader.GetString(1)
                            };
                        }
                        else
                        {
                            throw new TicketBookingException("Sports details not found for the event.");
                        }
                    }
                }
            }
        }
    }
