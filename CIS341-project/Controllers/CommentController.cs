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
                CommentContent = comment.CommentContent

            };

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
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CommentController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
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

        // GET: CommentController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CommentController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
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
