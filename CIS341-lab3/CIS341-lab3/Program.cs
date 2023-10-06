using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();

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

// "The delegate should be added to a position where it will run for all requests."
// "The delegate should be added using the Use extension with the appropriate parameters."
app.Use(async (context, next) =>
{
    // "The delegate should write the current time and request URL to the debug output."
    Debug.WriteLine($"Request Time: {DateTime.Now}");
    Debug.WriteLine($"Request URL: {context.Request.Path}");

    // "The delegate should appropriately pass the request to the next delegate."
    await next.Invoke();
});


app.MapRazorPages();

app.Run();