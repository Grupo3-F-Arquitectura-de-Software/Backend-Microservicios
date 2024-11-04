using MaintenanceService.Domain.Publishing.Models.Entities;

namespace MaintenanceService.Domain.Publishing.Repositories;

public interface IMaintenanceRepository
{
    Task<List<Maintenance>> GetAllAsync();
    Task<Maintenance> GetByIdAsync(int id);
    Task<int> SaveAsync(Maintenance data);
}