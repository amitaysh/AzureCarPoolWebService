using ManagementService.DomainModels;

namespace ManagementService.Services
{
    public interface ICarService
    {
        Task<List<Car>> GetAllCarsAsync();
        Task<List<Car>> GetAllAvailableCarsAsync();
        Task<Car?> GetCarByIdAsync(Guid carId);
        Task<Car?> GetCarByUserIdAsync(Guid userId);
        Task<Car?> AddCarAsync(Car car);
        Task<Car?> UpdateCarAsync(Guid carId, DTO.UpdateCar car);
        Task DeleteCarAsync(Guid carId);
        Task AssignCarToUser(Guid carId, Guid userId);
        Task ClearCarAssignment(Guid carId);
    }
}
