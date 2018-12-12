using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace travoul.Models
{
    public class LocationType
    {
        [Key]
        public int LocationTypeId { get; set; }

        [Required]
        public string Type { get; set; }

        public virtual ICollection<TripVisitLocation> TripVisitLocations { get; set; }
    }
}