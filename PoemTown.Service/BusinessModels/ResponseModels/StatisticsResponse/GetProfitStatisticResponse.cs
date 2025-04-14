using PoemTown.Repository.Enums.Transactions;

namespace PoemTown.Service.BusinessModels.ResponseModels.StatisticsResponse;

public class GetProfitStatisticResponse
{
    public IList<GetProfitTypeStatisticResponse> ProfitTypeStatisticResponses { get; set; }
}

public class GetProfitTypeStatisticResponse
{
    public TransactionProfitType ProfitType { get; set; }
    public GetStatisticResponse<int, decimal> Samples { get; set; }
}