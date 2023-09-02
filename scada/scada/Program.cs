using scada.Repositories;
using scada.Services;
using scada.Services.implementation;
using scada.Services.interfaces;
using scada.WebSockets;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowMyFrontend", builder =>
    {
        builder.WithOrigins("http://localhost:44438")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

// services
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IAlarmHistoryService, AlarmHistoryService>();
builder.Services.AddTransient<ITagHistoryService, TagHistoryService>();
builder.Services.AddTransient<ITagService, TagService>();
builder.Services.AddTransient<TagProcessingService>();

// repositories
builder.Services.AddTransient<TagHistoryRepository>();

// sockets
builder.Services.AddSignalR();

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

using var scope = app.Services.CreateScope();
scope.ServiceProvider.GetRequiredService<TagProcessingService>().Run();

app.MapHub<WebSocket>("/Hub/tag");

app.Run();