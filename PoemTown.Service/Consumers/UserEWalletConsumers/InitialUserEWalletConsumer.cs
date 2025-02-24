using MassTransit;
using PoemTown.Repository.Entities;
using PoemTown.Repository.Enums.Wallets;
using PoemTown.Repository.Interfaces;
using PoemTown.Service.Events.UserEWalletEvents;

namespace PoemTown.Service.Consumers.UserEWalletConsumers;

public class InitialUserEWalletConsumer : IConsumer<InitialUserEWalletEvent>
{
    private readonly IUnitOfWork _unitOfWork;
    public InitialUserEWalletConsumer(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task Consume(ConsumeContext<InitialUserEWalletEvent> context)
    {
        var message = context.Message;
        
        User? user = await _unitOfWork.GetRepository<User>().FindAsync(p => p.Id == message.UserId);
        if (user == null)
        {
            throw new Exception("User not found");
        }
        
        UserEWallet? userEWallet = await _unitOfWork.GetRepository<UserEWallet>().FindAsync(p => p.UserId == message.UserId);
        if (userEWallet != null)
        {
            return;
        }
        
        UserEWallet newUserEWallet = new UserEWallet()
        {
            UserId = message.UserId,
            WalletBalance = 0,
            WalletStatus = WalletStatus.Active
        };
        
        await _unitOfWork.GetRepository<UserEWallet>().InsertAsync(newUserEWallet);
        await _unitOfWork.SaveChangesAsync();
    }
}