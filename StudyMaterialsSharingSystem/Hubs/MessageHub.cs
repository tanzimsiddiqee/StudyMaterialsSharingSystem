using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudyMaterialsSharingSystem.Hubs
{
    public class MessageHub : Hub
    {
        public Task SendPrivateMessage(string Receiver, string message)
        {
            return Clients.User(Receiver).SendAsync("ReceiveMessage", message);
        }
    }
}
