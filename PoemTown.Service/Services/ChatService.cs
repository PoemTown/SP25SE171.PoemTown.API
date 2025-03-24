using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using PoemTown.Repository.Entities;
using PoemTown.Repository.Interfaces;
using PoemTown.Service.Interfaces;
using PoemTown.Service.SignalR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoemTown.Service.Services
{

    public class ChatService : IChatService
    {
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private static readonly ConcurrentDictionary<string, string> _userConnections = ChatHub.UserConnections;
        public ChatService(IHubContext<ChatHub> hubContext, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _hubContext = hubContext;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task SendMessageToAllAsync(string user, string message)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task SendPrivateMessageAsync(Guid fromUser, Guid toUser, string message)
        {
            // Gửi SignalR nếu user online
            if (_userConnections.TryGetValue(toUser.ToString(), out var connectionId))
            {
                await _hubContext.Clients.Client(connectionId)
                    .SendAsync("ReceiveMessage", fromUser, message);
            }
             
            // Lưu vào DB
            var msg = new Message
            {
                FromUserId = fromUser,
                ToUserId = toUser,
                MessageText = message ,
                CreatedTime = DateTime.UtcNow
            };

            await _unitOfWork.GetRepository<Message>().InsertAsync(msg);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
