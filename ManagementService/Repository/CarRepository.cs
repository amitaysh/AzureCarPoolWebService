using Microsoft.EntityFrameworkCore;
using ManagementService.DomainModels;

namespace ManagementService
{
    public class CarRepository : ICarRepository
    {
        private readonly ApplicationDbContext _context;

        // Car repository ctor
        public CarRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // get all available cars
        public async Task<List<Car>> GetAllAvailableCarsAsync()
        {
            return await _context.Cars.Where(car => car.UserId == null).ToListAsync();
        }

        // get all cars
        public async Task<List<Car>> GetAllCarsAsync()
        {
            return await _context.Cars.ToListAsync();
        }

        // get car by user id
        public async Task<Car?> GetCarByUserIdAsync(Guid userId)
        {
            return await _context.Cars.FirstOrDefaultAsync(car => car.UserId == userId);
        }

        // get car by car id
        public async Task<Car?> GetCarByIdAsync(Guid carId)
        {
            return await _context.Cars.FindAsync(carId);
        }

        // add new car to DB
        public async Task AddCarAsync(Car car)
        {
            _context.Cars.Add(car);
            await _context.SaveChangesAsync();
        }

        // update car info
        public async Task UpdateCarAsync(Car car)
        {
            _context.Entry(car).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        // delete car from db
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