using CIS341_project.Models;
using CIS341_project.ViewModels;
using CIS341_project.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.Design;
using CIS341_project.Services;
using static CIS341_project.Models.PostReaction;

namespace CIS341_project.Controllers
{
    /// <summary>
    /// CommentController manages all of the CRUD and CRUD-related functions for comments.
    /// </summary>
    public class CommentController : Controller
    {
        private readonly BlogContext _context;
        private readonly IUserService _userService;
        private readonly UserManager<IdentityUser> _userManager;

        public CommentController(BlogContext context, IUserService userService, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userService = userService;
            _userManager = userManager;
        }

        /// <summary>
        /// Populates a comment with the necessary data from the database.
        /// </summary>
        /// <param name="id">The ID of the specific comment.</param>
        /// <returns>A comment binded with the corresponding data based on the ID.</returns>
        // GET: CommentController/Details/5
        [Authorize]
        public ActionResult Details(int id)
        {
            var comment = _context.Comments
                                  //.Include(c => c.AuthorUsername)
                                  .FirstOrDefault(c => c.CommentId == id);

            if (comment == null)
            {
                return NotFound();
            }

            var commentDTO = new CommentDTO
            {
                CommentId = comment.CommentId,
                CommentContent = comment.CommentContent,
                BlogPostId = comment.BlogPostId
            };

            var comUpvoteCount = comment.CommentReactions.Count(c => c.Type == CommentReaction.ReactionType.Upvote && c.CommentId == id);
            var comDownvoteCount = comment.CommentReactions.Count(c => c.Type == CommentReaction.ReactionType.Downvote && c.CommentId == id);

            ViewData["comUpvoteCount"] = comUpvoteCount;
            ViewData["comDownvoteCount"] = comDownvoteCount;

            var userId = _userManager.GetUserId(User);
            var userReaction = _context.CommentReactions.FirstOrDefault(r => r.CommentId == id && r.ReactionAuthorId == userId);
            ViewData["UserCommentReactionType"] = userReaction?.Type.ToString();

            return View(commentDTO);
        }

        /// <summary>
        /// Processes the request for creation of a comment based on the data provided to the Create Comment Partial View "form".
        /// </summary>
        /// <returns>Redirects user to the same blog post the comment was posted on if successful; otherwise returns the same view but with validation errors.</returns>
        // POST: CommentController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("BlogPostId,CommentId,CommentContent,AuthorId,AuthorUsername")] CommentDTO commentDTO)
        {
            if (ModelState.IsValid && !User.IsInRole("Banned"))
            {
                var (userId, userName) = await _userService.GetUserDetailsAsync();

                var comment = new Comment
                {
                    CommentId = commentDTO.CommentId,
                    CommentContent = commentDTO.CommentContent,
                    BlogPostId = commentDTO.BlogPostId,
                    AuthorId = userId,
                    AuthorUsername = userName
                };

                _context.Comments.Add(comment);
                await _context.SaveChangesAsync();

                return RedirectToAction("Details", "BlogPost", new { id = commentDTO.BlogPostId });
            }

            return View();
        }

        [Authorize]
        public ActionResult Reply(int parentCommentId)
        {
            TempData["ParentCommentId"] = parentCommentId;
            return View(new CommentDTO());
        }

        /// <summary>
        /// Processes the request for creation of a reply to a comment based on the data provided to the Reply form.
        /// </summary>
        /// <param name="parentCommentId">The ID of the Parent(first/top) comment being replied to.</param>
        /// <returns>Redirects user to the same blog post the comment was posted on if successful; otherwise returns the same view but with validation errors.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> CreateReply(CommentDTO commentDTO, int parentCommentId)
        {
            var (userId, userName) = await _userService.GetUserDetailsAsync();

            if (ModelState.IsValid && !User.IsInRole("Banned"))
            {
                var reply = new Comment
                {
                    CommentId = commentDTO.CommentId,
                    CommentContent = commentDTO.CommentContent,
                    BlogPostId = commentDTO.BlogPostId,
                    ParentCommentId = parentCommentId,
                    AuthorId = userId,
                    AuthorUsername = userName
                };

                _context.Comments.Add(reply);
                await _context.SaveChangesAsync();

                return RedirectToAction("Details", "BlogPost", new { id = reply.BlogPostId });
            }

            return View(commentDTO);
        }
        // <summary>
        /// Presents the view containing the form for editing a specific comment.
        /// </summary>
        /// <param name="id">The ID of the comment to edit.</param>
        /// <returns>A view with the form for editing a comment. Otherwise, returns NotFound.</returns>
        // GET: CommentController/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            if (User.IsInRole("Banned"))
            {
                return NotFound();
            }

