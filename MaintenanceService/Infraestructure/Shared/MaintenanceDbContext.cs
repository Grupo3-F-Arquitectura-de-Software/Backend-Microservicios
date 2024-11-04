using MaintenanceService.Domain.Publishing.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace MaintenanceService.Infraestructure.Shared;

public class MaintenanceDbContext : DbContext
{
    private readonly IConfiguration _configuration;

    public MaintenanceDbContext(DbContextOptions<MaintenanceDbContext> options, IConfiguration configuration): base(options)
    {
        _configuration = configuration;
    }

    public DbSet<Maintenance> Maintenances { get; set; }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<Maintenance>().ToTable("Maintenance");
    }

}