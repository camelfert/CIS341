using CIS341_project.Models;
using CIS341_project.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static CIS341_project.Models.Reaction;

namespace CIS341_project.Controllers
{
    public class BlogPostController : Controller
    {
        // GET: BlogPostController
        public ActionResult Index()
        {
            return View();
        }

        // GET: BlogPostController/Details/5
        public ActionResult Details(int id)
        {
            var blogPost = new BlogPost
            {
                BlogPostId = id,
                Title = "Why Mozilla Firefox is The Best Web Browser of 2023",
                Content = "Mozilla Firefox is the best browser because it just is!",
                DatePublished = DateTime.Now,
                UpvoteCount = 10,
                DownvoteCount = 3,
                Comments = new List<Comment>(),
                PostAuthor = new Account { Username = "camelfert" }
            };

            var blogPostDTO = new BlogPostDTO
            {
                Title = blogPost.Title,
                Content = blogPost.Content,
                UpvoteCount = blogPost.UpvoteCount,
                DownvoteCount = blogPost.DownvoteCount,
                DatePublished = blogPost.DatePublished,
                PostAuthor = blogPost.PostAuthor.Username,
                CommentCount = blogPost.Comments.Count,
            };

            var commentsDTO = blogPost.Comments.Select(comment => new CommentDTO
            {
                CommentContent = comment.CommentContent,
                Author = comment.Author.Username, 
                UpvoteCount = comment.Reactions.Count(r => r.Type == ReactionType.Upvote),
                DownvoteCount = comment.Reactions.Count(r => r.Type == ReactionType.Downvote)
            }).ToList();

            return View(blogPostDTO);
        }

        // GET: BlogPostController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BlogPostController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
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
