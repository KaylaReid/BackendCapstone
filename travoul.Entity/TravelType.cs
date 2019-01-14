using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace travoul.Models
{
    public class TravelType
    {
        [Key]
        public int TravelTypeId { get; set; }

        [Required]
        public string Type { get; set; }

        public virtual ICollection<TripTravelType> TripTravelTypes { get; set; }
    }
}