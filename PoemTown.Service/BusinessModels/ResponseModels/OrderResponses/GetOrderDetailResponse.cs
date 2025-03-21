namespace PoemTown.Service.BusinessModels.ResponseModels.OrderResponses;

public class GetOrderDetailResponse
{
    public Guid Id { get; set; }
    public decimal ItemPrice { get; set; }
    public int ItemQuantity { get; set; }
    public GetSaleVersionInOrderDetailResponse? SaleVersion { get; set; }
    public GetPoemInOrderDetailResponse? Poem { get; set; }
    public GetRecordFileInOrderDetailResponse? RecordFile { get; set; }
    public GetMasterTemplateInOrderDetailResponse? MasterTemplate { get; set; }
}