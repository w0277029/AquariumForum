using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AquariumForum.Data;
using AquariumForum.Models;
using Azure;

namespace AquariumForum.Controllers
{
    public class CommentsController : Controller
    {
        private readonly AquariumForumContext _context;

        // Constructor
        public CommentsController(AquariumForumContext context)
        {
            _context = context;
        }

        // Deleted the following actions:
        // GET: Comments
        // GET: Comments/Details/5
        // GET: Comments/Edit/5
        // GET: Comments/Delete/5
        // POST: Comments/Edit/5
        // POST: Comments/Delete/5

        // GET: Comments/Create
        public IActionResult Create(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ViewData["CommentId"] = id;

            return View();

            //ViewData["DiscussionId"] = new SelectList(_context.Discussion, "DiscussionId", "DiscussionId");
            //return View();
        }

        // POST: Comments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CommentId,Content,CreateDate,DiscussionId")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(comment);
                await _context.SaveChangesAsync();

                //// TODO: Re-direct to Get Discussion page
                //return RedirectToAction("Edit", "Discussions", new { id = comment.DiscussionId });

                return RedirectToAction("Index", "Home"); // Placeholder
            }
            ViewData["DiscussionId"] = comment.DiscussionId;

            return View(comment);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("CommentId,Content,CreateDate,DiscussionId")] Comment comment)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(comment);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["DiscussionId"] = new SelectList(_context.Discussion, "DiscussionId", "DiscussionId", comment.DiscussionId);
        //    return View(comment);
        //}

        //private bool CommentExists(int id)
        //{
        //    return _context.Comment.Any(e => e.CommentId == id);
        //}
    }
}
