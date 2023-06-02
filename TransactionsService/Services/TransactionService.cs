using TransactionsService.DomainModels;
using TransactionsService.ServiceBus;

namespace TransactionsService.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IServiceBusPublisher _serviceBusPublisher;

        public TransactionService(ITransactionRepository transactionRepository, IServiceBusPublisher serviceBusPublisher) 
        {
            _transactionRepository = transactionRepository;
            _serviceBusPublisher = serviceBusPublisher;
        }

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

        public async Task DeleteTransactionAsync(string transactionId)
        {
            var transaction = await _transactionRepository.GetTransactionByIdAsync(transactionId);
            if (transaction == null)
                throw new Exception("Transaction not founod");

            await _transactionRepository.DeleteTransactionAsync(transactionId);
        }

        public async Task CloseTransactionAsync(DTO.CloseTransaction closeTransaction)
        {
            var existingTransaction = await _transactionRepository.GetTransactionByCarIdAsync(closeTransaction.CarId.ToString());
            if (existingTransaction == null)
                throw new Exception("Transaction not founod");

            existingTransaction.EndDate = closeTransaction.EndDate;

            await _transactionRepository.UpdateTransactionAsync(existingTransaction);
        }

        public async Task<Transaction> AddTransactionAsync(Transaction transaction)
        {
            var res = await _transactionRepository.AddTransactionAsync(transaction);
            PublishMessage(transaction);
            return res;
        }

        public async Task<List<Transaction>> GetTransactionsAsync()
        {
            return await _transactionRepository.GetTransactionsAsync();
        }

        public async Task<Transaction> GetTransactionByIdAsync(string transactionId)
        {
            return await _transactionRepository.GetTransactionByIdAsync(transactionId);
        }

        private void PublishMessage(Transaction transaction)
        {
            _ = _serviceBusPublisher.PublishTransaction(transaction); // not waiting for publish of message
        }
    }
}
