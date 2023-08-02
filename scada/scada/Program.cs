using scada.Database;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();

var app = builder.Build();

using (var dbContext = new ApplicationDbContext())
{
    // Perform database operations using dbContext
    var alarmHistory = dbContext.AlarmHistory.ToList();
    Console.WriteLine(alarmHistory.Count);

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

