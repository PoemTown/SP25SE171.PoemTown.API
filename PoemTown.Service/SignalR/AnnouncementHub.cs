using Microsoft.AspNetCore.SignalR;
using PoemTown.Service.SignalR.IReceiveClients;

namespace PoemTown.Service.SignalR;

public class AnnouncementHub : Hub<IAnnouncementClient>
{
    public static Dictionary<Guid, string> Connections = new();

    public override Task OnConnectedAsync()
    {
        string? userIdString = Context.User?.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
        if(Guid.TryParse(userIdString, out Guid userId))
        {
            Connections.Add(userId, Context.ConnectionId);
        }
        
        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        string? userIdString = Context.User?.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
        if(Guid.TryParse(userIdString, out Guid userId))
        {
            Connections.Remove(userId);
        }
        
        return base.OnDisconnectedAsync(exception);
    }
    public static string GetConnectionId(Guid? userId)
    {
        if(userId == null)
        {
            return string.Empty;
        }
        
        return Connections.TryGetValue(userId.Value, out var connectionId) ? connectionId : string.Empty;
    }
}