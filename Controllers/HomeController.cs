using System.Diagnostics;
using AquariumForum.Data;
using AquariumForum.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AquariumForum.Controllers
{
    public class HomeController : Controller
    {
        private readonly AquariumForumContext _context;

        //private readonly ILogger<HomeController> _logger;

        // Constructor
        public HomeController(AquariumForumContext context)
        {
            _context = context;
        }

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}

        //Home page - display all discussions - ../ or ../Home/Index
        public async Task<IActionResult> Index()
        {
            // Get the discussions from DB
            var discussions = await _context.Discussion.ToListAsync();

            return View(discussions);
        }

        //public IActionResult Index()
        //{
        //    return View();
        //}

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
