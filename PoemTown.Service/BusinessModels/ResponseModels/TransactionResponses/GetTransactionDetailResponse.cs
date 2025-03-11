using PoemTown.Repository.Enums.Transactions;

namespace PoemTown.Service.BusinessModels.ResponseModels.TransactionResponses;

public class GetTransactionDetailResponse
{
    public Guid Id { get; set; }
    public string? Description { get; set; }
    public TransactionType Type { get; set; }
    public decimal Amount { get; set; }
    public decimal? DiscountAmount { get; set; }
    public DateTimeOffset CreatedTime { get; set; }
    public decimal Balance { get; set; }
    public string? Token { get; set; }
    public string? AppId { get; set; }
    public string? BankCode { get; set; }
    public string? Checksum { get; set; }
    public GetUserInTransactionResponse User { get; set; }
    public GetUserInTransactionResponse? ReceiveUser { get; set; }
}