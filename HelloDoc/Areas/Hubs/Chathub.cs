using System;
using Microsoft.AspNetCore.SignalR;

namespace HelloDoc.Areas.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            var client = Context.ConnectionId;
            await Clients.Client(user).SendAsync("ReceiveMessage", user, message);
        }

        public string GetConnectionID()
        {
            var val = Context.ConnectionId;
            return val;
        }
    }
}

