using CIS341_project.Models;
using Microsoft.EntityFrameworkCore;

namespace CIS341_project.Data
{
    public static class DbInitializer
    {
        public static void Initialize(BlogContext context)
        {
            context.Database.EnsureCreated();

            //check if any blog posts exist
            if (context.BlogPosts.Any() || context.Accounts.Any())
            {
                return; 
            }

            // add account
            var admin = new Account
            {
                Username = "camelfert",
                Password = "testtesttest",
                Role = "Admin" 
            };

            context.Accounts.Add(admin);

            // dummy data seeding for DB
            var blogPosts = new BlogPost[]
            {
            new BlogPost {
                Title = "Why Mozilla Firefox is the Best Browser of 2023",
                PostAuthor = "camelfert",
                DatePublished = DateTime.Now,
                //Reactions = 
                Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. " +
                          "Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. " +
                          "Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. " +
                          "Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum",

            },
            new BlogPost
            {
                Title = "NVIDIA's RTX 40 SUPER Series Set to Debut at CES 2024 - Reaction",
                PostAuthor = "camelfert",
                DatePublished = DateTime.Now,
                //Reactions = 
                Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. " +
                          "Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. " +
                          "Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. " +
                          "Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum",
            },
            new BlogPost
            {

                Title = "Hello World",
                PostAuthor = "camelfert",
                DatePublished = DateTime.Now,
                //Reactions =
                Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. " +
                          "Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. " +
                          "Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. " +
                          "Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum",
            }

            };

            foreach (BlogPost bp in blogPosts)
            {
                context.BlogPosts.Add(bp);
            }

            context.SaveChanges();

        }
    }
}
