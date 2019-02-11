using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ChatServer.Models
{
    public class Comment
    {
        [Key]
        public int ID { get; set; }
        public string Message { get; set; }
        public DateTime Moment { get; set; }

        public int ConversationID { get; set; }
        public virtual Conversation Conversation { get; set; }
    }
}