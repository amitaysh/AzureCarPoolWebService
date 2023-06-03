using GarageService.DomainModels;

namespace GarageService.Services
{
    public class GarageService : IGarageService
    {
        private readonly IGarageRepository _garageRepository;

        public GarageService(IGarageRepository garageRepository) 
        {
            _garageRepository = garageRepository;
        }

        // Digest messages from service bus to create new garage requests
        public async Task DigestMessage(Guid requestCarId, Guid requestUserId)
        {
            var garage = new Request
            {
                CarId = requestCarId,
                UserId = requestUserId,
                Id = Guid.NewGuid().ToString()
            };
            await _garageRepository.AddGarageRequestAsync(garage);
        }

        // Get all garage requests
        public async Task<List<Request>> GetAllGarageRequestsAsync()
        {
            return await _garageRepository.GetAllGarageRequestsAsync();
        }

        public async Task<List<Request>> GetRequestsByCarIdAsync(string carId)
        {
            return await _garageRepository.GetRequestsByCarIdAsync(carId);
        }

        public async Task<List<Request>> GetRequestsByUserIdAsync(string userId)
        {
            return await _garageRepository.GetRequestsByUserIdAsync(userId);
        }
    }
}
