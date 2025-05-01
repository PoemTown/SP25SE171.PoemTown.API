using Microsoft.AspNetCore.Mvc;
using PoemTown.Repository.Enums.WithdrawalComplaints;

namespace PoemTown.Service.QueryOptions.FilterOptions.WithdrawalComplaintFilters;

public class GetWithdrawalComplaintFilterOption
{
    [FromQuery(Name = "withdrawalFormId")]
    public Guid? WithdrawalFormId { get; set; }
    [FromQuery(Name = "status")]
    public WithdrawalComplaintStatus? Status { get; set; }
}