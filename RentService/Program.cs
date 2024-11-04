using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using RentService.Application.Publishing.CommandServices;
using RentService.Application.Publishing.QueryServices;
using RentService.Domain.Publishing.Repositories;
using RentService.Domain.Publishing.Services;
using RentService.Infrastructure.Publishing.Persistence;
using RentService.Infrastructure.Shared.Context;
using RentService.Presentation.Mapper;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();


// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllPolicy", 
        policy => policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

// Configuration
var configuration = new ConfigurationBuilder()
    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
    .AddJsonFile("appsettings.json", true, true)
    .Build();

builder.Services.AddSingleton(configuration);

// Add controllers
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
});

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "RentService API",
        Description = "An ASP.NET Core Web API for managing rents",
    });
});

// Register the Rent services and repositories
builder.Services.AddScoped<IRentRepository, RentRepository>();
builder.Services.AddScoped<IRentCommandService, RentCommandService>();
builder.Services.AddScoped<IRentQueryService, RentQueryService>();

// Add AutoMapper (if necessary)
builder.Services.AddAutoMapper(typeof(RequestToModel), typeof(ModelToRequest), typeof(ModelToResponse));

// Database context for RentService
var connectionString = builder.Configuration.GetConnectionString("RentDB");

builder.Services.AddDbContext<RentDbContext>(dbContextOptions =>
{
    dbContextOptions.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

builder.Services.AddApplicationInsightsTelemetry();

var app = builder.Build();

// Apply migrations or ensure database exists
using (var scope = app.Services.CreateScope())
using (var context = scope.ServiceProvider.GetService<RentDbContext>())
{
    context.Database.EnsureCreated();
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "VehicleService API v1");
});

app.UseCors("AllowAllPolicy"); 
app.UseHttpsRedirection();



app.MapControllers();
app.Run();
