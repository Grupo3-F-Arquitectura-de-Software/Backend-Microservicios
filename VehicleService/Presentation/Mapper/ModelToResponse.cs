using AutoMapper;
using VehicleService.Domain.Publishing.Models.Entities;
using VehicleService.Domain.Publishing.Models.Response;

namespace VehicleService.Presentation.Mapper;

public class ModelToResponse : Profile
{
    public ModelToResponse()
    {
        CreateMap<Vehicle, VehicleResponse>();
    }
}