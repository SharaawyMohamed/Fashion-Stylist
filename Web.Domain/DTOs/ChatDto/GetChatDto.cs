using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Domain.DTOs.ChatDto
{
    public class GetChatDto
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string SenderUserId { get; set; }
        public string CreatedAtFormatted => CreatedAt.ToString("hh:mm tt");

        public string ReceiverUserId { get; set; }


        public string Content { get; set; }

        public bool IsRead { get; set; } = false;

        public int ChatId { get; set; }
    }
}
