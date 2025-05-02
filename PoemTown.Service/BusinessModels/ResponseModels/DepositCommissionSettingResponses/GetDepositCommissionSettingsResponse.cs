namespace PoemTown.Service.BusinessModels.ResponseModels.DepositCommissionSettingResponses;

public class GetDepositCommissionSettingsResponse
{
    public Guid Id { get; set; }
    public int AmountPercentage { get; set; }
    public bool? IsInUse { get; set; }
}