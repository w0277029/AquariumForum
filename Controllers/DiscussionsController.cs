using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AquariumForum.Data;
using AquariumForum.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;


namespace AquariumForum.Controllers
{
    [Authorize]
    public class DiscussionsController : Controller
    {
        private readonly AquariumForumContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        // Constructor
        public DiscussionsController(AquariumForumContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Photos - My Discussions: get all discussions by user id
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);

            var discussions = await _context.Discussion
                .Where(m => m.ApplicationUserId == userId) // filter by user id
                .ToListAsync();

            return View(discussions);
        }

        // GET: Discussions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var discussion = await _context.Discussion
                .Include(d => d.Comments)
                .FirstOrDefaultAsync(m => m.DiscussionId == id);

            if (discussion == null)
            {
                return NotFound();
            }

            return View(discussion);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddComment(int discussionId, string content)
        {
            if (!string.IsNullOrWhiteSpace(content))
            {
                var comment = new Comment
                {
                    DiscussionId = discussionId,
                    Content = content,
                    CreateDate = DateTime.Now
                };

                _context.Comment.Add(comment);

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Details), new { id = discussionId });
        }


        // GET: Discussions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Discussions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DiscussionId,Title,Content,ImageFile")] Discussion discussion)
        {
            // Set creation date
            discussion.CreateDate = DateTime.Now;

            // Rename the uploaded file to a guid (unique filename). Set before discussion saved in database.
            discussion.ImageFilename = Guid.NewGuid().ToString() + Path.GetExtension(discussion.ImageFile?.FileName);

            // Set the user ID of the logged-in user
            discussion.ApplicationUserId = _userManager.GetUserId(User);

            if (ModelState.IsValid)
            {
                // Save the discussion in database
                _context.Add(discussion); // _context = database, add discussion to database
                await _context.SaveChangesAsync();

                // Save the uploaded file after the discussion is saved in the database.
                if (discussion.ImageFile != null)
                {
                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", discussion.ImageFilename);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await discussion.ImageFile.CopyToAsync(fileStream);
                    }
                }

                return RedirectToAction(nameof(Index));
            }
            return View(discussion);
        }

        // GET: Discussions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Get the logged in User Id
            var userId = _userManager.GetUserId(User);

            // Include the Comments list
            var discussion = await _context.Discussion
                .Include(m => m.Comments)
                .Where(m => m.ApplicationUserId == userId) // Filter by user id
                .FirstOrDefaultAsync(m => m.DiscussionId == id);

            if (discussion == null)
            {
                return NotFound();
            }
            return View(discussion);
        }

        // POST: Discussions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DiscussionId,Title,Content,ImageFilename,ApplicationUserId")] Discussion discussion)
        {
            if (id != discussion.DiscussionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(discussion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DiscussionExists(discussion.DiscussionId))
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
            return View(discussion);
        }

        // GET: Discussions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Get the logged in User Id
            var userId = _userManager.GetUserId(User);

            var discussion = await _context.Discussion
                .Where(m => m.ApplicationUserId == userId) // Filter by user id
                .FirstOrDefaultAsync(m => m.DiscussionId == id);

            if (discussion == null)
            {
                return NotFound();
            }

            return View(discussion);
        }

        // POST: Discussions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Get the logged in User Id
            var userId = _userManager.GetUserId(User);

            var discussion = await _context.Discussion
                .Where(m => m.ApplicationUserId == userId) // Filter by user id
                .FirstOrDefaultAsync(m => m.DiscussionId == id);

            if (discussion == null)
            {
                return NotFound();
            }
            else
            {
                _context.Discussion.Remove(discussion);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DiscussionExists(int id)
        {
            return _context.Discussion.Any(e => e.DiscussionId == id);
        }
    }
}
