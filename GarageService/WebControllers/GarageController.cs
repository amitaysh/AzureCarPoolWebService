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
    }
}
