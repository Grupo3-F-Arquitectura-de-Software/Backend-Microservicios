using AutoMapper;
using VehicleService.Domain.Publishing.Models.Commands;
using VehicleService.Domain.Publishing.Models.Entities;

namespace VehicleService.Presentation.Mapper;

public class ModelToRequest : Profile
{
    public ModelToRequest()
    {
        CreateMap<Vehicle, CreateVehicleCommand>();
        CreateMap<Vehicle, UpdateVehicleCommand>();
        CreateMap<Vehicle, DeleteVehicleCommand>();
    }
}