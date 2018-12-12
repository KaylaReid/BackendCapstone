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

        //Do I need this?
        public virtual ICollection<VisitLocation> VisitLocations { get; set; }
    }
}