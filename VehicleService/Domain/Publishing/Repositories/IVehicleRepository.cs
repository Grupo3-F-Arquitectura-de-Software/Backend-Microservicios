using VehicleService.Domain.Publishing.Models.Entities;

namespace VehicleService.Domain.Publishing.Repositories;

public interface IVehicleRepository
{
    Task<List<Vehicle>> GetAllAsync();
    Task<Vehicle> GetByIdAsync(int id);
    Task<int> SaveAsync(Vehicle data);
    Task<bool> UpdateAsync(Vehicle data, int id);
    Task<bool> DeleteAsync(int id);
    Task<List<Vehicle>> GetByUserIdAsync(int userId);
}