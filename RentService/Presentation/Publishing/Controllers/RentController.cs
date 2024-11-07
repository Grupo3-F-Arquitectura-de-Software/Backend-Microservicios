using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RentService.Application.Publishing.CommandServices;
using RentService.Application.Publishing.QueryServices;
using RentService.Domain.Publishing.Models.Commands;
using RentService.Domain.Publishing.Models.Queries;
using RentService.Domain.Publishing.Models.Response;
using VehicleService.Infrastructure.Http;
using System.Net.Mime;
using RentService.Domain.Publishing.Models.Response;
using RentService.Domain.Publishing.Services;
using Microsoft.Extensions.Configuration;

namespace RentService.Presentation.Publishing.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RentController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IRentCommandService _rentCommandService;
        private readonly IRentQueryService _rentQueryService;
        private readonly AuthenticatedHttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public RentController(
            IRentQueryService rentQueryService,
            IRentCommandService rentCommandService,
            IMapper mapper,
            AuthenticatedHttpClient httpClient,
            IConfiguration configuration)
        {
            _rentQueryService = rentQueryService;
            _rentCommandService = rentCommandService;
            _mapper = mapper;
            _httpClient = httpClient;
            _configuration = configuration;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<RentResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllAsync()
        {
            var result = await _rentQueryService.Handle(new GetAllRentsQuery());
            if (result == null || !result.Any()) return NotFound("No se encontraron registros");
            return Ok(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(RentResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var result = await _rentQueryService.Handle(new GetRentByIdQuery(id));
            if (result == null) return NotFound("Registro no encontrado");
            return Ok(result);
        }

       [HttpPost]
[ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
[ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
[ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
public async Task<IActionResult> PostAsync([FromBody] CreateRentCommand command)
{
    if (!ModelState.IsValid) return BadRequest(ModelState);

    var result = await _rentCommandService.Handle(command);
    return Created($"/api/rent/{result}", result);
}

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PutAsync(int id, [FromBody] UpdateRentCommand command)
        {
            if (command == null) return BadRequest("El comando no puede ser nulo");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _rentCommandService.Handle(id, command);
            if (!result) return NotFound();

            return Ok(result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var command = new DeleteRentCommand { Id = id };
            
            var result = await _rentCommandService.Handle(command);
            if (!result) return NotFound();

            return Ok(result);
        }

       [HttpGet("RentWithVehicle/{rentId}/{vehicleId}")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetRentWithVehicleInfo(int rentId, int vehicleId)
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
            var rent = await _rentQueryService.Handle(new GetRentByIdQuery(rentId));

            if (rent == null) return NotFound("Alquiler no encontrado");

            var result = new
            {
                Rent = rent,
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