﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace travoul.Models.ViewModels
{
    public class FinishTripViewModel
    {
        public Trip Trip { get; set; }

        [Required(ErrorMessage = "Please fill out this field")]
        public string DoAgain { get; set; }

        [Required(ErrorMessage = "Please fill out this field")]
        public string DoDifferent { get; set; }

        public List<TripRetro> TripRetros { get; set; }

        public List<TravelType> TravelTypes { get; set; }

        public List<SelectListItem> FoodLocations { get; set; }

        public List<SelectListItem> PlaceLocations { get; set; }

        public List<int> SelectedLocationIds { get; set; }

        public List<TripVisitLocation> NewFoods { get; set; }

        public List<TripVisitLocation> NewPlaces { get; set; }
    }
}
