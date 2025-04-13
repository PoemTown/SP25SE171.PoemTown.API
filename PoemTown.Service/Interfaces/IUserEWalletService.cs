using PoemTown.Service.BusinessModels.RequestModels.UserEWalletRequests;
using PoemTown.Service.BusinessModels.RequestModels.WithdrawalFormRequests;
using PoemTown.Service.BusinessModels.ResponseModels.UserEWalletResponses;

namespace PoemTown.Service.Interfaces;

public interface IUserEWalletService
{
    Task<DepositUserEWalletResponse> DepositUserEWalletAsync(Guid userId, DepositUserEWalletRequest request);
    Task DonateUserEWalletAsync(Guid userId, DonateUserEWalletRequest request);
    Task<GetUserEWalletResponse> GetUserEWalletAsync(Guid userId);
    Task CreateWithdrawalForm(Guid userId, Guid userEWalletId, CreateWithdrawalFormRequest request);
}