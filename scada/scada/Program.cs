using scada.Database;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();

var app = builder.Build();

Console.WriteLine("Jadni pokusaj");

Console.WriteLine("Testttt");

using (var dbContext = new ApplicationDbContext())
{
    Console.WriteLine("Stigli");
    // Perform database operations using dbContext
    var products = dbContext.AlarmHistories.ToList();
    Console.WriteLine(products.Count);

    // Do something with the retrieved products...
}


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
}

app.UseStaticFiles();
app.UseRouting();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();

