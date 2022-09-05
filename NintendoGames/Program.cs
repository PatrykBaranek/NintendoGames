using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using NintendoGames.Entities;
using NintendoGames.Middleware;
using NintendoGames.Models.DevelopersModels;
using NintendoGames.Models.RatingModels;
using NintendoGames.Models.Validation.DevelopersValidator;
using NintendoGames.Models.Validation.RatingValidator;
using NintendoGames.Services.DataScraperService;
using NintendoGames.Services.DevelopersService;
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
builder.Services.AddScoped<IValidator<UpdateUserScoreDto>, UpdateUserScoreValidation>();
builder.Services.AddScoped<IValidator<AddDeveloperDto>, AddDeveloperValidation>();

builder.Services.AddScoped<IGamesService, GamesService>();
builder.Services.AddScoped<IDataScraperService, DataScraperService>();
builder.Services.AddScoped<IRatingService, RatingService>();
builder.Services.AddScoped<IDevelopersService, DevelopersService>();

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
