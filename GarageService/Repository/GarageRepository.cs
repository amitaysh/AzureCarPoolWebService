using Microsoft.Azure.Cosmos;
using GarageService.DomainModels;
using Microsoft.Azure.Cosmos.Linq;

namespace GarageService
{
    public class GarageRepository : IGarageRepository
    {
        private readonly Container _container;

        // GarageRepository ctor
        public GarageRepository(CosmosClient cosmosClient, string databaseName, string containerName)
        {
            _container = cosmosClient.GetContainer(databaseName, containerName);
        }

        // Get all garage requests
        public async Task<List<Request>> GetAllGarageRequestsAsync()
        {
            var query = _container.GetItemQueryIterator<Request>(new QueryDefinition("SELECT * FROM c"));
            var results = new List<Request>();

            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                results.AddRange(response.ToList());
            }

            return results;
        }

        // Add new garage requests
        public async Task<Request> AddGarageRequestAsync(Request garage)
        {
            var obj = await _container.CreateItemAsync(garage, new PartitionKey(garage.Id));
            return obj.Resource;
        }

        // get requests by car id
        public async Task<List<Request>> GetRequestsByCarIdAsync(string carId)
        {
            var query = _container.GetItemLinqQueryable<Request>().Where(t => t.CarId.ToString() == carId).ToFeedIterator();
            var results = new List<Request>();

            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                results.AddRange(response.ToList());
            }

            return results;
        }

        // get requests by user id
        public async Task<List<Request>> GetRequestsByUserIdAsync(string userId)
        {
            var query = _container.GetItemLinqQueryable<Request>().Where(t => t.UserId.ToString() == userId).ToFeedIterator();
            var results = new List<Request>();

            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                results.AddRange(response.ToList());
            }

            return results;
        }
    }
}
