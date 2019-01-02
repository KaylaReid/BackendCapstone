using System;
using System.Collections.Generic;
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
        public async Task<IActionResult> TripSearchAll(string Search, TripSearchViewModel viewModel)
        {
            List<Trip> trips = await _context.Trip
                .Include(t => t.Continent)
                .Where(t => t.IsPreTrip == false && t.Title.Contains(viewModel.Search) || t.Location.Contains(viewModel.Search) || t.Continent.Name.Contains(viewModel.Search)).ToListAsync();

            viewModel.Trips = trips;

            return View(viewModel);
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
