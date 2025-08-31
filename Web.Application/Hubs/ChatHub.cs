using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;

namespace Web.Application.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IConfiguration _configuration;

        public ChatHub(IConfiguration configuration)
        {
            this._configuration = configuration;
        }
        public async Task SendMessage(string senderId, string receiverId, string content)
        {
            var message = new
            {
                SenderUserId = senderId,
                ReceiverUserId = receiverId,
                Content = content,

                CreatedAtFormatted = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow/*.AddHours(int.Parse(_configuration["ChatSetting:EgyptTimeZone"]))*/, TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time")).ToString("hh:mm tt")




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