            if (id == null || _context.Comments == null)
            {
                return NotFound();
            }

            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }

            var commentDTO = new CommentDTO
            {
                CommentId = comment.CommentId,
                CommentContent = comment.CommentContent,
                AuthorId = comment.AuthorId, 
                AuthorUsername = comment.AuthorUsername,
                BlogPostId = comment.BlogPostId
            };

            return View(commentDTO);
        }

        /// <summary>
        /// Processes the request for editing a comment based on the data provided to the View & form, as well as the ID of the specific comment.
        /// </summary>
        /// <param name="id">The ID of the comment to edit.</param>
        /// <param name="commentDTO">The comment data transfer object containing the data to edit a specific comment..</param>
        /// <returns>Redirects user back to the blog post if successful; otherwise returns the same view but with validation errors.</returns>
        // POST: CommentController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, CommentDTO commentDTO)
        {
            if (User.IsInRole("Banned"))
            {
                return NotFound();
            }

            if (id != commentDTO.CommentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var commentToUpdate = await _context.Comments.FindAsync(id);
                if (commentToUpdate == null)
                {
                    return NotFound();
                }

                commentToUpdate.CommentContent = commentDTO.CommentContent;

                try
                {
                    _context.Update(commentToUpdate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommentExists(commentDTO.CommentId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "BlogPost", new { id = commentToUpdate.BlogPostId });
            }
            return View(commentDTO);
        }

        private bool CommentExists(int id)
        {
            return _context.Comments.Any(c => c.CommentId == id);
        }

        /// <summary>
        /// Confirms the deletion of a specific comment.
        /// </summary>
        /// <param name="id">The ID of the comment to delete.</param>
        /// <returns>A view asking for deletion confirmation if the comment is found and valid for deletion; otherwise, returns NotFound.</returns>
        // GET: CommentController/Delete/5
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (User.IsInRole("Banned"))
            {
                return NotFound();
            }

            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comments
                .Select(c => new CommentDTO
                {
                    CommentId = c.CommentId,
                    CommentContent = c.CommentContent,
                    AuthorUsername = c.AuthorUsername,
                    BlogPostId = c.BlogPostId
                })
                .FirstOrDefaultAsync(c => c.CommentId == id);

            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        /// <summary>
        /// Processes the deletion of a specific comment after user confirmation.
        /// </summary>
        /// <param name="id">The ID of the commentto delete.</param>
        /// <returns>Redirects to the specific blog post view for the deleted comment if successful.</returns>
        // POST: CommentController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeletionConfirmed(int id)
        {
            if (User.IsInRole("Banned"))
            {
                return NotFound();
            }

            var comment = await _context.Comments
                                        .Include(c => c.Replies)
                                        .SingleOrDefaultAsync(c => c.CommentId == id);

            if (comment != null)
            {
                if (comment.Replies.Any())
                {
                    _context.Comments.RemoveRange(comment.Replies);
                }

                _context.Comments.Remove(comment);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Details", "BlogPost", new { id = comment?.BlogPostId });
        }

        /// <summary>
        /// Removes/resets both Upvote and Downvote reaction counts for a specific comment.
        /// </summary>
        /// <param name="id">The ID of the comment to reset.</param>
        /// <returns>Redirects back to the blog post if successful.</returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ResetCommentReactions(int id)
        {
            var reactions = _context.CommentReactions.Where(r => r.CommentId == id);
            _context.CommentReactions.RemoveRange(reactions);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "BlogPost", new { id = _context.Comments.Find(id)?.BlogPostId });
        }
    }
}
