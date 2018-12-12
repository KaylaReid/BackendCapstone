using System.ComponentModel.DataAnnotations;

namespace travoul.Models
{
    public class VisitLocation
    {
        [Key]
        public int VisitLocationId { get; set; }

        [Required]
        public int TripId { get; set; }

        public Trip Trip { get; set; }

        [Required]
        public int LocationTypeId { get; set; }

        public LocationType LocationType { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public bool IsCompleted { get; set; }
    }
}