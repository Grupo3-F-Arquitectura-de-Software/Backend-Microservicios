using AutoMapper;
using VehicleService.Domain.Publishing.Models.Commands;
using VehicleService.Domain.Publishing.Models.Entities;
using VehicleService.Domain.Publishing.Models.Response;

namespace VehicleService.Presentation.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Vehicle, VehicleResponse>();
            CreateMap<CreateVehicleCommand, Vehicle>();
            CreateMap<UpdateVehicleCommand, Vehicle>();
        }
    }
}