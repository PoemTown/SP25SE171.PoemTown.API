using PoemTown.Repository.Enums.Transactions;

namespace PoemTown.Service.BusinessModels.ResponseModels.TransactionResponses;

public class GetTransactionResponse
{
    public Guid Id { get; set; }
    public string? Description { get; set; }
    public TransactionType Type { get; set; }
    public decimal Amount { get; set; }
    public decimal? DiscountAmount { get; set; }
    public decimal? Balance { get; set; } = default;
    public DateTimeOffset CreatedTime { get; set; }
    public string? TransactionCode { get; set; }
    public string? Status { get; set; }
    public string? BankCode { get; set; }
    public DateTimeOffset? PaidDate { get; set; }
    public DateTimeOffset? CancelledDate { get; set; }
    public bool? IsAddToWallet { get; set; }
    public string? Token { get; set; }
    public string? AppId { get; set; }
    public string? Checksum { get; set; }
    public string? TransactionToken { get; set; }
    public GetUserInTransactionResponse User { get; set; }
}