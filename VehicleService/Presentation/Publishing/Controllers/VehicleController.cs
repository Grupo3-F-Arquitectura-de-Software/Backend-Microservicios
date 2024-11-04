using System.Net.Mime;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VehicleService.Domain.Publishing.Models.Commands;
using VehicleService.Domain.Publishing.Models.Queries;
using VehicleService.Domain.Publishing.Models.Response;
using VehicleService.Domain.Publishing.Services;

namespace VehicleService.Presentation.Publishing.Controllers;

[Route("api/[controller]")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IVehicleCommandService _vehicleCommandService;
        private readonly IVehicleQueryService _vehicleQueryService;
        
        public VehicleController(IVehicleQueryService vehicleQueryService, IVehicleCommandService vehicleCommandService, IMapper mapper)
        {
            _vehicleQueryService = vehicleQueryService;
            _vehicleCommandService = vehicleCommandService;
            _mapper = mapper;
        }
        
        [HttpGet]
        [ProducesResponseType(typeof(List<VehicleResponse>), 200)]
        [ProducesResponseType(typeof(void),statusCode: StatusCodes.Status404NotFound)]
        [ProducesResponseType(500)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> GetAllAsync()
        {
            var result = await _vehicleQueryService.Handle(new GetAllVehiclesQuery());
            
            return Ok(result);
        }

        [HttpGet("{id}", Name = "Get")]
        [ProducesResponseType(typeof(VehicleResponse), 200)]
        [ProducesResponseType(typeof(void),statusCode: StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(void),StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var result = await _vehicleQueryService.Handle(new GetVehicleByIdQuery(id));
            
            if (result==null) StatusCode(StatusCodes.Status404NotFound);

            return Ok(result);
        }
        
        [HttpGet("Owner/{id}")]
        [ProducesResponseType(typeof(VehicleResponse), 200)]
        [ProducesResponseType(typeof(void),statusCode: StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(void),StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetByUserIdAsync(int id)
        {
            var result = await _vehicleQueryService.Handle(new GetVehicleByUserIdQuery(id));
            
            if (result==null) StatusCode(StatusCodes.Status404NotFound);

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(void), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(void),StatusCodes.Status500InternalServerError)]
        public async Task <IActionResult> PostAsync([FromBody] CreateVehicleCommand command)
        {
            if (!ModelState.IsValid) return BadRequest();
            
            var result = await _vehicleCommandService.Handle(command);
            
            return StatusCode( StatusCodes.Status201Created, result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(void),StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            DeleteVehicleCommand command = new DeleteVehicleCommand {Id = id};
            
            var result = await _vehicleCommandService.Handle(command);
            
            if (!result) return StatusCode(StatusCodes.Status404NotFound);
            
            return NoContent();
        }
    }