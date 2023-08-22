using scada.Data;
using scada.Data.Config;
using scada.Models;
using scada.Services;
using System.Collections.Generic;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// CONFIG FILE

// creating data for config file
List<Tag> tags = ConfigHelper.PopulateData();
string filePath = "Data/Config/config.xml";
// clearing xml file before writing anything to it
File.WriteAllText(filePath, string.Empty);
XmlSerializationHelper.SaveToXml(tags, filePath);

// loading tags from XML
List<Tag> loadedTags = XmlSerializationHelper.LoadFromXml<Tag>(filePath);
Console.WriteLine("Ucitavam...");
Console.WriteLine(loadedTags.Count);
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

