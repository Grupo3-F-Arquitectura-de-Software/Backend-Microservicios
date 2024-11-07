using AutoMapper;
using RentService.Domain.Publishing.Models.Commands;
using RentService.Domain.Publishing.Models.Entities;
using RentService.Domain.Publishing.Models.Response;

namespace RentService.Presentation.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Rent, RentResponse>();
            CreateMap<CreateRentCommand, Rent>();
            CreateMap<UpdateRentCommand, Rent>();
        }
    }
}