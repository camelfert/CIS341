using CIS341_project.Models;
using CIS341_project.ViewModels;
using CIS341_project.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static CIS341_project.Models.Reaction;

namespace CIS341_project.Controllers
{
    public class BlogPostController : Controller
    {
        private readonly BlogContext _context;

        public BlogPostController(BlogContext context)
        {
            _context = context;
        }

        // GET: BlogPostController
        public async Task<IActionResult> Index()
        {
            var blogPosts = await _context.BlogPosts
            .OrderByDescending(p => p.DatePublished)
            .ToListAsync();

            var blogPostDTOs = blogPosts.Select(bp => new BlogPostDTO
            {
                BlogPostId = bp.BlogPostId,
                Title = bp.Title,
                Content = bp.Content,
                DatePublished = bp.DatePublished,
                PostAuthor = bp.PostAuthor != null ? bp.PostAuthor : "unknown", 
                CommentCount = bp.Comments != null ? bp.Comments.Count : 0, 
            }).ToList();

            return View(blogPostDTOs);
        }

        // GET: BlogPostController/Details/5
        public ActionResult Details(int? id)
        {
            if (id != null)
            {
                var blogPost = _context.BlogPosts
                               .Include(bp => bp.Comments)
                               .ThenInclude(c => c.Author)
                               .Include(bp => bp.Reactions) // based on Reactions nav property
                               .FirstOrDefault(m => m.BlogPostId == id); // retrieval based on ID

                // return NotFound() error if no blog posts are found at this ID
                if (blogPost == null)
                {
                    return NotFound(); 
                }

                var blogPostDTO = new BlogPostDTO
                {
                    Title = blogPost.Title,
                    Content = blogPost.Content,
                    DatePublished = blogPost.DatePublished,
                    PostAuthor = blogPost.PostAuthor,
                    CommentCount = blogPost.Comments.Count,
                };

                var upvoteCount = blogPost.Reactions.Count(r => r.Type == ReactionType.Upvote);
                var downvoteCount = blogPost.Reactions.Count(r => r.Type == ReactionType.Downvote);
                
                // used to pass reaction counts to views
                ViewData["UpvoteCount"] = upvoteCount;
                ViewData["DownvoteCount"] = downvoteCount;

                return View(blogPostDTO);
            }

            return NotFound();
        }

        // GET: BlogPostController/Create
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        // POST: BlogPostController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("BlogPostId,Title,PostAuthor,Content,DatePublished")] BlogPostDTO blogPostDTO)
        {
            if (ModelState.IsValid)
            {
                var blogPost = new BlogPost
                {
                    Title = blogPostDTO.Title,
                    PostAuthor = blogPostDTO.PostAuthor,
                    Content = blogPostDTO.Content,
                    DatePublished = blogPostDTO.DatePublished
                };

                _context.BlogPosts.Add(blogPost);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            // used for debugging form submission issues related to model validity
            //else
            //{
            //    foreach (var modelStateKey in ViewData.ModelState.Keys)
            //    {
            //        var value = ViewData.ModelState[modelStateKey];
            //        foreach (var error in value.Errors)
            //        {
            //            var key = modelStateKey;
            //            var errorMessage = error.ErrorMessage;
            //        }
            //    }
            //}
                return View(blogPostDTO);
        }

        // GET: BlogPostController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: BlogPostController/Edit/5
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

        // GET: BlogPostController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: BlogPostController/Delete/5
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
