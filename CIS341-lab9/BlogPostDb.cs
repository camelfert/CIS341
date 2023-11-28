using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Emit;
using System.Security.Principal;
using System.Xml.Linq;

namespace CIS341_lab9
{
    public class BlogPostDb : DbContext
    {
        public BlogPostDb(DbContextOptions<BlogPostDb> options)
                : base(options) { }

        public DbSet<BlogPost> BlogPosts { get; set; }

    }
}
