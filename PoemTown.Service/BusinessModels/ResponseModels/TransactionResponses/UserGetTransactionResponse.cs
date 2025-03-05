using PoemTown.Repository.Enums.Transactions;

namespace PoemTown.Service.BusinessModels.ResponseModels.TransactionResponses;

public class UserGetTransactionResponse
{
    public Guid Id { get; set; }
    public string? Description { get; set; }
    public TransactionType Type { get; set; }
    public decimal Balance { get; set; }
    public decimal Amount { get; set; }
    public decimal? DiscountAmount { get; set; }
}