using PoemTown.Repository.Enums.Transactions;

namespace PoemTown.Service.BusinessModels.ResponseModels.TransactionResponses;

public class GetTransactionResponse
{
    public Guid Id { get; set; }
    public string? Description { get; set; }
    public TransactionType Type { get; set; }
    public decimal Amount { get; set; }
    public decimal? DiscountAmount { get; set; }
    public DateTimeOffset CreatedTime { get; set; }
    public GetUserInTransactionResponse User { get; set; }
}