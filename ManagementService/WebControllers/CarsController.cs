using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using ManagementService.DTO;
using dm = ManagementService.DomainModels;
using AutoMapper;
using ManagementService.Services;

namespace Managements.WebControllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarsController : Controller
    {
        private readonly ICarService _carService;
        private readonly IMapper _mapper;

        public CarsController(ICarService carService, IMapper mapper)
        {
            _carService = carService;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all cars
        /// </summary>
        [HttpGet]
        [SwaggerOperation(Summary = "Get all cars", Description = "Retrieve all cars")]
        public async Task<ActionResult<List<Car>>> GetCarsAsync()
        {
            var cars = _mapper.Map<List<Car>>(await _carService.GetAllCarsAsync());
            return Ok(cars);
        }

        /// <summary>
        /// Get a car by ID
        /// </summary>
        /// <param name="carId">The ID of the car</param>
        [HttpGet("{carId}")]
        [SwaggerOperation(Summary = "Get a car by ID", Description = "Retrieve a car by its ID")]
        public async Task<ActionResult<Car>> GetCarByIdAsync(Guid carId)
        {
            var car = _mapper.Map<Car>(await _carService.GetCarByIdAsync(carId));
            if (car == null)
                return NotFound();

            return Ok(car);
        }

        /// <summary>
        /// Add a new car
        /// </summary>
        /// <param name="car">The car to add</param>
        [HttpPost]
        [SwaggerOperation(Summary = "Add a new car", Description = "Add a new car to the system")]
        public async Task<Car> AddCarAsync(CreateCar car)
        {
            var dmCar = _mapper.Map<dm.Car>(car);

            return _mapper.Map<Car>(await _carService.AddCarAsync(dmCar));
        }

        /// <summary>
        /// Update a car
        /// </summary>
        /// <param name="carId">The ID of the car</param>
        /// <param name="car">The updated car data</param>
        [HttpPut("{carId}")]
        [SwaggerOperation(Summary = "Update a car", Description = "Update an existing car")]
        public async Task<ActionResult<Car>> UpdateCarAsync(Guid carId, UpdateCar car)
        {
            try
            {
                return Ok(_mapper.Map<Car>(await _carService.UpdateCarAsync(carId, car)));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Delete a car
        /// </summary>
        /// <param name="carId">The ID of the car to delete</param>
        [HttpDelete("{carId}")]
        [SwaggerOperation(Summary = "Delete a car", Description = "Delete a car from the system")]
        public async Task<ActionResult> DeleteCarAsync(Guid carId)
        {
            try
            {
                await _carService.DeleteCarAsync(carId);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return Ok();
        }
    }
}
