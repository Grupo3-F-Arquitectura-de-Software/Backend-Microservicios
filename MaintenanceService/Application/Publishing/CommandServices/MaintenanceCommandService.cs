using AutoMapper;
using MaintenanceService.Domain.Publishing.Models.Commands;
using MaintenanceService.Domain.Publishing.Models.Entities;
using MaintenanceService.Domain.Publishing.Repositories;
using MaintenanceService.Domain.Publishing.Services;

namespace MaintenanceService.Application.Publishing.CommandServices;

public class MaintenanceCommandService : IMaintenanceCommandService
{
    private readonly IMaintenanceRepository _maintenanceRepository;
    private readonly IMapper _mapper;
    
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

    public async Task<bool> Handle(UpdateMaintenanceCommand command)
    {
        var maintenance = _mapper.Map<UpdateMaintenanceCommand, Maintenance>(command);
        return await _maintenanceRepository.UpdateAsync(maintenance);
    }

    public async Task<bool> Handle(DeleteMaintenanceCommand command)
    {
        var existingMaintenance = await _maintenanceRepository.GetByIdAsync(command.Id);
        if (existingMaintenance == null) return false;
        return await _maintenanceRepository.DeleteAsync(command.Id);
    }
}