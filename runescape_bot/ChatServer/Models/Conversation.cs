using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ChatServer.Models
{
    public class Conversation
    {
        [Key]
        public int ID { get; set; }
        public string SpeakerName { get; set; }

        public string PlayerName { get; set; }
        public virtual Player Player { get; set; }

        public virtual List<Comment> Comments { get; set; }
    }
}