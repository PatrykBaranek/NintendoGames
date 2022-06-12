using GamesList.Entities;
using GamesList.Services;
using GamesList.Services.DataScraper;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<NintendoDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("NintendoConnectionString"));
});

builder.Services.AddScoped<INintendoService, NintendoService>();
builder.Services.AddSingleton<IDataScraper, DataScraper>();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
