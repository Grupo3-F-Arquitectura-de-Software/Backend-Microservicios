using System.Net.Mime;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RentService.Domain.Publishing.Models.Commands;
using RentService.Domain.Publishing.Models.Queries;
using RentService.Domain.Publishing.Models.Response;
using RentService.Domain.Publishing.Services;

namespace RentService.Presentation.Publishing.Controllers;

    [Route("api/[controller]")]
    [ApiController]
    public class RentController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IRentCommandService _rentCommandService;
        private readonly IRentQueryService _rentQueryService;
        private readonly HttpClient _httpClient;
        
        public RentController(IRentQueryService rentQueryService, IRentCommandService rentCommandService, IMapper mapper, HttpClient httpClient)
        {
            _rentQueryService = rentQueryService;
            _rentCommandService = rentCommandService;
            _mapper = mapper;
            _httpClient = httpClient; // Inyectamos HttpClient

        }
        
        [HttpGet]
        [ProducesResponseType(typeof(List<RentResponse>), 200)]
        [ProducesResponseType(typeof(void),statusCode: StatusCodes.Status404NotFound)]
        [ProducesResponseType(500)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> GetAllAsync()
        {
            var result = await _rentQueryService.Handle(new GetAllRentsQuery());
            
            return Ok(result);
        }

        [HttpGet("{id}", Name = "GetRentById")]
        [ProducesResponseType(typeof(RentResponse), 200)]
        [ProducesResponseType(typeof(void),statusCode: StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(void),StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var result = await _rentQueryService.Handle(new GetRentByIdQuery(id));
            
            if (result==null) StatusCode(StatusCodes.Status404NotFound);

            return Ok(result);
        }
        
        [HttpGet("Tenant/{id}", Name = "GetRentByUserId")]
        [ProducesResponseType(typeof(RentResponse), 200)]
        [ProducesResponseType(typeof(void),statusCode: StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(void),StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetByUserIdAsync(int id)
        {
            var result = await _rentQueryService.Handle(new GetRentByUserIdQuery(id));
            
            if (result==null) StatusCode(StatusCodes.Status404NotFound);

            return Ok(result);
        }
        
        [HttpPost]
        [ProducesResponseType(typeof(int), 200)]
        [ProducesResponseType(typeof(void),statusCode: StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void),StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PostAsync([FromBody] CreateRentCommand command)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest();

                var rent = await _rentCommandService.Handle(command);

                return CreatedAtRoute("GetRentById", new { id = rent }, rent);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null) return StatusCode(StatusCodes.Status500InternalServerError, ex.InnerException.Message);
            }
            
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(void), 200)]
        [ProducesResponseType(typeof(void),statusCode: StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(void),StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PutAsync(int id, [FromBody] UpdateRentCommand command)
        {
            if (!ModelState.IsValid) return BadRequest();
            var result = await _rentCommandService.Handle(id, command);
            
            if (!result) StatusCode(StatusCodes.Status404NotFound);

            return Ok();
        }
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(void), 200)]
        [ProducesResponseType(typeof(void),statusCode: StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(void),StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            DeleteRentCommand command = new DeleteRentCommand { Id = id };
            
            var result = await _rentCommandService.Handle(command);
            
            if (!result) StatusCode(StatusCodes.Status404NotFound);

            return Ok();
        }
        
        [HttpGet("RentWithVehicle/{rentId}/{vehicleId}")]
        public async Task<IActionResult> GetRentWithVehicleInfo(int rentId, int vehicleId)
        {
            // Llamar a la API del VehicleService para obtener información del vehículo
            var vehicleResponse = await _httpClient.GetAsync($"https://drivesafefunda.azurewebsites.net/api/vehicle/{vehicleId}");

            if (!vehicleResponse.IsSuccessStatusCode)
            {
                return StatusCode((int)vehicleResponse.StatusCode, "Error al obtener información del vehículo");
            }

            var vehicleInfo = await vehicleResponse.Content.ReadAsStringAsync();

            // Obtener información del alquiler desde el servicio actual
            var rent = await _rentQueryService.Handle(new GetRentByIdQuery(rentId));

            if (rent == null) return NotFound();

            // Combinar información del alquiler y del vehículo
            var result = new
            {
                Rent = rent,
                Vehicle = vehicleInfo
            };

            return Ok(result);
        }

    }