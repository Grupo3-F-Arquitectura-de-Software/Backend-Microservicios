using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VehicleService.Application.Publishing.CommandServices;
using VehicleService.Application.Publishing.QueryServices;
using VehicleService.Domain.Publishing.Models.Commands;
using VehicleService.Domain.Publishing.Models.Queries;
using VehicleService.Domain.Publishing.Models.Response;
using VehicleService.Infrastructure.Http;
using System.Net.Mime;
using VehicleService.Domain.Publishing.Services;
using Microsoft.Extensions.Configuration;
using VehicleService.Domain.Publishing.Models.Entities;

namespace VehicleService.Presentation.Publishing.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehicleController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IVehicleCommandService _vehicleCommandService;
        private readonly IVehicleQueryService _vehicleQueryService;
        private readonly AuthenticatedHttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public VehicleController(
            IVehicleQueryService vehicleQueryService,
            IVehicleCommandService vehicleCommandService,
            IMapper mapper,
            AuthenticatedHttpClient httpClient,
            IConfiguration configuration)
        {
            _vehicleQueryService = vehicleQueryService;
            _vehicleCommandService = vehicleCommandService;
            _mapper = mapper;
            _httpClient = httpClient;
            _configuration = configuration;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<VehicleResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> GetAllAsync()
        {
            var result = await _vehicleQueryService.Handle(new GetAllVehiclesQuery());
            if (result == null || !result.Any()) return NotFound("No se encontraron registros");
            return Ok(result);
        }

        [HttpGet("{id}", Name = "GetVehicleById")]
        [ProducesResponseType(typeof(VehicleResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var result = await _vehicleQueryService.Handle(new GetVehicleByIdQuery(id));
            if (result == null) return NotFound("Vehículo no encontrado");
            return Ok(result);
        }

        [HttpGet("Owner/{id}")]
        [ProducesResponseType(typeof(VehicleResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> GetByUserIdAsync(int id)
        {
            var result = await _vehicleQueryService.Handle(new GetVehicleByUserIdQuery(id));
            if (result == null) return NotFound("No se encontraron vehículos para este usuario");
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PostAsync([FromBody] CreateVehicleCommand command)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _vehicleCommandService.Handle(command);
            return CreatedAtRoute("GetVehicleById", new { id = result }, result);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PutAsync(int id, [FromBody] UpdateVehicleCommand command)
        {
            if (command == null) return BadRequest("El comando no puede ser nulo");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            command.Id = id;
            var result = await _vehicleCommandService.Handle(command);
            if (!result) return NotFound();

            return Ok(result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                var command = new DeleteVehicleCommand { Id = id };
                var result = await _vehicleCommandService.Handle(command);
                if (!result) return NotFound("Vehículo no encontrado");
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

       [HttpGet("VehicleWithMaintenance/{vehicleId}/{maintenanceId}")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetVehicleWithMaintenanceInfo(int vehicleId, int maintenanceId)
    {
        try
        {
            var gatewayUrl = _configuration["ApiGateway:Url"];
            var maintenanceResponse = await _httpClient.GetAsync($"{gatewayUrl}/gateway/maintenance/{maintenanceId}");

            if (!maintenanceResponse.IsSuccessStatusCode)
            {
                return StatusCode((int)maintenanceResponse.StatusCode, "Error al obtener información del mantenimiento");
            }

            var maintenanceInfo = await maintenanceResponse.Content.ReadAsStringAsync();
            var vehicle = await _vehicleQueryService.Handle(new GetVehicleByIdQuery(vehicleId));
            if (vehicle == null) return NotFound("Vehículo no encontrado");
            

            var result = new
            {
                Vehicle = vehicle,
                Maintenance = maintenanceInfo
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