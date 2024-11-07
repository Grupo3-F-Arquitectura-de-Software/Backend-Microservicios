using MaintenanceService.Domain.Publishing.Models.Commands;

namespace MaintenanceService.Domain.Publishing.Services;

public interface IMaintenanceCommandService
{
    Task<int> Handle(CreateMaintenanceCommand command);
    Task<bool> Handle(UpdateMaintenanceCommand command);
    Task<bool> Handle(DeleteMaintenanceCommand command);
}