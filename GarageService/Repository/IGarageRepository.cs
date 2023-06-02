using GarageService.DomainModels;

namespace GarageService
{
    public interface IGarageRepository
    {
        Task<List<Request>> GetAllGarageRequestsAsync();
        Task<Request> AddGarageRequestAsync(Request request);
    }
}