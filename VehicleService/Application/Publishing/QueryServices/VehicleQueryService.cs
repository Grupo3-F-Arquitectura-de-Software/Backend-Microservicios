using AutoMapper;
using VehicleService.Domain.Publishing.Models.Entities;
using VehicleService.Domain.Publishing.Models.Queries;
using VehicleService.Domain.Publishing.Models.Response;
using VehicleService.Domain.Publishing.Repositories;
using VehicleService.Domain.Publishing.Services;

namespace VehicleService.Application.Publishing.QueryServices;

public class VehicleQueryService : IVehicleQueryService
{
    private readonly IVehicleRepository _vehicleRepository;
    private readonly IMapper _mapper;
    
    public VehicleQueryService(IVehicleRepository vehicleRepository, IMapper mapper)
    {
        _vehicleRepository = vehicleRepository;
        _mapper = mapper;
    }
    
    public async Task<List<VehicleResponse>?> Handle(GetAllVehiclesQuery query)
    {
        var data = await _vehicleRepository.GetAllAsync();
        var result = _mapper.Map<List<Vehicle>, List<VehicleResponse>>(data);
        return result;
    }

    public async Task<VehicleResponse?> Handle(GetVehicleByIdQuery query)
    {
        var data = await _vehicleRepository.GetByIdAsync(query.Id);
        var result = _mapper.Map<Vehicle, VehicleResponse>(data);
        return result;
    }

    public async Task<List<VehicleResponse?>> Handle(GetVehicleByUserIdQuery query)
    {
        
        var data = await _vehicleRepository.GetByUserIdAsync(query.Id);
        var result = _mapper.Map<List<Vehicle>, List<VehicleResponse>>(data);
        return result;
    }
}