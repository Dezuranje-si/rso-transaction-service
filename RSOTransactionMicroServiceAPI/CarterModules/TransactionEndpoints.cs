using Carter;
using Microsoft.AspNetCore.Http.HttpResults;
using RSOTransactionMicroServiceAPI.Models;
using RSOTransactionMicroServiceAPI.Logic;

namespace RSOTransactionMicroServiceAPI.CarterModules;

public class TransactionEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        //app.Get("/api/transaction", async (req, res) => await res.WriteAsync("Hello from Carter!"));

        var group = app.MapGroup("/transactions/api");

        group.MapGet("/", GetAllTransactions).WithName(nameof(GetAllTransactions)).
            Produces(StatusCodes.Status200OK).
        Produces(StatusCodes.Status400BadRequest).
        Produces(StatusCodes.Status401Unauthorized).WithTags("Transactions");

        group.MapPost("/", CreateTransaction).WithName(nameof(CreateTransaction)).
            Produces(StatusCodes.Status201Created).
            Produces(StatusCodes.Status400BadRequest).
            Produces(StatusCodes.Status401Unauthorized).WithTags("Transactions");

        group.MapGet("user/{id}", GetTransactionsByUserId).WithName(nameof(GetTransactionsByUserId)).
            Produces(StatusCodes.Status201Created).
            Produces(StatusCodes.Status400BadRequest).
            Produces(StatusCodes.Status401Unauthorized).WithTags("Transactions");
    }

    public static async Task<Results<Ok<List<Transaction>>, BadRequest<string>>> GetTransactionsByUserId(ITransactionLogic transactionLogic, int id, Serilog.ILogger logger, HttpContext httpContext)
    {
        var requestId = httpContext?.TraceIdentifier ?? "Unknown";
        logger.Information("transaction-service: GetTransactionsByUserId method called. RequestID: {@requestId}", requestId);

        var ads = await transactionLogic.GetTransactionsByUserIdAsync(id);
        if (ads is null)
        {
            logger.Error("transaction-service: User doesn't have any past transactions.");
            return TypedResults.BadRequest("User doesn't have any past transactions.");
        }

        logger.Information("transaction-service: Exiting method GetTransactionsByUserId");
        return TypedResults.Ok(ads);
    }

    public static async Task<Results<Ok<List<Transaction>>, BadRequest<string>>> GetAllTransactions(ITransactionLogic transactionLogic, Serilog.ILogger logger, HttpContext httpContext)
    {
        var requestId = httpContext?.TraceIdentifier ?? "Unknown";
        logger.Information("transaction-service: GetAllTransactions method called. RequestID: {@requestId}", requestId);

        var transactions = await transactionLogic.GetAllTransactionsAsync();
        if (transactions is null)
        {
            logger.Error("transaction-service: Couldn't find any transactions.");
            return TypedResults.BadRequest("Couldn't find any transactions.");
        }

        logger.Information("transaction-service: Exiting method GetAllTransactions");
        return TypedResults.Ok(transactions);
    }

    public static async Task<Results<Created<Transaction>, BadRequest<string>>> CreateTransaction(ITransactionLogic transactionLogic, Transaction newTransaction, Serilog.ILogger logger, HttpContext httpContext)
    {
        var requestId = httpContext?.TraceIdentifier ?? "Unknown";
        logger.Information("transaction-service: Create method called. RequestID: {@requestId}", requestId);
        try
        {
            var transaction = await transactionLogic.CreateTransactionAsync(newTransaction);
            if (transaction is null)
            {
                return TypedResults.BadRequest("Couldn't create the transaction.");
            }

            logger.Information("transaction-service: Transaction created: {@Transaction}", transaction);
            logger.Information("transaction-service: Exiting method createTransaction");

            return TypedResults.Created("/", transaction);
        }
        catch (Exception ex)
        {
            logger.Error(ex, "transaction-service: Error while creating transaction: {@Transaction}", newTransaction);
            return TypedResults.BadRequest(ex.Message);
        }
    }
}
