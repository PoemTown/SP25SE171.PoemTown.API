using PoemTown.Repository.Enums.Transactions;
using PoemTown.Service.BusinessModels.ResponseModels.OrderResponses;
using PoemTown.Service.BusinessModels.ResponseModels.PaymentGatewayResponses;
using PoemTown.Service.BusinessModels.ResponseModels.WithdrawalFormResponses;

namespace PoemTown.Service.BusinessModels.ResponseModels.TransactionResponses;

public class UserGetTransactionDetailResponse
{
    public Guid Id { get; set; }
    public string? Description { get; set; }
    public TransactionType Type { get; set; }
    public decimal Amount { get; set; }
    public decimal? DiscountAmount { get; set; }
    public DateTimeOffset CreatedTime { get; set; }
    public decimal Balance { get; set; }
    public string? TransactionCode { get; set; }
    public TransactionStatus? Status { get; set; }
    public DateTimeOffset? PaidDate { get; set; }
    public DateTimeOffset? CancelledDate { get; set; }
    public bool? IsAddToWallet { get; set; }
    public GetUserInTransactionResponse User { get; set; }
    public GetUserInTransactionResponse? ReceiveUser { get; set; }
    public GetPaymentGatewayResponse? PaymentGateway { get; set; }
    public GetOrderWithOrderDetailResponse? Order { get; set; }
    public GetWithdrawalFormResponse? WithdrawalForm { get; set; }
}