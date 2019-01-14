using System.ComponentModel.DataAnnotations;

namespace travoul.Models
{
    public class TripTravelType
    {
        [Key]
        public int TripTravelTypeId { get; set; }

        [Required]
        public int TripId { get; set; }

        public Trip Trip { get; set; }

        [Required]
        public int TravelTypeId { get; set; }

        public TravelType TravelType { get; set; }
    }
}