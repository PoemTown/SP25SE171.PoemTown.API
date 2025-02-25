using PoemTown.Service.BusinessModels.RequestModels.UserEWalletRequests;
using PoemTown.Service.BusinessModels.ResponseModels.UserEWalletResponses;

namespace PoemTown.Service.Interfaces;

public interface IPaymentMethod
{
    Task<DepositUserEWalletResponse> DepositUserEWalletPayment(UserEWalletData request);
}