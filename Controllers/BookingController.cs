using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EventEase.Data;
using EventEase.Models;

namespace EventEase.Controllers
{
    public class BookingController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookingController(ApplicationDbContext context)
        {
            _context = context;
        }

       
        public async Task<IActionResult> Index(string search)
        {
            var bookings = _context.Bookings
                .Include(b => b.Event)
                    .ThenInclude(e => e.EventType) 
                .Include(b => b.Venue)
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                bookings = bookings.Where(b =>
       b.Event.Name.Contains(search) ||
       (b.Event.EventType != null && b.Event.EventType.Name.Contains(search)) ||
       b.Venue.Name.Contains(search) ||
       b.BookingDate.ToString().Contains(search)
   );
            }

            return View(await bookings.ToListAsync());
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var booking = await _context.Bookings
                .Include(b => b.Event)
                .Include(b => b.Venue)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (booking == null) return NotFound();

            return View(booking);
        }

        public IActionResult Create()
        {
            ViewData["EventId"] = new SelectList(_context.Events, "Id", "Name");
            ViewData["VenueId"] = new SelectList(_context.Venues, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,EventId,VenueId,BookingDate")] Booking booking)
        {
            if (!ModelState.IsValid)
            {
                ViewData["EventId"] = new SelectList(_context.Events, "Id", "Name", booking.EventId);
                ViewData["VenueId"] = new SelectList(_context.Venues, "Id", "Name", booking.VenueId);
                return View(booking);
            }

            bool isDoubleBooked = await _context.Bookings
                .AnyAsync(b => b.VenueId == booking.VenueId && b.BookingDate == booking.BookingDate);

            if (isDoubleBooked)
            {
                ModelState.AddModelError("", "❌ This venue is already booked on the selected date.");
                ViewData["EventId"] = new SelectList(_context.Events, "Id", "Name", booking.EventId);
                ViewData["VenueId"] = new SelectList(_context.Venues, "Id", "Name", booking.VenueId);
                return View(booking);
            }

            try
            {
                _context.Add(booking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "❌ Error saving booking: " + ex.Message);
            }

            ViewData["EventId"] = new SelectList(_context.Events, "Id", "Name", booking.EventId);
            ViewData["VenueId"] = new SelectList(_context.Venues, "Id", "Name", booking.VenueId);
            return View(booking);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null) return NotFound();

            ViewData["EventId"] = new SelectList(_context.Events, "Id", "Name", booking.EventId);
            ViewData["VenueId"] = new SelectList(_context.Venues, "Id", "Name", booking.VenueId);
            return View(booking);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EventId,VenueId,BookingDate")] Booking booking)
        {
            if (id != booking.Id) return NotFound();

            if (!ModelState.IsValid)
            {
                ViewData["EventId"] = new SelectList(_context.Events, "Id", "Name", booking.EventId);
                ViewData["VenueId"] = new SelectList(_context.Venues, "Id", "Name", booking.VenueId);
                return View(booking);
            }

            bool isDoubleBooked = await _context.Bookings
                .AnyAsync(b => b.Id != booking.Id &&
                               b.VenueId == booking.VenueId &&
                               b.BookingDate == booking.BookingDate);

            if (isDoubleBooked)
            {
                ModelState.AddModelError("", "❌ This venue is already booked on the selected date.");
                ViewData["EventId"] = new SelectList(_context.Events, "Id", "Name", booking.EventId);
                ViewData["VenueId"] = new SelectList(_context.Venues, "Id", "Name", booking.VenueId);
                return View(booking);
            }

            try
            {
                _context.Update(booking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookingExists(booking.Id))
                    return NotFound();
                else
                    throw;
            }
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var booking = await _context.Bookings
                .Include(b => b.Event)
                .Include(b => b.Venue)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (booking == null) return NotFound();

            return View(booking);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking != null)
            {
                _context.Bookings.Remove(booking);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool BookingExists(int id)
        {
            return _context.Bookings.Any(e => e.Id == id);
        }


        public async Task<IActionResult> DetailsView(string search)
        {
            var query = _context.Booking_Details
                .Select(b => new BookingDetailsViewModel
                {
                    BookingId = b.BookingId,
                    BookingDate = b.BookingDate,
                    EventName = b.EventName,
                    StartDate = b.StartDate,
                    EndDate = b.EndDate,
                    VenueName = b.VenueName,
                    Location = b.Location,
                    EventTypeName = b.EventTypeName 
                });

            if (!string.IsNullOrEmpty(search))
            {
                DateTime parsedDate;
                bool isDate = DateTime.TryParse(search, out parsedDate);

                if (isDate)
                {
                    query = query.Where(b => b.BookingDate.Date == parsedDate.Date);
                }
                else
                {
                    query = query.Where(b =>
                        b.EventName.Contains(search) ||
                        b.EventTypeName.Contains(search) || 
                        b.VenueName.Contains(search) ||
                        b.Location.Contains(search) ||
                        b.BookingId.ToString().Contains(search));
                }
            }

            var results = await query.ToListAsync();
            return View(results);
        }
    }
}


