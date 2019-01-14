using System.Collections.Generic;
using travoul.Models.ViewModels.PaginationModels;

namespace travoul.Models.ViewModels
{
    public class TripSearchViewModel
    {
        public string Search { get; set; }

        public List<Trip> Trips { get; set; }

        public Pager Pager { get; set; }
    }
}
