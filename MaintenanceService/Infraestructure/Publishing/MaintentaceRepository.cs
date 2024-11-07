using MaintenanceService.Domain.Publishing.Models.Entities;
using MaintenanceService.Domain.Publishing.Repositories;
using MaintenanceService.Infraestructure.Shared;
using Microsoft.EntityFrameworkCore;

namespace MaintenanceService.Infrastructure.Publishing;

public class MaintenanceRepository : IMaintenanceRepository
{
    private readonly MaintenanceDbContext _driveSafeDbContext;
    
    public MaintenanceRepository(MaintenanceDbContext driveSafeDbContext)
    {
        _driveSafeDbContext = driveSafeDbContext;
    }
    
    public async Task<List<Maintenance>> GetAllAsync()
    {
        return await _driveSafeDbContext.Maintenances.Where(m => m.IsActive).ToListAsync();
    }

    public async Task<Maintenance> GetByIdAsync(int id)
    {
        return await _driveSafeDbContext.Maintenances.FirstOrDefaultAsync(m => m.Id == id && m.IsActive);
    }

    public async Task<int> SaveAsync(Maintenance data)
    {
        await using var transaction = await _driveSafeDbContext.Database.BeginTransactionAsync();
        try
        {
            data.IsActive = true;
            _driveSafeDbContext.Maintenances.Add(data);
            await _driveSafeDbContext.SaveChangesAsync();
            await transaction.CommitAsync();
            return data.Id;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<bool> UpdateAsync(Maintenance data)
    {
        await using var transaction = await _driveSafeDbContext.Database.BeginTransactionAsync();
        try
        {
            var existingMaintenance = await GetByIdAsync(data.Id);
            if (existingMaintenance == null) return false;

            _driveSafeDbContext.Entry(existingMaintenance).CurrentValues.SetValues(data);
            await _driveSafeDbContext.SaveChangesAsync();
            await transaction.CommitAsync();
            return true;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        await using var transaction = await _driveSafeDbContext.Database.BeginTransactionAsync();
        try
        {
            var maintenance = await GetByIdAsync(id);
            if (maintenance == null) return false;

            maintenance.IsActive = false;
            await _driveSafeDbContext.SaveChangesAsync();
            await transaction.CommitAsync();
            return true;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}