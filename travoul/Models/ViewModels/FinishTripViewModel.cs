using Microsoft.AspNetCore.Mvc.Rendering;
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

        public TripRetro DoAgain { get; set; }

        public TripRetro DoDifferent { get; set; }

        //public List<TripRetro> TripRetros { get; set; }

        public List<TravelType> TravelTypes { get; set; }

        public List<SelectListItem> AllTravelTypes { get; set; }

        public List<int> SelectedTravelTypeIds { get; set; }

        public List<SelectListItem> FoodLocations { get; set; }

        public List<SelectListItem> PlaceLocations { get; set; }

        public List<int> SelectedLocationIds { get; set; }

        public List<TripVisitLocation> NewFoods { get; set; }

        public List<TripVisitLocation> NewPlaces { get; set; }

        public List<TripVisitLocation> AllLocations { get; set; }
    }
}