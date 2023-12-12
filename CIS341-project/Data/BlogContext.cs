using Microsoft.EntityFrameworkCore;
using CIS341_project.Models;
using static System.Formats.Asn1.AsnWriter;
using System.Reflection;

namespace CIS341_project.Data
{
    public class BlogContext : DbContext
    {
        public BlogContext(DbContextOptions<BlogContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // every blog post can have many comments associated to one specific post, FK BlogPostId
            modelBuilder.Entity<BlogPost>()
                .HasMany(b => b.Comments)
                .WithOne(c => c.BlogPost)
                .HasForeignKey(c => c.BlogPostId);

            // every comment has a parent comment that can have many replies attached, FK ParentCommentId
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.ParentComment)
                .WithMany(c => c.Replies)
                .HasForeignKey(c => c.ParentCommentId)
                .OnDelete(DeleteBehavior.Restrict);

            // every blog post can have many post reactions associated to one specific post, FK BlogPostId
            modelBuilder.Entity<BlogPost>()
                .HasMany(b => b.PostReactions)
                .WithOne(r => r.BlogPost)
                .HasForeignKey(r => r.BlogPostId);

            // every comment can have many comment reactions associated to one specific comment, FK CommentId
            modelBuilder.Entity<Comment>()
                .HasMany(b => b.CommentReactions)
                .WithOne(c => c.Comment)
                .HasForeignKey(c => c.CommentId);
        }

        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<PostReaction> PostReactions { get; set; }
        public DbSet<CommentReaction> CommentReactions { get; set; }

    }
}
