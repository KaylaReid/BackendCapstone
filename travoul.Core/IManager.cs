using System.Collections.Generic;
using travoul.Models;
using travoul.Models.ViewModels.PaginationModels;

namespace travoul.Core
{
    public interface IManager
    {
        List<Trip> GetPlannedTrips(string userId);
        List<Trip> SearchTrips(string userId, string search, bool preTrip, ref Pager pager);
        List<Trip> SearchTrips(string userId, string search, bool preTrip);
        List<Trip> GetMyTrips(string userId, ref Pager pager);
        Trip GetFinsihedTripDetails(int tripId);
        Trip GetFutureTripDetails(int tripId);
        List<TravelType> GetTravelTypes();
        List<Continent> GetContinents();
        void Create(Trip trip, List<int> SelectedTravelTypeIds, List<TripVisitLocation> enteredTripFoodLocations, List<TripVisitLocation> enteredTripVisitLocations);
    }
}
