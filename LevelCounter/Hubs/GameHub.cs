using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using System;

namespace LevelCounter.Hubs
{
    public class GameHub : Hub
    {
        public async Task Send(string gameJson, string gameId)
        {
            await Clients.Group(gameId)
                .SendAsync("broadcastMessage", gameJson);
        }

        public async Task AddToGroup(string gameId, string userId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, gameId);
        }

        public async Task RemoveFromGroup(string gameId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, gameId);
        }

        public async Task HeartBeat(string gameId, string userId) {
            await Clients.Group(gameId)
                .SendAsync("isOnline", userId);
        }

        public async Task SyncUser(string gameId, string inGameUser) {
            await Clients.Group(gameId)
                .SendAsync("user", inGameUser);
        }

        public async Task SyncTime(string gameId, string time) {
            await Clients.Group(gameId)
                .SendAsync("time", time);
        }

        public async Task SendGameMessage(string gameId, string message) {
            await Clients.Group(gameId).SendAsync("message", message);
        }
    }
}
