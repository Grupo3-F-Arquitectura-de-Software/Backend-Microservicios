using AutoMapper;
using MaintenanceService.Domain.Publishing.Models.Commands;
using MaintenanceService.Domain.Publishing.Models.Entities;
using MaintenanceService.Domain.Publishing.Models.Response;

namespace MaintenanceService.Presentation.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Maintenance, MaintenanceResponse>();
            CreateMap<CreateMaintenanceCommand, Maintenance>();
            CreateMap<UpdateMaintenanceCommand, Maintenance>();
        }
    }
} 