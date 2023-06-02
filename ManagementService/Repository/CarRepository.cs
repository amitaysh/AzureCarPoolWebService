using Microsoft.EntityFrameworkCore;
using ManagementService.DomainModels;

namespace ManagementService
{
    public class CarRepository : ICarRepository
    {
        private readonly ApplicationDbContext _context;

        public CarRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Car>> GetAllAvailableCarsAsync()
        {
            return await _context.Cars.Where(car => car.UserId == null).ToListAsync();
        }

        public async Task<List<Car>> GetAllCarsAsync()
        {
            return await _context.Cars.ToListAsync();
        }

        public async Task<Car?> GetCarByUserIdAsync(Guid userId)
        {
            return await _context.Cars.FirstOrDefaultAsync(car => car.UserId == userId);
        }
        public async Task<Car?> GetCarByIdAsync(Guid carId)
        {
            return await _context.Cars.FindAsync(carId);
        }

        public async Task AddCarAsync(Car car)
        {
            _context.Cars.Add(car);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCarAsync(Car car)
        {
            _context.Entry(car).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCarAsync(Guid carId)
        {
            var car = await _context.Cars.FindAsync(carId);
            if (car == null)
                throw new KeyNotFoundException($"Could not find car with id {carId}");

            _context.Cars.Remove(car);
            await _context.SaveChangesAsync();
        }
    }
}