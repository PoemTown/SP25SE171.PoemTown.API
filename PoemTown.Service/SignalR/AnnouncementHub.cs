using Microsoft.AspNetCore.SignalR;
using PoemTown.Service.SignalR.IReceiveClients;

namespace PoemTown.Service.SignalR;

public class AnnouncementHub : Hub<IAnnouncementClient>
{
    public static Dictionary<Guid, string> Connections = new();

    /*public override Task OnConnectedAsync()
    {
        /*string? userIdString = Context.User?.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
        if(Guid.TryParse(userIdString, out Guid userId))
        {
            Connections.Add(userId, Context.ConnectionId);
        }#1#
        var connectionId = Context.ConnectionId;
        Connections.TryAdd(connectionId, connectionId);
        
        return base.OnConnectedAsync();
    }*/
    public Task RegisterUserConnection(Guid userId)
    {
        Connections[userId] = Context.ConnectionId;
        return Task.CompletedTask;
    }
    
    public override Task OnDisconnectedAsync(Exception? exception)
    {
        /*string? userIdString = Context.User?.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
        if(Guid.TryParse(userIdString, out Guid userId))
        {
            Connections.Remove(userId);
        }*/
        var userId = Connections.FirstOrDefault(x => x.Value == Context.ConnectionId).Key;
        Connections.Remove(userId, out _);
        
        return base.OnDisconnectedAsync(exception);
    }
    /*public override Task OnDisconnectedAsync(Exception? exception)
    {
        /*string? userIdString = Context.User?.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
        if(Guid.TryParse(userIdString, out Guid userId))
        {
            Connections.Remove(userId);
        }#1#
        var connectionId = Context.ConnectionId;
        Connections.Remove(connectionId, out _);
        
        return base.OnDisconnectedAsync(exception);
    }*/
    
    public static string GetConnectionId(Guid? userId)
    {
        if(userId == null)
        {
            return string.Empty;
        }
        
        return Connections.TryGetValue(userId.Value, out var connectionId) ? connectionId : string.Empty;
    }
    
    /*public static string GetConnectionId(string connectionId)
    {
        if(string.IsNullOrWhiteSpace(connectionId))
        {
            return string.Empty;
        }
        
        return Connections.ContainsKey(connectionId) ? connectionId : string.Empty;
    }*/
}