using AutoMapper;
using MaintenanceService.Domain.Publishing.Models.Commands;
using MaintenanceService.Domain.Publishing.Models.Entities;

namespace  MaintenanceService.Presentation.Mapper;

public class RequestToModel : Profile
{
    public RequestToModel()
    {

        CreateMap<CreateMaintenanceCommand, Maintenance>();
    }
}