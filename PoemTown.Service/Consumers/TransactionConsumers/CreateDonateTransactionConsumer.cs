using AutoMapper;
using MassTransit;
using PoemTown.Repository.Entities;
using PoemTown.Repository.Enums.Transactions;
using PoemTown.Repository.Interfaces;
using PoemTown.Service.Events.TransactionEvents;

namespace PoemTown.Service.Consumers.TransactionConsumers;

public class CreateDonateTransactionConsumer : IConsumer<CreateDonateTransactionEvent>
{
    private readonly IUnitOfWork _unitOfWork;
    
    public CreateDonateTransactionConsumer(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task Consume(ConsumeContext<CreateDonateTransactionEvent> context)
    {
        var message = context.Message;
        
        var userEWallet = await _unitOfWork.GetRepository<UserEWallet>().FindAsync(p => p.Id == message.UserEWalletId);
        // Check if user e-wallet is null
        if (userEWallet == null)
        {
            throw new Exception("User e-wallet not found");
        }

        var receiveUserEWallet = await _unitOfWork.GetRepository<UserEWallet>().FindAsync(p => p.Id == message.ReceiveUserEWalletId);
        // Check if receive user e-wallet is null
        if (receiveUserEWallet == null)
        {
            throw new Exception("Receive user e-wallet not found");
        }
        
        // Create transaction lists
        List<Transaction> transactions =
        [
            // User Transaction
            new Transaction()
            {
                Amount = message.Amount,
                Description = $"Quyên tặng {message.Amount}VND tới người dùng: " + receiveUserEWallet.User.FullName,
                ReceiveUserEWallet = receiveUserEWallet,
                UserEWallet = userEWallet,
                Type = TransactionType.Donate,
                Balance = userEWallet.WalletBalance,
            },
            
            // Receive User Transaction
            new Transaction()
            {
                Amount = message.Amount,
                Description = $"Nhận {message.Amount}VND từ người dùng: " + userEWallet.User.FullName,
                UserEWallet = receiveUserEWallet,
                Type = TransactionType.Donate,
                Balance = receiveUserEWallet.WalletBalance
            }
        ];

        await _unitOfWork.GetRepository<Transaction>().InsertRangeAsync(transactions);
        await _unitOfWork.SaveChangesAsync();
    }
}