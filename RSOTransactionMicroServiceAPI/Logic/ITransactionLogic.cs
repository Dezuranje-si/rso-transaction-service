using RSOTransactionMicroServiceAPI.Models;

namespace RSOTransactionMicroServiceAPI.Logic;


public interface ITransactionLogic
{
    public Task<List<Transaction>> GetAllTransactionsAsync();
    public Task<Transaction> CreateTransactionAsync(Transaction newTransaction);
    public Task<List<Transaction>> GetTransactionsByUserIdAsync(int userId);
}