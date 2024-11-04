using AutoMapper;
using RentService.Domain.Publishing.Models.Entities;
using RentService.Domain.Publishing.Models.Response;

namespace RentService.Presentation.Mapper;

public class ModelToResponse : Profile
{
    public ModelToResponse()
    {
        CreateMap<Rent, RentResponse>();
    }
}