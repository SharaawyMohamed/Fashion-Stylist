using Azure.Core;
using FirebaseAdmin.Messaging;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Writers;
using System;
using System.Security.Claims;
using System.Threading;
using Web.Application.Hubs;
using Web.Domain.DTOs.CartDTO;
using Web.Domain.DTOs.ChatDto;
using Web.Domain.Entites;
using Web.Infrastructure.Data;

namespace Web.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IHubContext<ChatHub> _hubContext;
        public ChatController(AppDbContext context, IHubContext<ChatHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }
        private string GetUserId()
        {

            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
        }
        [Authorize]
        [HttpPost("SendMassage")]
        public async Task<IActionResult> AddNewMassage([FromBody] SendMassageDto dto)

        {
            var userId = GetUserId();
            Chat? chat = null;

            if (dto.ChatId == 0 || dto.ChatId == null)
            {
                chat = await _context.Chats
                  .FirstOrDefaultAsync(c =>
                  (c.FirstUserId == userId && c.SecondUserId == dto.ReceiverUserId) ||
                  (c.SecondUserId == userId && c.FirstUserId == dto.ReceiverUserId));

                if (chat == null)
                {
                    chat = new Chat
                    {
                        FirstUserId = userId,
                        SecondUserId = dto.ReceiverUserId
                    };
                    await _context.Chats.AddAsync(chat);
                    await _context.SaveChangesAsync();
                }
            }
            else
            {
                chat = await _context.Chats.FindAsync(dto.ChatId);
                if (chat == null)
                    return BadRequest( "Chat does not exist");
            }

            var message = new ChatMessage
            {
                SenderUserId = userId,
                ReceiverUserId = dto.ReceiverUserId,
                Content = dto.Content,
                ChatId = chat.id
            };

            await _context.ChatMessages.AddAsync(message);
            await _context.SaveChangesAsync();

       
            await _context.Entry(message).ReloadAsync();

           await _hubContext.Clients.User(userId).SendAsync("MessageSent", message);
            await _hubContext.Clients.User(dto.ReceiverUserId).SendAsync("ReceiveMessage", message);
            var reciver = await _context.Users.FirstOrDefaultAsync(u => u.Id == dto.ReceiverUserId);
           // if (reciver.FCM_Token != null)
            //{
            //    var massage = new Message()
            //    {
            //        Token = reciver.FCM_Token,
            //        Notification = new Notification
            //        {
            //            Title =reciver.FullName,
            //            Body = dto.Content
            //        }
            //    };

            //    string response = await FirebaseMessaging.DefaultInstance.SendAsync(massage);
            //    return Ok("Notification was sended to this User");
            //}

            return Ok("Message sent");
        }
    
       
           [HttpDelete("{MessageId }")]
            public async Task<IActionResult> DeleteMassage([FromRoute] int MessageId)
               {
            var message = await _context.ChatMessages.FindAsync(MessageId);
            if (message == null)
            {
                return Ok( "Massage was deleted");
            }

            _context.ChatMessages.Remove(message);
            await _context.SaveChangesAsync();

            return Ok("Message deleted");
             }


            [Authorize]
            [HttpGet("history/{otherUserId}")]
            public async Task<IActionResult> GetChatHistoryMassage(string otherUserId)
            {
            var userId = GetUserId();
            var chat = await _context.Chats
           .Include(c => c.Messages)
           .FirstOrDefaultAsync(m =>
               (m.FirstUserId == userId && m.SecondUserId ==otherUserId) ||
               (m.SecondUserId ==userId && m.FirstUserId == otherUserId));

            if (chat == null)
                return Ok("لا يوجد محادثة");

            var message = chat.Messages
                .OrderBy(m => m.CreatedAt)
                .Select(m => new GetChatDto
                {
                    Id = m.id,
                    ChatId = m.ChatId,
                    Content = m.Content,
                    IsRead = m.IsRead,
                    SenderUserId = m.SenderUserId,
                    CreatedAt = m.CreatedAt,
                    ReceiverUserId = m.ReceiverUserId
                })
            .ToList();

            return Ok(message);
        }
            [HttpGet("GetAllChats")]
           [Authorize]
            public async Task<IActionResult> GetAllChats()
            {
               var   userId = GetUserId();

            var chats = await _context.Chats
            .AsNoTracking()
            .Where(u => u.FirstUserId == userId || u.SecondUserId == userId)
            .Include(m => m.Messages)
            .ToListAsync();

            var userIds = chats
                .Select(c => c.FirstUserId == userId ? c.SecondUserId : c.FirstUserId)
                .Distinct()
                .ToList();

            var users = await _context.Users
                .Where(u => userIds.Contains(u.Id))
                .ToDictionaryAsync(u => u.Id);

            var results = new List<ChatDto>();

            foreach (var chat in chats)
            {
                var LastMessage = chat.Messages
                    .OrderByDescending(m => m.CreatedAt)
                    .FirstOrDefault();

                var otherUserId = chat.FirstUserId == userId ? chat.SecondUserId : chat.FirstUserId;
                var user = users[otherUserId];

                var unreadCount = chat.Messages.Count(m => m.IsRead == false && m.ReceiverUserId == userId);

                var chatDto = new ChatDto
                {
                    ChatId = chat.id,
                    SenderId = user.Id,
                    UserName = user.UserName!,
                   
                    UnReaded = unreadCount,
                    LastMessage = LastMessage?.Content,
                    Date = LastMessage?.CreatedAt ?? chat.CreatedAt
                };

                results.Add(chatDto);
            }

            return Ok( results);
        }
            [HttpPut("MakeAllMassageRead")]
            public async Task<IActionResult> MakeAllMassageRead(int ChatId)
            {
            var chatExists = await _context.Chats.AnyAsync(c => c.id == ChatId);
            if (!chatExists)
                return BadRequest( "لا يوجد محادثة");

            var unReadMessages = await _context.ChatMessages
                .Where(m => m.ChatId == ChatId && !m.IsRead)
                .ToListAsync();

            foreach (var message in unReadMessages)
                message.IsRead = true;

            await _context.SaveChangesAsync();
            return Ok("All messages marked as read");

        }
    }
}
