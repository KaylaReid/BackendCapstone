using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace travoul.Models
{
    public class Trip
    {
        [Key]
        public int TripId { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public ApplicationUser User { get; set; }

        [Required]
        public int ContinentId { get; set; }

        [Required]
        public Continent Continent { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        public string TripDates { get; set; }

        [Required]
        public string Accommodation { get; set; }

        [Required]
        public string Title { get; set; }

        public string Budget { get; set; }

        [Required]
        public bool PreTrip { get; set; }

        public virtual ICollection<TripTravelType> TripTravelTypes { get; set; }

        public virtual ICollection<VisitLocation> VisitLocations { get; set; }

        public virtual ICollection<Retro> Retros { get; set; }
    }
}