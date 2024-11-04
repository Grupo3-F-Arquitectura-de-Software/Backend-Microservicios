using AutoMapper;
using RentService.Domain.Publishing.Models.Commands;
using RentService.Domain.Publishing.Models.Entities;

namespace RentService.Presentation.Mapper;

public class ModelToRequest : Profile
{
    public ModelToRequest()
    {
        CreateMap<Rent, CreateRentCommand>();
        CreateMap<Rent, UpdateRentCommand>();
        CreateMap<Rent, DeleteRentCommand>();
    }
}