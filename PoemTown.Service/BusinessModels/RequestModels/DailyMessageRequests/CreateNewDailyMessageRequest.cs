namespace PoemTown.Service.BusinessModels.RequestModels.DailyMessageRequests;

public class CreateNewDailyMessageRequest
{
    public string Message { get; set; } = String.Empty;
    public bool? IsInUse { get; set; } = false;
}