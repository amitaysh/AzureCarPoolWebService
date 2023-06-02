using Microsoft.AspNetCore.Mvc;
using TransactionsService.DomainModels;

namespace TransactionsService.Services
{
    public interface ITransactionService
    {
        Task DigestMessage(Guid assignCarUserCarId, Guid assignCarUserUserId);
        Task DeleteTransactionAsync(string transactionId);
        Task CloseTransactionAsync(DTO.CloseTransaction closeTransaction);
        Task<Transaction> AddTransactionAsync(Transaction transaction);
        Task<List<Transaction>> GetTransactionsAsync();
        Task<Transaction> GetTransactionByIdAsync(string transactionId);
    }
}
