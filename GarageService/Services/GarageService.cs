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

        public async Task<List<Request>> GetAllGarageRequestsAsync()
        {
            return await _garageRepository.GetAllGarageRequestsAsync();
        }
    }
}
