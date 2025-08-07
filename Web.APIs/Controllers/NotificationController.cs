using Azure;
using Azure.Core;
using FirebaseAdmin.Messaging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.Domain.DTOs.NotificationDTO;
using Web.Infrastructure.Data;

namespace Web.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly AppDbContext _context;
        public NotificationController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("{FCMToken}")]
        public async Task<IActionResult> AddNewNorificationToUser([FromRoute]string FCMToken, [FromBody] NotificationDto dto)
        {
            if (FCMToken == null)
            {
                var message = new Message()
                {
                    Token = FCMToken,
                    Notification = new Notification
                    {
                        Title = dto.Title,
                        Body = dto.Description
                    }
                };

                string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
                return Ok("Notification was sended to this User");
            }
            return Ok("You Can't Send Notification to this User");
        }

        [HttpPost("SendToAllUser")]
        public async Task<IActionResult> AddNewNorificationToAllUser([FromBody] NotificationDto dto)
        {
            var message = new Message()
            {
                Topic = "allUsers",
                Notification = new Notification
                {
                    Title = dto.Title,
                    Body = dto.Description
                }
            };

            string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
            return Ok("Successfully sent message to all users");

        }

    }
}