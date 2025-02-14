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

            //var discussions = await _context.Discussion.ToListAsync();

            // Get the discussions from DB descending, including comments
            var discussions = await _context.Discussion
                .OrderByDescending(d => d.CreateDate)
                .Include(d => d.Comments)
                .ToListAsync();

            return View(discussions);
        }

        // Display a discussion by ID - ../Home/DiscussionDetails/328
        public async Task<IActionResult> DiscussionDetails(int id)
        {
            // Get discussion from DB by ID
            var discussion = await _context.Discussion
                .Include(m => m.Comments)
                .FirstOrDefaultAsync(m => m.DiscussionId == id);

            return View(discussion);
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
