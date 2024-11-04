using VehicleService.Domain.Publishing.Models.Queries;
using VehicleService.Domain.Publishing.Models.Response;

namespace VehicleService.Domain.Publishing.Services;

public interface IVehicleQueryService
{
    Task<List<VehicleResponse>?> Handle(GetAllVehiclesQuery query);
    
    Task<VehicleResponse?> Handle(GetVehicleByIdQuery query);
    
    Task<List<VehicleResponse?>> Handle(GetVehicleByUserIdQuery query);
}