using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using PoemTown.Repository.Base;
using PoemTown.Repository.CustomException;
using PoemTown.Repository.Entities;
using PoemTown.Repository.Interfaces;
using PoemTown.Service.BusinessModels.ResponseModels.ChatResponse;
using PoemTown.Service.BusinessModels.ResponseModels.RecordFileResponses;
using PoemTown.Service.BusinessModels.ResponseModels.UserResponses;
using PoemTown.Service.Interfaces;
using PoemTown.Service.QueryOptions.FilterOptions.MessageFilters;
using PoemTown.Service.QueryOptions.FilterOptions.PoemFilters;
using PoemTown.Service.QueryOptions.RequestOptions;
using PoemTown.Service.QueryOptions.SortOptions.MessageSorts;
using PoemTown.Service.QueryOptions.SortOptions.PoemSorts;
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

        public async Task<GetMesssageWithPartner>SendPrivateMessageAsync(Guid fromUser, Guid toUser, string message)
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
            };

            await _unitOfWork.GetRepository<Message>().InsertAsync(msg);
            await _unitOfWork.SaveChangesAsync();

            // Lấy lại message với thông tin user
            var fromUserEntity = await _unitOfWork.GetRepository<User>()
                .AsQueryable()
                .FirstOrDefaultAsync(u => u.Id == fromUser);

            var toUserEntity = await _unitOfWork.GetRepository<User>()
                .AsQueryable()
                .FirstOrDefaultAsync(u => u.Id == toUser);

            var mappedMessage = _mapper.Map<GetMesssageWithPartner>(msg);

            if (fromUserEntity != null)
                mappedMessage.FromUser = _mapper.Map<GetBasicUserInformationResponse>(fromUserEntity);

            if (toUserEntity != null)
                mappedMessage.ToUser = _mapper.Map<GetBasicUserInformationResponse>(toUserEntity);

            return mappedMessage;
        }





        public async Task<PaginationResponse<GetChatPartner>> GetChatPartners(Guid? userId, RequestOptionsBase<GetChatPartnerFilter, GetChatPartnerSort> request)
        {
            var message = _unitOfWork.GetRepository<Message>().AsQueryable();
            var fromUsers = message
                .Where(m => m.FromUserId == userId && m.DeletedTime == null)
                .Select(m => m.ToUser); // Chỉ chọn ToUser

            var toUsers = message
                .Where(m => m.ToUserId == userId && m.DeletedTime == null)
                .Select(m => m.FromUser); // Chỉ chọn FromUser

            var partnerUsers = fromUsers.Concat(toUsers).Distinct();
            //----------------------------------------------------------------------------------------------------------------------------------//
            var queryPaging = await _unitOfWork.GetRepository<User>()
                            .GetPagination(partnerUsers, request.PageNumber, request.PageSize);

            IList<GetChatPartner> partners = new List<GetChatPartner>();
            foreach (var partnerUser in queryPaging.Data)
            {
                var chatPartner = _mapper.Map<GetChatPartner>(partnerUser);
                partners.Add(chatPartner);
                var lastMessage = await _unitOfWork.GetRepository<Message>()
                        .AsQueryable()
                        .Where(m =>
                            (m.FromUserId == userId && m.ToUserId == partnerUser.Id) ||
                            (m.FromUserId == partnerUser.Id && m.ToUserId == userId))
                        .OrderByDescending(m => m.CreatedTime)
                        .FirstOrDefaultAsync();

                if (lastMessage != null)
                {
                    chatPartner.LastMessage = _mapper.Map<GetMesssageWithPartner>(lastMessage);
                }
            }
            return new PaginationResponse<GetChatPartner>(partners, queryPaging.PageNumber, queryPaging.PageSize,
            queryPaging.TotalRecords, queryPaging.CurrentPageRecords);
        }


        public async Task<PaginationResponse<GetMesssageWithPartner>> GetPrivateMessagesWithUser(Guid? fromUserId, Guid toUserId, RequestOptionsBase<object, object> request)
        {
            var message = _unitOfWork.GetRepository<Message>().AsQueryable();
            var messageContent = message.Where(m =>
                        ((m.FromUserId == fromUserId && m.ToUserId == toUserId) ||
                         (m.FromUserId == toUserId && m.ToUserId == fromUserId)) &&
                         m.DeletedTime == null
                    ).OrderBy(m => m.CreatedTime);    


            var queryPaging = await _unitOfWork.GetRepository<Message>()
                .GetPagination(messageContent, request.PageNumber, request.PageSize);

            IList<GetMesssageWithPartner> messages = new List<GetMesssageWithPartner>();
            foreach (var content in queryPaging.Data)
            {
                var chatContent = _mapper.Map<GetMesssageWithPartner>(content);
                messages.Add(chatContent);
                var fromUser = await _unitOfWork.GetRepository<User>()
                        .AsQueryable()
                        .Where(m =>
                            (m.Id == content.FromUserId))
                        .FirstOrDefaultAsync();
                var toUser = await _unitOfWork.GetRepository<User>()
                        .AsQueryable()
                        .Where(m =>
                            (m.Id == content.ToUserId))
                        .FirstOrDefaultAsync();
                if (fromUser != null || toUser != null)
                {
                    chatContent.FromUser = _mapper.Map<GetBasicUserInformationResponse>(fromUser);
                    chatContent.ToUser = _mapper.Map<GetBasicUserInformationResponse>(toUser);
                }
            }

            return new PaginationResponse<GetMesssageWithPartner>(messages, queryPaging.PageNumber, queryPaging.PageSize,
            queryPaging.TotalRecords, queryPaging.CurrentPageRecords);
        }

    }
}
