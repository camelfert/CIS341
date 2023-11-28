using CIS341_project.Data;
using CIS341_project.Models;
using CIS341_project.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CIS341_project.Controllers
{
    public class PostReactionController : Controller
    {
        private readonly BlogContext _context;
        private readonly IUserService _userService;
        private readonly UserManager<IdentityUser> _userManager;

        public PostReactionController(BlogContext context, IUserService userService, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userService = userService;
            _userManager = userManager;
        }

        // GET: PostReactionsController
        public ActionResult Index()
        {
            return View();
        }

        // GET: PostReactionsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PostReactionsController/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: PostReactionsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create(int blogPostId, string type)
        {
            var userId = _userManager.GetUserId(User);

            var existingReaction = await _context.PostReactions.FirstOrDefaultAsync(r => r.BlogPostId == blogPostId && r.ReactionAuthorId == userId);

            if (existingReaction != null)
            {
                existingReaction.Type = type == "Upvote" ? PostReaction.ReactionType.Upvote : PostReaction.ReactionType.Downvote;
                _context.Update(existingReaction);
            }
            else
            {
                var newReaction = new PostReaction
                {
                    BlogPostId = blogPostId,
                    ReactionAuthorId = userId,
                    Type = type == "Upvote" ? PostReaction.ReactionType.Upvote : PostReaction.ReactionType.Downvote
                };
                _context.PostReactions.Add(newReaction);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "BlogPost", new { id = blogPostId });
        }

        // GET: PostReactionsController/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PostReactionsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PostReactionsController/Delete/5
        [Authorize]
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PostReactionsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
