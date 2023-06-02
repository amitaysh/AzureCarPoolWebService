using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using TransactionsService.DomainModels;

namespace TransactionsService
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly Container _container;

        public TransactionRepository(CosmosClient cosmosClient, string databaseName, string containerName)
        {
            _container = cosmosClient.GetContainer(databaseName, containerName);
        }

        public async Task<List<Transaction>> GetTransactionsAsync()
        {
            var query = _container.GetItemQueryIterator<Transaction>(new QueryDefinition("SELECT * FROM c"));
            var results = new List<Transaction>();

            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                results.AddRange(response.ToList());
            }

            return results;
        }

        public async Task<Transaction> GetTransactionByIdAsync(string transactionId)
        {
            var response = await _container.ReadItemAsync<Transaction>(transactionId, new PartitionKey(transactionId));
            return response.Resource;
        }

        public async Task<Transaction> AddTransactionAsync(Transaction transaction)
        {
            var obj = await _container.CreateItemAsync(transaction, new PartitionKey(transaction.Id));
            return obj.Resource;
        }

        public async Task UpdateTransactionAsync(Transaction transaction)
        {
            await _container.UpsertItemAsync(transaction, new PartitionKey(transaction.Id));
        }

        public async Task DeleteTransactionAsync(string transactionId)
        {
            await _container.DeleteItemAsync<Transaction>(transactionId, new PartitionKey(transactionId));
        }

        public async Task<Transaction> GetTransactionByCarIdAsync(string carId)
        {
            var transaction = (await _container.GetItemLinqQueryable<Transaction>().Where(t => t.CarId.ToString() == carId).Take(1).ToFeedIterator().ReadNextAsync()).FirstOrDefault();

            return transaction;
        }
    }
}
