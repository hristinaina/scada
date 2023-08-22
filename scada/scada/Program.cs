using scada.Data;
using scada.Models;
using scada.Services;
using System.Collections.Generic;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// creating data for config file
List<Tag> tags = new List<Tag>
    {
        new DOTag { Id = 1, TagName = "DO_Tag", Value = 1 },
        new DITag { Id = 2, TagName = "DI_Tag", IsScanning = true, ScanTime = 1000, Driver = DriverEnum.SIM },
        new AOTag { Id = 3, TagName = "AO_Tag", Units = "V", LowLimit = 0, HighLimit = 10, Value = 5.5 },
        new AITag
        {
            Id = 4,
            TagName = "AI_Tag",
            IsScanning = true,
            ScanTime = 2000,
            Driver = DriverEnum.RTU,
            Units = "C",
            LowLimit = -10,
            HighLimit = 50,
            Alarms = new List<Alarm>
            {
                new Alarm { Id = 1, Type = AlarmType.HIGH, Priority = 1, Border = 40 },
                new Alarm { Id = 2, Type = AlarmType.LOW, Priority = 2, Border = -5 }
            }
        }
    };
string filePath = "Data/Config/config.xml";
// clearing xml file before writing anything to it
File.WriteAllText(filePath, string.Empty);
XmlSerializationHelper.SaveToXml(tags, filePath);

// loading AITags from XML
/*List<object> loadedAITags = XmlSerializationHelper.LoadFromXml<object>(filePath);
Console.WriteLine("Ucitavam...");
Console.WriteLine(loadedAITags.Count);*/

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

