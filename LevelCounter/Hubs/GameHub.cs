﻿using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace LevelCounter.Hubs
{
    public class GameHub : Hub
    {
        public class ChatHub : Hub
        {
            public Task SendMessageToGroup(string groupId, string game)
            {
                return Clients.Group(groupId).SendAsync("Send", $"{Context.ConnectionId}: {game}");
            }

            public async Task AddToGroup(string groupId)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, groupId);

                await Clients.Group(groupId).SendAsync("Send", $"{Context.ConnectionId} has joined the group {groupId}.");
            }

            public async Task RemoveFromGroup(string groupId)
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupId);

                await Clients.Group(groupId).SendAsync("Send", $"{Context.ConnectionId} has left the group {groupId}.");
            }

            public Task SendPrivateMessage(string user, string message)
            {
                return Clients.User(user).SendAsync("ReceiveMessage", message);
            }
        }
    }
}