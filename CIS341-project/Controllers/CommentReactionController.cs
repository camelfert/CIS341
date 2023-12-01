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
    public class CommentReactionController : Controller
    {
        private readonly BlogContext _context;
        private readonly IUserService _userService;
        private readonly UserManager<IdentityUser> _userManager;

        public CommentReactionController(BlogContext context, IUserService userService, UserManager<IdentityUser> userManager)
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

        // POST: CommentReactionsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create(int commentId, string type)
        {
            var userId = _userManager.GetUserId(User);

            var existingReaction = await _context.CommentReactions
                .FirstOrDefaultAsync(r => r.CommentId == commentId && r.ReactionAuthorId == userId);

            if (existingReaction != null)
            {
                existingReaction.Type = type == "Upvote" ? CommentReaction.ReactionType.Upvote : CommentReaction.ReactionType.Downvote;
                _context.Update(existingReaction);
            }
            else
            {
                var newReaction = new CommentReaction
                {
                    CommentId = commentId,
                    ReactionAuthorId = userId,
                    Type = type == "Upvote" ? CommentReaction.ReactionType.Upvote : CommentReaction.ReactionType.Downvote
                };
                _context.CommentReactions.Add(newReaction);
            }

            await _context.SaveChangesAsync();

            var blogPostId = _context.Comments.Where(c => c.CommentId == commentId).Select(c => c.BlogPostId).FirstOrDefault();
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
