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
    /// <summary>
    /// PostReactionController manages the creation of reactions (Upvotes/Downvotes) on blog posts.
    /// </summary>
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

        /// <summary>
        /// Processes the request for adding either an upvote or downvote on a specific blog post.
        /// </summary>
        /// <param name="blogPostId">The ID of the specific blog post being reacted to.</param>
        /// <param name="type">The type of reaction, whether it is an Upvote or Downvote.</param>
        /// <returns>Redirects user back to the specific blog post in a refresh-like fashion.</returns>
        // POST: PostReactionsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create(int blogPostId, string type)
        {
            if (User.IsInRole("Banned"))
            {
                return NotFound();
            }

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

    }
}
