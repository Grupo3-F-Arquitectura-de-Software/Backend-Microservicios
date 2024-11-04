using MaintenanceService.Domain.Publishing.Models.Queries;
using MaintenanceService.Domain.Publishing.Models.Response;

namespace MaintenanceService.Domain.Publishing.Services;

public interface IMaintenanceQueryService
{
    Task<List<MaintenanceResponse>?> Handle(GetAllMaintenancesQuery query);
    
    Task<MaintenanceResponse?> Handle(GetMaintenanceByIdQuery query);
}