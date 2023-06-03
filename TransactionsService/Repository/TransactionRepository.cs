using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using TransactionsService.DomainModels;

namespace TransactionsService
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly Container _container;

        // transaction repository ctor
        public TransactionRepository(CosmosClient cosmosClient, string databaseName, string containerName)
        {
            _container = cosmosClient.GetContainer(databaseName, containerName);
        }

        // get all transactions
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

        // get all open transactions
        public async Task<List<Transaction>> GetAllOpenTransactionsAsync()
        {
            var query = _container.GetItemQueryIterator<Transaction>(new QueryDefinition("SELECT * FROM c where is_null(c.endDate)"));
            var results = new List<Transaction>();

            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                results.AddRange(response.ToList());
            }

            return results;
        }

        // get single transaction by id
        public async Task<Transaction> GetTransactionByIdAsync(string transactionId)
        {
            var response = await _container.ReadItemAsync<Transaction>(transactionId, new PartitionKey(transactionId));
            return response.Resource;
        }

        // add new transaction
        public async Task<Transaction> AddTransactionAsync(Transaction transaction)
        {
            var obj = await _container.CreateItemAsync(transaction, new PartitionKey(transaction.Id));
            return obj.Resource;
        }

        // update transaction (closing it for now, maybe in the future more properties)
        public async Task UpdateTransactionAsync(Transaction transaction)
        {
            await _container.UpsertItemAsync(transaction, new PartitionKey(transaction.Id));
        }

        // delete transaction
        public async Task DeleteTransactionAsync(string transactionId)
        {
            await _container.DeleteItemAsync<Transaction>(transactionId, new PartitionKey(transactionId));
        }

        // get transactions by car id
        public async Task<List<Transaction>> GetTransactionsByCarIdAsync(string carId)
        {
            var query = _container.GetItemLinqQueryable<Transaction>().Where(t => t.CarId.ToString() == carId).ToFeedIterator();
            var results = new List<Transaction>();

            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                results.AddRange(response.ToList());
            }

            return results;
        }

        // get transactions by user id
        public async Task<List<Transaction>> GetTransactionsByUserIdAsync(string userId)
        {
            var query = _container.GetItemLinqQueryable<Transaction>().Where(t => t.UserId.ToString() == userId).ToFeedIterator();
            var results = new List<Transaction>();

            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                results.AddRange(response.ToList());
            }

            return results;
        }
    }
}
