using Athena.CacheHelper;
using Common.Implementations;
using Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ShoppingCard.Api.Configurations;
using ShoppingCard.Api.ExtensionMethods;
using ShoppingCard.Domain.Interfaces;
using ShoppingCard.Domain.Models;
using ShoppingCard.Repository;
using ShoppingCard.Repository.Implementations;

// todo: implement exceptionMiddleware
// todo: implement actionFilters

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var filePath = Path.Combine(AppContext.BaseDirectory, "basket.xml");
    c.IncludeXmlComments(filePath);
});

// Add DbContext to the application
builder.Services.AddApplicationDbContext(builder.Configuration);

// Add BaseRepository services
builder.Services.AddRepositories();

// Add AutoMapper service
builder.Services.AddAutoMapper(typeof(AutoMapperConfiguration));

// Add CacheHelper
builder.Services.AddCacheServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
