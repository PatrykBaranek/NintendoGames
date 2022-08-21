using System.Reflection;
using Microsoft.EntityFrameworkCore;
using NintendoGames.Entities;
using NintendoGames.Middleware;
using NintendoGames.Services.DataScraper;
using NintendoGames.Services.Games;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Services.AddDbContext<NintendoDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("NintendoConnectionString"));
});

// Middleware
builder.Services.AddScoped<ErrorHandlingMiddleware>();

builder.Services.AddScoped<IGamesService, GamesService>();
builder.Services.AddScoped<IDataScraperService, DataScraperService>();

builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
