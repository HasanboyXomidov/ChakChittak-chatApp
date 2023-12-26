using Chat.DataAcces.Data;
using ChatCore;
using Microsoft.AspNetCore.SignalR;

namespace SignalServer.Hubs
{
    public class ChatHub : Hub
    {
        private readonly static Dictionary<string, Users> _users = new();
        public async Task SendMessage(Message message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }
        public async Task Register(Users users)
        {
            var id = this.Context.ConnectionId;
            if(_users.ContainsKey(id))
            {
                return;
            }
            _users.Add(id, users);
            await Clients.Others.SendAsync("Connected", users);

            var msg = new Message(users, $"{users.Name} joined the chat");
            await Clients.Others.SendAsync("ReceiveMessage", msg);
            await Clients.Others.SendAsync("Connected", users);
        }
        public IEnumerable<Users> GetOnlineUsers()
        {
            return _users.Values;
        }
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var id = this.Context?.ConnectionId;
            if(!_users.TryGetValue(id,out Users? users))
            {
                users = Users.Unknown();
            }
            _users.Remove(id);
            var msg = new Message(users, $"{users.Name} has left the chat");
            await Clients.Others.SendAsync("ReceiveMessage",msg);
            await Clients.Others.SendAsync("Disconnected", users);
        }

        // For database 
        public async Task RefreshEmployees(List<ChatMessage> employees)
        {

            await Clients.All.SendAsync("RefreshEmployees", employees);
        }

    }
}
