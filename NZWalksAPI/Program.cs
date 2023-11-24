using Microsoft.EntityFrameworkCore;
using NZWalksAPI.Data;
using NZWalksAPI.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//In that DBContext controller passing that options param, with using the SQLServer. and then connection string from appsettings.json
builder.Services.AddDbContext<NZWalksDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("NZWalksConnectionString")));

//builder.Services.AddScoped<IRegionRepository, InMemoryRepository>(); -- Like this we can change the Repository to a differnt database
builder.Services.AddScoped<IRegionRepository, SQLRegionRepository>(); //type and the name of Repository to add the Repository Pattern
var app = builder.Build();

// Configure the HTTP request pipeline.
//These are middlewares
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
