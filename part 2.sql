CREATE DATABASE EventEase;
USE EventEase;

CREATE TABLE Venues (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(255) NOT NULL,
    Location NVARCHAR(255),
    Capacity INT NOT NULL,
    ImageUrl NVARCHAR(500)
);

-- Create Events Table
CREATE TABLE Events (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(255) NOT NULL,
    StartDate DATETIME NOT NULL,
    EndDate DATETIME NOT NULL,
    ImageUrl NVARCHAR(500),
    VenueId INT NULL,
    FOREIGN KEY (VenueId) REFERENCES Venues(Id) ON DELETE SET NULL
);

-- Create Bookings Table
CREATE TABLE Bookings (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    EventId INT NOT NULL,
    VenueId INT NOT NULL,
    BookingDate DATETIME NOT NULL,
    FOREIGN KEY (EventId) REFERENCES Events(Id) ON DELETE CASCADE,
    FOREIGN KEY (VenueId) REFERENCES Venues(Id) ON DELETE CASCADE
);

-- Insert Dummy Venues
INSERT INTO Venues (Name, Location, Capacity, ImageUrl) VALUES
('Grand Hall', 'Cape Town City Centre', 500, 'https://biz-file.com/c/1712/421425.jpg?2'),
('Skyline Arena', 'Johannesburg CBD', 1200, 'https://th.bing.com/th/id/OIP.UOt2o0e0mctUDRru1GgJCwHaE8?rs=1&pid=ImgDetMain'),
('Ocean View Center', 'Durban Beachfront', 800, 'https://th.bing.com/th/id/R.7cb9e3135e2046d250eda1d2dc700142?rik=O%2fCwl4%2frs8yhIw&pid=ImgRaw&r=0');

-- Insert Dummy Events 
-- Corrected Insert for Dummy Events using existing Venue IDs
INSERT INTO Events (Name, StartDate, EndDate, ImageUrl, VenueId) VALUES
('Art Expo 2025', '2025-05-05', '2025-05-07', 'https://art.newcity.com/wp-content/uploads/2023/04/Mindy-Solomon-scaled.jpeg', 27),
('Food Market Day', '2025-06-12', '2025-06-12', 'https://th.bing.com/th/id/R.48c0c10ab9e7c4a6aa8b4afcc22b0452?rik=%2b%2fKH7%2bpQaP1t9A&pid=ImgRaw&r=0', 28),
('Startup Meetup', '2025-07-20', '2025-07-20', 'https://th.bing.com/th/id/OIP.y7p_MIp-Q7TFXXjuQNA4KwHaEA?rs=1&pid=ImgDetMain', 29);

-- Insert Dummy Bookings
INSERT INTO Bookings (EventId, VenueId, BookingDate) VALUES
(22, 27, '2025-04-25'), 
(23, 28, '2025-05-30'), 
(24, 29, '2025-06-10');
-- View existing data 
SELECT * FROM Venues;
SELECT * FROM Events;
SELECT * FROM Bookings;

-- PART2
USE EventEase;
GO
CREATE VIEW Booking_Details AS
SELECT 
    Bookings.Id AS BookingId,
    Bookings.BookingDate,
    Events.Name AS EventName,
    Events.StartDate,
    Events.EndDate,
    Venues.Name AS VenueName,
    Venues.Location
FROM Bookings
JOIN Events ON Bookings.EventId = Events.Id
JOIN Venues ON Bookings.VenueId = Venues.Id;
GO
SELECT * FROM Booking_Details

CREATE TABLE EventType (
    EventTypeId INT PRIMARY KEY IDENTITY,
    TypeName NVARCHAR(50) NOT NULL
);