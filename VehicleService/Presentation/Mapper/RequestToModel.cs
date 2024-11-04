using AutoMapper;
using VehicleService.Domain.Publishing.Models.Commands;
using VehicleService.Domain.Publishing.Models.Entities;

namespace VehicleService.Presentation.Mapper;

public class RequestToModel : Profile
{
    public RequestToModel()
    {

        CreateMap<CreateVehicleCommand, Vehicle>();
        CreateMap<UpdateVehicleCommand, Vehicle>();
        CreateMap<DeleteVehicleCommand, Vehicle>();

    }
}