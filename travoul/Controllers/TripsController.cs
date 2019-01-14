using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using travoul.Core;
using travoul.Models;
using travoul.Models.ViewModels;
using travoul.Models.ViewModels.PaginationModels;

namespace travoul.Controllers
{
    [Authorize]
    public class TripsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IManager _manager;

        public TripsController(UserManager<ApplicationUser> userManager, IManager manager)
        {
            _userManager = userManager;
            _manager = manager;
        }

        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        // GET: PlannedTrips --trips planned for the future
        public async Task<IActionResult> PlannedTrips()
        {
            var user = await GetCurrentUserAsync();

            return View(_manager.GetPlannedTrips(user.Id));
        }

        //search method
        public async Task<IActionResult> TripSearch(bool preTrip, int? page, string search)
        {
            var user = await GetCurrentUserAsync();

            var viewModel = new TripSearchViewModel();

            if (preTrip == false)
            {
                var pager = new Pager(0, page);
                viewModel.Search = search;
                viewModel.Trips = _manager.SearchTrips(user.Id, search, preTrip, ref pager);
                viewModel.Pager = pager;

                return View("FinishedTripSearch", viewModel);
            }

            viewModel.Trips = _manager.SearchTrips(user.Id, search, preTrip);
            viewModel.Search = search;

            return View("PlannedTripSearch", viewModel);
        }


        // GET: MyTrips --all finished trips
        public async Task<IActionResult> Index(int? page)
        {
            var user = await GetCurrentUserAsync();
            var pager = new Pager(0, page);
            var trips = _manager.GetMyTrips(user.Id, ref pager);

            ViewData["scripts"] = new List<string>
            {
                "Loader"
            };

            return View(new FinishedTripIndexViewModel
            {
                Trips = trips,
                Pager = pager
            });

        }

        // GET: MyTrips/Details/5  --get details for finsihed trips
        public async Task<IActionResult> Details(int? id)
        {
            if (id.HasValue)
            {
                return NotFound();
            }

            var trip = _manager.GetFinsihedTripDetails(id.Value);

            if (trip == null)
            {
                return NotFound();
            }

            return View(trip);
        }

