using AutoMapper;
using MaintenanceService.Domain.Publishing.Models.Commands;
using MaintenanceService.Domain.Publishing.Models.Entities;

namespace MaintenanceService.Presentation.Mapper;

public class ModelToRequest : Profile
{
    public ModelToRequest()
    {
        CreateMap<Maintenance, CreateMaintenanceCommand>();
    }
}