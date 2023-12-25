using ChatCore;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Client
{
    public class ChatClient : IChatClient
    {
        private readonly string _hubUrl;
        private Users? _user;
        private bool _started;
        private HubConnection _hubConnection;
        public event EventHandler<Message>? OnMessageReceived;
        public event EventHandler<Users>? OnConnected;
        public event EventHandler<Users>? OnDisconnected;

        public ChatClient(string hubUrl)
        {
            if(string.IsNullOrEmpty(hubUrl))
                    throw new ArgumentNullException(nameof(hubUrl));
            this._hubUrl = hubUrl.TrimEnd('/');
        }       
        public async Task<IEnumerable<Users>> GetOnlineUsers()
        {
            
            if (!_started)            
                throw new InvalidOperationException("Client not started");
            return await _hubConnection.InvokeAsync<IEnumerable<Users>>("GetOnlineUsers");

        }

        public async Task SendMessageAsync(string message)
        {
            if (!_started)
                throw new InvalidOperationException("Client not started");
            var msg = new Message(_user!, message);
            await this._hubConnection.SendAsync("SendMessage", msg);
        }

        public async Task StartAsync(Users users)
        {
            if (_started)
                return;
            
            _hubConnection = new HubConnectionBuilder()
                .WithUrl(this._hubUrl)
                .Build();

            await _hubConnection.StartAsync();

            _hubConnection.On<Message>("ReceiveMessage", message =>
            {
                OnMessageReceived?.Invoke(this, message);
            });

            _hubConnection.On<Users>("Connected", user =>
            {
                OnConnected?.Invoke(this, user);
            });

            _hubConnection.On<Users>("Disconnected", user =>
            {
                OnDisconnected?.Invoke(this, user);
            });

            await _hubConnection.SendAsync("Register" , users);
            _user = users;
            _started = true;

        }

        public async Task StopAsync()
        {
            if (!_started)
                return;
            await _hubConnection.StopAsync();
            await _hubConnection.DisposeAsync();
            _hubConnection = null!;
            _started= false;    
        }
        public async ValueTask DisposeAsync()
        {
            await this.StopAsync();
        }
    }
}
