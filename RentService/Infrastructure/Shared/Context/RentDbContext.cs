using Microsoft.EntityFrameworkCore;
using RentService.Domain.Publishing.Models.Entities;

namespace RentService.Infrastructure.Shared.Context;

public class RentDbContext : DbContext
{
    private readonly IConfiguration _configuration;

    public RentDbContext(DbContextOptions<RentDbContext> options, IConfiguration configuration) : base(options)
    {
        _configuration = configuration;
    }

    public DbSet<Rent> Rents { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var serverVersion = new MySqlServerVersion(new Version(8, 0, 29));
            optionsBuilder.UseMySql(_configuration["ConnectionStrings:RentDB"], serverVersion);
        }
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<Rent>().ToTable("Rents");
    }
}
