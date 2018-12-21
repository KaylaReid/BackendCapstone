using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using travoul.Data;
using travoul.Models;
using travoul.Models.ViewModels;

namespace travoul.Controllers
{
    [Authorize]
    public class TripsController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<ApplicationUser> _userManager;

        public TripsController(ApplicationDbContext ctx,
                          UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _context = ctx;
        }

        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        // GET: PlannedTrips --trips planned for the future
        public async Task<IActionResult> PlannedTrips()
        {
            var User = await GetCurrentUserAsync();

            var UserTrips = _context.Trip
                .Include(t => t.Continent)
                .Where(t => t.UserId == User.Id && t.IsPreTrip == true).ToListAsync();

            return View(await UserTrips);
        }

        // GET: MyTrips --all finished trips
        public async Task<IActionResult> Index()
        {
            var User = await GetCurrentUserAsync();

            var UserTrips = _context.Trip
                .Include(t => t.Continent)
                .Where(t => t.UserId == User.Id && t.IsPreTrip == false).ToListAsync();

            return View(await UserTrips);
        }

        // GET: MyTrips/Details/5  --get details for finsihed trips
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trip = await _context.Trip
                .Include(t => t.Continent)
                .Include(t => t.User)
                .Include(t => t.TripTravelTypes)
                .ThenInclude(tt => tt.TravelType)
                .Include(t => t.TripVisitLocations)
                .ThenInclude(tvl => tvl.LocationType)
                .Include(t => t.TripRetros)
                .ThenInclude(tr => tr.RetroType)
                .FirstOrDefaultAsync(t => t.TripId == id);
            if (trip == null)
            {
                return NotFound();
            }

