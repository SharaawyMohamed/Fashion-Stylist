using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Domain.DTOs.ChatDto
{
    public class ChatDto
    {
        public int ChatId { get; set; }
        public string SenderId { get; set; }

        public string LastMessage { get; set; }

        public int UnReaded { get; set; }

        public string UserName { get; set; }
      

        public DateTime Date { get; set; }
    }
}
