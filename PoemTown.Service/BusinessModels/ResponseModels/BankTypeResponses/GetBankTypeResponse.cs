using PoemTown.Repository.Enums.BankTypes;

namespace PoemTown.Service.BusinessModels.ResponseModels.BankTypeResponses;

public class GetBankTypeResponse
{
    public Guid Id { get; set; }
    public string? BankName { get; set; }
    public string? BankCode { get; set; }
    public string? ImageIcon { get; set; }
    public BankTypeStatus Status { get; set; }
}