using ManagementService.DomainModels;

namespace ManagementService.Services
{
    public class CarService : ICarService
    {
        private readonly ICarRepository _carRepository;

        public CarService(ICarRepository carRepository)
        {
            _carRepository = carRepository;
        }

        public async Task<Car?> AddCarAsync(Car car)
        {
            await _carRepository.AddCarAsync(car);

            return await _carRepository.GetCarByIdAsync(car.Id);
        }

        public async Task DeleteCarAsync(Guid carId)
        {
            var dmCar = await _carRepository.GetCarByIdAsync(carId);
            if (dmCar?.UserId != null)
            {
                throw new Exception("Cannot delete car since a user is assigned to it");
            }
            await _carRepository.DeleteCarAsync(carId);
        }

        public async Task<List<Car>> GetAllAvailableCarsAsync()
        {
            return await _carRepository.GetAllAvailableCarsAsync();
        }

        public async Task<List<Car>> GetAllCarsAsync()
        {
            return await _carRepository.GetAllCarsAsync();
        }

        public async Task<Car?> GetCarByUserIdAsync(Guid userId)
        {
            return await _carRepository.GetCarByUserIdAsync(userId);
        }

        public async Task<Car?> GetCarByIdAsync(Guid carId)
        {
            return await _carRepository.GetCarByIdAsync(carId);
        }

        public async Task<Car?> UpdateCarAsync(Guid carId, DTO.UpdateCar car)
        {
            var dmCar = await _carRepository.GetCarByIdAsync(carId);
            if (dmCar == null)
            {
                throw new Exception("Could not find car");
            }
            dmCar.PhotoUrl = car.PhotoUrl;
            await _carRepository.UpdateCarAsync(dmCar);

            return dmCar;
        }

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
