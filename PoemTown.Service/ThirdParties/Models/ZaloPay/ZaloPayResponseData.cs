namespace PoemTown.Service.ThirdParties.Models.ZaloPay;

public class ZaloPayResponseData
{  
    public string? OrderCode { get; set; }
    public int? ReturnCode { get; set; }
    public string? ReturnMessage { get; set; }
    public int? SubReturnCode { get; set; }
    public string? SubReturnMessage { get; set; }
    public object OrderUrl { get; set; }
    public string? ZpTransToken { get; set; }
    public string? OrderToken { get; set; }
    public string? QrCode { get; set; }
}