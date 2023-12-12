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
            if (context.BlogPosts.Any())
            {
                return; 
            }

            // dummy blog posts seeding for DB
            var blogPosts = new BlogPost[]
            {
            new BlogPost {
                Title = "Why Mozilla Firefox is the Best Browser of 2023",
                PostAuthor = "admin@lunchbox.com",
                DatePublished = DateTime.Now,
                Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. " +
                          "Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. " +
                          "Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. " +
                          "Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum",

            },
            new BlogPost
            {
                Title = "NVIDIA's RTX 40 SUPER Series Set to Debut at CES 2024 - Reaction",
                PostAuthor = "sam@lunchbox.com",
                DatePublished = DateTime.Now,
                Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. " +
                          "Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. " +
                          "Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. " +
                          "Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum",
            },
            new BlogPost
            {

                Title = "Hello World",
                PostAuthor = "johnsmith@lunchbox.com",
                DatePublished = DateTime.Now,
                Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. " +
                          "Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. " +
                          "Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. " +
                          "Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum",
            }

            };

            // adds posts to DB and saves them
            context.BlogPosts.AddRange(blogPosts);
            context.SaveChanges();

            // dummy/example comments for dummy posts
            var comments = new Comment[]
            {
                new Comment
                {
                    CommentContent = "Great article!",
                    AuthorUsername = "jim562@gmail.com",
                    AuthorId = "userId0",
                    BlogPost = blogPosts[0]
                },
                new Comment
                {
                    CommentContent = "This was such a great read! Thank you!",
                    AuthorUsername = "ronw99@rocketmail.com",
                    AuthorId = "userId1",
                    BlogPost = blogPosts[0]
                },
                new Comment
                {
                    CommentContent = "Fascinating!",
                    AuthorUsername = "timothy@lunchbox.com",
                    AuthorId = "userId2",
                    BlogPost = blogPosts[1]
                },
                new Comment
                {
                    CommentContent = "Glad to see the test worked!",
                    AuthorUsername = "timothy@lunchbox.com",
                    AuthorId = "userId1",
                    BlogPost = blogPosts[2]
                }
            };

            // adds comments to DB and saves them
            context.Comments.AddRange(comments);
            context.SaveChanges();

            // sample post reactions on dummy posts
            var postReactions = new PostReaction[]
            {
                new PostReaction
                {
                    Type = PostReaction.ReactionType.Downvote,
                    ReactionAuthorId = "userId0",
                    BlogPost = blogPosts[0] 
                },
                new PostReaction
                {
                    Type = PostReaction.ReactionType.Downvote,
                    ReactionAuthorId = "userId1", 
                    BlogPost = blogPosts[0] 
                },
                new PostReaction
                {
                    Type = PostReaction.ReactionType.Downvote,
                    ReactionAuthorId = "userId2",
                    BlogPost = blogPosts[0]
                },
                new PostReaction
                {
                    Type = PostReaction.ReactionType.Upvote,
                    ReactionAuthorId = "userId1",
                    BlogPost = blogPosts[1]
                },
                new PostReaction
                {
                    Type = PostReaction.ReactionType.Upvote,
                    ReactionAuthorId = "userId2",
                    BlogPost = blogPosts[2]
                }
            };

            // adds post reactions to DB and saves them
            context.PostReactions.AddRange(postReactions);
            context.SaveChanges();

            // sample comment reactions on dummy comments
            var commentReactions = new CommentReaction[]
            {
                new CommentReaction
                {
                    Type = CommentReaction.ReactionType.Upvote,
                    ReactionAuthorId = "userId0",
                    Comment = comments[0] 
                },
                new CommentReaction
                {
                    Type = CommentReaction.ReactionType.Upvote,
                    ReactionAuthorId = "userId1",
                    Comment = comments[0]
                },
                new CommentReaction
                {
                    Type = CommentReaction.ReactionType.Upvote,
                    ReactionAuthorId = "userId2",
                    Comment = comments[0]
                },
            };

            // adds post reactions to DB and saves them
            context.CommentReactions.AddRange(commentReactions);
            context.SaveChanges();
        }
    }
}
