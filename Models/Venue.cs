
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace EventEase.Models
{
    public class Venue
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        public int Capacity { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        [ValidateNever]
        public ICollection<Event> Events { get; set; }

        [ValidateNever]
        public ICollection<Booking> Bookings { get; set; }
    }
}
