using System.Text.Json.Serialization;

namespace PoemTown.Service.BusinessModels.ResponseModels.UserEWalletResponses;

public class DepositUserEWalletResponse
{
    public string PaymentUrl { get; set; }
    public string Message { get; set; }
    public string? Token { get; set; }
    public int? Code { get; set; }
    public bool IsSuccess { get; set; }
    public string orderCode { get; set; }
    
    [JsonIgnore]
    public string? OrderCode { get; set; }
}