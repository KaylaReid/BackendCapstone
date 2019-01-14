using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using travoul.Data;
using travoul.Models;
using travoul.Models.ViewModels.PaginationModels;

namespace travoul.Core
{
    public class Manager : IManager
    {
        private readonly ApplicationDbContext _context;

        public Manager(ApplicationDbContext ctx)
        {
            _context = ctx;
        }

        public List<Trip> GetPlannedTrips(string userId)
        {
            return _context.Trip
                .Include(t => t.Continent)
                .Where(t => t.UserId == userId && t.IsPreTrip)
                .ToList();
        }

        public List<Trip> SearchTrips(string userId, string search, bool preTrip, ref Pager pager)
        {
            pager.TotalItems = _context.Trip
                .Include(t => t.Continent)
                .Count(t => t.UserId == userId && t.IsPreTrip == preTrip &&
                            (t.Title.Contains(search) || t.Location.Contains(search) ||
                             t.Continent.Name.Contains(search)));
            pager.adjustPages();

            return _context.Trip
                .Include(t => t.Continent)
                .Where(t => t.UserId == userId && t.IsPreTrip == preTrip &&
                            (t.Title.Contains(search) || t.Location.Contains(search) ||
                             t.Continent.Name.Contains(search)))
                .Skip((pager.CurrentPage - 1) * pager.PageSize)
                .Take(pager.PageSize)
                .OrderByDescending(t => t.DateFinished)
                .ToList();
        }

        public List<Trip> SearchTrips(string userId, string search, bool preTrip)
        {
            return _context.Trip
                .Include(t => t.Continent)
                .Where(t => t.UserId == userId && t.IsPreTrip == preTrip &&
                            (t.Title.Contains(search) || t.Location.Contains(search) ||
                             t.Continent.Name.Contains(search)))
                .OrderByDescending(t => t.DateFinished)
                .ToList();
        }

        public List<Trip> GetMyTrips(string userId, ref Pager pager)
        {
            pager.TotalItems = _context.Trip
                .Include(t => t.Continent)
                .Count(t => t.UserId == userId && t.IsPreTrip == false);

            pager.adjustPages();

            return _context.Trip
                .Include(t => t.Continent)
                .Where(t => t.UserId == userId && t.IsPreTrip == false)
                .Skip((pager.CurrentPage - 1) * pager.PageSize)
                .Take(pager.PageSize)
                .OrderByDescending(t => t.DateFinished)
                .ToList();
        }

        public Trip GetFinsihedTripDetails(int tripId)
        {
            return _context.Trip
                .Include(t => t.Continent)
                .Include(t => t.User)
                .Include(t => t.TripTravelTypes)
                .ThenInclude(tt => tt.TravelType)
                .Include(t => t.TripVisitLocations)
                .ThenInclude(tvl => tvl.LocationType)
                .Include(t => t.TripRetros)
                .ThenInclude(tr => tr.RetroType)
                .FirstOrDefault(t => t.TripId == tripId);
        }

        public Trip GetFutureTripDetails(int tripId)
        {
            return _context.Trip
                .Include(t => t.Continent)
                .Include(t => t.User)
                .Include(t => t.TripTravelTypes)
                .ThenInclude(tt => tt.TravelType)
                .Include(t => t.TripVisitLocations)
                .ThenInclude(tvl => tvl.LocationType)
                .FirstOrDefault(t => t.TripId == tripId);
        }

        public List<TravelType> GetTravelTypes()
        {
            return _context.TravelType.ToList();
        }

        public List<Continent> GetContinents()
        {
            return _context.Continent.ToList();
        }

        public void Create(Trip trip, List<int> SelectedTravelTypeIds, List<TripVisitLocation> enteredTripFoodLocations,  List<TripVisitLocation> enteredTripVisitLocations)
        {
            _context.Add(trip);
            if (SelectedTravelTypeIds != null)
            {
                //makes joiner table for TripTravelType 
                foreach (var typeId in SelectedTravelTypeIds)
                {
                    var newTripTt = new TripTravelType
                    {   //pulls tripid out of context bag 
                        TripId = trip.TripId,
                        TravelTypeId = typeId
                    };

                    _context.Add(newTripTt);
                }
            }

            //this runs though all the inputed food places and makes a joiner table for it
            if (enteredTripFoodLocations != null)
            {
                foreach (var foodL in enteredTripFoodLocations)
                {
                    var newTripVl = new TripVisitLocation
                    {
                        TripId = trip.TripId,
                        LocationTypeId = 1,
                        Name = foodL.Name,
                        Description = foodL.Description,
                        IsCompleted = false
                    };

                    _context.Add(newTripVl);
                }
            }

            //this runs though all the inputed food places and makes a joiner table for it
            if (enteredTripVisitLocations != null)
            {
                foreach (var placeL in enteredTripVisitLocations)
                {
                    var newTripVl = new TripVisitLocation
                    {
                        TripId = trip.TripId,
                        LocationTypeId = 2,
                        Name = placeL.Name,
                        Description = placeL.Description,
                        IsCompleted = false
                    };

                    _context.Add(newTripVl);
                }
            }

            _context.SaveChangesAsync();
        }
    }
}