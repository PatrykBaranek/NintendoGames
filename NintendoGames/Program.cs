using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using NintendoGames.Entities;
using NintendoGames.Middleware;
using NintendoGames.Models.RatingModels;
using NintendoGames.Models.Validation;
using NintendoGames.Services.DataScraper;
using NintendoGames.Services.Games;
using NintendoGames.Services.RatingService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Services.AddDbContext<NintendoDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("NintendoConnectionString"));
});

// Middleware
builder.Services.AddScoped<ErrorHandlingMiddleware>();

// Validation
builder.Services.AddScoped<IValidator<UpdateUserScoreDto>, RatingValidation>();

builder.Services.AddScoped<IGamesService, GamesService>();
builder.Services.AddScoped<IDataScraperService, DataScraperService>();
builder.Services.AddScoped<IRatingService, RatingService>();

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
