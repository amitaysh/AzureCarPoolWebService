using Microsoft.Azure.Cosmos;
using GarageService.DomainModels;

namespace GarageService
{
    public class GarageRepository : IGarageRepository
    {
        private readonly Container _container;

        public GarageRepository(CosmosClient cosmosClient, string databaseName, string containerName)
        {
            _container = cosmosClient.GetContainer(databaseName, containerName);
        }

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

        public async Task<Request> AddGarageRequestAsync(Request garage)
        {
            var obj = await _container.CreateItemAsync(garage, new PartitionKey(garage.Id));
            return obj.Resource;
        }
    }
}
