namespace PoemTown.Service.BusinessModels.ResponseModels.OrderResponses;

public class AdminGetOrderResponse : GetOrderResponse
{
    public GetUserInOrderResponse User { get; set; }
}