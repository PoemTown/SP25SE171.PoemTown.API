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
    }
}
