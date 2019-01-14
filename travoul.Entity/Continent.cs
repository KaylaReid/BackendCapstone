using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace travoul.Models
{
    public class Continent
    {
        [Key]
        public int ContinentId { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<Trip> Trips { get; set; }
    }
}