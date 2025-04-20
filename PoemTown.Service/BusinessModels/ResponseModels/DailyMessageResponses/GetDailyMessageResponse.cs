namespace PoemTown.Service.BusinessModels.ResponseModels.DailyMessageResponses;

public class GetDailyMessageResponse
{
    public Guid Id { get; set; }
    public string Message { get; set; } = String.Empty;
    public bool IsInUse { get; set; } = false;
}