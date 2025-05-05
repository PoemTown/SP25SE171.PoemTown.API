using System.ComponentModel.DataAnnotations;
using PoemTown.Repository.Enums.BankTypes;

namespace PoemTown.Service.BusinessModels.RequestModels.BankTypeRequests;

public class UpdateBankTypeRequest
{
    public Guid Id { get; set; }
    [Required(ErrorMessage = "Tên ngân hàng không được để trống")]
    public required string BankName { get; set; }
    
    [Required(ErrorMessage = "Mã ngân hàng không được để trống")]
    public required string BankCode { get; set; }
    
    [Required(ErrorMessage = "Icon ngân hàng không được để trống")]
    public required string ImageIcon { get; set; }

    public BankTypeStatus? Status { get; set; } = BankTypeStatus.Inactive;
}