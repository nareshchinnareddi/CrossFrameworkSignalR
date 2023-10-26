using Microsoft.AspNetCore.SignalR.Client;

string url = "http://localhost:5000/messageHub";
Console.WriteLine("Server running on {0}", url);

var hubConnection = new HubConnectionBuilder().WithUrl(url).Build();

await hubConnection.StartAsync();

while (true)
{
    Console.WriteLine("Enter your message:");
    string line = Console.ReadLine();
    await hubConnection.InvokeAsync<string>("Publish", "Group1", "Publish", line);
    Console.ReadLine();
}

