using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace travoul.Models
{
    public class RetroType
    {
        [Key]
        public int RetroTypeId { get; set; }

        [Required]
        public string Type { get; set; }

        public virtual ICollection<TripRetro> TripRetros { get; set; }
    }
}