using ChatCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Client
{
    public interface IChatClient : IAsyncDisposable
    {
        event EventHandler<Message> OnMessageReceived;
        event EventHandler<Users> OnConnected;
        event EventHandler<Users> OnDisconnected;

        Task<IEnumerable<Users>> GetOnlineUsers();
        Task SendMessageAsync(string message);
        Task StartAsync(Users users);
        Task StopAsync();
    }
}
