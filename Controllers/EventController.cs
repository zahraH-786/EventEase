

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EventEase.Data;
using EventEase.Models;
using Microsoft.AspNetCore.Http;

namespace EventEase.Controllers
{
    public class EventController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EventController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var events = _context.Events
                .Include(e => e.Venue);
            return View(await events.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var @event = await _context.Events.Include(e => e.Venue).FirstOrDefaultAsync(m => m.Id == id);
            if (@event == null) return NotFound();

            return View(@event);
        }

        
        public IActionResult Create()
        {
            ViewData["VenueId"] = new SelectList(_context.Venues, "Id", "Name");
            //ViewData["EventTypeId"] = new SelectList(_context.EventTypes, "Id", "Name"); // added
            return View();
        }
   
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Event eventModel)
        {
            Console.WriteLine("Create POST method hit.");

            if (!ModelState.IsValid)
            {
                Console.WriteLine("ModelState is invalid.");
                foreach (var modelState in ModelState)
                {
                    foreach (var error in modelState.Value.Errors)
                    {
                        Console.WriteLine($"Field: {modelState.Key}, Error: {error.ErrorMessage}");
                    }
                }

                ViewData["VenueId"] = new SelectList(_context.Venues, "Id", "Name", eventModel.VenueId);
                ViewData["EventTypeId"] = new SelectList(_context.EventTypes, "Id", "Name", eventModel.EventType);
                return View(eventModel);
            }

            try
            {
                Console.WriteLine("ModelState is valid. Attempting to save...");
                Console.WriteLine($"Name: {eventModel.Name}");
                Console.WriteLine($"StartDate: {eventModel.StartDate}");
                Console.WriteLine($"EndDate: {eventModel.EndDate}");
                Console.WriteLine($"VenueId: {eventModel.VenueId}");
                Console.WriteLine($"EventTypeId: {eventModel.EventType}");

                _context.Add(eventModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error saving event: " + ex.Message);
                ModelState.AddModelError("", "Unable to save changes. Please try again.");
            }

            ViewData["VenueId"] = new SelectList(_context.Venues, "Id", "Name", eventModel.VenueId);
            return View(eventModel);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var eventModel = await _context.Events.FindAsync(id);
            if (eventModel == null) return NotFound();

            ViewData["VenueId"] = new SelectList(_context.Venues, "Id", "Name", eventModel.VenueId);
            return View(eventModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,StartDate,EndDate,ImageUrl,VenueId")] Event eventModel)
        {
            if (id != eventModel.Id) return NotFound();

            if (!ModelState.IsValid)
            {
                Console.WriteLine("Edit model state invalid.");
                ViewData["VenueId"] = new SelectList(_context.Venues, "Id", "Name", eventModel.VenueId);
                return View(eventModel);
            }

            try
            {
                _context.Update(eventModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventExists(eventModel.Id))
                    return NotFound();
                else
                    throw;
            }
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var @event = await _context.Events
                .Include(e => e.Venue)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (@event == null) return NotFound();

            return View(@event);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @event = await _context.Events
                .Include(e => e.Bookings)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (@event == null) return NotFound();

            if (@event.Bookings != null && @event.Bookings.Any())
            {
                ViewData["ErrorMessage"] = "❌ Cannot delete this event because it has active bookings.";
                ViewData["VenueId"] = new SelectList(_context.Venues, "Id", "Name", @event.VenueId);
                return View("Delete", @event);
            }

            _context.Events.Remove(@event);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventExists(int id)
        {
            return _context.Events.Any(e => e.Id == id);
        }
    }
}
