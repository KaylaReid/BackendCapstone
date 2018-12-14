using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace travoul.Models.ViewModels
{
    public class CreateTripViewModel
    {
        

        public Trip Trip { get; set; }

        public List<SelectListItem> AllContinentOptions { get; set; }

        public List<SelectListItem> AllTravelTypes { get; set; }

       // public List<TravelType> AllTravelTypes { get; set; }

        public List<int> SelectedTravelTypeIds { get; set; }

    }
}