        // GET: PlannedTrip/Details/5  --get details for future trips
        public async Task<IActionResult> PlannedTripDetails(int? id)
        {
            if (id.HasValue)
            {
                return NotFound();
            }


            var trip = _manager.GetFutureTripDetails(id.Value);

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
            var allContinentOptions = _manager.GetContinents().AsEnumerable()
                .Select(li => new SelectListItem
                {
                    Text = li.Name,
                    Value = li.ContinentId.ToString()
                }).ToList();

            allContinentOptions.Insert(0, new SelectListItem
            {
                Text = "Select Continent",
                Value = "0"
            });

            //get TravelTypes to build out secect checkboxes in the the viewmodel
            // viewmodel.LocationTypes = await _context.LocationType.ToListAsync();

            ViewData["scripts"] = new List<string>
            {
                "CreateTrip"
            };

            return View(new CreateTripViewModel
            {
                AllContinentOptions = allContinentOptions,
                AllTravelTypes = _manager.GetTravelTypes()
                    .AsEnumerable()
                    .Select(li => new SelectListItem
                    {
                        Text = li.Type,
                        Value = li.TravelTypeId.ToString()
                    }).ToList()
            });
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

            var user = await GetCurrentUserAsync();

            viewmodel.Trip.UserId = user.Id;
            viewmodel.Trip.IsPreTrip = true;

            if (ModelState.IsValid)
            {
                _manager.Create(viewmodel.Trip, viewmodel.SelectedTravelTypeIds, viewmodel.EnteredTripFoodLocations, viewmodel.EnteredTripVisitLocations);

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
            var trip = await _context.Trip
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


            var viewmodel = new FinishTripViewModel
            {
                Trip = trip,
                //TravelTypes = travelTypes,
                AllLocations = trip.TripVisitLocations.ToList(),
                FoodLocations = trip.TripVisitLocations.Where(tvl => tvl.LocationTypeId == 1)
                    //.AsEnumerable()
                    .Select(li => new SelectListItem
                    {
                        Text = li.Name,
                        Value = li.TripVisitLocationId.ToString(),
                        Selected = false
                    }).ToList(),
                PlaceLocations = trip.TripVisitLocations.Where(tvl => tvl.LocationTypeId == 2)
                    .AsEnumerable()
                    .Select(li => new SelectListItem
                    {
                        Text = li.Name,
                        Value = li.TripVisitLocationId.ToString(),
                        Selected = false
                    }).ToList(),
                TravelTypes = trip.TripTravelTypes.Select(ttt => ttt.TravelType).ToList()
            };

            var allTravelTypes = await _context.TravelType.ToListAsync();

            //makes an empty list to hold selectListItems
            var displayTripTravelTypes = new List<SelectListItem>();

            foreach (var travelType in allTravelTypes)
            {
                var newList = viewmodel.TravelTypes.Any(item => item.TravelTypeId == travelType.TravelTypeId);

                displayTripTravelTypes.Add(new SelectListItem
                {
                    Text = travelType.Type,
                    Value = travelType.TravelTypeId.ToString(),
                    Selected = newList
                });
            }

            viewmodel.AllTravelTypes = displayTripTravelTypes;

            ViewData["scripts"] = new List<string>
            {
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

            var trip = await _context.Trip
                .Include(t => t.TripTravelTypes)
                .Include(t => t.TripVisitLocations)
                .FirstOrDefaultAsync(t => t.TripId == id);


            //This checks if there are any joiner tables of this kind for this trip,
            //then it foreaches over the joiner table and delets each one from the db
            //this deletes all TripTravelTypes the joiner tables 
            if (trip.TripTravelTypes.Count > 0)
            {
                foreach (var travelType in trip.TripTravelTypes)
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
                foreach (var typeId in viewModel.SelectedTravelTypeIds)
                {
                    var newTripTt = new TripTravelType
                    {
                        TripId = trip.TripId,
                        TravelTypeId = typeId
                    };

                    _context.Add(newTripTt);
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
                foreach (var foodVl in viewModel.NewFoods)
                {
                    foodVl.LocationTypeId = 1;
                    foodVl.TripId = trip.TripId;
                    _context.Add(foodVl);
                }
            }

            if (viewModel.NewPlaces != null)
            {
                foreach (var placeVl in viewModel.NewPlaces)
                {
                    placeVl.LocationTypeId = 2;
                    placeVl.TripId = trip.TripId;
                    _context.Add(placeVl);
                }
            }

            _context.Add(new TripRetro
            {
                TripId = id,
                RetroTypeId = 1,
                Description = viewModel.DoAgain.Description
            });

            _context.Add(new TripRetro
            {
                TripId = id,
                RetroTypeId = 2,
                Description = viewModel.DoDifferent.Description
            });


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
            var trip = await _context.Trip
             .Include(t => t.TripTravelTypes)
             .Include(t => t.TripVisitLocations)
             .FirstOrDefaultAsync(t => t.TripId == id);

            if (trip == null)
            {
                return NotFound();
            }

            var allContinents = await _context.Continent.ToListAsync();

            var allContinentOptions = new List<SelectListItem>();

            foreach (var c in allContinents)
            {
                var sli = new SelectListItem
                {
                    Text = c.Name,
                    Value = c.ContinentId.ToString()
                };
                allContinentOptions.Add(sli);
            };

            var viewmodel = new EditPlannedTripViewModel
            {
                AllContinentOptions = allContinentOptions,
                Trip = trip,
                CurrentFoodLocations = trip.TripVisitLocations.Where(visitLoc => visitLoc.LocationTypeId == 1).ToList(),
                CurrentVisitLocations = trip.TripVisitLocations.Where(visitLoc => visitLoc.LocationTypeId == 2).ToList()
            };

            //get TravelTypes
            var allTravelTypes = _context.TravelType.ToList();

            //get a list of the travelTypes for this trip
            var prevSelectedTravelTypes = _context.TripTravelType
                .Include(t => t.TravelType)
                .Where(t => t.TripId == trip.TripId)
                .Select(t => t.TravelType)
                .ToList();

            //makes an empty list to hold selectListItems
            var displayTripTravelTypes = new List<SelectListItem>();

            //this loops over allTravelTypes
            //any returns a bool of true or false base on if the condition that was passed in is met
            //I use the bool value it returns to set the checked value on the selectListItems for my check boxes
            foreach (var travelType in allTravelTypes)
            {
                var newList = prevSelectedTravelTypes.Any(item => item.TravelTypeId == travelType.TravelTypeId);
                displayTripTravelTypes.Add(new SelectListItem
                {
                    Text = travelType.Type,
                    Value = travelType.TravelTypeId.ToString(),
                    Selected = newList
                });
            }

            viewmodel.AllTravelTypes = displayTripTravelTypes;

            ViewData["scripts"] = new List<string>
            {
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
                    var trip = await _context.Trip
                    .Include(t => t.TripTravelTypes)
                    .Include(t => t.TripVisitLocations)
                    .SingleOrDefaultAsync(t => t.TripId == id);

                    //This checks if there are any joiner tables of this kind for this trip,
                    //then it foreaches over the joiner table and delets each one from the db
                    //this deletes all TripTravelTypes the joiner tables 
                    if (trip.TripTravelTypes.Count > 0)
                    {
                        foreach (var travelType in trip.TripTravelTypes)
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
                        foreach (var typeId in viewModel.SelectedTravelTypeIds)
                        {
                            var newTripTt = new TripTravelType
                            {   //pulls tripid out of context bag 
                                TripId = trip.TripId,
                                TravelTypeId = typeId
                            };

                            _context.Add(newTripTt);
                        }
                    }

                    // This deletes all the TripVisitLocations joiner tables 
                    if (trip.TripVisitLocations.Count > 0)
                    {
                        foreach (var location in trip.TripVisitLocations)
                        {
                            _context.Remove(location);
                        }
                    }

                    //this builds up the TripVisitLocation for food and adds it to the db context 
                    if (viewModel.NewFoodLocations.Count > 0)
                    {
                        foreach (var location in viewModel.NewFoodLocations)
                        {
                            var newTripVl = new TripVisitLocation
                            {
                                TripId = trip.TripId,
                                LocationTypeId = 1,
                                Name = location.Name,
                                Description = location.Description,
                                IsCompleted = false
                            };
                            _context.Add(newTripVl);
                        }
                    }

                    //this builds up the TripVisitLocation for places and adds it to the db context 
                    if (viewModel.NewVisitLocations.Count > 0)
                    {
                        foreach (var location in viewModel.NewVisitLocations)
                        {
                            var newTripVl = new TripVisitLocation
                            {
                                TripId = trip.TripId,
                                LocationTypeId = 2,
                                Name = location.Name,
                                Description = location.Description,
                                IsCompleted = false
                            };
                            _context.Add(newTripVl);
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

                    throw;
                }
            }

            return View(viewModel);
        }
        //----------------------------------------------------------END PLANNED TRIP EDIT
        //-----------------------------------------------------------START FINISHED TRIP EDIT 

        //GET
        public async Task<IActionResult> FinishedTripEdit(int id)
        {
            var trip = await _context.Trip
             .Include(t => t.TripTravelTypes)
             .Include(t => t.TripVisitLocations)
             .Include(t => t.TripRetros)
             .FirstOrDefaultAsync(t => t.TripId == id);

            if (trip == null)
            {
                return NotFound();
            }

            var allContinents = await _context.Continent.ToListAsync();

            var allContinentOptions = new List<SelectListItem>();

            foreach (var c in allContinents)
            {
                var sli = new SelectListItem
                {
                    Text = c.Name,
                    Value = c.ContinentId.ToString()
                };
                allContinentOptions.Add(sli);
            };

            var viewModel = new EditFinishedTripViewModel
            {
                AllContinentOptions = allContinentOptions,
                Trip = trip,
                CurrentFoodLocations = trip.TripVisitLocations.Where(visitLoc => visitLoc.LocationTypeId == 1).ToList(),
                CurrentVisitLocations = trip.TripVisitLocations.Where(visitLoc => visitLoc.LocationTypeId == 2).ToList(),
                DoAgainRetro = trip.TripRetros.Single(tr => tr.RetroTypeId == 1),
                DoDifferentRetro = trip.TripRetros.Single(tr => tr.RetroTypeId == 2)
            };

            //get TravelTypes
            var allTravelTypes = _context.TravelType.ToList();

            //get a list of the travelTypes for this trip
            var prevSelectedTravelTypes = _context.TripTravelType
                .Include(t => t.TravelType)
                .Where(t => t.TripId == trip.TripId)
                .Select(t => t.TravelType)
                .ToList();

            //makes an empty list to hold selectListItems
            var displayTripTravelTypes = new List<SelectListItem>();

            //this loops over allTravelTypes
            //any returns a bool of true or false base on if the condition that was passed in is met
            //I use the bool value it returns to set the checked value on the selectListItems for my check boxes
            foreach (var travelType in allTravelTypes)
            {
                var newList = prevSelectedTravelTypes.Any(item => item.TravelTypeId == travelType.TravelTypeId);
                displayTripTravelTypes.Add(new SelectListItem
                {
                    Text = travelType.Type,
                    Value = travelType.TravelTypeId.ToString(),
                    Selected = newList
                });
            }

            viewModel.AllTravelTypes = displayTripTravelTypes;

            ViewData["scripts"] = new List<string>
            {
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


            var foodLocations = new List<TripVisitLocation>();
            var visitLocations = new List<TripVisitLocation>();


            foreach (var foodLocation in viewModel.NewFoodLocations)
            {
                if (foodLocation.Name != null)
                {
                    foodLocations.Add(foodLocation);
                }
            }

            foreach (var location in viewModel.NewVisitLocations)
            {
                if (location.Name != null)
                {
                    visitLocations.Add(location);
                }
            }

            viewModel.NewFoodLocations = foodLocations;
            viewModel.NewVisitLocations = visitLocations;

            var trip = await _context.Trip
                .Include(t => t.TripTravelTypes)
                .Include(t => t.TripVisitLocations)
                .Include(t => t.TripRetros)
                .SingleOrDefaultAsync(t => t.TripId == id);


            //This checks if there are any joiner tables of this kind for this trip,
            //then it foreaches over the joiner table and delets each one from the db
            //this deletes all TripTravelTypes the joiner tables 
            if (trip.TripTravelTypes.Count > 0)
            {
                foreach (var travelType in trip.TripTravelTypes)
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
                foreach (var typeId in viewModel.SelectedTravelTypeIds)
                {
                    var newTripTt = new TripTravelType
                    {   //pulls tripid out of context bag 
                        TripId = trip.TripId,
                        TravelTypeId = typeId
                    };

                    _context.Add(newTripTt);
                }
            }

            // This deletes all the TripVisitLocations joiner tables 
            if (trip.TripVisitLocations.Count > 0)
            {
                foreach (var location in trip.TripVisitLocations)
                {
                    _context.Remove(location);
                }
            }

            //this builds up the TripVisitLocation for food and adds it to the db context
            if (viewModel.NewVisitLocations.Count > 0)
            {
                foreach (var location in viewModel.NewVisitLocations)
                {
                    var newTripVl = new TripVisitLocation
                    {
                        TripId = trip.TripId,
                        LocationTypeId = 2,
                        Name = location.Name,
                        Description = location.Description,
                        IsCompleted = location.IsCompleted
                    };
                    _context.Add(newTripVl);
                }
            }

            //this builds up the TripVisitLocation for food and adds it to the db context 
            if (viewModel.NewFoodLocations.Count > 0)
            {
                foreach (var location in viewModel.NewFoodLocations)
                {
                    var newTripVl = new TripVisitLocation
                    {
                        TripId = trip.TripId,
                        LocationTypeId = 1,
                        Name = location.Name,
                        Description = location.Description,
                        IsCompleted = location.IsCompleted
                    };
                    _context.Add(newTripVl);
                }
            }

            //this gets the DoAgain retro 
            var doAgainTripRetro = await _context.TripRetro
                .Where(tr => tr.TripId == id && tr.RetroTypeId == 1).FirstOrDefaultAsync();

            //This updates the DoAgain retro
            doAgainTripRetro.Description = viewModel.DoAgainRetro.Description;
            _context.Update(doAgainTripRetro);


            var doDifferentTripRetro = await _context.TripRetro
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
            var trip = await _context.Trip
                .Include(t => t.TripTravelTypes)
                .Include(t => t.TripVisitLocations)
                .Include(t => t.TripRetros)
                .SingleOrDefaultAsync(t => t.TripId == id);

            //This checks if there are any joiner tables of this kind for this trip,
            //then it foreaches over the joiner table and delets each one from the db
            if (trip.TripTravelTypes.Count > 0)
            {
                foreach (var travelType in trip.TripTravelTypes)
                {
                    //this says for each one of the joiner tables put it in the _context bag to get deleted on _context.SaveChangesAsync
                    _context.Remove(travelType);
                }
            }
            //this does the same thing the one above does ^
            if (trip.TripVisitLocations.Count > 0)
            {
                foreach (var visitLocation in trip.TripVisitLocations)
                {
                    _context.Remove(visitLocation);
                }
            }
            //this does the same thing 
            if (trip.TripRetros.Count > 0)
            {
                foreach (var tripRetro in trip.TripRetros)
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
