using RSOTransactionMicroServiceAPI.Models;

namespace RSOTransactionMicroServiceAPI.Repository
{
    public class TransactionRepository : GenericRepository<Transaction>, ITransactionRepository
    {
        public TransactionRepository(transaction_service_dbContext context) : base(context) { }
    }
}
