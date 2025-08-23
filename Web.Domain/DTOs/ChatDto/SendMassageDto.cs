using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Domain.DTOs.ChatDto
{
    public class SendMassageDto
    {
        [Required]
        public string ReceiverUserId { get; set; }
        [Required]
        public string Content { get; set; }
       
       
        public int ChatId { get; set; } = 0;
    }
}
