// See https://aka.ms/new-console-template for more information
using Core.SignalR.Subscriber;
using Microsoft.AspNetCore.SignalR.Client;

string url = "http://localhost:5000/messageHub";

Console.WriteLine("Client running");
BaseSubscriber sub = new BaseSubscriber(url);
await sub.Connect("Group1");
sub.Subscribe<string>("Publish");
sub.publishedEvent += Receive;
Console.ReadLine();

static void Receive(object sender, PublishedEventArgs e)
{
    Console.WriteLine(e.Data);
}

