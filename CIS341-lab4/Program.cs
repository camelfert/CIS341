using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.HttpLogging;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

builder.Logging.AddConsole();
builder.Services.AddLogging();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

app.Use(async (context, next) =>
{
    Console.WriteLine($"Path: {context.Request.Path}");
    Console.WriteLine($"Endpoint: {context.GetEndpoint()?.DisplayName}");
    Console.WriteLine(string.Join(Environment.NewLine, context.Request.RouteValues));

    await next();
});

app.UseStatusCodePagesWithReExecute("/Error/{0}", "?statusCode={0}");

app.MapRazorPages();

app.Run();