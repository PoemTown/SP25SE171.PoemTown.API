using PoemTown.Repository.Enums.Wallets;

namespace PoemTown.Service.BusinessModels.ResponseModels.UserEWalletResponses;

public class GetUserEWalletResponse
{
    public Guid Id { get; set; }
    public decimal WalletBalance { get; set; }
    public WalletStatus WalletStatus { get; set; }
}