using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;

namespace Framework.SignalR.Hubs
{
    public class BaseHub : Hub
    {
        public override Task OnConnected()
        {
            // My code OnConnected
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            // My code OnDisconnected
            return base.OnDisconnected(stopCalled);
        }

        public async Task JoinGroup(string groupName)
        {
            await Groups.Add(Context.ConnectionId, groupName);
            //var message = connectionId + " joined " + groupName;
            //Clients.Group("Associate").Notify(message);
        }

        public async Task LeaveGroup(string groupName)
        {
            await Groups.Remove(Context.ConnectionId, groupName);
        }

        public async Task Publish(string groupName, object data)
        {
            await Clients.Group(groupName).Publish(data);
        }
    }
}
