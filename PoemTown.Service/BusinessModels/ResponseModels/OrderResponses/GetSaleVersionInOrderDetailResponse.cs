using PoemTown.Service.BusinessModels.ResponseModels.SaleVersionResponses;

namespace PoemTown.Service.BusinessModels.ResponseModels.OrderResponses;

public class GetSaleVersionInOrderDetailResponse : GetSaleVersionResponse
{
    public GetPoemInOrderDetailResponse? Poem { get; set; }
}