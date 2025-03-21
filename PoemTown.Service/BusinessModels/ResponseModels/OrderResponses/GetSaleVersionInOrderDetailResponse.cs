namespace PoemTown.Service.BusinessModels.ResponseModels.OrderResponses;

public class GetSaleVersionInOrderDetailResponse
{
    public Guid Id { get; set; }
    public int CommissionPercentage { get; set; }
    public int Price { get; set; }
}