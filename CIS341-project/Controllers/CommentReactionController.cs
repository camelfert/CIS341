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
    /// PostReactionController manages the creation of reactions (Upvotes/Downvotes) on comments.
    /// </summary>
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

        /// <summary>
        /// Processes the request for adding either an upvote or downvote on a specific comment.
        /// </summary>
        /// <param name="commentId">The ID of the specific comment being reacted to.</param>
        /// <param name="type">The type of reaction, whether it is an Upvote or Downvote.</param>
        /// <returns>Redirects user back to the specific blog post where the comment was posted in a refresh-like fashion.</returns>
        // POST: CommentReactionsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create(int commentId, string type)
        {
            if (User.IsInRole("Banned"))
            {
                return NotFound();
            }

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

    }
}
