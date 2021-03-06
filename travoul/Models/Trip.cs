﻿using System;
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
        [Range(1, int.MaxValue, ErrorMessage = "Please Select a Continent")]
        public int ContinentId { get; set; }

        public Continent Continent { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        public string TripDates { get; set; }

        [Required]
        public string Accommodation { get; set; }

        [Required]
        public string Title { get; set; }

        public double? Budget { get; set; }

        public string ImagePath { get; set; }

        public DateTime DateFinished { get; set; }

        [Required]
        public bool IsPreTrip { get; set; }

        public virtual ICollection<TripTravelType> TripTravelTypes { get; set; }

        public virtual ICollection<TripVisitLocation> TripVisitLocations { get; set; }

        public virtual ICollection<TripRetro> TripRetros { get; set; }
    }
}