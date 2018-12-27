using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace travoul.Models.ViewModels
{
    public class EditFinishedTripViewModel : EditPlannedTripViewModel
    {
        public TripRetro DoAgainRetro { get; set; }

        public TripRetro DoDifferentRetro { get; set; }
    }
}
