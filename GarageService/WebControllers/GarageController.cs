using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using GarageService.DTO;
using AutoMapper;
using GarageService.Services;

namespace GarageService.WebControllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GaragesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IGarageService _garageService;

        public GaragesController(IMapper mapper, IGarageService garageService)
        {
            _mapper = mapper;
            _garageService = garageService;
        }

        [HttpGet]
        [SwaggerOperation("GetAllGarageRequests")]
        public async Task<ActionResult<List<Request>>> GetAllGarageRequestsAsync()
        {
            var garages = _mapper.Map<List<Request>>(await _garageService.GetAllGarageRequestsAsync());
            return Ok(garages);
        }

        [HttpGet("GetByCar/{carId}")]
        [SwaggerOperation("GetRequestsByCarId")]
        public async Task<ActionResult<List<Request>>> GetRequestsByCarIdAsync(string carId)
        {
            var Requests = _mapper.Map<List<Request>>(await _garageService.GetRequestsByCarIdAsync(carId));
            return Ok(Requests);
        }

        [HttpGet("GetByUser/{userId}")]
        [SwaggerOperation("GetRequestsByUserId")]
        public async Task<ActionResult<List<Request>>> GetRequestsByUserIdAsync(string userId)
        {
            var Requests = _mapper.Map<List<Request>>(await _garageService.GetRequestsByUserIdAsync(userId));
            return Ok(Requests);
        }
    }
}
