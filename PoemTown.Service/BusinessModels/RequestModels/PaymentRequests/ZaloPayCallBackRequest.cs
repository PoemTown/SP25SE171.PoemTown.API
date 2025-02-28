using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace PoemTown.Service.BusinessModels.RequestModels.PaymentRequests;

public class ZaloPayCallBackRequest
{
    [FromQuery(Name = "amount")] 
    public int? Amount { get; set; }
    
    [FromQuery(Name = "appid")]
    public int? AppId { get; set; }
    
    [FromQuery(Name = "apptransid")]
    public string? ApptransId { get; set; }
    
    [FromQuery(Name = "bankcode")]
    public string? BankCode { get; set; }
    
    [FromQuery(Name = "checksum")]
    public string? Checksum { get; set; }
    
    [FromQuery(Name = "discountamount")]
    public int? DiscountAmount { get; set; }
    
    [FromQuery(Name = "pmcid")]
    public int? PmcId { get; set; }
    
    [FromQuery(Name = "status")]
    public int? Status { get; set; }
}