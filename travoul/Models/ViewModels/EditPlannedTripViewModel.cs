using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace travoul.Models.ViewModels
{
    public class EditPlannedTripViewModel
    {
        public Trip Trip { get; set; }

        public List<SelectListItem> AllContinentOptions { get; set; }

        //for TravelTypes
        public List<SelectListItem> AllTravelTypes { get; set; }

        public List<int> SelectedTravelTypeIds { get; set; }

        //for visitLocations
        public List<TripVisitLocation> CurrentVisitLocations { get; set; }

        public List<TripVisitLocation> NewVisitLocations { get; set; }

        //for eatLocations
        public List<TripVisitLocation> CurrentFoodLocations { get; set; }

        public List<TripVisitLocation> NewFoodLocations { get; set; }
    }
}
