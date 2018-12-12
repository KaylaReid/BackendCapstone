using System.ComponentModel.DataAnnotations;

namespace travoul.Models
{
    public class Retro
    {
        [Key]
        public int RetroId { get; set; }

        [Required]
        public int TripId { get; set; }

        public Trip Trip { get; set; }

        [Required]
        public int RetroTypeId { get; set; }

        public RetroType RetroType { get; set; }

        [Required]
        public string Description { get; set; }

    }
}