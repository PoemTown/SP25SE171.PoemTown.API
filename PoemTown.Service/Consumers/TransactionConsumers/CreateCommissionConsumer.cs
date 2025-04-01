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

namespace PoemTown.Service.Consumers.TransactionConsumers
{
    public class CreateCommissionConsumer : IConsumer<CreateCommissionTransactionEvent>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateCommissionConsumer(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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

            // Create transaction
            Transaction transaction = new Transaction()
            {
                Amount = message.Amount,
                Description = message.Description,
                Type = message.Type,
                UserEWallet = userEWallet,
                Balance = userEWallet.WalletBalance
            };

            await _unitOfWork.GetRepository<Transaction>().InsertAsync(transaction);
            await _unitOfWork.SaveChangesAsync();

        }
    }
}
