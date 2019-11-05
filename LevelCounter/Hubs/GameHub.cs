using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace LevelCounter.Hubs
{
    public class GameHub : Hub
    {
        public async Task Send(string gameJson, string gameId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, gameId);
            await Clients.Group(gameId)
                .SendAsync("broadcastMessage", gameJson);
        }

        public async Task AddToGroup(string gameId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, gameId);
        }

        public async Task RemoveFromGroup(string gameId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, gameId);
        }
    }
}
