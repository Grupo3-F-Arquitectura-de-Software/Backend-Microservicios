using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using VehicleService.Application.Publishing.CommandServices;
using VehicleService.Application.Publishing.QueryServices;
using VehicleService.Domain.Publishing.Repositories;
using VehicleService.Domain.Publishing.Services;
using VehicleService.Infrastructure.Publishing.Persistence;
using VehicleService.Infrastructure.Shared.Context;
using VehicleService.Presentation.Mapper;

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
        Title = "VehicleService API",
        Description = "An ASP.NET Core Web API for managing vehicles",
    });
});

// Register the Vehicle services and repositories
builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();
builder.Services.AddScoped<IVehicleCommandService, VehicleCommandService>();
builder.Services.AddScoped<IVehicleQueryService, VehicleQueryService>();

// Add AutoMapper (if necessary)
builder.Services.AddAutoMapper(typeof(RequestToModel), typeof(ModelToRequest), typeof(ModelToResponse));

// Database context for VehicleService
var connectionString = builder.Configuration.GetConnectionString("VehicleDB");

builder.Services.AddDbContext<VehicleDbContext>(dbContextOptions =>
{
    dbContextOptions.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString),
        mySqlOptions => mySqlOptions.EnableRetryOnFailure())  // Reintentos en caso de errores transitorios
        .LogTo(Console.WriteLine, LogLevel.Information);      // Registra eventos de la base de datos
});

builder.Services.AddApplicationInsightsTelemetry();

var app = builder.Build();

// Apply migrations or ensure database exists
using (var scope = app.Services.CreateScope())
using (var context = scope.ServiceProvider.GetService<VehicleDbContext>())
{
    context.Database.EnsureCreated();
}


// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "VehicleService API v1");
});


app.UseCors("AllowAllPolicy"); 
app.UseHttpsRedirection();

app.MapControllers();
app.Run();
