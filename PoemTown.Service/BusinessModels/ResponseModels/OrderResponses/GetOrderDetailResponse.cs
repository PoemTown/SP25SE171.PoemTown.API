using PoemTown.Repository.Enums.Orders;

namespace PoemTown.Service.BusinessModels.ResponseModels.OrderResponses;

public class GetOrderDetailResponse
{
    public Guid Id { get; set; }
    /*public string OrderCode { get; set; }
    public OrderType Type { get; set; }
    public decimal Amount { get; set; }
    public string OrderDescription { get; set; }
    public OrderStatus Status { get; set; }*/
    public decimal ItemPrice { get; set; }
    public int ItemQuantity { get; set; }
    public GetSaleVersionInOrderDetailResponse? SaleVersion { get; set; }
    public GetRecordFileInOrderDetailResponse? RecordFile { get; set; }
    public GetMasterTemplateInOrderDetailResponse? MasterTemplate { get; set; }
}