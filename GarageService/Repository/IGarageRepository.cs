using GarageService.DomainModels;

namespace GarageService
{
    public interface IGarageRepository
    {
        Task<List<Request>> GetAllGarageRequestsAsync();
        Task<Request> AddGarageRequestAsync(Request request);
        Task<List<Request>> GetRequestsByCarIdAsync(string carId);
        Task<List<Request>> GetRequestsByUserIdAsync(string userId);
    }
}