using PoemTown.Repository.Base;
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
        Task SendPrivateMessageAsync(Guid fromUser, Guid toUser, string message);
        Task<PaginationResponse<GetBasicUserInformationResponse>> GetChatPartners(Guid? userId, RequestOptionsBase<GetChatPartnerFilter, GetChatPartnerSort> request);
    }
}
