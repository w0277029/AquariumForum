using System.Diagnostics;
using AquariumForum.Data;
using AquariumForum.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AquariumForum.Controllers
{
    public class HomeController : Controller
    {
        private readonly AquariumForumContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        //private readonly ILogger<HomeController> _logger;

        // Constructor
        public HomeController(AquariumForumContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Display profile by id - ../Home/Profile/5
        public async Task<IActionResult> Profile(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            var discussions = await _context.Discussion
                .Where(d => d.ApplicationUserId == id)
                .Include(d => d.Comments)
                .OrderByDescending(d => d.CreateDate)
                .ToListAsync();

            ViewBag.User = user;

            return View("Profile", discussions);
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
                .Include(d => d.ApplicationUser)
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
                    .ThenInclude(m => m.ApplicationUser) // Comment author
                .Include(m => m.ApplicationUser) // Discussion author
                .FirstOrDefaultAsync(m => m.DiscussionId == id);

            if (discussion == null)
            {
                return NotFound();
            }

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
