using GarageService.DomainModels;

namespace GarageService.Services
{
    public interface IGarageService
    {
        Task DigestMessage(Guid requestCarId, Guid requestUserId);
        Task<List<Request>> GetAllGarageRequestsAsync();
        Task<List<Request>> GetRequestsByCarIdAsync(string carId);
        Task<List<Request>> GetRequestsByUserIdAsync(string userId);
    }
}
