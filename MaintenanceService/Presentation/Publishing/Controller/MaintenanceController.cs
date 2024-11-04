using System.Net.Mime;
using AutoMapper;
using MaintenanceService.Domain.Publishing.Models.Commands;
using MaintenanceService.Domain.Publishing.Models.Queries;
using MaintenanceService.Domain.Publishing.Models.Response;
using MaintenanceService.Domain.Publishing.Services;
using Microsoft.AspNetCore.Mvc;

namespace MaintenanceService.Presentation.Publishing.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaintenanceController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IMaintenanceCommandService _maintenanceCommandService;
        private readonly IMaintenanceQueryService _maintenanceQueryService;
        
        public MaintenanceController(IMaintenanceQueryService maintenanceQueryService, IMaintenanceCommandService maintenanceCommandService, IMapper mapper)
        {
            _maintenanceQueryService = maintenanceQueryService;
            _maintenanceCommandService = maintenanceCommandService;
            _mapper = mapper;
        }
        
        [HttpGet]
        [ProducesResponseType(typeof(List<MaintenanceResponse>), 200)]
        [ProducesResponseType(typeof(void),statusCode: StatusCodes.Status404NotFound)]
        [ProducesResponseType(500)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> GetAllAsync()
        {
            var result = await _maintenanceQueryService.Handle(new GetAllMaintenancesQuery());
            
            return Ok(result);
        }

        [HttpGet("{id}", Name = "GetMaintenanceById")]
        [ProducesResponseType(typeof(List<MaintenanceResponse>), 200)]
        [ProducesResponseType(typeof(void),statusCode: StatusCodes.Status404NotFound)]
        [ProducesResponseType(500)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var result = await _maintenanceQueryService.Handle(new GetMaintenanceByIdQuery(id));
            
            if (result==null) StatusCode(StatusCodes.Status404NotFound);

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(void), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PostAsync([FromBody] CreateMaintenanceCommand command)
        {
            if (!ModelState.IsValid) return BadRequest();
    
            var result = await _maintenanceCommandService.Handle(command);
    
            // Cambiar la ruta a "GetMaintenanceById" para que coincida con la ruta definida
            return CreatedAtRoute("GetMaintenanceById", new { id = result }, result);
        }

        
    }
}
