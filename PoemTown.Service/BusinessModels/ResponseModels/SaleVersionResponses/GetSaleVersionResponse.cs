using PoemTown.Repository.Enums.SaleVersions;

namespace PoemTown.Service.BusinessModels.ResponseModels.SaleVersionResponses;

public class GetSaleVersionResponse
{
    public Guid Id { get; set; }
    public int CommissionPercentage { get; set; }
    public decimal Price { get; set; }
    public int DurationTime { get; set; }
    public SaleVersionStatus Status { get; set; }
    public bool IsInUse { get; set; }
}