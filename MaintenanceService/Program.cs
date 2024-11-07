using MaintenanceService.Application.Publishing.CommandServices;
using MaintenanceService.Application.Publishing.QueryServices;
using MaintenanceService.Domain.Publishing.Repositories;
using MaintenanceService.Domain.Publishing.Services;
using MaintenanceService.Infrastructure.Publishing;
using MaintenanceService.Infraestructure.Shared;
using MaintenanceService.Presentation.Mapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using VehicleService.Infrastructure.Http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<AuthenticatedHttpClient>();

// Configuración de AutoMapper
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

// Configuración de la base de datos
builder.Services.AddDbContext<MaintenanceDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection")))
        .EnableSensitiveDataLogging()
        .EnableDetailedErrors());

// Registro de servicios
builder.Services.AddScoped<IMaintenanceRepository, MaintenanceRepository>();
builder.Services.AddScoped<IMaintenanceCommandService, MaintenanceCommandService>();
builder.Services.AddScoped<IMaintenanceQueryService, MaintenanceQueryService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<AuthenticatedHttpClient>();

// Configuración JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Secret"])),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

// Configuración CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllPolicy",
        builder => builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

// Configuración Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "MaintenanceService API", Version = "v1" });
    
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
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
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Ensure database is created
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
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();