using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using AquariumForum.Data;

namespace AquariumForum.Controllers
{

    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UsersController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        //public async Task<IActionResult> Profile(string id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var user = await _userManager.FindByIdAsync(id);

        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    return View("Index", user);
        //}

        //public IActionResult Index()
        //{
        //    return View();
        //}

        public async Task<IActionResult> Profile(string id)
        {
            return RedirectToAction("Profile", "Home", new { id });
        }
    }
}
