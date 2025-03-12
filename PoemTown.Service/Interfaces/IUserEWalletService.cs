using PoemTown.Service.BusinessModels.RequestModels.UserEWalletRequests;
using PoemTown.Service.BusinessModels.ResponseModels.UserEWalletResponses;

namespace PoemTown.Service.Interfaces;

public interface IUserEWalletService
{
    Task<DepositUserEWalletResponse> DepositUserEWalletAsync(Guid userId, DepositUserEWalletRequest request);
    Task DonateUserEWalletAsync(Guid userId, DonateUserEWalletRequest request);
}