using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using travoul.Data;
using travoul.Models;
using travoul.Models.ViewModels;
using travoul.Models.ViewModels.PaginationModels;

namespace travoul.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ApplicationDbContext ctx,
                          UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _context = ctx;
        }

        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        //Home page method
        [Authorize]
        public async Task<IActionResult> Index()
        {
            return View();
        }

        //Results of searching all trips
        public async Task<IActionResult> TripSearchAll(int? page, string search, TripSearchViewModel viewModel)
        {
            var trips = await _context.Trip
                .Include(t => t.Continent)
                .Include(t => t.User)
                .Where(t => t.IsPreTrip == false && (t.Title.Contains(viewModel.Search) || t.Location.Contains(viewModel.Search) || t.Continent.Name.Contains(viewModel.Search)))
                .OrderByDescending(t => t.DateFinished)
                .ToListAsync();

            viewModel.Pager = new Pager(trips.Count(), page);
            viewModel.Trips = trips.Skip((viewModel.Pager.CurrentPage - 1) * viewModel.Pager.PageSize).Take(viewModel.Pager.PageSize).ToList();
            
            return View(viewModel);
        }

        //This method gets details for all trips from the search results from the home page
        public async Task<IActionResult> AllTripsDetails(int? id)
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

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
