using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using System;

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

        public async Task HeartBeat(string gameId, string userId)
        {
            await Clients.Group(gameId)
                .SendAsync("isOnline", userId);
        }

        public async Task SyncUser(string gameId, string inGameUser, string senderId)
        {
            Console.WriteLine("--->" + inGameUser + " sender: " + senderId);
            await Clients.Group(gameId)
                .SendAsync("user", inGameUser);
        }

        public async Task SyncTime(string gameId, string startTime)
        {
            Console.WriteLine("--->" + " Time: " + startTime);
            await Clients.Group(gameId)
                .SendAsync("time", startTime);
        }

        public async Task SendGameMessage(string gameId, string message)
        {
            await Clients.Group(gameId).SendAsync("message", message);
        }

        public async Task SyncRound(string gameId, int round)
        {
            Console.WriteLine("--->" + " Round: " + round);
            await Clients.Group(gameId).SendAsync("round", round);
        }
    }
}
