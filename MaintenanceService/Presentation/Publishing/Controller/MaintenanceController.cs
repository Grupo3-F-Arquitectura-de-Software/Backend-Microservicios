using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MaintenanceService.Application.Publishing.CommandServices;
using MaintenanceService.Application.Publishing.QueryServices;
using MaintenanceService.Domain.Publishing.Models.Commands;
using MaintenanceService.Domain.Publishing.Models.Queries;
using MaintenanceService.Domain.Publishing.Models.Response;
using VehicleService.Infrastructure.Http;
using System.Net.Mime;
using MaintenanceService.Domain.Publishing.Services;
using Microsoft.Extensions.Configuration;

namespace MaintenanceService.Presentation.Publishing.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MaintenanceController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IMaintenanceCommandService _maintenanceCommandService;
        private readonly IMaintenanceQueryService _maintenanceQueryService;
        private readonly AuthenticatedHttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public MaintenanceController(
            IMaintenanceQueryService maintenanceQueryService,
            IMaintenanceCommandService maintenanceCommandService,
            IMapper mapper,
            AuthenticatedHttpClient httpClient,
            IConfiguration configuration)
        {
            _maintenanceQueryService = maintenanceQueryService;
            _maintenanceCommandService = maintenanceCommandService;
            _mapper = mapper;
            _httpClient = httpClient;
            _configuration = configuration;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<MaintenanceResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> GetAllAsync()
        {
            var result = await _maintenanceQueryService.Handle(new GetAllMaintenancesQuery());
            if (result == null || !result.Any()) return NotFound("No se encontraron registros");
            return Ok(result);
        }

        [HttpGet("{id}", Name = "GetMaintenanceById")]
        [ProducesResponseType(typeof(MaintenanceResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var result = await _maintenanceQueryService.Handle(new GetMaintenanceByIdQuery(id));
            if (result == null) return NotFound("Registro no encontrado");
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PostAsync([FromBody] CreateMaintenanceCommand command)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _maintenanceCommandService.Handle(command);
            return CreatedAtRoute("GetMaintenanceById", new { id = result }, result);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PutAsync(int id, [FromBody] UpdateMaintenanceCommand command)
        {
            if (command == null) return BadRequest("El comando no puede ser nulo");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            command.Id = id;
            var result = await _maintenanceCommandService.Handle(command);
            if (!result) return NotFound();

            return Ok(result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var command = new DeleteMaintenanceCommand { Id = id };
            var result = await _maintenanceCommandService.Handle(command);
            if (!result) return NotFound();

            return Ok(result);
        }

        [HttpGet("MaintenanceWithVehicle/{maintenanceId}/{vehicleId}")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMaintenanceWithVehicleInfo(int maintenanceId, int vehicleId)
        {
            try
            {
                var gatewayUrl = _configuration["ApiGateway:Url"];
                var vehicleResponse = await _httpClient.GetAsync($"{gatewayUrl}/gateway/vehicle/{vehicleId}");

                if (!vehicleResponse.IsSuccessStatusCode)
                {
                    return StatusCode((int)vehicleResponse.StatusCode, "Error al obtener información del vehículo");
                }

                var vehicleInfo = await vehicleResponse.Content.ReadAsStringAsync();
                var maintenance = await _maintenanceQueryService.Handle(new GetMaintenanceByIdQuery(maintenanceId));

                if (maintenance == null) return NotFound("Mantenimiento no encontrado");

                var result = new
                {
                    Maintenance = maintenance,
                    Vehicle = vehicleInfo
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }
    }
}