using System;
using Microsoft.Owin.Hosting;
using Owin;
using Microsoft.Owin.Cors;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNet.SignalR.Client;
using System.Threading.Tasks;
using Framework.SignalR.SignalR.Hubs;

namespace Framework.SignalR.Publish
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            string url = "http://localhost:8080";
            using (WebApp.Start(url))
            {
                Console.WriteLine("Server running on {0}", url);

                var hubConnection = new HubConnection(url);
                IHubProxy hubProxy = hubConnection.CreateHubProxy("CustomHub");

                await hubConnection.Start();

                while (true)
                {
                    Console.WriteLine("Enter your message:");
                    string line = Console.ReadLine();
                    await hubProxy.Invoke<string>("Publish", "Group1", line);
                    Console.ReadLine();
                }
            }
        }
    }
    class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(CorsOptions.AllowAll);
            app.MapSignalR();
        }
    }

    [HubName("CustomHub")]
    public class CustomHub : BaseHub
    {
        public CustomHub() : base()
        {

        }
    }
}