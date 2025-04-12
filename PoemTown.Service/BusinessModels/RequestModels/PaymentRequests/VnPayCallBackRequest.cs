using Microsoft.AspNetCore.Mvc;

namespace PoemTown.Service.BusinessModels.RequestModels.PaymentRequests;

public class VnPayCallBackRequest
{
    [FromQuery(Name = "vnp_Amount")]
    public long? VnpAmount { get; set; }
    
    [FromQuery(Name = "vnp_BankCode")]
    public string? VnpBankCode { get; set; }
    
    [FromQuery(Name = "vnp_BankTranNo")]
    public string? VnpBankTranNo { get; set; }
    
    [FromQuery(Name = "vnp_CardType")]
    public string? VnpCardType { get; set; }
    
    [FromQuery(Name = "vnp_OrderInfo")]
    public string? VnpOrderInfo { get; set; }
    
    [FromQuery(Name = "vnp_PayDate")]
    public string? VnpPayDate { get; set; }
    
    [FromQuery(Name = "vnp_ResponseCode")]
    public string? VnpResponseCode { get; set; }
    
    [FromQuery(Name = "vnp_TransactionNo")]
    public string? VnpTransactionNo { get; set; }
    
    [FromQuery(Name = "vnp_TransactionStatus")]
    public int? VnpTransactionStatus { get; set; }
    
    [FromQuery(Name = "vnp_TmnCode")]
    public string? VnpTmnCode { get; set; }
    
    [FromQuery(Name = "vnp_TxnRef")]
    public string? VnpTxnRef { get; set; }
    
    [FromQuery(Name = "vnp_SecureHash")]
    public string? VnpSecureHash { get; set; }
}