            return View(trip);
        }

        // GET: PlannedTrip/Details/5  --get details for future trips
        public async Task<IActionResult> PlannedTripDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trip = await _context.Trip
                .Include(t => t.Continent)
                .Include(t => t.User)
                .Include(t => t.TripTravelTypes)
                .ThenInclude(tt => tt.TravelType)
                .Include(t => t.TripVisitLocations)
                .ThenInclude(tvl => tvl.LocationType)
                .FirstOrDefaultAsync(t => t.TripId == id);
            if (trip == null)
            {
                return NotFound();
            }

            return View(trip);
        }
        // ------------------------------------------------------------------START OF CREATE NEW TRIP

        // GET: Trips/Create
        public async Task<IActionResult> Create()
        {
            //get continents to build out drop down in viewmodel
            List<Continent> AllContinents = await _context.Continent.ToListAsync();


            List<SelectListItem> allContinentOptions = new List<SelectListItem>();

            foreach(Continent c in AllContinents) 
            {
                SelectListItem sli = new SelectListItem();
                sli.Text = c.Name;
                sli.Value = c.ContinentId.ToString();
                allContinentOptions.Add(sli);
            };


            SelectListItem defaultSli = new SelectListItem
            {
                Text = "Select Continent",
                Value = "0"
            };

            allContinentOptions.Insert(0, defaultSli);

            CreateTripViewModel viewmodel = new CreateTripViewModel
            {
                AllContinentOptions = allContinentOptions
            };

            //get TravelTypes to build out secect checkboxes in the the viewmodel
            viewmodel.AllTravelTypes = _context.TravelType
                .AsEnumerable()
                .Select(li => new SelectListItem
                {
                    Text = li.Type,
                    Value = li.TravelTypeId.ToString()
                }).ToList();
            ;

           // viewmodel.LocationTypes = await _context.LocationType.ToListAsync();


            ViewData["scripts"] = new List<string>() {
                "visitLocation"
            };

            return View(viewmodel);
        }

        // POST: Trips/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateTripViewModel viewmodel)
        {

            ModelState.Remove("Trip.User");
            ModelState.Remove("Trip.UserId");
            
            ApplicationUser user = await GetCurrentUserAsync();

            viewmodel.Trip.UserId = user.Id;
            viewmodel.Trip.IsPreTrip = true;

            if (ModelState.IsValid)
            {
                _context.Add(viewmodel.Trip);

                //checks to see if there are selectedTravelTypeIds to loop over 
                if (viewmodel.SelectedTravelTypeIds != null)
                { 
                //makes joiner table for TripTravelType 
                    foreach (int TypeId in viewmodel.SelectedTravelTypeIds)
                    {
                        TripTravelType newTripTT = new TripTravelType()
                        {   //pulls tripid out of context bag 
                            TripId = viewmodel.Trip.TripId,
                            TravelTypeId = TypeId
                        };

                        _context.Add(newTripTT);
                    }
                }

                //this runs though all the inputed food places and makes a joiner table for it
                if (viewmodel.EnteredTripFoodLocations != null)
                {
                    foreach (TripVisitLocation foodL in viewmodel.EnteredTripFoodLocations)
                    {
                        TripVisitLocation newTripVL = new TripVisitLocation()
                        {
                            TripId = viewmodel.Trip.TripId,
                            LocationTypeId = 1,
                            Name = foodL.Name,
                            Description = foodL.Description,
                            IsCompleted = false
                        };

                        _context.Add(newTripVL);
                    }
                }

                //this runs though all the inputed food places and makes a joiner table for it
                if (viewmodel.EnteredTripVisitLocations != null)
                {
                    foreach (TripVisitLocation placeL in viewmodel.EnteredTripVisitLocations)
                    {
                        TripVisitLocation newTripVL = new TripVisitLocation()
                        {
                            TripId = viewmodel.Trip.TripId,
                            LocationTypeId = 2,
                            Name = placeL.Name,
                            Description = placeL.Description,
                            IsCompleted = false
                        };

                        _context.Add(newTripVL);
                    }
                }
           
                await _context.SaveChangesAsync();

                return RedirectToAction("PlannedTrips", "Trips");
            }

            return View(viewmodel);
        }
        // ------------------------------------------------------------------------END OF CREATE NEW TRIP

        //--------------------------------------------------------------------------START FINISH TRIP CREATE

        public async Task<IActionResult> FinishTrip(int id)
        {
            Trip trip = await _context.Trip
                .Include(t => t.Continent)
                .Include(t => t.TripTravelTypes)
                .Include(t => t.TripVisitLocations)
                .FirstOrDefaultAsync(t => t.TripId == id);

            List<TravelType> travelTypes = await _context.TripTravelType
                .Include(t => t.TravelType)
                .Where(t => t.TripId == trip.TripId)
                .Select(t => t.TravelType)
                .ToListAsync();

            FinishTripViewModel viewmodel = new FinishTripViewModel
            {
                Trip = trip,
                TravelTypes = travelTypes
            };
   

            //this builds up the foodlocations in checkbox form so the user can select which ones they made it too
            viewmodel.FoodLocations = trip.TripVisitLocations.Where(tvl => tvl.LocationTypeId == 1)
                //.AsEnumerable()
                .Select(li => new SelectListItem
                {
                    Text = li.Name,
                    Value = li.TripVisitLocationId.ToString()
                }).ToList();
            ;

            //this builds up the foodlocations in checkbox form so the user can select which ones they made it too
            viewmodel.PlaceLocations = trip.TripVisitLocations.Where(tvl => tvl.LocationTypeId == 2)
                .AsEnumerable()
                .Select(li => new SelectListItem
                {
                    Text = li.Name,
                    Value = li.TripVisitLocationId.ToString()
                }).ToList();
            ;


            return View(viewmodel);
        }

        [HttpPost]
        public async Task<IActionResult> FinishTrip(int id, FinishTripViewModel viewModel)
        {
            ModelState.Remove("Trip.User");
            ModelState.Remove("Trip.UserId");
            
            ApplicationUser user = await GetCurrentUserAsync();

            //viewmodel.Trip.UserId = user.Id;
            //viewmodel.Trip.IsPreTrip = true;

            if (ModelState.IsValid)
            {

                Trip trip = await _context.Trip
                    .Include(t => t.TripVisitLocations)
                    .FirstOrDefaultAsync(t => t.TripId == id);

                if (viewModel.SelectedFoodLocationIds != null)
                {
                    foreach (var tvl in trip.TripVisitLocations)
                    {
                    //this checks the selcted foodLocIds again the list of foodLocs to see which ones were selected with the checkboxed so it can find the ones in needs to update the status of
                        if (viewModel.SelectedFoodLocationIds.Any(item => item == tvl.TripVisitLocationId))
                        {
                            tvl.IsCompleted = true;
                            _context.Update(tvl);
                        }
                    }
                }

                foreach (TripRetro tripRetro in viewModel.TripRetros)
                {
                    tripRetro.TripId = id;
                    _context.Add(tripRetro);
                };

                trip.IsPreTrip = false;

                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Trips");
            }
            return View("Index");
        }

        //--------------------------------------------------------------------------END FINISH TRIP CREATE

        //-----------------------------------------------------------------------START PLANNED TRIP EDIT
        // GET: Trips/Edit/5
        public async Task<IActionResult> PlannedTripEdit(int id)
        {
            Trip trip = await _context.Trip
             .Include(t => t.TripTravelTypes)
             .Include(t => t.TripVisitLocations)
             .FirstOrDefaultAsync(t => t.TripId == id);

            if (trip == null)
            {
                return NotFound();
            }

            List<Continent> AllContinents = await _context.Continent.ToListAsync();

            List<SelectListItem> allContinentOptions = new List<SelectListItem>();

            foreach (Continent c in AllContinents)
            {
                SelectListItem sli = new SelectListItem();
                sli.Text = c.Name;
                sli.Value = c.ContinentId.ToString();
                allContinentOptions.Add(sli);
            };

            EditPlannedTripViewModel viewmodel = new EditPlannedTripViewModel
            {
                AllContinentOptions = allContinentOptions,
                Trip = trip,
                CurrentFoodLocations = trip.TripVisitLocations.Where(VisitLoc => VisitLoc.LocationTypeId == 1).ToList(),
                CurrentVisitLocations = trip.TripVisitLocations.Where(VisitLoc => VisitLoc.LocationTypeId == 2).ToList()
            };

            //get TravelTypes
            List<TravelType> AllTravelTypes = _context.TravelType.ToList();

            //get a list of the travelTypes for this trip
            List<TravelType> PrevSelectedTravelTypes = _context.TripTravelType
                .Include(t => t.TravelType)
                .Where(t => t.TripId == trip.TripId)
                .Select(t => t.TravelType)
                .ToList();

            //makes an empty list to hold selectListItems
            List<SelectListItem> DisplayTripTravelTypes = new List<SelectListItem>();

            //this loops over allTravelTypes
            //any returns a bool of true or false base on if the condition that was passed in is met
            //I use the bool value it returns to set the checked value on the selectListItems for my check boxes
            foreach (TravelType TravelType in AllTravelTypes)
            {   
                bool newList = PrevSelectedTravelTypes.Any(item => item.TravelTypeId == TravelType.TravelTypeId);
                DisplayTripTravelTypes.Add(new SelectListItem
                {
                    Text = TravelType.Type,
                    Value = TravelType.TravelTypeId.ToString(),
                    Selected = newList
                });
            }

            viewmodel.AllTravelTypes = DisplayTripTravelTypes;

            ViewData["scripts"] = new List<string>() {
                "EditPlannedTrip"
            };

            return View(viewmodel);
        }

        // POST: Trips/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PlannedTripEdit(int id, EditPlannedTripViewModel viewModel)
        {
            ModelState.Remove("Trip.User");
            ModelState.Remove("Trip.UserId");

            //ApplicationUser user = await GetCurrentUserAsync();

            //viewModel.Trip.UserId = user.Id;

            if (ModelState.IsValid)
            {
                try
                {
                    Trip trip = await _context.Trip
                    .Include(t => t.TripTravelTypes)
                    .Include(t => t.TripVisitLocations)
                    .SingleOrDefaultAsync(t => t.TripId == id);

                    //This checks if there are any joiner tables of this kind for this trip,
                    //then it foreaches over the joiner table and delets each one from the db
                    //this deletes all TripTravelTypes the joiner tables 
                    if (trip.TripTravelTypes.Count > 0)
                    {
                        foreach (TripTravelType travelType in trip.TripTravelTypes)
                        {
                            //this says for each one of the joiner tables put it in the _context bag to get deleted on _context.SaveChangesAsync
                            _context.Remove(travelType);
                        }
                    }

                    //this builds up TripTravelType tables for each TravelType thats selected 
                    //checks to see if there are selectedTravelTypeIds to loop over 
                    if (viewModel.SelectedTravelTypeIds != null)
                    {
                        //makes joiner table for TripTravelType 
                        foreach (int TypeId in viewModel.SelectedTravelTypeIds)
                        {
                            TripTravelType newTripTT = new TripTravelType()
                            {   //pulls tripid out of context bag 
                                TripId = viewModel.Trip.TripId,
                                TravelTypeId = TypeId
                            };

                            _context.Add(newTripTT);
                        }
                    }

                    // This deletes all the TripVisitLocations joiner tables 
                    if (trip.TripVisitLocations.Count > 0)
                    {
                        foreach (TripVisitLocation location in trip.TripVisitLocations)
                        {
                            _context.Remove(location);
                        }
                    }

                    //this builds up the TripVisitLocation for food and adds it to the db context 
                    if (viewModel.NewFoodLocations.Count > 0)
                    {
                        foreach (TripVisitLocation location in viewModel.NewFoodLocations)
                        {
                            TripVisitLocation newTripVL = new TripVisitLocation()
                            {
                                TripId = viewModel.Trip.TripId,
                                LocationTypeId = 1,
                                Name = location.Name,
                                Description = location.Description,
                                IsCompleted = false
                            };
                                _context.Add(newTripVL);
                        }
                    }

                    //this builds up the TripVisitLocation for places and adds it to the db context 
                    if (viewModel.NewVisitLocations.Count > 0)
                    { 
                        foreach (TripVisitLocation location in viewModel.NewVisitLocations)
                        {
                            TripVisitLocation newTripVL = new TripVisitLocation()
                            {
                                TripId = viewModel.Trip.TripId,
                                LocationTypeId = 2,
                                Name = location.Name,
                                Description = location.Description,
                                IsCompleted = false
                            };
                                _context.Add(newTripVL);
                        }
                    }

                    trip.Location = viewModel.Trip.Location;
                    trip.Accommodation = viewModel.Trip.Accommodation;
                    trip.Budget = viewModel.Trip.Budget;
                    trip.ContinentId = viewModel.Trip.ContinentId;
                    trip.IsPreTrip = true;
                    trip.Title = viewModel.Trip.Title;
                    trip.TripDates = viewModel.Trip.TripDates;

                    _context.Update(trip);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TripExists(viewModel.Trip.TripId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("PlannedTrips", "Trips");
            }

            return View(viewModel);
        }
        //----------------------------------------------------------END PLANNED TRIP EDIT
  
        // GET: Trips/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var trip = await _context.Trip.FindAsync(id);
        //    if (trip == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["ContinentId"] = new SelectList(_context.Continent, "ContinentId", "Code", trip.ContinentId);
        //    ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", trip.UserId);
        //    return View(trip);
        //}

        // POST: Trips/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("TripId,UserId,ContinentId,Location,TripDates,Accommodation,Title,Budget,IsPreTrip")] Trip trip)
        //{
        //    if (id != trip.TripId)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(trip);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!TripExists(trip.TripId))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["ContinentId"] = new SelectList(_context.Continent, "ContinentId", "Code", trip.ContinentId);
        //    ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", trip.UserId);
        //    return View(trip);
        //}

        //------------------------------------------------------------------START OF PLANNED TRIP DELETE
        // GET: Trips/Delete/5
        public async Task<IActionResult> PlannedTripDelete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            //This gets the trip I selected details so I can diplay them in the delete confirm
            var trip = await _context.Trip
                .Include(t => t.Continent)
                .FirstOrDefaultAsync(m => m.TripId == id);
            if (trip == null)
            {
                return NotFound();
            }

            return View(trip);
        }

        // POST: Trips/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PlannedTripDeleteConfirmed(int id)
        {
            //This gets the trip and includes the joiner tables 
            var trip = await _context.Trip
                .Include(t => t.TripTravelTypes)
                .Include(t => t.TripVisitLocations)
                .SingleOrDefaultAsync(t => t.TripId == id);

                //This checks if there are any joiner tables of this kind for this trip,
                //then it foreaches over the joiner table and delets each one from the db
            if (trip.TripTravelTypes.Count > 0)
            {
                foreach (TripTravelType travelType in trip.TripTravelTypes) 
                {
                    //this says for each one of the joiner tables put it in the _context bag to get deleted on _context.SaveChangesAsync
                    _context.Remove(travelType);
                }
            }
            //this does the same thing the one above does ^
            if (trip.TripVisitLocations.Count > 0)
            {
                foreach (TripVisitLocation visitLocation in trip.TripVisitLocations)
                {
                    _context.Remove(visitLocation);
                }
            }
            //this removes the trip adds the trip to the _context bag before is saves all the changes 
            _context.Remove(trip);
            await _context.SaveChangesAsync();
            return RedirectToAction("PlannedTrips", "Trips");
        }

        //-------------------------------------------------------------------END OF PLANNED TRIP DELETE 

        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var trip = await _context.Trip
        //        .Include(t => t.Continent)
        //        .Include(t => t.User)
        //        .FirstOrDefaultAsync(m => m.TripId == id);
        //    if (trip == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(trip);
        //}

        // POST: Trips/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var trip = await _context.Trip.FindAsync(id);
        //    _context.Trip.Remove(trip);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        private bool TripExists(int id)
        {
            return _context.Trip.Any(e => e.TripId == id);
        }
    }
}
