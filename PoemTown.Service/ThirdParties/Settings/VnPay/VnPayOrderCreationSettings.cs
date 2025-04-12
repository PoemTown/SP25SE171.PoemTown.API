namespace PoemTown.Service.ThirdParties.Settings.VnPay;

public class VnPayOrderCreationSettings
{
    public string TransactionCode { get; set; }
    public string TransactionDescription { get; set; }
    public decimal Amount { get; set; }
    public string UserIpAddress { get; set; }
}