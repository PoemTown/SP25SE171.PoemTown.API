using MassTransit;
using PoemTown.Repository.Entities;
using PoemTown.Repository.Enums.Transactions;
using PoemTown.Repository.Interfaces;
using PoemTown.Service.Events.TransactionEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PoemTown.Repository.Enums.Announcements;
using PoemTown.Service.Events.AnnouncementEvents;

namespace PoemTown.Service.Consumers.TransactionConsumers
{
    public class CreateCommissionConsumer : IConsumer<CreateCommissionTransactionEvent>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPublishEndpoint _publishEndpoint;

        public CreateCommissionConsumer(IUnitOfWork unitOfWork,
            IPublishEndpoint publishEndpoint)
        {
            _unitOfWork = unitOfWork;
            _publishEndpoint = publishEndpoint;
        }
        public async Task Consume(ConsumeContext<CreateCommissionTransactionEvent> context)
        {
            var message = context.Message;

            var userEWallet = await _unitOfWork.GetRepository<UserEWallet>().FindAsync(p => p.Id == message.UserEWalletId);
            // Check if user e-wallet is null
            if (userEWallet == null)
            {
                throw new Exception("User e-wallet not found");
            }

            Guid transactionId = Guid.NewGuid();
            // Create transaction
            Transaction transaction = new Transaction()
            {
                Id = transactionId,
                Amount = message.Amount,
                Description = message.Description,
                Type = message.Type,
                UserEWallet = userEWallet,
                Balance = userEWallet.WalletBalance
            };

            await _unitOfWork.GetRepository<Transaction>().InsertAsync(transaction);
            await _unitOfWork.SaveChangesAsync();

            Poem? poem = await _unitOfWork.GetRepository<Poem>().FindAsync(p => p.Id == message.PoemId);
            if (poem == null)
            {
                return;
            }
            
            // Send usage right commission percentage announcement to Poem UsageRight Holder 
            await _publishEndpoint.Publish(new SendUserAnnouncementEvent()
            {
                UserId = userEWallet.UserId,
                Title = $"Tiền hoa hồng từ quyền sử dụng bài thơ {poem.Title}",
                Content =
                    $"Bạn đã nhận được: {message.Amount} từ khoản tiền hoa hồng quyền sử dụng bài thơ '{poem.Title}'",
                Type = AnnouncementType.Transaction,
                TransactionId = transactionId,
                IsRead = false
            });
        }
    }
}
