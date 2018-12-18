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
        // ------------------------------------------------------------------START OF CREATE

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
        // ------------------------------------------------------------------------END OF CREATE

        //-----------------------------------------------------------------------START PLANNED TRIPS EDIT
        // GET: Trips/Edit/5
        public async Task<IActionResult> PlannedTripEdit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trip = await _context.Trip.FindAsync(id);
            if (trip == null)
            {
                return NotFound();
            }
            ViewData["ContinentId"] = new SelectList(_context.Continent, "ContinentId", "Code", trip.ContinentId);
            ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", trip.UserId);
            return View(trip);
        }

        // POST: Trips/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PlannedTripEdit(int id, [Bind("TripId,UserId,ContinentId,Location,TripDates,Accommodation,Title,Budget,IsPreTrip")] Trip trip)
        {
            if (id != trip.TripId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(trip);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TripExists(trip.TripId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ContinentId"] = new SelectList(_context.Continent, "ContinentId", "Code", trip.ContinentId);
            ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", trip.UserId);
            return View(trip);
        }
        //----------------------------------------------------------END PLANNED TRIPS EDIT
  
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

        //------------------------------------------------------------------START OF PLANNED TRIPS DELETE
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

        //-------------------------------------------------------------------END OF DELETE PLANNED TRIPS 

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
