using System.Collections;
using PoemTown.Repository.Enums.Transactions;

namespace PoemTown.Service.BusinessModels.ResponseModels.StatisticsResponse;

public class GetIncomeStatisticResponse
{
    public IList<GetIncomeTypeStatisticResponse> IncomeTypeStatistics { get; set; }
}

public class GetIncomeTypeStatisticResponse
{
    public TransactionIncomeType IncomeType { get; set; }
    public GetStatisticResponse<int, decimal> Samples { get; set; }
}