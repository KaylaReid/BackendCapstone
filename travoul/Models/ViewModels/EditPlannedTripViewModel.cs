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

        //the ones they already have selected 
        //public List<SelectListItem> PrevSelectedTravelTypes { get; set; }

        public List<int> SelectedTravelTypeIds { get; set; }

        //The newly selected travel types in a list
        //public List<SelectListItem> NewlySelectedTravelTypes { get; set; }

        //for visitLocations
        public List<TripVisitLocation> EnteredTripVisitLocations { get; set; }

        //for eatLocations
        public List<TripVisitLocation> EnteredTripFoodLocations { get; set; }
    }
}
