using LevelCounter.Services;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace LevelCounter.Hubs
{
    public class GameHub : Hub
    {
        //public Task SendMessageToGroup(string groupId, string game)
        //{
        //    return Clients.Group(groupId).SendAsync("Send", $"{Context.ConnectionId}: {game}");
        //}

        //public async Task AddToGroup(string groupId)
        //{
        //    await Groups.AddToGroupAsync(Context.ConnectionId, groupId);

        //    await Clients.Group(groupId).SendAsync("Send", $"{Context.ConnectionId} has joined the group {groupId}.");
        //}

        //public async Task RemoveFromGroup(string groupId)
        //{
        //    await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupId);

        //    await Clients.Group(groupId).SendAsync("Send", $"{Context.ConnectionId} has left the group {groupId}.");
        //}

        //public Task SendPrivateMessage(string user, string message)
        //{
        //    return Clients.User(user).SendAsync("ReceiveMessage", message);
        //}

        //public void Send(string name, string message)
        //{
        //    // Call the broadcastMessage method to update clients.
        //    Clients.All.SendAsync("broadcastMessage", name, message);
        //}

        public async Task Send(string gameJson)
        {
            // Call the broadcastMessage method to update clients.
            await Clients.All.SendAsync("broadcastMessage", gameJson);
        }
    }
}
