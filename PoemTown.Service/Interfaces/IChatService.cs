using PoemTown.Repository.Base;
using PoemTown.Service.BusinessModels.ResponseModels.ChatResponse;
using PoemTown.Service.BusinessModels.ResponseModels.UserResponses;
using PoemTown.Service.QueryOptions.FilterOptions.MessageFilters;
using PoemTown.Service.QueryOptions.RequestOptions;
using PoemTown.Service.QueryOptions.SortOptions.MessageSorts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoemTown.Service.Interfaces
{
    public interface IChatService
    {
        Task SendMessageToAllAsync(string user, string message);
        Task MarkAsReadAsync(Guid fromUserId, Guid toUserId);
        Task<GetMesssageWithPartner> SendPrivateMessageAsync(Guid fromUser, Guid toUser, string message);
        Task<PaginationResponse<GetChatPartner>> GetChatPartners(Guid? userId, RequestOptionsBase<GetChatPartnerFilter, GetChatPartnerSort> request);
        Task<PaginationResponse<GetMesssageWithPartner>> GetPrivateMessagesWithUser(Guid? fromUserId, Guid toUserId, RequestOptionsBase<object, object> request);
    }
}
