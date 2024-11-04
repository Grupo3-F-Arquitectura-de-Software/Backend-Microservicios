using MaintenanceService.Application.Publishing.CommandServices;
using MaintenanceService.Application.Publishing.QueryServices;
using MaintenanceService.Domain.Publishing.Repositories;
using MaintenanceService.Domain.Publishing.Services;
using MaintenanceService.Infraestructure.Publishing;
using MaintenanceService.Infraestructure.Shared;
using MaintenanceService.Presentation.Mapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();


//Ad cors
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllPolicy", 
        policy => policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

//Configuration
var configuration = new ConfigurationBuilder()
    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
    .AddJsonFile("appsettings.json", true, true)
    .Build();

builder.Services.AddSingleton(configuration);

// Add services to the container.
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
        Title = "MaintenanceService API",
        Description = "An ASP.NET Core Web API for managing maintenances",
    });
});


builder.Services.AddScoped<IMaintenanceRepository, MaintenanceRepository>();
builder.Services.AddScoped<IMaintenanceCommandService, MaintenanceCommandService>();
builder.Services.AddScoped<IMaintenanceQueryService, MaintenanceQueryService>();

builder.Services.AddAutoMapper(
    typeof(RequestToModel),
    typeof(ModelToRequest),
    typeof(ModelToResponse));

var connectionString = builder.Configuration.GetConnectionString("DriveSafeDB");

builder.Services.AddDbContext<MaintenanceDbContext>(dbContextOptions =>
{
    dbContextOptions.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});


builder.Services.AddApplicationInsightsTelemetry();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
using (var context = scope.ServiceProvider.GetService<MaintenanceDbContext>())
{
    context.Database.EnsureCreated();
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "MaintenanceService API v1");
});

app.UseCors("AllowAllPolicy"); 
app.UseHttpsRedirection();

app.MapControllers();
app.Run();