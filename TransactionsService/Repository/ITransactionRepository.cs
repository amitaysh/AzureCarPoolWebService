using TransactionsService.DomainModels;

namespace TransactionsService
{
    public interface ITransactionRepository
    {
        Task<List<Transaction>> GetTransactionsAsync();
        Task<Transaction> GetTransactionByIdAsync(string transactionId);
        Task<Transaction> GetTransactionByCarIdAsync(string carId);
        Task<Transaction> AddTransactionAsync(Transaction transaction);
        Task UpdateTransactionAsync(Transaction transaction);
        Task DeleteTransactionAsync(string transactionId);
    }
}