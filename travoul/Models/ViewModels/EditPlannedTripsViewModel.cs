using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace travoul.Models.ViewModels
{
    public class EditPlannedTripsViewModel
    {
        public Trip Trip { get; set; }

        public List<SelectListItem> AllContinentOptions { get; set; }

        //for TravelTypes
        public List<SelectListItem> AllTravelTypes { get; set; }

        public List<int> SelectedTravelTypeIds { get; set; }

        //for visitLocations
        public List<TripVisitLocation> EnteredTripVisitLocations { get; set; }

        //for eatLocations
        public List<TripVisitLocation> EnteredTripFoodLocations { get; set; }
    }
}
