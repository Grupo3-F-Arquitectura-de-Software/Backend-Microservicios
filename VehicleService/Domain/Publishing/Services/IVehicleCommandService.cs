using VehicleService.Domain.Publishing.Models.Commands;

namespace VehicleService.Domain.Publishing.Services;

public interface IVehicleCommandService
{
    Task<int> Handle(CreateVehicleCommand command);
    
    Task<bool> Handle(UpdateVehicleCommand command);
    
    Task<bool> Handle(DeleteVehicleCommand command);
}