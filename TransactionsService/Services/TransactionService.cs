using TransactionsService.DomainModels;
using TransactionsService.ServiceBus;

namespace TransactionsService.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IServiceBusPublisher _serviceBusPublisher;

        // transaction service ctor
        public TransactionService(ITransactionRepository transactionRepository, IServiceBusPublisher serviceBusPublisher) 
        {
            _transactionRepository = transactionRepository;
            _serviceBusPublisher = serviceBusPublisher;
        }

        // digest messages of new transactions from service bus
        public async Task DigestMessage(Guid assignCarUserCarId, Guid assignCarUserUserId)
        {
            var transaction = new Transaction
            {
                CarId = assignCarUserCarId,
                UserId = assignCarUserUserId,
                Id = Guid.NewGuid().ToString(),
                StartDate = DateTime.Now
            };
            await _transactionRepository.AddTransactionAsync(transaction);
            PublishMessage(transaction);
        }

        // delete transactions
        public async Task DeleteTransactionAsync(string transactionId)
        {
            var transaction = await _transactionRepository.GetTransactionByIdAsync(transactionId);
            if (transaction == null)
                throw new Exception("Transaction not founod");

            await _transactionRepository.DeleteTransactionAsync(transactionId);
        }

        // close transactions
        public async Task CloseTransactionAsync(DTO.CloseTransaction closeTransaction)
        {
            //get all transactions for this car
            var existingTransactions = await _transactionRepository.GetTransactionsByCarIdAsync(closeTransaction.CarId.ToString());

            // find the single open transaction for it
            var openTransaction = existingTransactions.FirstOrDefault(t => t.EndDate == null);
            if (openTransaction == null)
                throw new Exception("Transaction not founod");

            openTransaction.EndDate = closeTransaction.EndDate;
            await _transactionRepository.UpdateTransactionAsync(openTransaction);
        }

        // add new trasaction
        public async Task<Transaction> AddTransactionAsync(Transaction transaction)
        {
            var res = await _transactionRepository.AddTransactionAsync(transaction);
            PublishMessage(transaction);
            return res;
        }

        // get all transactions
        public async Task<List<Transaction>> GetTransactionsAsync()
        {
            return await _transactionRepository.GetTransactionsAsync();
        }

        // get all open transactions
        public async Task<List<Transaction>> GetAllOpenTransactionsAsync()
        {
            return await _transactionRepository.GetAllOpenTransactionsAsync();
        }

        // get transactions by user id
        public async Task<List<Transaction>> GetTransactionsByUserIdAsync(string userId)
        {
            return await _transactionRepository.GetTransactionsByUserIdAsync(userId);
        }

        // get transactions by car id
        public async Task<List<Transaction>> GetTransactionsByCarIdAsync(string carId)
        {
            return await _transactionRepository.GetTransactionsByCarIdAsync(carId);
        }

        // get single transaction by id
        public async Task<Transaction> GetTransactionByIdAsync(string transactionId)
        {
            return await _transactionRepository.GetTransactionByIdAsync(transactionId);
        }

        // private method to publish messages to service bus
        private void PublishMessage(Transaction transaction)
        {
            _ = _serviceBusPublisher.PublishTransaction(transaction); // not waiting for publish of message
        }
    }
}
