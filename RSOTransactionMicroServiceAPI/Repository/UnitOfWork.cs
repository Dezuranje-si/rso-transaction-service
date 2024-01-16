using RSOTransactionMicroServiceAPI.Models;


namespace RSOTransactionMicroServiceAPI.Repository;

/// <summary>
/// Implements the <see cref="IUnitOfWork"/> interface.
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly transaction_service_dbContext _transaction_service_dbContext;
    private bool disposed;

    /// <summary>
    /// Constructor for the <see cref="UnitOfWork"/> class.
    /// </summary>
    /// <param name="transaction_service_dbContext ">The <see cref="transaction_service_dbContext "/> context for the database access.</param>
    /// <param name="transactionRepository">transactionRepository instance.</param>
    public UnitOfWork(transaction_service_dbContext transaction_service_dbContext, ITransactionRepository transactionRepository)
    {
        _transaction_service_dbContext = transaction_service_dbContext;
        TransactionRepository = transactionRepository;
    }

    ///<inheritdoc/>
    public ITransactionRepository TransactionRepository { get; }

    ///<inheritdoc/>
    public async Task<int> SaveChangesAsync() => await _transaction_service_dbContext.SaveChangesAsync();

    /// <summary>
    /// Implements the <see cref="IDisposable"/> interface. Called when we'd like to the dispose the <see cref="UnitOfWork"/> object.
    /// </summary>
    /// <param name="disposing">The </param>
    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                _transaction_service_dbContext.Dispose();
            }
        }
        disposed = true;
    }

    /// <summary>
    /// Disposes the <see cref="UnitOfWork"/> object and acts as a wrapper for <see cref="Dispose(bool)"/> method.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
