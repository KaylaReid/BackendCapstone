using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace travoul.Models.ViewModels
{
    public class FinishTripViewModel
    {
        public Trip Trip { get; set; }

        public List<TripRetro> TripRetros { get; set; }
    }
}
