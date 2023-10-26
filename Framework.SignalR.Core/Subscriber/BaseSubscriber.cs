using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Framework.SignalR.Core.Subscriber
{
    public class BaseSubscriber
    {
        protected HubConnection _hubConnection;
        protected string _groupName = string.Empty;
        protected bool _disconnecting = false;
        protected string _hubUrl;
        public event EventHandler<PublishedEventArgs> publishedEvent;
        public event EventHandler connectionOpened;
        public event EventHandler connectionClosed;
        public event EventHandler joinGroup;
        public BaseSubscriber(string hubUrl)
        {
            _hubUrl = hubUrl;
        }

        public bool IsConnected()
        {
            if (_hubConnection != null && _hubConnection.State == HubConnectionState.Connected) return true;
            return false;
        }

        public async Task Connect(string groupName, bool reconnect = true)
        {
            _groupName = groupName;
            _disconnecting = false;
            await Connect(reconnect);
        }

        protected async Task Connect(bool reconnect = true)
        {
            try
            {
                if (!_disconnecting)
                {
                    if (_hubConnection == null)
                    {
                        _hubConnection = await CreateHubConnection(_hubUrl);
                        _hubConnection.On<string>("JoinGroup", JoinGroup);
                        _hubConnection.Closed += async (Exception) =>
                        {
                            OnClosed(new EventArgs());
                            if (reconnect)
                            {
                                await Reconnect();
                            }
                        };
                    }

                    if (_hubConnection.State == HubConnectionState.Disconnected)
                    {
                        await _hubConnection.StartAsync();
                    }

                    OnOpened(new EventArgs());

                    await _hubConnection.SendAsync("JoinGroup", _groupName);
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public async Task Disconnect()
        {
            _disconnecting = true;
            if (_hubConnection != null)
            {
                await _hubConnection.SendAsync("LeaveGroup", _groupName);
                await _hubConnection.StopAsync();
            }
        }

        public async Task Reconnect()
        {
            await Task.Delay(5000);

            try
            {
                await Connect();
            }
            catch (Exception ex)
            {
                await Reconnect();
            }
        }

        public void Subscribe<T>(string messageType) where T : class
        {
            if (_hubConnection != null)
            {
                _hubConnection.On<T>(messageType, HandlePublishedChange);
            }
            else
            {
                throw new WebException("Subscriber failed o connected.");
            }
        }

        private void JoinGroup(string groupName)
        {
            OnJoinGroup(new EventArgs());
        }

        protected static Task<HubConnection> CreateHubConnection(string hubUrl)
        {
            HubConnection hubConnection = new HubConnectionBuilder()
                    .WithUrl(hubUrl)
                    .Build();

            return Task.FromResult(hubConnection);
        }

        protected void HandlePublishedChange(object data)
        {
            OnPublishChangeEvent(new PublishedEventArgs(data));
        }

        protected virtual void OnPublishChangeEvent(PublishedEventArgs e)
        {
            publishedEvent?.Invoke(this, e);
        }

        protected virtual void OnOpened(EventArgs e)
        {
            connectionOpened?.Invoke(this, e);
        }

        protected virtual void OnClosed(EventArgs e)
        {
            connectionClosed?.Invoke(this, e);
        }

        protected virtual void OnJoinGroup(EventArgs e)
        {
            joinGroup?.Invoke(this, e);
        }
    }
}