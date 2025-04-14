using MassTransit;
using PoemTown.Repository.Entities;
using PoemTown.Repository.Enums.Announcements;
using PoemTown.Repository.Enums.Transactions;
using PoemTown.Repository.Enums.Wallets;
using PoemTown.Repository.Interfaces;
using PoemTown.Repository.Utils;
using PoemTown.Service.Events.AnnouncementEvents;
using PoemTown.Service.Events.TransactionEvents;

namespace PoemTown.Service.Consumers.TransactionConsumers;

public class UpdatePaidTransactionConsumer : IConsumer<UpdatePaidTransactionEvent>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPublishEndpoint _publishEndpoint;
    
    public UpdatePaidTransactionConsumer(IUnitOfWork unitOfWork,
        IPublishEndpoint publishEndpoint)
    {
        _unitOfWork = unitOfWork;
        _publishEndpoint = publishEndpoint;
    }
    
    public async Task Consume(ConsumeContext<UpdatePaidTransactionEvent> context)
    {
        var message = context.Message;

        // Find the transaction by OrderCode
        var transaction = await _unitOfWork.GetRepository<Transaction>().FindAsync(t => t.TransactionCode == message.TransactionCode);
        
        // Check if transaction is null
        if (transaction == null)
        {
            throw new Exception("Transaction not found");
        }
        
        message.CommissionAmount ??= 0;
        // Update transaction details
        transaction.BankCode = message.BankCode;
        transaction.DiscountAmount = message.DiscountAmount;
        transaction.Checksum = message.Checksum;
        transaction.Status = TransactionStatus.Paid;
        transaction.PaidDate = DateTimeHelper.SystemTimeNow;
        transaction.Balance = transaction.UserEWallet!.WalletBalance;
        transaction.Amount = message.Amount!.Value - message.CommissionAmount.Value;
        transaction.AppId = message.AppId;
        transaction.Description = $"Nạp: '{message.Amount}VNĐ' vào ví điện tử, trừ đi: '{message.CommissionAmount}VNĐ' (5% phí dịch vụ), còn lại: '{message.Amount - message.CommissionAmount}VNĐ'";
        transaction.IsAddToWallet = true;

        // Save changes to the database
        _unitOfWork.GetRepository<Transaction>().Update(transaction);
        
        
        // Create commission fee transaction for admin
        var userAdmin = await _unitOfWork.GetRepository<User>().FindAsync(p => p.UserRoles.Any(ur => ur.Role.Name == "ADMIN"));
        
        // Check if user admin is null
        if (userAdmin == null)
        {
            throw new Exception("User admin not found");
        }
        
        // Find the admin's e-wallet
        UserEWallet? adminEWallet = await _unitOfWork.GetRepository<UserEWallet>().FindAsync(p => p.UserId == userAdmin.Id);
        
        // Check if admin e-wallet is null
        if (adminEWallet == null)
        {
            adminEWallet = new UserEWallet()
            {
                UserId = userAdmin.Id,
                WalletBalance = 0,
                WalletStatus = WalletStatus.Active
            };
            await _unitOfWork.GetRepository<UserEWallet>().InsertAsync(adminEWallet);
        }
        
        // Update admin e-wallet balance
        adminEWallet.WalletBalance += message.CommissionAmount.Value;
        _unitOfWork.GetRepository<UserEWallet>().Update(adminEWallet);
        
        
        // Create transaction for admin about commission
        var transactionAdmin = new Transaction()
        {
            Amount = message.CommissionAmount.Value,
            Description = $"Phí dịch vụ nạp tiền: '{message.CommissionAmount}VNĐ (5% phí dịch vụ)' từ người dùng: '{transaction.UserEWallet.User.UserName}'",
            TransactionCode = OrderCodeGenerator.Generate(),
            UserEWallet = adminEWallet,
            Type = TransactionType.DepositCommissionFee,
            Balance = adminEWallet.WalletBalance,
            Status = TransactionStatus.Paid,
            PaidDate = DateTimeHelper.SystemTimeNow,
            IsAddToWallet = true
        };
        await _unitOfWork.GetRepository<Transaction>().InsertAsync(transactionAdmin);
        await _unitOfWork.SaveChangesAsync();
        
        // Publish event create announcement
        await _publishEndpoint.Publish(new SendUserAnnouncementEvent()
        {
            Title = "Hóa đơn nạp tiền",
            Content = $"Hóa đơn: '{transaction.Description}' đã khởi tạo thành công",
            UserId = transaction.UserEWallet.UserId,
            Type = AnnouncementType.Transaction,
            TransactionId = transaction.Id,
            IsRead = false
        });
    }
}