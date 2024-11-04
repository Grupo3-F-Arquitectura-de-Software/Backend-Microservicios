using AutoMapper;
using MaintenanceService.Domain.Publishing.Models.Entities;
using MaintenanceService.Domain.Publishing.Models.Response;

namespace MaintenanceService.Presentation.Mapper;

public class ModelToResponse : Profile
{
    public ModelToResponse()
    {
        CreateMap<Maintenance, MaintenanceResponse>();
    }
}