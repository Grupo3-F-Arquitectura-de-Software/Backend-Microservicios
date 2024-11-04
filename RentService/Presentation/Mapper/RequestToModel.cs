using AutoMapper;
using RentService.Domain.Publishing.Models.Commands;
using RentService.Domain.Publishing.Models.Entities;

namespace RentService.Presentation.Mapper;

public class RequestToModel : Profile
{
    public RequestToModel()
    {
        CreateMap<CreateRentCommand, Rent>();
        CreateMap<UpdateRentCommand, Rent>();
        CreateMap<DeleteRentCommand, Rent>();
    }
}