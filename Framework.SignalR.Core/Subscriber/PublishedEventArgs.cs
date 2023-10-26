namespace Framework.SignalR.Core.Subscriber
{
    public class PublishedEventArgs
    {
        public PublishedEventArgs(object data)
        {
            Data = data;
        }
        public object Data { get; set; }
    }
}