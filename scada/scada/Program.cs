using scada.Data;
using scada.Models;
using scada.Services;
using System.Collections.Generic;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

List<AITag> aiTags = new List<AITag>();
AITag tag = new AITag(true, 1000, DriverEnum.SIM, "celzius", 10, 25, new List<Alarm>());
AITag tag2 = new AITag(true, 500, DriverEnum.SIM, "celzius", 0, 15, new List<Alarm>());
aiTags.Add(tag);
aiTags.Add(tag2);

string filePath = "Data/Config/config.xml";
XmlSerializationHelper.SaveToXml(aiTags, filePath);

// Loading AITags from XML
List<AITag> loadedAITags = XmlSerializationHelper.LoadFromXml<AITag>(filePath);
// Now you have your AITags loaded from the XML file.
Console.WriteLine("Ucitavam...");
Console.WriteLine(loadedAITags.Count);

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

