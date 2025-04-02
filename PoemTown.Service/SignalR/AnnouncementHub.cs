using Microsoft.AspNetCore.SignalR;

namespace PoemTown.Service.SignalR;

public class AnnouncementHub : Hub
{
    private static readonly Dictionary<Guid, string> _connections = null;

    public override Task OnConnectedAsync()
    {
        string? userIdString = Context.User?.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
        if(Guid.TryParse(userIdString, out Guid userId))
        {
            _connections.Add(userId, Context.ConnectionId);
        }
        
        return base.OnConnectedAsync();
    }

    public static string GetConnectionId(Guid userId)
    {
        return _connections.TryGetValue(userId, out var connectionId) ? connectionId : string.Empty;
    }
}