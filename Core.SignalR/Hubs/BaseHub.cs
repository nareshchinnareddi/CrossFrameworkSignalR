using Microsoft.AspNetCore.SignalR;

namespace Core.SignalR.Hubs
{
    /// <summary>
    /// Base class for generic site based SignalR Hubs
    /// </summary>
    public class BaseHub : Hub
    {
        public BaseHub()
        {
        }

        public async Task JoinGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            await Clients.Caller.SendAsync("JoinGroup", groupName);
        }

        public async Task RemoveGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);

            await Clients.Caller.SendAsync("LeaveGroup", groupName);
        }

        public async Task Publish(string groupName, string method, object data)
        {
            await Clients.Group(groupName).SendAsync(method, data);
        }
    }
}
