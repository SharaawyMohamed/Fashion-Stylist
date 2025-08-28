using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Application.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string senderId, string receiverId, string content)
        {
            var message = new
            {
                SenderUserId = senderId,
                ReceiverUserId = receiverId,
                Content = content,

                CreatedAtFormatted = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time")).ToString("hh:mm tt")


        

            };


            await Clients.User(receiverId).SendAsync("ReceiveMessage", message);

            
            await Clients.User(senderId).SendAsync("MessageSent", message);
        }

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await base.OnDisconnectedAsync(exception);
        }
    }
}
