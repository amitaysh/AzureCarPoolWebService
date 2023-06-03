using ManagementService.DomainModels;

namespace ManagementService.Services
{
    public class CarService : ICarService
    {
        private readonly ICarRepository _carRepository;

        // car service ctor
        public CarService(ICarRepository carRepository)
        {
            _carRepository = carRepository;
        }

        // add car
        public async Task<Car?> AddCarAsync(Car car)
        {
            await _carRepository.AddCarAsync(car);

            return await _carRepository.GetCarByIdAsync(car.Id);
        }

        // delete car
        public async Task DeleteCarAsync(Guid carId)
        {
            var dmCar = await _carRepository.GetCarByIdAsync(carId);
            if (dmCar?.UserId != null)
            {
                throw new Exception("Cannot delete car since a user is assigned to it");
            }
            await _carRepository.DeleteCarAsync(carId);
        }

        // get all available cars
        public async Task<List<Car>> GetAllAvailableCarsAsync()
        {
            return await _carRepository.GetAllAvailableCarsAsync();
        }

        // get all cars
        public async Task<List<Car>> GetAllCarsAsync()
        {
            return await _carRepository.GetAllCarsAsync();
        }

        // get car by user id
        public async Task<Car?> GetCarByUserIdAsync(Guid userId)
        {
            return await _carRepository.GetCarByUserIdAsync(userId);
        }

        // get car by car id
        public async Task<Car?> GetCarByIdAsync(Guid carId)
        {
            return await _carRepository.GetCarByIdAsync(carId);
        }

        // update car
        public async Task<Car?> UpdateCarAsync(Guid carId, DTO.UpdateCar car)
        {
            var dmCar = await _carRepository.GetCarByIdAsync(carId);
            if (dmCar == null)
            {
                throw new Exception("Could not find car");
            }
            dmCar.PhotoName = car.PhotoName;
            await _carRepository.UpdateCarAsync(dmCar);

            return dmCar;
        }

        // assign car to user (starting transaction)
        public async Task AssignCarToUser(Guid carId, Guid userId)
        {
            var dmCar = await _carRepository.GetCarByIdAsync(carId);
            if (dmCar == null)
                throw new Exception("Could not find car");
            if (dmCar.UserId != null)
                throw new Exception("The car already assigned with a user. Please first clear the assignment and then assign new user.");
            dmCar.UserId = userId;
            await _carRepository.UpdateCarAsync(dmCar);
        }

        // clear car and user (closing transaction)
        public async Task ClearCarAssignment(Guid carId)
        {
            var dmCar = await _carRepository.GetCarByIdAsync(carId);
            if (dmCar == null)
                throw new Exception("Could not find car");
            dmCar.UserId = null;
            await _carRepository.UpdateCarAsync(dmCar);
        }
    }
}
