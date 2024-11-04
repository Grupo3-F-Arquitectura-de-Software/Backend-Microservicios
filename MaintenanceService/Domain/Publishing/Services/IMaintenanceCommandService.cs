using MaintenanceService.Domain.Publishing.Models.Commands;

namespace MaintenanceService.Domain.Publishing.Services;

public interface IMaintenanceCommandService
{
    Task<int> Handle(CreateMaintenanceCommand command);
}