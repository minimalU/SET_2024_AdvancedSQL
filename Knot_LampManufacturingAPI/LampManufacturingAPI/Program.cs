// https://youtu.be/DCYVfLT5_QI?feature=shared
// https://learn.microsoft.com/en-us/ef/ef6/modeling/code-first/workflows/existing-database
// https://www.c-sharpcorner.com/article/asp-net-core-6-web-api-crud-with-entity-framework/
// https://youtu.be/ip3Z4ZcAgA8?feature=shared
// https://learn.microsoft.com/en-us/aspnet/core/blazor/tutorials/signalr-blazor?view=aspnetcore-9.0&tabs=visual-studio
// https://www.youtube.com/watch?v=O7oaxFgNuYo
// https://learn.microsoft.com/en-us/aspnet/core/signalr/background-services?view=aspnetcore-9.0

using LampManufacturingAPI;
using LampManufacturingAPI.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.ResponseCompression;
using LampManufacturingAPI.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// builder.Services.AddControllersWithViews();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSignalR();
builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        ["application/octet-strem"]);
});

builder.Services.AddHostedService<SimulationService>();
builder.Services.AddCors();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();


// app.UseResponseCompression();
app.MapHub<MessageHub>("hub");

// var simulation_service = app.Services.GetRequiredService<SimulationService>();
// Task simulation = Task.Run(() => simulation_service.RunSimulation("Service:Time:"));

app.Run();
