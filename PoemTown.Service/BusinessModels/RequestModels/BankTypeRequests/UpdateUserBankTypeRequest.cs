using System.ComponentModel.DataAnnotations;

namespace PoemTown.Service.BusinessModels.RequestModels.BankTypeRequests;

public class UpdateUserBankTypeRequest
{
    public Guid Id { get; set; }
    [Required(ErrorMessage = "Tên chủ tài khoản không được để trống")]
    public required string AccountName { get; set; }
    [Required(ErrorMessage = "Số tài khoản không được để trống")]
    [RegularExpression("^[0-9]+$", ErrorMessage = "Số tài khoản chỉ được chứa chữ số")]
    public required string AccountNumber { get; set; }
    public required Guid BankTypeId { get; set; }
}