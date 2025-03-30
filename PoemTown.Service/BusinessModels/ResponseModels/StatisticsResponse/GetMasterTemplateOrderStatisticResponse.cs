namespace PoemTown.Service.BusinessModels.ResponseModels.StatisticsResponse;

public class GetMasterTemplateOrderStatisticResponse
{
    public int TotalDataSamples { get; set; }
    public IList<GetMasterTemplateOrderSampleResponse>? Samples { get; set; }
}

public class GetMasterTemplateOrderSampleResponse
{
    public string TemplateName { get; set; }
    public string TagName { get; set; }
    public int TotalOrders { get; set; }
}