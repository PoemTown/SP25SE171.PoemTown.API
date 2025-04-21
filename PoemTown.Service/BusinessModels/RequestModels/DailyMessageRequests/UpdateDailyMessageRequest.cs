namespace PoemTown.Service.BusinessModels.RequestModels.DailyMessageRequests;

public class UpdateDailyMessageRequest
{
    public Guid Id { get; set; }
    public string Message { get; set; } = String.Empty;
    public bool? IsInUse { get; set; } = false;
}