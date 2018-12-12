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

        //Do I need this?
        public virtual ICollection<Retro> Retro { get; set; }
    }
}