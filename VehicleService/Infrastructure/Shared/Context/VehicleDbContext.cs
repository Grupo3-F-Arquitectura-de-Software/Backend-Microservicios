using Microsoft.EntityFrameworkCore;
using VehicleService.Domain.Publishing.Models.Entities;

namespace VehicleService.Infrastructure.Shared.Context;

public class VehicleDbContext : DbContext
{
    private readonly IConfiguration _configuration;

    public VehicleDbContext(DbContextOptions<VehicleDbContext> options, IConfiguration configuration) : base(options)
    {
        _configuration = configuration;
    }

    public DbSet<Vehicle> Vehicles { get; set; }



    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<Vehicle>().ToTable("Vehicles");


    }
}
