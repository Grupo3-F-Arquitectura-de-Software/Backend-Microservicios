using AutoMapper;
using MaintenanceService.Domain.Publishing.Models.Commands;
using MaintenanceService.Domain.Publishing.Models.Entities;
using MaintenanceService.Domain.Publishing.Repositories;
using MaintenanceService.Domain.Publishing.Services;

namespace MaintenanceService.Application.Publishing.CommandServices;

public class MaintenanceCommandService : IMaintenanceCommandService
{
    public readonly IMaintenanceRepository _maintenanceRepository;
    public readonly IMapper _mapper;
    
    public MaintenanceCommandService(IMaintenanceRepository maintenanceRepository, IMapper mapper)
    {
        _maintenanceRepository = maintenanceRepository;
        _mapper = mapper;
    }
    
    public async Task<int> Handle(CreateMaintenanceCommand command)
    {
        var maintenance = _mapper.Map<CreateMaintenanceCommand, Maintenance>(command);
        
        return await _maintenanceRepository.SaveAsync(maintenance);
    }
}