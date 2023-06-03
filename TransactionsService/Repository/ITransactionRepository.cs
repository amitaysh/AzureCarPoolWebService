using TransactionsService.DomainModels;

namespace TransactionsService
{
    public interface ITransactionRepository
    {
        Task<List<Transaction>> GetTransactionsAsync();
        Task<List<Transaction>> GetAllOpenTransactionsAsync();
        Task<Transaction> GetTransactionByIdAsync(string transactionId);
        Task<List<Transaction>> GetTransactionsByCarIdAsync(string carId);
        Task<List<Transaction>> GetTransactionsByUserIdAsync(string userId);
        Task<Transaction> AddTransactionAsync(Transaction transaction);
        Task UpdateTransactionAsync(Transaction transaction);
        Task DeleteTransactionAsync(string transactionId);
    }
}