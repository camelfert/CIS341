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
    public class CommentController : Controller
    {
        private readonly BlogContext _context;
        private readonly IUserService _userService;

        public CommentController(BlogContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        // GET: CommentController
        public ActionResult Index()
        {
            return View();
        }

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

            return View(commentDTO);
        }


        // POST: CommentController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("BlogPostId,CommentId,CommentContent,AuthorId,AuthorUsername")] CommentDTO commentDTO)
        {
            if (ModelState.IsValid)
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> CreateReply(CommentDTO commentDTO, int parentCommentId)
        {
            var (userId, userName) = await _userService.GetUserDetailsAsync();

            if (ModelState.IsValid)
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

        // GET: CommentController/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
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

        // POST: CommentController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, CommentDTO commentDTO)
        {
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

        // GET: CommentController/Delete/5
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null || _context.Comments == null)
            {
                return NotFound();
            }

            var comment = await _context.Comments
                .FirstOrDefaultAsync(c => c.CommentId == id);

            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        // POST: CommentController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeletionConfirmed (int id)
        {
            if (_context.Comments == null)
            {
                return Problem("Entity set 'BlogContext.Comment'  is null.");
            }

            var comment = await _context.Comments.FindAsync(id);

            if (comment != null)
            {
                _context.Comments.Remove(comment);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "BlogPost", new { id = comment.BlogPostId });
        }

    }
}
