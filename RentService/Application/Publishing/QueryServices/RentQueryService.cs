using AutoMapper;
using RentService.Domain.Publishing.Models.Entities;
using RentService.Domain.Publishing.Models.Queries;
using RentService.Domain.Publishing.Models.Response;
using RentService.Domain.Publishing.Repositories;
using RentService.Domain.Publishing.Services;

namespace RentService.Application.Publishing.QueryServices;

public class RentQueryService : IRentQueryService
{
    private readonly IRentRepository _rentRepository;
    private readonly IMapper _mapper;
    
    public RentQueryService(IRentRepository rentRepository, IMapper mapper)
    {
        _rentRepository = rentRepository;
        _mapper = mapper;
    }
    
    public async Task<List<RentResponse>?> Handle(GetAllRentsQuery query)
    {
        var data = await _rentRepository.GetAllAsync();
        var result = _mapper.Map<List<Rent>, List<RentResponse>>(data);
        return result;
    }
    public async Task<RentResponse?> Handle(GetRentByIdQuery query)
    {
        var data = await _rentRepository.GetByIdAsync(query.Id);
        var result = _mapper.Map<Rent, RentResponse>(data);
        return result;
    }

    public async Task<List<RentResponse?>> Handle(GetRentByUserIdQuery query)
    {
        var data = await _rentRepository.GetByUserIdAsync(query.Id);
        var result = _mapper.Map<List<Rent>, List<RentResponse>>(data);
        return result;
    }
}