using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Domain.Entites
{
    public class Chat : BaseEntity<int>
    {
        public string FirstUserId { get; set; }

        public string SecondUserId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public List<ChatMessage> Messages { get; set; } = new List<ChatMessage>();
    }
}