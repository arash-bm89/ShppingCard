using Athena.CacheHelper;
using Athena.RabbitMQHelper;
using Hangfire;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using ShoppingCard.Api.Configurations;
using ShoppingCard.Api.ExtensionMethods;
using ShoppingCard.Api.Middlewares;
using Swashbuckle.AspNetCore.SwaggerUI;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
        {
            options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var filePath = Path.Combine(AppContext.BaseDirectory, "basket.xml");
    c.IncludeXmlComments(filePath);

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using Bearer scheme."
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });

});

// Add Hangfire Services
builder.Services.AddHangfireServices(builder.Configuration);

// Add Hangfire Server
builder.Services.AddHangfireServer();

// Add DbContext to the application
builder.Services.AddApplicationDbContext(builder.Configuration);

// Add BaseRepository services
builder.Services.AddRepositories();

// Add AutoMapper service
builder.Services.AddAutoMapper(typeof(AutoMapperConfiguration));

// Add CacheHelper
builder.Services.AddCacheServices(builder.Configuration);

// Add Producers
builder.Services.AddAsyncPublishers();

// Add RabbitMQ
builder.Services.AddRabbit(builder.Configuration.GetSection("Rabbit"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.Interceptors = new InterceptorFunctions
        {
            RequestInterceptorFunction = "function (req) { req.headers['Authorization'] = 'bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJJZCI6IjZhYzlkOGNlLTg4M2MtNGM5YS1iNGMxLTllNjFlZmI0NjFiYyIsIk5hbWUiOiJhcmFzaCJ9.dlZa17YaHjCwYyWSBpyeRj8BP969H5zqHdLFyQ/F+Z0'; return req; }"
        };
    });
}

app.UseHttpsRedirection();

app.UseHangfireDashboard();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseMiddleware<CustomAuthorizationMiddleware>();

app.UseMiddleware<RequestLoggingMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.MapHangfireDashboard();

app.Run();