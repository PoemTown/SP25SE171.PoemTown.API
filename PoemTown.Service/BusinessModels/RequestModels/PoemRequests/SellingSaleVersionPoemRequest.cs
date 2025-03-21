namespace PoemTown.Service.BusinessModels.RequestModels.PoemRequests;

public class SellingSaleVersionPoemRequest
{
    public Guid PoemId { get; set; }
    public required int CommissionPercentage { get; set; }
    public required int DurationTime { get; set; }
    public required decimal Price { get; set; }
}