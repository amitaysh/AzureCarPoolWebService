using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using ManagementService.DTO;
using AutoMapper;
using ManagementService.Services;

namespace Managements.WebControllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AssignmentController : Controller
    {
        private readonly IUserService _userService;
        private readonly ICarService _carService;
        private readonly IMapper _mapper;
        private readonly IAssignmentService _assignmentService;

        public AssignmentController(IUserService userService, ICarService carService, IMapper mapper, IAssignmentService assignmentService)
        {
            _userService = userService;
            _carService = carService;
            _mapper = mapper;
            _assignmentService = assignmentService;
        }

        /// <summary>
        /// Get all available cars
        /// </summary>
        [HttpGet]
        [SwaggerOperation(Summary = "Get all available cars", Description = "Retrieve all available cars")]
        public async Task<ActionResult<List<Car>>> GetAvailableCarsAsync()
        {
            var cars = _mapper.Map<List<Car>>(await _carService.GetAllAvailableCarsAsync());
            return Ok(cars);
        }

        /// <summary>
        /// Get a car by User ID
        /// </summary>
        /// <param name="userId">The ID of the user</param>
        [HttpGet("/ByUserId/{userId}")]
        [SwaggerOperation(Summary = "Get a car by user ID", Description = "Retrieve a car by user ID")]
        public async Task<ActionResult<Car>> GetCarByIdAsync(Guid userId)
        {
            var car = _mapper.Map<Car>(await _carService.GetCarByUserIdAsync(userId));
            if (car == null)
                return NotFound();

            return Ok(car);
        }

        /// <summary>
        /// Get a user by car ID
        /// </summary>
        /// <param name="carId">The ID of the car</param>
        [HttpGet("/ByCarId{carId}")]
        [SwaggerOperation(Summary = "Get a user by car ID", Description = "Retrieve a user by car ID")]
        public async Task<ActionResult<User>> GetUserByIdAsync(Guid carId)
        {
            var user = _mapper.Map<User>(await _userService.GetUserByCarIdAsync(carId));
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        /// <summary>
        /// Assign car and user
        /// </summary>
        /// <param name="assignCarUser">The car to add</param>
        [HttpPost("/AssignCarUser")]
        [SwaggerOperation(Summary = "Assign car and user", Description = "Assign car and user")]
        public async Task<IActionResult> AssignCarUserAsync(AssignCarUser assignCarUser)
        {
            try
            {
                await _assignmentService.AssignCarUserAsync(assignCarUser);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return Ok("Successfully assigned car and user");
        }

        /// <summary>
        /// Clear assignment by car id
        /// </summary>
        /// <param name="carId">The car id</param>
        [HttpPost("/ClearByCarId/{clearByCarId}")]
        [SwaggerOperation(Summary = "Clear assignment by car id", Description = "Clear assignment by car id")]
        public async Task<IActionResult> ClearAssignmentByCarIdAsync(Guid clearByCarId)
        {
            try
            {
                await _assignmentService.ClearAssignmentByCarIdAsync(clearByCarId);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return Ok("Successfully clear car and user");
        }


        /// <summary>
        /// Clear assignment by user id
        /// </summary>
        /// <param name="userId">The user id</param>
        [HttpPost("/ClearByUserId/{clearByUserId}")]
        [SwaggerOperation(Summary = "Clear assignment by user id", Description = "Clear assignment by user id")]
        public async Task<IActionResult> ClearAssignmentByUserIdAsync(Guid clearByUserId)
        {
            try
            {
                await _assignmentService.ClearAssignmentByUserIdAsync(clearByUserId);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return Ok("Successfully clear car and user");
        }
    }
}
