using System.Reflection;
using System.Text;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NintendoGames;
using NintendoGames.Entities;
using NintendoGames.Middleware;
using NintendoGames.Models.AccountModels;
using NintendoGames.Models.DevelopersModels;
using NintendoGames.Models.GenresModels;
using NintendoGames.Models.RatingModels;
using NintendoGames.Models.Validators.AccountValidator;
using NintendoGames.Models.Validators.DevelopersValidator;
using NintendoGames.Models.Validators.GenreValidator;
using NintendoGames.Models.Validators.RatingValidator;
using NintendoGames.Models.Validators.WishListValidator;
using NintendoGames.Models.WishListModels;
using NintendoGames.Services;
using NintendoGames.Services.AccountService;
using NintendoGames.Services.DataScraperService;
using NintendoGames.Services.DevelopersService;
using NintendoGames.Services.GamesService;
using NintendoGames.Services.GenresService;
using NintendoGames.Services.RatingService;
using NintendoGames.Services.WishListService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
builder.Services.AddScoped<IValidator<AddGenreDto>, AddGenreValidation>();
builder.Services.AddScoped<IValidator<CreateUserDto>, CreateUserValidation>();
builder.Services.AddScoped<IValidator<LoginDto>, LoginValidation>();
builder.Services.AddScoped<IValidator<AddGameToWishListDto>, AddGameToWishListValidation>();


builder.Services.AddScoped<IGamesService, GamesService>();
builder.Services.AddScoped<IDataScraperService, DataScraperService>();
builder.Services.AddScoped<IRatingService, RatingService>();
builder.Services.AddScoped<IDevelopersService, DevelopersService>();
builder.Services.AddScoped<IGenresService, GenresService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IWishListService, WishListService>();

builder.Services.AddScoped<IUserContextService, UserContextService>();
builder.Services.AddHttpContextAccessor();

// Authentication
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

var authenticationSettings = new AuthenticationSettings();

builder.Configuration.GetSection("Authentication").Bind(authenticationSettings);
builder.Services.AddSingleton(authenticationSettings);
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(cfg =>
    {
        cfg.RequireHttpsMetadata = false;
        cfg.SaveToken = true;
        cfg.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = authenticationSettings.JwtIssuer,
            ValidAudience = authenticationSettings.JwtIssuer,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JwtKey))
        };
    });
builder.Services.AddAuthorization();


var app = builder.Build();

app.UseAuthentication();

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
