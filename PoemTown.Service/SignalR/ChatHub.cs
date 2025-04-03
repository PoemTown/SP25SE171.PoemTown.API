using PoemTown.Service.Interfaces;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;
using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;

namespace PoemTown.Service.SignalR
{
    public class ChatHub : Hub
    {
        public static ConcurrentDictionary<string, string> UserConnections = new();

        public override async Task OnConnectedAsync()
        {
            // Get user id from token
            var userId = Context.User?.FindFirst("UserId")?.Value
                          ?? Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value
                          ?? Context.User?.Claims.FirstOrDefault(p => p.Type == "UserId")?.Value;

            // Add user connection
            if (!string.IsNullOrEmpty(userId))
            {
                UserConnections[userId] = Context.ConnectionId;
            }
            await base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = UserConnections.FirstOrDefault(x => x.Value == Context.ConnectionId).Key;
            if (!string.IsNullOrEmpty(userId))
            {
                UserConnections.TryRemove(userId, out _);
            }
            return base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessagehihi(string message)
        {
            await Clients.All.SendAsync("ChatMessage", $"{Context.ConnectionId}: {message}");
        }
    }


}
