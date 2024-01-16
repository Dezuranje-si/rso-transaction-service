using RSOTransactionMicroServiceAPI.Models;
using RSOTransactionMicroServiceAPI.Repository;
using System.Linq.Expressions;

namespace RSOTransactionMicroServiceAPI.Logic;

public class TransactionLogic : ITransactionLogic
{
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes the <see cref="UserLogic"/> class.
    /// </summary>
    /// <param name="unitOfWork"><see cref="IUnitOfWork"/> instance.</param>
    public TransactionLogic(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Transaction> CreateTransactionAsync(Transaction newTransaction)
    {
        try
        {
            var existingTransactions = await _unitOfWork.TransactionRepository.GetAllAsync();
            // if empty
            if (existingTransactions.Count == 0)
            {
                newTransaction.ID = 1;
            }
            else
            {
                newTransaction.ID = existingTransactions.Max(ea => ea.ID) + 1;
            }
            newTransaction.DateTime = DateTime.UtcNow;
            
            var transaction = await _unitOfWork.TransactionRepository.InsertAsync(newTransaction);
            await _unitOfWork.SaveChangesAsync();
            return transaction;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public async Task<List<Transaction>> GetTransactionsByUserIdAsync(int userId)
    {
        try
        {
            Expression<Func<Transaction, bool>> filter = transaction => transaction.BuyerId == userId || transaction.SellerId == userId;

            return await _unitOfWork.TransactionRepository.GetManyAsync(filter);
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public async Task<List<Transaction>> GetAllTransactionsAsync()
    {
        try
        {
            return await _unitOfWork.TransactionRepository.GetAllAsync();
        }
        catch (Exception ex)
        {
            return null;
        }
    }
}