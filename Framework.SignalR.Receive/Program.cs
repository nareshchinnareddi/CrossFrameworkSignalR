using Framework.SignalR.SignalR.Subscriber;
using System;
using System.Threading.Tasks;

namespace Framework.SignalR.Receive
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            string url = "http://localhost:8080";
            Console.WriteLine("Client running");
            BaseSubscriber sub = new BaseSubscriber(url);
            await sub.Connect("CustomHub", "Group1");
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
