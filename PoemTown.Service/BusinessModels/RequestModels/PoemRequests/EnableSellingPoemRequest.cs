namespace PoemTown.Service.BusinessModels.RequestModels.PoemRequests;

public class EnableSellingPoemRequest
{
    public Guid PoemId { get; set; }
    public decimal Price { get; set; }
}