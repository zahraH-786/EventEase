EventEase

EventEase is a web-based Event Management System developed using ASP.NET Core MVC, Entity Framework Core and SQL Server. This project was originally developed as a second-year Cloud Development project and has since been updated with additional features and improvements.

Overview

The application allows users to manage venues, events, event types and bookings through an intuitive web interface. It demonstrates the use of the MVC architectural pattern, Entity Framework Core for database access and SQL Server for data storage while implementing full CRUD functionality across multiple related entities.
Features
• Create, edit, view and delete venues.
• Create, edit, view and delete events.
• Create, edit, view and delete event types.
• Create, edit, view and delete bookings.
• Assign venues to events.
• Categorise events using event types.
• Upload and display venue and event images.
• Search bookings by event name, event type, venue or booking date.
• Server-side validation using Data Annotations.
• Relational database management using Entity Framework Core.

Technologies Used

• ASP.NET Core MVC
• C#
• Entity Framework Core
• SQL Server
• HTML
• CSS
• Bootstrap
• Razor Views

Database Structure

The application consists of the following entities:

• Venue
• Event
• Event Type
• Booking

Relationships

• One venue can have many events.
• One venue can have many bookings.
• One event type can have many events.
• One event can have many bookings.

Installation

1. Clone the repository.
2. Open the solution in Visual Studio.
3. Update the SQL Server connection string in appsettings.json.
4. Restore the NuGet packages.
5. Run the application.

Project Structure

Controllers/
Data/
Models/
Views/
wwwroot/
Program.cs
appsettings.json

Purpose

This project was developed to demonstrate the use of ASP.NET Core MVC for building a database-driven web application. It showcases CRUD operations, entity relationships, model validation, SQL Server integration and the MVC design pattern. Originally developed as a second-year Cloud Development project, it has since been revisited and enhanced with additional functionality and improvements.

Author

Zahra H

Final Year Computer and Information Sciences Student.
