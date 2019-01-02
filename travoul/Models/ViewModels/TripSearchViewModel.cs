using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace travoul.Models.ViewModels
{
    public class TripSearchViewModel
    {
        public string Search { get; set; }

        public List<Trip> Trips { get; set; }
    }
}
