using CIS341_lab9;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Swashbuckle.AspNetCore;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<BlogPostDb>(opt => opt.UseInMemoryDatabase("BlogPostList"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Lunchbox Blog API",
    });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
});

string apiPrefix = "/api/v1/blogposts/";

// get all
app.MapGet(apiPrefix, async (BlogPostDb db) =>
    await db.BlogPosts.ToListAsync())
    .WithDescription("Get all blog posts")
    .WithOpenApi();

//app.MapGet("/blogposts/complete", async (BlogPostDb db) =>
//    await db.BlogPosts.Where(t => t.Title).ToListAsync());

app.MapGet(apiPrefix + "{id}", async (int id, BlogPostDb db) =>
    await db.BlogPosts.FindAsync(id)
        is BlogPost post
            ? Results.Ok(post)
            : Results.NotFound());

app.MapPost(apiPrefix, async (BlogPost post, BlogPostDb db) =>
{
    db.BlogPosts.Add(post);
    await db.SaveChangesAsync();

    return Results.Created($"/blogposts/{post.BlogPostId}", post);
});

//app.MapPut(apiPrefix + "{id}", async (int id, BlogPost inputPost, BlogPostDb db) =>
//{
//    var post = await db.BlogPosts.FindAsync(id);

//    if (post is null) return Results.NotFound();

//    post.Title = inputPost.Title;
//    post.Content = inputPost.Content;

//    await db.SaveChangesAsync();

//    return Results.NoContent();
//});

app.MapDelete(apiPrefix + "{id}", async (int id, BlogPostDb db) =>
{
    if (await db.BlogPosts.FindAsync(id) is BlogPost post)
    {
        db.BlogPosts.Remove(post);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }

    return Results.NotFound();
});

app.Run();