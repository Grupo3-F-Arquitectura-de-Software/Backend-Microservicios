using RentService.Domain.Publishing.Models.Queries;
using RentService.Domain.Publishing.Models.Response;

namespace RentService.Domain.Publishing.Services;

public interface IRentQueryService
{
    Task<List<RentResponse>?> Handle(GetAllRentsQuery query);
    Task<RentResponse?> Handle(GetRentByIdQuery query);
    Task<List<RentResponse?>> Handle(GetRentByUserIdQuery query);
}