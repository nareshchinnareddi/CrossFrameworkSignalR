using Framework.SignalR.Core.Subscriber;
using System;
using System.Threading.Tasks;

namespace Framework.SignalR.Core.Receive
{
    internal class Program
    {
        static async Task Main(string[] args)
        {

            string url = "http://localhost:5000/messageHub";

            Console.WriteLine("Client running");
            BaseSubscriber sub = new BaseSubscriber(url);
            await sub.Connect("Group1");
            sub.Subscribe<string>("Publish");
            sub.publishedEvent += Receive;
            Console.ReadLine();
        }

        static void Receive(object sender, PublishedEventArgs e)
        {
            Console.WriteLine(e.Data);
        }
    }
}
