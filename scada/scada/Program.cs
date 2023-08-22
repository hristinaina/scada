using scada.Data;
using scada.Data.Config;
using scada.Models;
using scada.Services;
using scada.Services.implementation;
using scada.Services.interfaces;
using System.Collections.Generic;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// CONFIG FILE
// creating data for config file
//List<Tag> tags = ConfigHelper.PopulateData();
//XmlSerializationHelper.SaveToXml(tags, filePath);

// loading tags from XML
List<Tag> loadedTags = XmlSerializationHelper.LoadFromXml<Tag>();
Console.WriteLine("Ucitavam...");
Console.WriteLine(loadedTags.Count);
// parsing ao tags from loaded tags
List<AOTag> aoTags = ConfigHelper.ParseLoadedObjects<AOTag>(loadedTags);
Console.WriteLine(aoTags[0].Id);
// END OF WORKING WITH CONFIG FILE

builder.Services.AddControllersWithViews();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowMyFrontend", builder =>
    {
        builder.WithOrigins("*")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IAlarmHistoryService, AlarmHistoryService>();
builder.Services.AddTransient<ITagHistoryService, TagHistoryService>();
builder.Services.AddTransient<ITagService, TagService>();

var app = builder.Build();

app.UseCors("AllowMyFrontend");

app.MapGet("/", () => "Hello World!");

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

