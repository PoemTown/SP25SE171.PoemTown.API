using PoemTown.Service.BusinessModels.ResponseModels.BankTypeResponses;

namespace PoemTown.Service.BusinessModels.RequestModels.BankTypeRequests;

public class GetUserBankTypeResponse
{
    public Guid Id { get; set; }
    public string? AccountNumber { get; set; }
    public string? AccountName { get; set; }
    public GetBankTypeResponse? BankType { get; set; }    
}