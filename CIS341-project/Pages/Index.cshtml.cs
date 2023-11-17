using CIS341_project.Models;
using CIS341_project.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;

namespace CIS341_project.Pages
{

    [AllowAnonymous]
    public class IndexModel : PageModel
    {
        private readonly BlogContext _context;

        public List<Models.BlogPost> LatestPosts { get; set; }

        public IndexModel(BlogContext context)
        {
            _context = context;
        }

        public async Task OnGetAsync() => LatestPosts = await _context.BlogPosts
                .OrderByDescending(p => p.DatePublished)
                .Take(3)
                .ToListAsync();
    }
}
