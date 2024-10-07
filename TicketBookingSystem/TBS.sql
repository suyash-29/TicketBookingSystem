CREATE DATABASE TBS1
use TBS1

CREATE TABLE Venues (
    VenueId INT IDENTITY(1,1) PRIMARY KEY,
    VenueName NVARCHAR(100) NOT NULL,
    Address NVARCHAR(200) NOT NULL
);

CREATE TABLE Events (
    EventId INT IDENTITY(1,1) PRIMARY KEY,
    EventName NVARCHAR(100) NOT NULL,
    EventDate DATE NOT NULL,
    EventTime TIME NOT NULL,
    VenueId INT FOREIGN KEY REFERENCES Venues(VenueId),
    TotalSeats INT NOT NULL,
    AvailableSeats INT NOT NULL,
    TicketPrice DECIMAL(18,2) NOT NULL,
    EventType NVARCHAR(50) NOT NULL
);

CREATE TABLE Movies (
    MovieId INT IDENTITY(1,1) PRIMARY KEY,
    EventId INT NOT NULL UNIQUE FOREIGN KEY REFERENCES Events(EventId) ON DELETE CASCADE,
    Genre NVARCHAR(50) NOT NULL,
    ActorName NVARCHAR(100) NOT NULL,
    ActressName NVARCHAR(100) NOT NULL
);

CREATE TABLE Concerts (
    ConcertId INT IDENTITY(1,1) PRIMARY KEY,
    EventId INT NOT NULL UNIQUE FOREIGN KEY REFERENCES Events(EventId) ON DELETE CASCADE,
    Artist NVARCHAR(100) NOT NULL,
    Type NVARCHAR(50) NOT NULL CHECK (Type IN ('Theatrical', 'Classical', 'Rock', 'Recital'))
);

CREATE TABLE Sports (
    SportId INT IDENTITY(1,1) PRIMARY KEY,
    EventId INT NOT NULL UNIQUE FOREIGN KEY REFERENCES Events(EventId) ON DELETE CASCADE,
    SportName NVARCHAR(100) NOT NULL,
    TeamsName NVARCHAR(100) NOT NULL
);


CREATE TABLE Customers (
    CustomerId INT IDENTITY(1,1) PRIMARY KEY,
    CustomerName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    PhoneNumber NVARCHAR(20) NOT NULL
);

CREATE TABLE Bookings (
    BookingId INT IDENTITY(1,1) PRIMARY KEY,
    EventId INT FOREIGN KEY REFERENCES Events(EventId),
    NumTickets INT NOT NULL,
    TotalCost DECIMAL(18,2) NOT NULL,
    BookingDate DATETIME NOT NULL
);

CREATE TABLE BookingCustomers (
    BookingId INT FOREIGN KEY REFERENCES Bookings(BookingId),
    CustomerId INT FOREIGN KEY REFERENCES Customers(CustomerId),
    PRIMARY KEY (BookingId, CustomerId)
);

SELECT * FROM Events
SELECT * from Venues
SELECT * from Movies
SELECT * from Concerts
SELECT * FROM Sports
SELECT * FROM Customers
SELECT * FROM Bookings
SELECT * FROM BookingCustomers
DELETE from Movies
DELETE from Venues
DELETE from Events

