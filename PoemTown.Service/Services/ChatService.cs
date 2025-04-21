using AutoMapper;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using PoemTown.Repository.Base;
using PoemTown.Repository.CustomException;
using PoemTown.Repository.Entities;
using PoemTown.Repository.Enums.Announcements;
using PoemTown.Repository.Interfaces;
using PoemTown.Service.BusinessModels.ResponseModels.ChatResponse;
using PoemTown.Service.BusinessModels.ResponseModels.RecordFileResponses;
using PoemTown.Service.BusinessModels.ResponseModels.UserResponses;
using PoemTown.Service.Events.AnnouncementEvents;
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
using static Betalgo.Ranul.OpenAI.ObjectModels.SharedModels.IOpenAIModels;
using static MassTransit.Util.ChartTable;

namespace PoemTown.Service.Services
{

    public class ChatService : IChatService
    {
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPublishEndpoint _publishEndpoint;

        private static readonly ConcurrentDictionary<string, string> _userConnections = ChatHub.UserConnections;
        public ChatService(IHubContext<ChatHub> hubContext, IUnitOfWork unitOfWork, IMapper mapper, IPublishEndpoint publishEndpoint)
        {
            _hubContext = hubContext;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _publishEndpoint = publishEndpoint;

        }

        public async Task SendMessageToAllAsync(string user, string message)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task<GetMesssageWithPartner>SendPrivateMessageAsync(Guid fromUser, Guid toUser, string message)
        {
            // Gửi SignalR nếu user online
           /* if (_userConnections.TryGetValue(toUser.ToString(), out var connectionId))
            {
                await _hubContext.Clients.Client(connectionId)
                    .SendAsync("ReceiveMessage", fromUser, message);
            }*/
            bool isOnline = _userConnections.TryGetValue(toUser.ToString(), out var connectionId);

            // Lưu vào 
            var msg = new Message
            {
                FromUserId = fromUser,
                ToUserId = toUser,
                IsRead = isOnline,
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

            if (isOnline)
            {
                await _hubContext.Clients.Client(connectionId)
                    .SendAsync("ReceiveMessage", fromUser, message);
            }
            else
            {
                // Nếu người nhận offline, tạo thông báo
                await _publishEndpoint.Publish(new SendUserAnnouncementEvent()
                {
                    UserId = toUser,
                    Title = "Tin nhắn mới",
                    Content = $"Tin nhắn từ {fromUserEntity.DisplayName}: \"{message}\"",
                    IsRead = false,
                    Type = AnnouncementType.Chat,
                });
            }

            

            var mappedMessage = _mapper.Map<GetMesssageWithPartner>(msg);

            if (fromUserEntity != null)
                mappedMessage.FromUser = _mapper.Map<GetBasicUserInformationResponse>(fromUserEntity);

            if (toUserEntity != null)
                mappedMessage.ToUser = _mapper.Map<GetBasicUserInformationResponse>(toUserEntity);

            return mappedMessage;
        }

        public async Task MarkAsReadAsync(Guid fromUserId, Guid toUserId)
        {
            var unreadMessages = await _unitOfWork.GetRepository<Message>()
                .AsQueryable()
                .Where(m => m.FromUserId == fromUserId && m.ToUserId == toUserId && m.IsRead == false)
                .ToListAsync();
            if (unreadMessages == null || unreadMessages.Count == 0)
                return;
            foreach (var msg in unreadMessages)
            {
                msg.IsRead = true;
                _unitOfWork.GetRepository<Message>().Update(msg);
            }
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<PaginationResponse<GetChatPartner>> GetChatPartners(
     Guid? userId,
     RequestOptionsBase<GetChatPartnerFilter, GetChatPartnerSort> request)
        {
            var messageRepo = _unitOfWork.GetRepository<Message>();
            var userRepo = _unitOfWork.GetRepository<User>();

            // B1: Lấy tất cả tin nhắn liên quan đến user, order trước để dùng First() sau này
            var allMessages = await messageRepo.AsQueryable()
                .Where(m => m.DeletedTime == null && (m.FromUserId == userId || m.ToUserId == userId))
                .OrderByDescending(m => m.CreatedTime)
                .Select(m => new
                {
                    Message = m,
                    PartnerId = m.FromUserId == userId ? m.ToUserId : m.FromUserId
                })
                .ToListAsync();

            // B2: Lấy tin nhắn mới nhất với mỗi partner
            var lastMessages = allMessages
                .GroupBy(x => x.PartnerId)
                .Select(g => g.First()) // đã sort sẵn nên First là mới nhất
                .OrderByDescending(x => x.Message.CreatedTime)
                .ToList();

            var partnerIds = lastMessages.Select(x => x.PartnerId).ToList();

            // B3: Truy vấn 1 lần toàn bộ partner user
            var users = await userRepo.AsQueryable()
                .Where(u => partnerIds.Contains(u.Id))
                .ToDictionaryAsync(u => u.Id);

            // B4: Ghép user với message, sắp xếp theo CreatedTime, rồi phân trang
            var fullData = lastMessages
                .Where(x => users.ContainsKey(x.PartnerId)) // loại null user nếu có
                .Select(x => new
                {
                    User = users[x.PartnerId],
                    LastMessage = x.Message
                })
                .OrderByDescending(x => x.LastMessage.CreatedTime)
                .ToList();

            var totalRecords = fullData.Count;
            var skip = (request.PageNumber - 1) * request.PageSize;
            var paged = fullData.Skip(skip).Take(request.PageSize).ToList();

            // B5: Mapping
            var result = paged.Select(x =>
            {
                var partner = _mapper.Map<GetChatPartner>(x.User);
                partner.LastMessage = _mapper.Map<GetMesssageWithPartner>(x.LastMessage);
                return partner;
            }).ToList();

            // B6: Trả về phân trang chuẩn
            return new PaginationResponse<GetChatPartner>(
                result,
                request.PageNumber,
                request.PageSize,
                totalRecords,
                result.Count
            );
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
