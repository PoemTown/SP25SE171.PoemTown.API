using System.ComponentModel.DataAnnotations;

namespace PoemTown.Service.BusinessModels.RequestModels.DepositCommissionSettingRequests;

public class UpdateDepositCommissionSettingRequest
{
    public Guid Id { get; set; }
    [Required(ErrorMessage = "Amount percentage is required.")]
    [Range(0, 100, ErrorMessage = "AmountPercentage must be between 0 and 100.")]
    public required int AmountPercentage { get; set; }
    public bool? IsInUse { get; set; } = false;
}