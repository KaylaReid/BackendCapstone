﻿using System;
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
using travoul.Models.ViewModels.PaginationModels;

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

            var UserTrips = await _context.Trip
                .Include(t => t.Continent)
                .Where(t => t.UserId == User.Id && t.IsPreTrip == true).ToListAsync();

            return View(UserTrips);
        }

        //search method
        public async Task<IActionResult> TripSearch(bool preTrip, int? page, string search)
        {
            var User = await GetCurrentUserAsync();
           
            List<Trip> trips = await _context.Trip
                .Include(t => t.Continent)
                .Where(t => t.UserId == User.Id && t.IsPreTrip == preTrip && (t.Title.Contains(search) || t.Location.Contains(search) || t.Continent.Name.Contains(search)))
                .OrderByDescending(t => t.DateFinished)
                .ToListAsync();

            TripSearchViewModel viewModel = new TripSearchViewModel();

            if (preTrip == false)
            {
                Pager pager = new Pager(trips.Count(), page);

                viewModel.Trips = trips.Skip((pager.CurrentPage - 1) * pager.PageSize)
                .Take(pager.PageSize).ToList();

                viewModel.Pager = pager;

                viewModel.Search = search;

                return View("FinishedTripSearch", viewModel);
            }
            else 
            {
                viewModel.Trips = trips;

                viewModel.Search = search;

                return View("PlannedTripSearch", viewModel);
            }
        }


        // GET: MyTrips --all finished trips
        public async Task<IActionResult> Index(int? page)
        {
            ApplicationUser User = await GetCurrentUserAsync();

            List<Trip> UserTrips = await _context.Trip
                .Include(t => t.Continent)
                .Where(t => t.UserId == User.Id && t.IsPreTrip == false)
                .OrderByDescending(t => t.DateFinished)
                .ToListAsync();

            Pager pager = new Pager(UserTrips.Count(), page);
            
            FinishedTripIndexViewModel viewmodel = new FinishedTripIndexViewModel();

            viewmodel.Trips = UserTrips.Skip((pager.CurrentPage - 1) * pager.PageSize)
                .Take(pager.PageSize).ToList();
            viewmodel.Pager = pager;

            ViewData["scripts"] = new List<string>() {
                "Loader"
            };

            return View(viewmodel);

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
                "CreateTrip"
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

        //public async Task<FinishTripViewModel> FinishTripView(int id)
        //{
        //    Trip trip = await _context.Trip
        //           .Include(t => t.Continent)
        //           .Include(t => t.TripTravelTypes)
        //           .Include(t => t.TripVisitLocations)
        //           .FirstOrDefaultAsync(t => t.TripId == id);

        //    List<TravelType> travelTypes = await _context.TripTravelType
        //        .Include(t => t.TravelType)
        //        .Where(t => t.TripId == trip.TripId)
        //        .Select(t => t.TravelType)
        //        .ToListAsync();

        //    FinishTripViewModel viewmodel = new FinishTripViewModel
        //    {
        //        Trip = trip,
        //        TravelTypes = travelTypes
        //    };


        //    //this builds up the foodlocations in checkbox form so the user can select which ones they made it too
        //    viewmodel.FoodLocations = trip.TripVisitLocations.Where(tvl => tvl.LocationTypeId == 1)
        //        //.AsEnumerable()
        //        .Select(li => new SelectListItem
        //        {
        //            Text = li.Name,
        //            Value = li.TripVisitLocationId.ToString()
        //        }).ToList();
        //    ;

        //    //this builds up the foodlocations in checkbox form so the user can select which ones they made it too
        //    viewmodel.PlaceLocations = trip.TripVisitLocations.Where(tvl => tvl.LocationTypeId == 2)
        //        .AsEnumerable()
        //        .Select(li => new SelectListItem
        //        {
        //            Text = li.Name,
        //            Value = li.TripVisitLocationId.ToString()
        //        }).ToList();
        //    ;

        //    ViewData["scripts"] = new List<string>() {
        //        "FinishTrip"
        //    };

        //    return viewmodel;
        //}

        public async Task<IActionResult> FinishTrip(int id)
        {
            Trip trip = await _context.Trip
                .Include(t => t.Continent)
                .Include(t => t.TripTravelTypes)
                .ThenInclude(tt => tt.TravelType)
                .Include(t => t.TripVisitLocations)
                .FirstOrDefaultAsync(t => t.TripId == id);

            //List<TravelType> travelTypes = await _context.TripTravelType
            //    .Include(t => t.TravelType)
            //    .Where(t => t.TripId == trip.TripId)
            //    .Select(t => t.TravelType)
            //    .ToListAsync();


            FinishTripViewModel viewmodel = new FinishTripViewModel
            {
                Trip = trip,
                //TravelTypes = travelTypes,
                AllLocations = trip.TripVisitLocations.ToList(),

            };


            //this builds up the foodlocations in checkbox form so the user can select which ones they made it too
            viewmodel.FoodLocations = trip.TripVisitLocations.Where(tvl => tvl.LocationTypeId == 1)
                //.AsEnumerable()
                .Select(li => new SelectListItem
                {
                    Text = li.Name,
                    Value = li.TripVisitLocationId.ToString(),
                    Selected = false
                }).ToList();
            ;

            //this builds up the foodlocations in checkbox form so the user can select which ones they made it too
            viewmodel.PlaceLocations = trip.TripVisitLocations.Where(tvl => tvl.LocationTypeId == 2)
                .AsEnumerable()
                .Select(li => new SelectListItem
                {
                    Text = li.Name,
                    Value = li.TripVisitLocationId.ToString(),
                    Selected = false
                }).ToList();
            ;

            List<TravelType> AllTravelTypes = await _context.TravelType.ToListAsync();


            List<TravelType> PrevSelectedTravelTypes = trip.TripTravelTypes.Select(ttt => ttt.TravelType).ToList();

            viewmodel.TravelTypes = PrevSelectedTravelTypes;

            //makes an empty list to hold selectListItems
            List<SelectListItem> DisplayTripTravelTypes = new List<SelectListItem>();


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
                "FinishTrip"
            };

            return View(viewmodel);
        }

        [HttpPost]
        public async Task<IActionResult> FinishTrip(int id, FinishTripViewModel viewModel)
        {
            ModelState.Remove("Trip.User");
            ModelState.Remove("Trip.UserId");
            ModelState.Remove("Trip.TripDates");
            ModelState.Remove("Trip.Location");
            ModelState.Remove("Trip.Accommodation");
            ModelState.Remove("Trip.Title");
            ModelState.Remove("Trip.ContinentId");

            
            if (!ModelState.IsValid)
            {

                return View(viewModel);
            }

            Trip trip = await _context.Trip
                .Include(t => t.TripTravelTypes)
                .Include(t => t.TripVisitLocations)
                .FirstOrDefaultAsync(t => t.TripId == id);


            //This checks if there are any joiner tables of this kind for this trip,
            //then it foreaches over the joiner table and delets each one from the db
            //this deletes all TripTravelTypes the joiner tables 
            if (trip.TripTravelTypes.Count > 0)
            {
                foreach (TripTravelType travelType in trip.TripTravelTypes)
                {
                    // wipe away previous trip travel types
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
                    {
                        TripId = trip.TripId,
                        TravelTypeId = TypeId
                    };

                    _context.Add(newTripTT);
                }
            }

            if (viewModel.SelectedLocationIds != null)
            {
                foreach (var tvl in trip.TripVisitLocations)
                {
                    //this checks the selcted foodLocIds again the list of Locs to see which ones were selected with the checkboxed so it can find the ones in needs to update the status of
                    if (viewModel.SelectedLocationIds.Any(item => item == tvl.TripVisitLocationId))
                    {
                        tvl.IsCompleted = true;
                        _context.Update(tvl);
                    }
                }
            }

            if (viewModel.NewFoods != null)
            {
                foreach (TripVisitLocation foodVL in viewModel.NewFoods)
                {
                    foodVL.LocationTypeId = 1;
                    foodVL.TripId = trip.TripId;
                    _context.Add(foodVL);
                }
            }

            if (viewModel.NewPlaces != null)
            {
                foreach (TripVisitLocation placeVL in viewModel.NewPlaces)
                {
                    placeVL.LocationTypeId = 2;
                    placeVL.TripId = trip.TripId;
                    _context.Add(placeVL);
                }
            }

            TripRetro DoAgainRetro = new TripRetro();
            DoAgainRetro.TripId = id;
            DoAgainRetro.RetroTypeId = 1;
            DoAgainRetro.Description = viewModel.DoAgain.Description;

            _context.Add(DoAgainRetro);

            TripRetro DoDifferent = new TripRetro();
            DoDifferent.TripId = id;
            DoDifferent.RetroTypeId = 2;
            DoDifferent.Description = viewModel.DoDifferent.Description;

            _context.Add(DoDifferent);


            trip.IsPreTrip = false;
            trip.ImagePath = viewModel.Trip.ImagePath;
            trip.DateFinished = DateTime.Now;

            _context.Update(trip);

            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Trips");
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
                                TripId = trip.TripId,
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
                                TripId = trip.TripId,
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
                                TripId = trip.TripId,
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

                    return RedirectToAction("PlannedTripDetails", "Trips", new { id = trip.TripId });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TripExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return View(viewModel);
        }
        //----------------------------------------------------------END PLANNED TRIP EDIT
        //-----------------------------------------------------------START FINISHED TRIP EDIT 

        //GET
        public async Task<IActionResult> FinishedTripEdit(int id)
        {
            Trip trip = await _context.Trip
             .Include(t => t.TripTravelTypes)
             .Include(t => t.TripVisitLocations)
             .Include(t => t.TripRetros)
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

            EditFinishedTripViewModel viewModel = new EditFinishedTripViewModel
            {
                AllContinentOptions = allContinentOptions,
                Trip = trip,
                CurrentFoodLocations = trip.TripVisitLocations.Where(VisitLoc => VisitLoc.LocationTypeId == 1).ToList(),
                CurrentVisitLocations = trip.TripVisitLocations.Where(VisitLoc => VisitLoc.LocationTypeId == 2).ToList(),
                DoAgainRetro = trip.TripRetros.Where(tr => tr.RetroTypeId == 1).Single(),
                DoDifferentRetro = trip.TripRetros.Where(tr => tr.RetroTypeId == 2).Single()
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

            viewModel.AllTravelTypes = DisplayTripTravelTypes;

            ViewData["scripts"] = new List<string>() {
                "FinishedTripEdit"
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> FinishedTripEdit(int id, EditFinishedTripViewModel viewModel)
        {
            //ModelState.Remove("Trip.User");
            //ModelState.Remove("Trip.UserId");


            List<TripVisitLocation> FoodLocations = new List<TripVisitLocation>();
            List<TripVisitLocation> VisitLocations = new List<TripVisitLocation>();


            for (var i = 0; i < viewModel.NewFoodLocations.Count; i++) 
            {
                if (viewModel.NewFoodLocations[i].Name != null)
                {
                    FoodLocations.Add(viewModel.NewFoodLocations[i]);
                }
            }

            for (var i = 0; i < viewModel.NewVisitLocations.Count; i++)
            {
                if (viewModel.NewVisitLocations[i].Name != null)
                {
                    VisitLocations.Add(viewModel.NewVisitLocations[i]);
                }
            }

            viewModel.NewFoodLocations = FoodLocations;
            viewModel.NewVisitLocations = VisitLocations;

            Trip trip = await _context.Trip
            .Include(t => t.TripTravelTypes)
            .Include(t => t.TripVisitLocations)
            .Include(t => t.TripRetros)
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
                        TripId = trip.TripId,
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
            if (viewModel.NewVisitLocations.Count > 0)
            {
                foreach (TripVisitLocation location in viewModel.NewVisitLocations)
                {
                    TripVisitLocation newTripVL = new TripVisitLocation()
                    {
                        TripId = trip.TripId,
                        LocationTypeId = 2,
                        Name = location.Name,
                        Description = location.Description,
                        IsCompleted = location.IsCompleted
                    };
                    _context.Add(newTripVL);
                }
            }

            //this builds up the TripVisitLocation for food and adds it to the db context 
            if (viewModel.NewFoodLocations.Count > 0)
            {
                foreach (TripVisitLocation location in viewModel.NewFoodLocations)
                {
                    TripVisitLocation newTripVL = new TripVisitLocation()
                    {
                        TripId = trip.TripId,
                        LocationTypeId = 1,
                        Name = location.Name,
                        Description = location.Description,
                        IsCompleted = location.IsCompleted
                    };
                    _context.Add(newTripVL);
                }
            }


            //this gets the DoAgain retro 
            TripRetro doAgainTripRetro = await _context.TripRetro
                .Where(tr => tr.TripId == id && tr.RetroTypeId == 1).FirstOrDefaultAsync();

            //This updates the DoAgain retro
            doAgainTripRetro.Description = viewModel.DoAgainRetro.Description;
            _context.Update(doAgainTripRetro);


            TripRetro doDifferentTripRetro = await _context.TripRetro
                .Where(tr => tr.TripId == id && tr.RetroTypeId == 2).FirstOrDefaultAsync();

            doDifferentTripRetro.Description = viewModel.DoDifferentRetro.Description;
            _context.Update(doDifferentTripRetro);

            trip.Location = viewModel.Trip.Location;
            trip.Accommodation = viewModel.Trip.Accommodation;
            trip.Budget = viewModel.Trip.Budget;
            trip.ContinentId = viewModel.Trip.ContinentId;
            trip.IsPreTrip = false;
            trip.Title = viewModel.Trip.Title;
            trip.TripDates = viewModel.Trip.TripDates;
            trip.ImagePath = viewModel.Trip.ImagePath;                    

            _context.Update(trip);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Trips", new { id = trip.TripId });
        }
        

        ////------------------------------------------------------------------START OF PLANNED TRIP DELETE
        //// POST: Trips/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> PlannedTripDeleteConfirmed(int id)
        //{
        //    //This gets the trip and includes the joiner tables 
        //    Trip trip = await _context.Trip
        //        .Include(t => t.TripTravelTypes)
        //        .Include(t => t.TripVisitLocations)
        //        .SingleOrDefaultAsync(t => t.TripId == id);

        //        //This checks if there are any joiner tables of this kind for this trip,
        //        //then it foreaches over the joiner table and delets each one from the db
        //    if (trip.TripTravelTypes.Count > 0)
        //    {
        //        foreach (TripTravelType travelType in trip.TripTravelTypes) 
        //        {
        //            //this says for each one of the joiner tables put it in the _context bag to get deleted on _context.SaveChangesAsync
        //            _context.Remove(travelType);
        //        }
        //    }
        //    //this does the same thing the one above does ^
        //    if (trip.TripVisitLocations.Count > 0)
        //    {
        //        foreach (TripVisitLocation visitLocation in trip.TripVisitLocations)
        //        {
        //            _context.Remove(visitLocation);
        //        }
        //    }
        //    //this removes the trip adds the trip to the _context bag before is saves all the changes 
        //    _context.Remove(trip);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction("PlannedTrips", "Trips");
        //}

        ////-------------------------------------------------------------------END OF PLANNED TRIP DELETE 

        //-------------------------------------------------------------------START OF DELETE TRIP

        //POST: Trips/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteTrip(int id)
        {
            //This gets the trip and includes the joiner tables 
            Trip trip = await _context.Trip
                .Include(t => t.TripTravelTypes)
                .Include(t => t.TripVisitLocations)
                .Include(t => t.TripRetros)
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
            //this does the same thing 
            if (trip.TripRetros.Count > 0)
            {
                foreach (TripRetro tripRetro in trip.TripRetros)
                {
                    _context.Remove(tripRetro);
                }
            }
            _context.Trip.Remove(trip);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Trips");
        }
        //-------------------------------------------------------------------------------END OF DELETE TRIP

        private bool TripExists(int id)
        {
            return _context.Trip.Any(e => e.TripId == id);
        }
    }
}
