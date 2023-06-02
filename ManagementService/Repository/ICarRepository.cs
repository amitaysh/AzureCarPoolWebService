using ManagementService.DomainModels;

namespace ManagementService
{
    public interface ICarRepository
    {
        Task<List<Car>> GetAllCarsAsync();
        Task<List<Car>> GetAllAvailableCarsAsync();
        Task<Car?> GetCarByIdAsync(Guid carId);
        Task<Car?> GetCarByUserIdAsync(Guid userId);
        Task AddCarAsync(Car car);
        Task UpdateCarAsync(Car car);
        Task DeleteCarAsync(Guid carId);
    }
}