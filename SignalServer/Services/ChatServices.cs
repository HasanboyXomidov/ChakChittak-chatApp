using Chat.DataAcces.Data;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SignalServer.Hubs;

namespace SignalServer.Services
{
    public class ChatServices
    {
        private readonly IHubContext<ChatHub> _context;
        private readonly AppDbContext _dbContext;
        private readonly Timer _timer;

        public ChatServices(IHubContext<ChatHub> context, AppDbContext dbContext)
        {
            _context = context;
            _dbContext = dbContext;

            // Set up a timer for polling every 5 seconds (adjust as needed)
            _timer = new Timer(RefreshEmployees, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
        }

        private async void RefreshEmployees(object state)
        {
            var employees = await GetAllEmployees();
            await _context.Clients.All.SendAsync("RefreshEmployees", employees);
        }

        private async Task<List<ChatMessage>> GetAllEmployees()
        {
            return await _dbContext.messages.AsNoTracking().ToListAsync();
        }


    }
}
