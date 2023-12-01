using CIS341_project.Models;
using CIS341_project.ViewModels;
using CIS341_project.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static CIS341_project.Models.PostReaction;
using Microsoft.AspNetCore.Authorization;
using CIS341_project.Services;
using Microsoft.AspNetCore.Identity;

namespace CIS341_project.Controllers
{
    public class BlogPostController : Controller
    {
        private readonly BlogContext _context;
        private readonly IUserService _userService;
        private readonly UserManager<IdentityUser> _userManager;

        public BlogPostController(BlogContext context, IUserService userService, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userService = userService;
            _userManager = userManager;
        }

        // GET: BlogPostController
        [AllowAnonymous]
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

        [AllowAnonymous]
        public ActionResult Details(int? id)
        {
            if (id != null)
            {
                var blogPost = _context.BlogPosts
                               .Include(bp => bp.Comments)
                               .Include(bp => bp.PostReactions) // based on Reactions nav property
                               .FirstOrDefault(m => m.BlogPostId == id); // retrieval based on ID

                // return NotFound() error if no blog posts are found at this ID
                if (blogPost == null)
                {
                    return NotFound();
                }

                var userId = _userManager.GetUserId(User);
                // MAKE SURE to add any new items for proper data passing HERE, inc. comments
                var blogPostDTO = new BlogPostDTO
                {
                    BlogPostId = blogPost.BlogPostId,
                    Title = blogPost.Title,
                    Content = blogPost.Content,
                    DatePublished = blogPost.DatePublished,
                    PostAuthor = blogPost.PostAuthor,
                    CommentCount = blogPost.Comments.Count,
                    Comments = PopulateReplies(blogPost.Comments.Where(c => c.ParentCommentId == null), userId).ToList()
                };

                ViewData["BlogPostId"] = blogPost.BlogPostId;

                var postUpvoteCount = _context.PostReactions.Count(r => r.Type == PostReaction.ReactionType.Upvote && r.BlogPostId == id);
                var postDownvoteCount = _context.PostReactions.Count(r => r.Type == PostReaction.ReactionType.Downvote && r.BlogPostId == id);

                // used to pass reaction counts to views
                ViewData["postUpvoteCount"] = postUpvoteCount;
                ViewData["postDownvoteCount"] = postDownvoteCount;

                var userReaction = _context.PostReactions.FirstOrDefault(r => r.BlogPostId == id && r.ReactionAuthorId == userId);
                ViewData["UserReactionType"] = userReaction?.Type.ToString();

                return View(blogPostDTO);
            }

            return NotFound();
        }

        private IEnumerable<CommentDTO> PopulateReplies(IEnumerable<Comment> comments, string userId)
        {
            foreach (var comment in comments)
            {
                var commentDTO = new CommentDTO
                {
                    CommentId = comment.CommentId,
                    CommentContent = comment.CommentContent,
                    BlogPostId = comment.BlogPostId,
                    AuthorId = comment.AuthorId,
                    AuthorUsername = comment.AuthorUsername,
                    ParentCommentId = comment.ParentCommentId,
                    Replies = PopulateReplies(comment.Replies, userId).ToList()
                };

                int commentUpvoteCount = _context.CommentReactions.Count(r => r.Type == CommentReaction.ReactionType.Upvote && r.CommentId == comment.CommentId);
                int commentDownvoteCount = _context.CommentReactions.Count(r => r.Type == CommentReaction.ReactionType.Downvote && r.CommentId == comment.CommentId);

                ViewData[$"commentUpvoteCount{comment.CommentId}"] = commentUpvoteCount;
                ViewData[$"commentDownvoteCount{comment.CommentId}"] = commentDownvoteCount;

                var userCommentReaction = _context.CommentReactions.FirstOrDefault(r => r.CommentId == comment.CommentId && r.ReactionAuthorId == userId);
                ViewData[$"UserCommentReactionType{comment.CommentId}"] = userCommentReaction?.Type.ToString();

                yield return commentDTO;
            }
        }

        // GET: BlogPostController/Create
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: BlogPostController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Title,Content,DatePublished")] BlogPostDTO blogPostDTO)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);

                var blogPost = new BlogPost
                {
                    Title = blogPostDTO.Title,
                    PostAuthor = user?.UserName,
                    Content = blogPostDTO.Content,
                    DatePublished = DateTime.Now
                };

                _context.BlogPosts.Add(blogPost);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(blogPostDTO);
        }

        // GET: BlogPostController/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.BlogPosts == null)
            {
                return NotFound();
            }

            var blogPost = await _context.BlogPosts.FindAsync(id);
            if (blogPost == null)
            {
                return NotFound();
            }

            var blogPostDTO = new BlogPostDTO
            {
                BlogPostId = blogPost.BlogPostId,
                Title = blogPost.Title,
                Content = blogPost.Content,
                DatePublished = blogPost.DatePublished,
                PostAuthor = blogPost.PostAuthor
            };

            return View(blogPostDTO);
        }

        // POST: BlogPostController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, BlogPostDTO blogPostDTO)
        {
            if (id != blogPostDTO.BlogPostId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var blogPostToUpdate = await _context.BlogPosts.FindAsync(id);
                if (blogPostToUpdate == null)
                {
                    return NotFound();
                }

                var user = await _userManager.GetUserAsync(User);

                blogPostToUpdate.Title = blogPostDTO.Title;
                blogPostToUpdate.Content = blogPostDTO.Content;
                blogPostToUpdate.DatePublished = DateTime.Now;
                blogPostToUpdate.PostAuthor = user?.UserName;       

                try
                {
                    _context.Update(blogPostToUpdate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BlogPostExists(blogPostDTO.BlogPostId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(blogPostDTO);
        }

        private bool BlogPostExists(int id)
        {
            return _context.BlogPosts.Any(e => e.BlogPostId == id);
        }

        // GET: BlogPostController/Delete/5
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.BlogPosts == null)
            {
                return NotFound();
            }

            var blogPost = await _context.BlogPosts
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.BlogPostId == id);

            if (blogPost == null)
            {
                return NotFound();
            }

            var blogPostDTO = new BlogPostDTO
            {
                BlogPostId = blogPost.BlogPostId,
                Title = blogPost.Title,
                Content = blogPost.Content,
                DatePublished = blogPost.DatePublished,
                PostAuthor = blogPost.PostAuthor
            };

            return View(blogPostDTO);
        }


        // POST: BlogPostController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeletionConfirmed(int id)
        {
            if (_context.BlogPosts == null)
            {
                return Problem("Entity set 'BlogContext.Post'  is null.");
            }

            var post = await _context.BlogPosts.FindAsync(id);

            if (post != null)
            {
                _context.BlogPosts.Remove(post);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ResetPostReactions(int id)
        {
            var reactions = _context.PostReactions.Where(r => r.BlogPostId == id);
            _context.PostReactions.RemoveRange(reactions);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = id });
        }
    }
}
