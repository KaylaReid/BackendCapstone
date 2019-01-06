using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using travoul.Models.ViewModels.PaginationModels;

namespace travoul.Models.ViewModels
{
    public class FinishedTripIndexViewModel
    {
        public string Search { get; set; }

        public List<Trip> Trips { get; set; }

        public Pager Pager { get; set; }
    }
}
